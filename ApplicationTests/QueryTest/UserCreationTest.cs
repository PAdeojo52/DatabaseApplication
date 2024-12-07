using DatabaseApplication.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.QueryTest
{
    public class UserCreationTest
    {
        private readonly Supabase.Client _supabaseClient;

        public UserCreationTest()
        {
            _supabaseClient = new Supabase.Client(
                "https://fgplemjtxynlxftuwsit.supabase.co", // Replace with your Supabase URL
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZncGxlbWp0eHlubHhmdHV3c2l0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzE2NDI2NDcsImV4cCI6MjA0NzIxODY0N30.lCwTE-hKQCiRksCQRzvRbjfcCzmLIhkK9VjGDmU90mA", // Replace with your Supabase API key
                new Supabase.SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true
                });
        }

        [Fact]
        public async Task InsertRandomUsersTest()
        {
            var random = new Random();

            // Define options for each field
            var firstNames = new[] { "John", "Jane", "Alex", "Emma", "Chris", "Olivia", "Daniel", "Sophia", "James", "Ava", "Michael", "Isabella", "Ethan", "Mia", "Liam", "Amelia", "Noah", "Charlotte", "Lucas", "Harper" };
            var lastNames = new[] { "Smith", "Johnson", "Brown", "Williams", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Wilson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson", "White", "Harris" };
            var emailDomains = new[] { "example.com", "test.com", "demo.com", "mock.com", "sample.com" };
            var passwords = Enumerable.Range(1, 30).Select(i => $"Password{i}").ToArray();

            // Insert 30 users
            for (int i = 0; i < 30; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var email = $"{firstName.ToLower()}.{lastName.ToLower()}@{emailDomains[random.Next(emailDomains.Length)]}";
                var password = passwords[random.Next(passwords.Length)];
                var hashedPassword = HashPassword(password); // Simulate password hashing

                var user = new NewUser.User
                {
                    Id = random.Next(1, int.MaxValue),
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Password = password,
                    HashedPassword = hashedPassword
                };

                // Insert the user into the database
                var response = await _supabaseClient.From<NewUser.User>().Insert(user);

                Assert.True(response.Models.Count > 0, "User insertion failed.");
            }
        }

        private string HashPassword(string password)
        {
            // Simulate password hashing (replace with actual hashing logic if required)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
