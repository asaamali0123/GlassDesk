using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GlassDesk.Models
{
    public static class ComplaintDepartments
    {
        public const string MunicipalServices = "Municipal Services";
        public const string WaterAndSanitation = "Water and Sanitation";
        public const string PublicHealth = "Public Health-Service Administration";
        public const string PublicEducation = "Public Education-Service Administration";
        public const string TransportAndRoads = "Transport and Road Services";

        public static readonly List<string> All = new()
        {
            MunicipalServices,
            WaterAndSanitation,
            PublicHealth,
            PublicEducation,
            TransportAndRoads
        };
    }

    public static class ComplaintStatus
    {
        public const string Submitted = "Submitted";
        public const string UnderVerification = "UnderVerification";
        public const string Assigned = "Assigned";
        public const string InProgress = "InProgress";
        public const string AwaitingCitizenReview = "AwaitingCitizenReview";
        public const string Resolved = "Resolved";
        public const string Disputed = "Disputed";
        public const string ClosedNoResponse = "Closed - No Citizen Response";
        public const string Reopened = "Reopened";
        public const string Rejected = "Rejected";
    }

    public static class ComplaintPriority
    {
        public const string Critical = "Critical";
        public const string High = "High";
        public const string Normal = "Normal";
        public const string Low = "Low";

        public static readonly List<string> All = new()
        {
            Critical,
            High,
            Normal,
            Low
        };
    }

    public class Complaint
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Public Tracking ID shown to citizens and public lookup (e.g. GD-7K4P-9X2M)
        public string PublicId { get; set; } = string.Empty;

        // Citizen Details (Private - never exposed in public views)
        [BsonRepresentation(BsonType.ObjectId)]
        public string CitizenId { get; set; } = string.Empty;
        public string CitizenName { get; set; } = string.Empty;
        public string CitizenEmail { get; set; } = string.Empty;
        public string CitizenPhone { get; set; } = string.Empty;

        // Complaint Core Content
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        // Approximate area (safe for public statistics, e.g. "Sector G-9")
        public string ApproximateLocation { get; set; } = string.Empty;

        // Urgency reported by citizen (Low, Medium, High)
        public string CitizenReportedUrgency { get; set; } = "Medium";

        // Official Priority assigned by Verification Officer (Critical, High, Normal, Low)
        public string OfficialPriority { get; set; } = ComplaintPriority.Normal;
        public string? PriorityReason { get; set; }

        // Assignment
        public string AssignedDepartment { get; set; } = string.Empty;
        [BsonRepresentation(BsonType.ObjectId)]
        public string? AssignedOfficerId { get; set; }
        public string? AssignedOfficerName { get; set; }

        // State Machine Status
        public string Status { get; set; } = ComplaintStatus.Submitted;

        // Resolution proposal by Department Officer
        public string? ProposedResolutionSummary { get; set; }
        public DateTime? ProposedResolutionDate { get; set; }

        // 7-day citizen review deadline
        public DateTime? CitizenReviewDeadline { get; set; }

        // Optional citizen feedback rating (1 to 5 stars) and comment
        public int? CitizenRating { get; set; }
        public string? CitizenFeedbackComment { get; set; }

        // Timestamps
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? AcknowledgedAt { get; set; }
        public DateTime? TargetAcknowledgmentDate { get; set; }
        public DateTime? TargetResolutionDate { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // Flags for SLA & Escalation
        public bool IsAcknowledged { get; set; } = false;
        public bool IsOverdue { get; set; } = false;
        public bool IsEscalated { get; set; } = false;
    }
}