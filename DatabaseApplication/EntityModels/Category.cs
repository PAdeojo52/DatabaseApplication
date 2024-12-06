using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace DatabaseApplication.EntityModels
{
    [Table("Category")] // Maps to the "Category" table in Supabase
    public class Category : BaseModel
    {
        [PrimaryKey("id")] // Supabase Primary Key
        [Column("id")]
        public int Id { get; set; } // Maps to the "id" column

        [Column("Name")]
        public string Name { get; set; } // Maps to the "Name" column
    }
}
