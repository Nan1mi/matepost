using MatePost.Models;
using Microsoft.EntityFrameworkCore;

namespace MatePost.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Parcel> Parcels => Set<Parcel>();
    public DbSet<TrackingEvent> TrackingEvents => Set<TrackingEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Role).HasDefaultValue("User");
        });

        modelBuilder.Entity<Parcel>(e =>
        {
            e.HasOne(p => p.Sender).WithMany(u => u.SentParcels)
                .HasForeignKey(p => p.SenderId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.SenderCity).WithMany()
                .HasForeignKey(p => p.SenderCityId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.ReceiverCity).WithMany()
                .HasForeignKey(p => p.ReceiverCityId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne(p => p.CurrentCity).WithMany()
                .HasForeignKey(p => p.CurrentCityId).OnDelete(DeleteBehavior.SetNull);
            e.Property(p => p.Price).HasPrecision(10, 2);
            e.Property(p => p.WeightKg).HasPrecision(8, 3);
        });

        modelBuilder.Entity<TrackingEvent>(e =>
        {
            e.HasOne(t => t.Parcel).WithMany(p => p.Events)
                .HasForeignKey(t => t.ParcelId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(t => t.City).WithMany()
                .HasForeignKey(t => t.CityId).OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<City>().HasData(UkrainianCities);
    }

    private static readonly City[] UkrainianCities =
    {
        new() { Id=1,  Name="Київ",                Region="Київська",           Latitude=50.4501, Longitude=30.5234 },
        new() { Id=2,  Name="Харків",              Region="Харківська",         Latitude=49.9935, Longitude=36.2304 },
        new() { Id=3,  Name="Одеса",               Region="Одеська",            Latitude=46.4825, Longitude=30.7233 },
        new() { Id=4,  Name="Дніпро",              Region="Дніпропетровська",   Latitude=48.4647, Longitude=35.0462 },
        new() { Id=5,  Name="Запоріжжя",           Region="Запорізька",         Latitude=47.8388, Longitude=35.1396 },
        new() { Id=6,  Name="Львів",               Region="Львівська",          Latitude=49.8397, Longitude=24.0297 },
        new() { Id=7,  Name="Кривий Ріг",          Region="Дніпропетровська",   Latitude=47.9078, Longitude=33.3428 },
        new() { Id=8,  Name="Миколаїв",            Region="Миколаївська",       Latitude=46.9750, Longitude=31.9946 },
        new() { Id=9,  Name="Маріуполь",           Region="Донецька",           Latitude=47.0956, Longitude=37.5494 },
        new() { Id=10, Name="Луганськ",            Region="Луганська",          Latitude=48.5740, Longitude=39.3078 },
        new() { Id=11, Name="Вінниця",             Region="Вінницька",          Latitude=49.2328, Longitude=28.4682 },
        new() { Id=12, Name="Херсон",              Region="Херсонська",         Latitude=46.6354, Longitude=32.6169 },
        new() { Id=13, Name="Полтава",             Region="Полтавська",         Latitude=49.5883, Longitude=34.5514 },
        new() { Id=14, Name="Чернігів",            Region="Чернігівська",       Latitude=51.4982, Longitude=31.2893 },
        new() { Id=15, Name="Черкаси",             Region="Черкаська",          Latitude=49.4444, Longitude=32.0598 },
        new() { Id=16, Name="Суми",                Region="Сумська",            Latitude=50.9077, Longitude=34.7981 },
        new() { Id=17, Name="Житомир",             Region="Житомирська",        Latitude=50.2547, Longitude=28.6587 },
        new() { Id=18, Name="Рівне",               Region="Рівненська",         Latitude=50.6199, Longitude=26.2516 },
        new() { Id=19, Name="Івано-Франківськ",    Region="Івано-Франківська",  Latitude=48.9226, Longitude=24.7111 },
        new() { Id=20, Name="Тернопіль",           Region="Тернопільська",      Latitude=49.5535, Longitude=25.5948 },
        new() { Id=21, Name="Луцьк",               Region="Волинська",          Latitude=50.7472, Longitude=25.3254 },
        new() { Id=22, Name="Ужгород",             Region="Закарпатська",       Latitude=48.6208, Longitude=22.2879 },
        new() { Id=23, Name="Хмельницький",        Region="Хмельницька",        Latitude=49.4229, Longitude=26.9964 },
        new() { Id=24, Name="Кам'янець-Подільський", Region="Хмельницька",     Latitude=48.6792, Longitude=26.5833 },
        new() { Id=25, Name="Кропивницький",       Region="Кіровоградська",     Latitude=48.5132, Longitude=32.2597 },
    };
}