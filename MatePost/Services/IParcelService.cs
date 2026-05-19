using MatePost.DTOs;

namespace MatePost.Services;

public interface IParcelService
{
    Task<List<CityDto>> GetCitiesAsync();
    Task<ParcelDto> CreateAsync(CreateParcelDto dto, int userId);
    Task<List<ParcelDto>> GetByUserAsync(int userId);
    Task<List<ParcelDto>> GetAllAsync();
    Task<ParcelDto> GetByIdAsync(int id);
    Task<ParcelDto> GetByTrackingNumberAsync(string trackingNumber);
    Task<ParcelDto> UpdateStatusAsync(int id, UpdateParcelStatusDto dto);
    Task<List<ParcelDto>> GetMapParcelsAsync();
    Task<bool> DeleteAsync(int id, int userId);
}