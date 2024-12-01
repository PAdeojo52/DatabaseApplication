namespace DatabaseApplication.EntityModels
{
    public class Item
    {
        public long id { get; set; } // Maps to "id" column
        public DateTime CreatedAt { get; set; } // Maps to "created_at"
        public string Name { get; set; } // Maps to "name"
        public string Description { get; set; } // Maps to "description"
        public string Category { get; set; } // Maps to "category"
        public long Stock { get; set; } // Maps to "stock"
        public double Price { get; set; } // Maps to "price"
        public string? Photo { get; set; } // Maps to "photo"
        public long Creator { get; set; } // Maps to "creater"
        public long? ItemLocation { get; set; } // Maps to "item_location"
        public long StockOut { get; set; } // Maps to "stockout"
    }
}
