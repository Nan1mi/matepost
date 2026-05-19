using MatePost.Data;
using MatePost.DTOs;
using MatePost.Models;
using Microsoft.EntityFrameworkCore;

namespace MatePost.Services;

public class ParcelService : IParcelService
{
    private readonly AppDbContext _db;

    public ParcelService(AppDbContext db) => _db = db;

    // ── CITIES ──────────────────────────────────────────────
    public async Task<List<CityDto>> GetCitiesAsync()
    {
        // ✅ FIX: спочатку .ToListAsync(), потім маппінг у пам'яті
        var cities = await _db.Cities.OrderBy(c => c.Name).ToListAsync();
        return cities.Select(MapCity).ToList();
    }

    // ── CREATE ──────────────────────────────────────────────
    public async Task<ParcelDto> CreateAsync(CreateParcelDto dto, int userId)
    {
        var senderCity = await _db.Cities.FindAsync(dto.SenderCityId)
            ?? throw new KeyNotFoundException("Місто відправника не знайдено.");
        var receiverCity = await _db.Cities.FindAsync(dto.ReceiverCityId)
            ?? throw new KeyNotFoundException("Місто отримувача не знайдено.");

        var distKm = HaversineKm(senderCity.Latitude, senderCity.Longitude,
                                  receiverCity.Latitude, receiverCity.Longitude);
        var price = CalculatePrice(dto.WeightKg, distKm);
        var days = distKm < 200 ? 1 : distKm < 500 ? 2 : 3;

        var parcel = new Parcel
        {
            TrackingNumber = GenerateTracking(),
            SenderId = userId,
            SenderName = dto.SenderName,
            SenderPhone = dto.SenderPhone,
            SenderCityId = dto.SenderCityId,
            ReceiverName = dto.ReceiverName,
            ReceiverPhone = dto.ReceiverPhone,
            ReceiverCityId = dto.ReceiverCityId,
            CurrentCityId = dto.SenderCityId,
            Description = dto.Description,
            WeightKg = dto.WeightKg,
            Price = price,
            Status = ParcelStatus.Created,
            IsPaid = false,
            CreatedAt = DateTime.UtcNow,
            EstimatedDelivery = DateTime.UtcNow.AddDays(days)
        };

        parcel.Events.Add(new TrackingEvent
        {
            Status = ParcelStatus.Created,
            Description = $"Посилку зареєстровано у відділенні {senderCity.Name}.",
            CityId = dto.SenderCityId,
            Timestamp = DateTime.UtcNow
        });

        _db.Parcels.Add(parcel);

        var sender = await _db.Users.FindAsync(userId);
        if (sender != null) sender.LoyaltyPoints += (int)price;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(parcel.Id);
    }

    // ── QUERIES ──────────────────────────────────────────────
    public async Task<List<ParcelDto>> GetByUserAsync(int userId)
    {
        // ✅ FIX: ToListAsync() перед Select з кастомним методом
        var parcels = await QueryParcels()
            .Where(p => p.SenderId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return parcels.Select(MapParcel).ToList();
    }

    public async Task<List<ParcelDto>> GetAllAsync()
    {
        // ✅ FIX
        var parcels = await QueryParcels()
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return parcels.Select(MapParcel).ToList();
    }

    public async Task<ParcelDto> GetByTrackingNumberAsync(string trackingNumber)
    {
        var p = await QueryParcels().FirstOrDefaultAsync(x => x.TrackingNumber == trackingNumber)
            ?? throw new KeyNotFoundException("Посилку не знайдено.");
        return MapParcel(p);
    }

    public async Task<ParcelDto> GetByIdAsync(int id)
    {
        var p = await QueryParcels().FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new KeyNotFoundException("Посилку не знайдено.");
        return MapParcel(p);
    }

    // ── UPDATE & DELETE ──────────────────────────────────────
    public async Task<ParcelDto> UpdateStatusAsync(int id, UpdateParcelStatusDto dto)
    {
        var parcel = await _db.Parcels.FindAsync(id)
            ?? throw new KeyNotFoundException("Посилку не знайдено.");

        parcel.Status = dto.Status;
        if (dto.CurrentCityId.HasValue)
            parcel.CurrentCityId = dto.CurrentCityId;

        var city = dto.CurrentCityId.HasValue
            ? await _db.Cities.FindAsync(dto.CurrentCityId.Value) : null;

        var description = string.IsNullOrWhiteSpace(dto.Description)
            ? DefaultDescription(dto.Status, city?.Name)
            : dto.Description;

        _db.TrackingEvents.Add(new TrackingEvent
        {
            ParcelId = id,
            Status = dto.Status,
            Description = description,
            CityId = dto.CurrentCityId,
            Timestamp = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var parcel = await _db.Parcels.FirstOrDefaultAsync(p => p.Id == id && p.SenderId == userId);
        if (parcel == null) return false;
        _db.Parcels.Remove(parcel);
        await _db.SaveChangesAsync();
        return true;
    }

    // ── MAP ──────────────────────────────────────────────────
    public async Task<List<ParcelDto>> GetMapParcelsAsync()
    {
        // ✅ FIX: ToListAsync() перед Select
        var parcels = await QueryParcels()
            .Where(p => p.Status != ParcelStatus.Delivered &&
                        p.Status != ParcelStatus.Cancelled &&
                        p.CurrentCityId != null)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return parcels.Select(MapParcel).ToList();
    }

    // ── HELPERS ──────────────────────────────────────────────
    private IQueryable<Parcel> QueryParcels() =>
        _db.Parcels
            .Include(p => p.SenderCity)
            .Include(p => p.ReceiverCity)
            .Include(p => p.CurrentCity)
            .Include(p => p.Events).ThenInclude(e => e.City);

    private static ParcelDto MapParcel(Parcel p) => new()
    {
        Id = p.Id,
        TrackingNumber = p.TrackingNumber,
        SenderName = p.SenderName,
        SenderPhone = p.SenderPhone,
        SenderCity = p.SenderCity != null ? MapCity(p.SenderCity) : null,
        ReceiverName = p.ReceiverName,
        ReceiverPhone = p.ReceiverPhone,
        ReceiverCity = p.ReceiverCity != null ? MapCity(p.ReceiverCity) : null,
        CurrentCity = p.CurrentCity != null ? MapCity(p.CurrentCity) : null,
        Description = p.Description,
        WeightKg = p.WeightKg,
        Price = p.Price,
        Status = p.Status.ToString(),
        IsPaid = p.IsPaid,
        CreatedAt = p.CreatedAt,
        EstimatedDelivery = p.EstimatedDelivery,
        Events = p.Events
            .OrderByDescending(e => e.Timestamp)
            .Select(e => new TrackingEventDto
            {
                Id = e.Id,
                Status = e.Status.ToString(),
                Description = e.Description,
                City = e.City != null ? MapCity(e.City) : null,
                Timestamp = e.Timestamp
            }).ToList()
    };

    private static CityDto MapCity(City c) => new()
    {
        Id = c.Id,
        Name = c.Name,
        Region = c.Region,
        Latitude = c.Latitude,
        Longitude = c.Longitude
    };

    private static string GenerateTracking()
    {
        var rnd = new Random();
        return $"MP{DateTime.UtcNow:yyMMdd}{rnd.Next(1000, 9999)}UA";
    }

    private static decimal CalculatePrice(decimal weightKg, double distKm) =>
        Math.Round(50m + weightKg * 10m + (decimal)distKm * 0.05m, 2);

    private static double HaversineKm(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = ToRad(lat2 - lat1);
        var dLon = ToRad(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
              + Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2))
              * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        return R * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
    }

    private static double ToRad(double deg) => deg * Math.PI / 180;

    private static string DefaultDescription(ParcelStatus status, string? city) => status switch
    {
        ParcelStatus.Processing => "Посилку прийнято в обробку.",
        ParcelStatus.InTransit => city != null ? $"Посилку відправлено до {city}." : "Посилка в дорозі.",
        ParcelStatus.ArrivedAtCity => city != null ? $"Посилка прибула до {city}." : "Посилка прибула на склад.",
        ParcelStatus.OutForDelivery => "Кур'єр вирушив з доставкою.",
        ParcelStatus.Delivered => "Посилку успішно вручено.",
        ParcelStatus.Returned => "Посилку повернуто відправнику.",
        ParcelStatus.Cancelled => "Посилку скасовано.",
        _ => "Статус оновлено."
    };
}