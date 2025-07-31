// Ignore Spelling: xunit reqnroll zipcode

using Bogus;
using Reqnroll;
using selenium_xunit_reqnroll_framework.PageObjects;
using selenium_xunit_reqnroll_framework.Utilities;
using System.IO;

namespace selenium_xunit_reqnroll_framework.StepDefinitions
{
    [Binding]
    public class WhenSteps
    {
        // Page objects
        private readonly HomePage homePage = new();
        private readonly SignupLoginPage signupLoginPage = new();
        private readonly AccountInfoPage accountInfoPage = new();
        private readonly UserProfilePage userProfilePage = new();
        private readonly AccountCreatedPage accountCreatedPage = new();
        private readonly ContactUsPage contactUsPage = new();

        private readonly ScenarioContext _scenarioContext;

        public WhenSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [When("I click on Signup\\/Login button")]
        public static void WhenIClickOnSignupLoginButton() => HomePage.ClickSignupLogin();

        [When("I enter name \"(.*)\" and email \"(.*)\"")]
        public void WhenIEnterNameAndEmail(string name, string email)
            => signupLoginPage.EnterNameAndEmail(name, email);

        [When("I click Signup button")]
        public void WhenIClickSignupButton() => signupLoginPage.ClickSignupButton();

        [When("I fill account details with title \"(.*)\", password \"(.*)\", day \"(.*)\", month \"(.*)\", year \"(.*)\"")]
        public void WhenIFillAccountDetails(string title, string password, string day, string month, string year)
            => accountInfoPage.FillAccountInfo(title, password, day, month, year);

        [When("I select newsletter and offers")]
        public void WhenISelectNewsletterAndOffers()
        {
            accountInfoPage.SelectNewsletter();
            accountInfoPage.SelectOffers();
        }

        [When(@"I fill address details with first name ""(.*)"", last name ""(.*)"", company ""(.*)"", address1 ""(.*)"", address2 ""(.*)"", country ""(.*)"", state ""(.*)"", city ""(.*)"", zipcode ""(.*)"", mobile ""(.*)""")]
        public void WhenIFillAddressDetails(string firstName, string lastName, string company, string address1, string address2, string country, string state, string city, string zipcode, string mobile)
            => accountInfoPage.FillAddressInfo(firstName, lastName, company, address1, address2, country, state, city, zipcode, mobile);

        [When("I click Create Account button")]
        public void WhenIClickCreateAccountButton() => accountInfoPage.ClickCreateAccount();

        [When("I click Continue after account creation")]
        public void WhenIClickContinueAfterAccountCreation() => accountCreatedPage.ClickContinue();

        [When("I enter login email \"(.*)\" and password \"(.*)\"")]
        public void WhenIEnterLoginEmailAndPassword(string email, string password)
            => signupLoginPage.EnterLoginCredentials(email, password);

        [When("I enter random login credentials from db")]
        public void WhenIEnterRandomLoginCredentialsFromDb()
        {
            var user = UserCredentialDb.GetRandomUser();
            signupLoginPage.EnterLoginCredentials(user.Email, user.Password);
        }

        [When("I enter login email and password from db")]
        public void WhenIEnterLoginEmailAndPasswordFromDb()
        {
            // Use the user registered in this scenario if available
            if (_scenarioContext.TryGetValue("LoggedInUser", out var userObj) && userObj is UserCredential user)
            {
                signupLoginPage.EnterLoginCredentials(user.Email, user.Password);
            }
            else
            {
                // fallback to random user if not found
                var fallbackUser = UserCredentialDb.GetRandomUser();
                signupLoginPage.EnterLoginCredentials(fallbackUser.Email, fallbackUser.Password);
                _scenarioContext["LoggedInUser"] = fallbackUser;
            }
        }

        [When("I click login button")]
        public void WhenIClickLoginButton() => signupLoginPage.ClickLoginButton();

        [When("I click Delete Account button")]
        public void WhenIClickDeleteAccountButton() => userProfilePage.ClickDeleteAccount();

        [When("I click Continue after account deletion")]
        public void WhenIClickContinueAfterAccountDeletion() => accountCreatedPage.ClickContinue();

        [When(@"I delete (.*) from the database")]
        public void WhenIDeleteUserFromTheDatabase(string username)
        {
            UserCredentialDb.DeleteUserByUsername(username);
            _scenarioContext["DeletedUsername"] = username;
        }

        [When("I register a new random user")]
        public void WhenIRegisterANewRandomUser()
        {
            var faker = new Faker("en");
            var allowedCountries = new[] { "India", "United States", "Canada", "Australia", "Israel", "New Zealand", "Singapore", "Turkey", "United Kingdom" };
            var account = new AccountDetailsModel
            {
                Title = faker.PickRandom(new[] { "Mr", "Mrs", "Miss" }),
                Password = faker.Internet.Password(8, false, "", "Pass!"),
                Day = faker.Random.Int(1, 28).ToString(),
                Month = faker.Date.Month(),
                Year = faker.Random.Int(1970, 2005).ToString()
            };
            var address = new AddressDetailsModel
            {
                FirstName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Company = faker.Company.CompanyName(),
                Address1 = faker.Address.StreetAddress(),
                Address2 = faker.Address.SecondaryAddress(),
                Country = faker.PickRandom(allowedCountries),
                State = "TestState",
                City = faker.Address.City(),
                ZipCode = faker.Address.ZipCode(),
                Mobile = faker.Phone.PhoneNumber("##########")
            };
            var username = faker.Internet.UserName();
            var email = faker.Internet.Email();

            _scenarioContext["Account"] = account;
            _scenarioContext["Address"] = address;
            _scenarioContext["Username"] = username;
            _scenarioContext["Email"] = email;
            _scenarioContext["LoggedInUser"] = new UserCredential { Username = username, Email = email, Password = account.Password };

            var driver = Utilities.WebDriverManager.Driver;
            driver.Navigate().GoToUrl(ConfigManager.BaseUrl);
            HomePage.GoTo();
            HomePage.ClickSignupLogin();
            var signupLoginPage = new PageObjects.SignupLoginPage();
            signupLoginPage.EnterNameAndEmail(username, email);
            signupLoginPage.ClickSignupButton();
            var accountInfoPage = new PageObjects.AccountInfoPage();
            accountInfoPage.FillAccountInfo(account.Title, account.Password, account.Day, account.Month, account.Year);
            accountInfoPage.SelectNewsletter();
            accountInfoPage.SelectOffers();
            accountInfoPage.FillAddressInfo(
                address.FirstName, address.LastName, address.Company, address.Address1, address.Address2,
                address.Country, address.State, address.City, address.ZipCode, address.Mobile
            );
            accountInfoPage.ClickCreateAccount();
        }

        [When("I click on Contact Us button")]
        public void WhenIClickOnContactUsButton() => ContactUsPage.GoToContactUs();

        [When("I enter contact name, email, subject and message")]
        public void WhenIEnterContactDetails()
        {
            ContactUsPage.FillContactForm("Test User", "testuser@example.com", "Test Subject", "Test message body");
        }

        [When("I upload a file")]
        public void WhenIUploadAFile()
        {
            // Use a small dummy file for upload
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "dummy.txt");
            if (!File.Exists(filePath)) File.WriteAllText(filePath, "dummy content");
            ContactUsPage.UploadFile(filePath);
        }

        [When("I click Submit button")]
        public void WhenIClickSubmitButton() => ContactUsPage.ClickSubmit();

        [When("I accept the alert")]
        public void WhenIAcceptTheAlert() => ContactUsPage.AcceptAlert();

        [When("I click Home button")]
        public void WhenIClickHomeButton() => ContactUsPage.ClickHome();
    }
}