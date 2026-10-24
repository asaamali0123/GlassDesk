using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GlassDesk.Models
{
    public static class ActionTypes
    {
        public const string Submitted = "Submitted";
        public const string Verified = "Verified";
        public const string PriorityAssigned = "Priority Assigned";
        public const string DepartmentAssigned = "Department Assigned";
        public const string OfficerAssigned = "Officer Assigned";
        public const string Acknowledged = "Acknowledged";
        public const string ProgressUpdated = "Progress Updated";
        public const string Transferred = "Transferred";
        public const string ProposedResolution = "Proposed Resolution";
        public const string CitizenConfirmed = "Citizen Confirmed Resolution";
        public const string CitizenDisputed = "Citizen Disputed Resolution";
        public const string ClosedNoResponse = "Closed (No Citizen Response)";
        public const string Reopened = "Reopened by Supervisor";
        public const string Rejected = "Rejected";
    }

    public class ComplaintAction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ComplaintId { get; set; } = string.Empty;

        // One of ActionTypes
        public string ActionType { get; set; } = ActionTypes.Submitted;

        // Details on what changed
        public string? PreviousStatus { get; set; }
        public string? NewStatus { get; set; }

        // Remarks / explanation / progress notes
        public string Notes { get; set; } = string.Empty;

        // Who performed this action?
        [BsonRepresentation(BsonType.ObjectId)]
        public string? PerformedByUserId { get; set; }
        public string PerformedByName { get; set; } = "System";
        public string PerformedByRole { get; set; } = "System";

        // Timestamp of action
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Privacy: true = visible only to staff; false = visible on citizen timeline
        public bool IsInternal { get; set; } = false;
    }
}