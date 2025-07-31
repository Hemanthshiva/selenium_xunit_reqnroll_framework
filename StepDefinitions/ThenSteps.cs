using Reqnroll;
using selenium_xunit_reqnroll_framework.PageObjects;
using selenium_xunit_reqnroll_framework.Utilities;

namespace selenium_xunit_reqnroll_framework.StepDefinitions
{
    [Binding]
    public class ThenSteps
    {
        private readonly SignupLoginPage signupLoginPage = new();
        private readonly AccountInfoPage accountInfoPage = new();
        private readonly AccountCreatedPage accountCreatedPage = new();
        private readonly AccountDeletedPage accountDeletedPage = new();
        private readonly ContactUsPage contactUsPage = new();
        private readonly ScenarioContext scenarioContext;

        public ThenSteps(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Then("the page title should contain \"(.*)\"")]
        public static void ThenThePageTitleShouldContain(string expectedTitle)
        {
            var title = Utilities.WebDriverManager.Driver.Title;
            Assert.Contains(expectedTitle, title);
        }

        [Then("New User Signup should be visible")]
        public void ThenNewUserSignupShouldBeVisible() => Assert.True(signupLoginPage.IsNewUserSignupVisible());

        [Then("Enter Account Information should be visible")]
        public void ThenEnterAccountInformationShouldBeVisible() => Assert.True(accountInfoPage.IsEnterAccountInfoVisible());

        [Then("Account Created should be visible")]
        public void ThenAccountCreatedShouldBeVisible() => Assert.True(accountCreatedPage.IsAccountCreatedVisible());

        [Then("Logged in as username should be visible")]
        public void ThenLoggedInAsUsernameShouldBeVisible()
        {
            // Use the user registered in this scenario if available
            if (scenarioContext.TryGetValue("LoggedInUser", out var userObj) && userObj is UserCredential user)
            {
                Assert.True(HomePage.IsLoggedInAsVisible(user.Username));
            }
            else if (scenarioContext.TryGetValue("Username", out var usernameObj) && usernameObj is string username)
            {
                Assert.True(HomePage.IsLoggedInAsVisible(username));
            }
            else
            {
                throw new Xunit.Sdk.XunitException("Username is null or not set in the ScenarioContext.");
            }
        }

        [Then("Account Deleted should be visible")]
        public void ThenAccountDeletedShouldBeVisible() => Assert.True(accountDeletedPage.IsAccountDeletedVisible());

        [Then("Login to your account should be visible")]
        public void ThenLoginToYourAccountShouldBeVisible() => Assert.True(signupLoginPage.IsLoginHeaderVisible());

        [Then(@"""(.*)"" text should be visible")]
        public void ThenTextShouldBeVisible(string expectedText)
        {
            // Only implement for "Login to your account" for now
            if (expectedText == "Login to your account")
                Assert.True(signupLoginPage.IsLoginHeaderVisible());
            else
                throw new Xunit.Sdk.XunitException($"No implementation for text: {expectedText}");
        }

        [Then(@"User should be deleted from the database")]
        public void ThenUserShouldBeDeletedFromTheDatabase()
        {
            var username = scenarioContext["DeletedUsername"] as string;
            var users = UserCredentialDb.GetAllUsers();
            Assert.DoesNotContain(users, u => u.Username == username);
        }

        [Then("GET IN TOUCH is visible")]
        public void ThenGetInTouchIsVisible() => Assert.True(ContactUsPage.IsGetInTouchVisible());

        [Then("Success message is visible")]
        public void ThenSuccessMessageIsVisible() => Assert.True(ContactUsPage.IsSuccessMessageVisible());

    }
}
