
namespace MatePost.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User"; // User | Operator | Admin
    public int LoyaltyPoints { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Parcel> SentParcels { get; set; } = new();
}