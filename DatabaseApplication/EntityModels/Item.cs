
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase;


namespace DatabaseApplication.EntityModels
{
    [Table("Items")] // Maps to the "Items" table in Supabase
    public class Item : BaseModel
    {
        [PrimaryKey("id")] // Supabase Primary Key
        [Column("id")]
        public long Id { get; set; } // Maps to the "id" column

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } // Maps to the "created_at" column

        [Column("name")]
        public string? Name { get; set; } // Maps to the "name" column

        [Column("description")]
        public string? Description { get; set; } // Maps to the "description" column

        [Column("category")]
        public int? Category { get; set; } // Maps to the "category" column

        [Column("price")]
        public double Price { get; set; } // Maps to the "price" column

        [Column("photo")]
        public string? Photo { get; set; } // Maps to the "photo" column

        [Column("creater")]
        public long Creator { get; set; } // Maps to the "creater" column

        [Column("item_location")]
        public long? ItemLocation { get; set; } // Maps to the "item_location" column

        [Column("Make")]
        public string? Make { get; set; } // Maps to the "Make" column

        [Column("Model")]
        public string? Model { get; set; } // Maps to the "Model" column

        [Column("CheckedIn")]
        public bool? CheckedIn { get; set; } // Maps to the "CheckedIn" column
    }
}
