using static DatabaseApplication.NewFolder.NewUser;
using static Supabase.Realtime.Constants;

namespace ApplicationTests.SupabaseTest
{
    public class SupabaseConnectionTests
    {
        private readonly Supabase.Client _client;

        public SupabaseConnectionTests()
        {
            _client = new Supabase.Client(
                "https://fgplemjtxynlxftuwsit.supabase.co", // Replace with your Supabase URL
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZncGxlbWp0eHlubHhmdHV3c2l0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzE2NDI2NDcsImV4cCI6MjA0NzIxODY0N30.lCwTE-hKQCiRksCQRzvRbjfcCzmLIhkK9VjGDmU90mA", // Replace with your Supabase API key
                new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = false,
                });
        }

        [Fact]
        public async Task SupabaseClient_ShouldInitializeSuccessfully()
        {
            // Act
            await _client.InitializeAsync();

            // Assert
            Assert.NotNull(_client);
        }

        [Fact]
        public async Task SupabaseClient_ShouldFetchUsersSuccessfully()
        {
            // Arrange
            await _client.InitializeAsync();

            // Act
            var users = await _client.From<User>().Get();

            // Assert
            Assert.NotNull(users);
            Assert.NotEmpty(users.Models); // Ensure there is at least one record
            Assert.True(users.Models.Count > 0, "No users found in the table.");
        }

        [Fact]
        public async Task SupabaseClient_ShouldInsertUserSuccessfully()
        {
            // Arrange
            await _client.InitializeAsync();

            var newUser = new User
            {
                FirstName = "Peter",
                LastName = "Test",
                Email = "testuser@example.com"
            };

            // Act
            var response = await _client.From<User>().Insert(newUser);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(newUser.Email, response.Models[0].Email);
        }

        [Fact]
        public async Task SupabaseClient_ShouldSubscribeToTableRealtime()
        {
            // Arrange
            await _client.InitializeAsync();

            var isUpdateReceived = false;

            // Subscribing to real-time changes
           /* _client.From<User>("Users").On(EventType.INSERT, payload =>
            {
                isUpdateReceived = true;
            });*/

            // Connect to Realtime
            _client.Realtime.Connect();

            // Act
            // Manually trigger a change in the "Users" table while the test is running
            await Task.Delay(5000); // Wait to see if an update is received

            // Assert
            Assert.True(isUpdateReceived, "No real-time updates received from the 'Users' table.");
        }

        [Fact,]
        public async Task SupabaseClient_ShouldFailWithInvalidCredentials()
        {
            
            // Arrange
            var client = new Supabase.Client(
                "https://invalid.supabase.co",
                "invalid-api-key",
                new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = false,
                });

            try
            {
                // Act
                await client.InitializeAsync();
                client.Auth.LoadSession();

                // If initialization succeeds, the test should fail
                Assert.True(false, "The Supabase client initialized successfully with invalid credentials, which is unexpected.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.NotNull(ex); // Ensure an exception is thrown
                Assert.Contains("Failed to initialize", ex.Message); // Optionally check the exception message
            }
        }
    }
}