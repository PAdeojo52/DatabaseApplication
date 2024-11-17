using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseApplication.NewFolder
{
    public class NewUser
    {
        [Table("User")]
        public class User
        {
            [Key]
            public int id { get; set; }

            [Required]
            public string first_name { get; set; }

            [Required]
            public string last_name { get; set; }

            [Required]
            [EmailAddress]
            public string email { get; set; }

            [Required]
            [Column("password")] // Map to the "password" column
            public string password { get; set; }

            [BindNever]
            public string hashed_password { get; set; }
        }

    }
}
