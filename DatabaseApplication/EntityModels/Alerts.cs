using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseApplication.EntityModels
{
    [Table("Alerts")]
    public class Alerts: BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; } // Primary key

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; } // Timestamp of when the alert was created

        [Column("message")]
        public string? Message { get; set; } // Alert message text

        [Column("status")]
        public bool? Status { get; set; } // Status of the alert (e.g., active/inactive)
    }
}

