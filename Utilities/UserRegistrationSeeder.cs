// Ignore Spelling: xunit

using selenium_xunit_reqnroll_framework.PageObjects;

namespace selenium_xunit_reqnroll_framework.Utilities;

public static class UserRegistrationSeeder
{
    private static readonly string[] FirstNames = { "Alice", "Bob", "Charlie", "Diana", "Eve" };
    private static readonly string[] LastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones" };
    private static readonly Random Rand = new();

    public static void RegisterUsersViaUI(int count = 5)
    {
        UserCredentialDb.InitializeDb();
        for (int i = 0; i < count; i++)
        {
            var firstName = FirstNames[Rand.Next(FirstNames.Length)];
            var lastName = LastNames[Rand.Next(LastNames.Length)];
            var username = $"{firstName}{lastName}{Rand.Next(1000, 9999)}";
            var email = $"{username.ToLower()}@example.com";
            var password = $"Pass{Rand.Next(1000, 9999)}!";

            // Register via UI
            var driver = WebDriverManager.Driver;
            driver.Navigate().GoToUrl(ConfigManager.BaseUrl);
            HomePage.GoTo();
            HomePage.ClickSignupLogin();
            
            SignupLoginPage.EnterNameAndEmail(username, email);
            SignupLoginPage.ClickSignupButton();
            
            AccountInfoPage.FillAccountInfo("Mr", password, "1", "January", "2000");
            AccountInfoPage.SelectNewsletter();
            AccountInfoPage.SelectOffers();
            AccountInfoPage.FillAddressInfo(firstName, lastName, "TestCo", "123 Main St", "Apt 1", "Canada", "ON", "Toronto", "A1A1A1", "1234567890");
            AccountInfoPage.ClickCreateAccount();
            
            if (AccountCreatedPage.IsAccountCreatedVisible())
            {
                AccountCreatedPage.ClickContinue();
            }
            // Save to DB
            UserCredentialDb.InsertUser(new UserCredential { Username = username, Email = email, Password = password });
            // Delete account to avoid polluting the app (optional, comment out if you want to keep users)
            UserProfilePage.ClickDeleteAccount();
            Thread.Sleep(1000); // Wait for deletion
            WebDriverManager.Quit(); // Restart browser for next user
        }
    }
}
