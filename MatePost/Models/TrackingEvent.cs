namespace MatePost.Models;

public class TrackingEvent
{
    public int Id { get; set; }
    public int ParcelId { get; set; }
    public Parcel Parcel { get; set; } = null!;
    public ParcelStatus Status { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? CityId { get; set; }
    public City? City { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}