using Bogus;
using Reqnroll;
using RestSharp;
using selenium_xunit_reqnroll_framework.Utilities;

namespace selenium_xunit_reqnroll_framework.StepDefinitions
{
    [Binding]
    public class GivenSteps
    {
        private static readonly string[] Titles = ["Mr", "Mrs", "Miss"];

        [Given("I am on the home page")]
        public static void GivenIAmOnTheHomePage()
        {
            // Use the correct fully qualified name for WebDriverManager
            var driver = Utilities.WebDriverManager.Driver;
            driver.Navigate().GoToUrl(ConfigManager.BaseUrl);
            // Handle cookie popup if present
            PageObjects.HomePage.HandleCookiePopup();
        }

        [Given(@"(.*) users are registered via API")]
        public static async Task Given5UsersAreRegisteredViaAPI(int count)
        {
            var faker = new Faker("en");
            var client = new RestClient("https://automationexercise.com/api/");
            UserCredentialDb.InitializeDb();
            for (int i = 0; i < count; i++)
            {
                var user = new UserRegistrationModel
                {
                    Name = faker.Internet.UserName(),
                    Email = faker.Internet.Email(),
                    Password = faker.Internet.Password(8, false, "", "Pass!"),
                    Title = faker.PickRandom(Titles),
                    BirthDate = faker.Random.Int(1, 28).ToString(),
                    BirthMonth = faker.Date.Month(),
                    BirthYear = faker.Random.Int(1970, 2005).ToString(),
                    FirstName = faker.Name.FirstName(),
                    LastName = faker.Name.LastName(),
                    Company = faker.Company.CompanyName(),
                    Address1 = faker.Address.StreetAddress(),
                    Address2 = faker.Address.SecondaryAddress(),
                    Country = faker.Address.Country(),
                    ZipCode = faker.Address.ZipCode(),
                    State = faker.Address.StateAbbr(),
                    City = faker.Address.City(),
                    MobileNumber = faker.Phone.PhoneNumber("##########")
                };

                var request = new RestRequest("createAccount", Method.Post);
                request.AddParameter("name", user.Name);
                request.AddParameter("email", user.Email);
                request.AddParameter("password", user.Password);
                request.AddParameter("title", user.Title);
                request.AddParameter("birth_date", user.BirthDate);
                request.AddParameter("birth_month", user.BirthMonth);
                request.AddParameter("birth_year", user.BirthYear);
                request.AddParameter("firstname", user.FirstName);
                request.AddParameter("lastname", user.LastName);
                request.AddParameter("company", user.Company);
                request.AddParameter("address1", user.Address1);
                request.AddParameter("address2", user.Address2);
                request.AddParameter("country", user.Country);
                request.AddParameter("zipcode", user.ZipCode);
                request.AddParameter("state", user.State);
                request.AddParameter("city", user.City);
                request.AddParameter("mobile_number", user.MobileNumber);
                request.AlwaysMultipartFormData = true; // Ensure form data

                var response = await client.ExecuteAsync(request);

                // Fix for CS8602: Ensure response.Content is not null before accessing it
                if (response.Content != null &&
                    ((int)response.StatusCode != 200 && (int)response.StatusCode != 201 &&
                    !response.Content.Contains("\"responseCode\": 201") && !response.Content.Contains("User created!")))
                {
                    throw new Exception($"User registration failed: {response.Content}");
                }

                // Save to DB
                UserCredentialDb.InsertUser(new UserCredential
                {
                    Username = user.Name,
                    Email = user.Email,
                    Password = user.Password
                });
            }
        }

        [Given(@"5 users are registered")]
        public static void Given5UsersAreRegistered()
        {
            UserRegistrationSeeder.RegisterUsersViaUI(5);
        }
    }
}
