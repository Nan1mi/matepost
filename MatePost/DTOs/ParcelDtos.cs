using System.ComponentModel.DataAnnotations;
using MatePost.Models;

namespace MatePost.DTOs;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class CreateParcelDto
{
    [Required] public string SenderName { get; set; } = string.Empty;
    [Required] public string SenderPhone { get; set; } = string.Empty;
    [Required] public int SenderCityId { get; set; }

    [Required] public string ReceiverName { get; set; } = string.Empty;
    [Required] public string ReceiverPhone { get; set; } = string.Empty;
    [Required] public int ReceiverCityId { get; set; }

    public string Description { get; set; } = string.Empty;
    [Required][Range(0.01, 999)] public decimal WeightKg { get; set; }
}

public class UpdateParcelStatusDto
{
    [Required] public ParcelStatus Status { get; set; }
    public int? CurrentCityId { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class ParcelDto
{
    public int Id { get; set; }
    public string TrackingNumber { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SenderPhone { get; set; } = string.Empty;
    public CityDto? SenderCity { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public string ReceiverPhone { get; set; } = string.Empty;
    public CityDto? ReceiverCity { get; set; }
    public CityDto? CurrentCity { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
    public List<TrackingEventDto> Events { get; set; } = new();
}

public class TrackingEventDto
{
    public int Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public CityDto? City { get; set; }
    public DateTime Timestamp { get; set; }
}