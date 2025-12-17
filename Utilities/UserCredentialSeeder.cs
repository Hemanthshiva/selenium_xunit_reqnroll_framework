namespace selenium_xunit_reqnroll_framework.Utilities;

public static class UserCredentialSeeder
{
    private static readonly string[] FirstNames = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
    private static readonly string[] LastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones" };
    private static readonly Random Rand = new Random();

    public static void SeedUsers(int count = 5)
    {
        UserCredentialDb.InitializeDb();
        for (int i = 0; i < count; i++)
        {
            var firstName = FirstNames[Rand.Next(FirstNames.Length)];
            var lastName = LastNames[Rand.Next(LastNames.Length)];
            var username = $"{firstName}{lastName}{Rand.Next(1000, 9999)}";
            var email = $"{username.ToLower()}@example.com";
            var password = $"Pass{Rand.Next(1000, 9999)}!";
            var user = new UserCredential { Username = username, Email = email, Password = password };
            UserCredentialDb.InsertUser(user);
        }
    }
}
