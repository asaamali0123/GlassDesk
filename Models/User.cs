using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GlassDesk.Models
{
    public static class UserRoles
    {
        public const string Citizen = "Citizen";
        public const string VerificationOfficer = "VerificationOfficer";
        public const string DepartmentOfficer = "DepartmentOfficer";
        public const string Supervisor = "Supervisor";
        public const string Administrator = "Administrator";
    }

    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        // Stores a secure, salted hash (never plain text)
        public string PasswordHash { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // Role: Citizen, VerificationOfficer, DepartmentOfficer, Supervisor, Administrator
        public string Role { get; set; } = UserRoles.Citizen;

        // For Department Officers, Verification Officers, or Supervisors
        // E.g., "Water and Sanitation", "Municipal Services"
        public string? Department { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}