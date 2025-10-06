// Ignore Spelling: xunit

using selenium_xunit_reqnroll_framework.PageObjects;

namespace selenium_xunit_reqnroll_framework.Utilities
{
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
                HomePage.GoTo(); // Corrected to use the static method directly
                HomePage.ClickSignupLogin(); // Corrected to use the static method directly
                var signupLoginPage = new PageObjects.SignupLoginPage();
                signupLoginPage.EnterNameAndEmail(username, email);
                signupLoginPage.ClickSignupButton();
                var accountInfoPage = new PageObjects.AccountInfoPage();
                accountInfoPage.FillAccountInfo("Mr", password, "1", "January", "2000");
                accountInfoPage.SelectNewsletter();
                accountInfoPage.SelectOffers();
                accountInfoPage.FillAddressInfo(firstName, lastName, "TestCo", "123 Main St", "Apt 1", "Canada", "ON", "Toronto", "A1A1A1", "1234567890");
                accountInfoPage.ClickCreateAccount();
                var accountCreatedPage = new PageObjects.AccountCreatedPage();
                if (AccountCreatedPage.IsAccountCreatedVisible())
                {
                    AccountCreatedPage.ClickContinue();
                }
                // Save to DB
                UserCredentialDb.InsertUser(new UserCredential { Username = username, Email = email, Password = password });
                // Delete account to avoid polluting the app (optional, comment out if you want to keep users)
                var userProfilePage = new PageObjects.UserProfilePage();
                userProfilePage.ClickDeleteAccount();
                Thread.Sleep(1000); // Wait for deletion
                WebDriverManager.Quit(); // Restart browser for next user
            }
        }
    }
}