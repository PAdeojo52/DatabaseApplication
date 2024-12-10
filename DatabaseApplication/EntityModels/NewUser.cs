
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase;


namespace DatabaseApplication.NewFolder
{

    public class NewUser : BaseModel
    {
        [Table("User")] // Map the class to the "User" table
        public class User : BaseModel
        {
            [PrimaryKey("id")] // Supabase Primary Key
            [Column("id")]
            public int? Id { get; set; }

            [Column("first_name")]
            public string FirstName { get; set; }

            [Column("last_name")]
            public string LastName { get; set; }

            [Column("email")]
            public string Email { get; set; }

            [Column("password")] // Map to the "password" column
            public string Password { get; set; }

            [Column("hashed_password")] // Map to the "hashed_password" column
            public string HashedPassword { get; set; }

            [Column("Autherization")]
            public int? Autherization { get; set; }

        }
    }
}
