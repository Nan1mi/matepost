using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatePost.Models;
public enum ParcelStatus
{
    Created = 0,
    Processing = 1,
    InTransit = 2,
    ArrivedAtCity = 3,
    OutForDelivery = 4,
    Delivered = 5,
    Returned = 6,
    Cancelled = 7
}
public class Parcel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;
    public string SenderName { get; set; } = string.Empty;
    public string SenderPhone { get; set; } = string.Empty;
    public int SenderCityId { get; set; }
    public City SenderCity { get; set; } = null!;
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverPhone { get; set; } = string.Empty;
    public int ReceiverCityId { get; set; }
    public City ReceiverCity { get; set; } = null!;
    public int? CurrentCityId { get; set; }
    public City? CurrentCity { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public decimal Price { get; set; }
    public ParcelStatus Status { get; set; } = ParcelStatus.Created;
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? EstimatedDelivery { get; set; }
    public List<TrackingEvent> Events { get; set; } = new();
}
