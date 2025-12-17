// Ignore Spelling: xunit reqnroll zipcode

using Bogus;
using OpenQA.Selenium;
using Reqnroll;
using selenium_xunit_reqnroll_framework.PageObjects;
using selenium_xunit_reqnroll_framework.Utilities;

namespace selenium_xunit_reqnroll_framework.StepDefinitions;

[Binding]
public class WhenSteps(ScenarioContext scenarioContext)
{
    private readonly ScenarioContext _scenarioContext = scenarioContext;

    [When("I click on Signup\\/Login button")]
    public static void WhenIClickOnSignupLoginButton()
    {
        HomePage.ClickSignupLogin();
    }

    [When("I enter name \"(.*)\" and email \"(.*)\"")]
    public static void WhenIEnterNameAndEmail(string name, string email)
    {
        SignupLoginPage.EnterNameAndEmail(name, email);
    }

    [When("I click Signup button")]
    public static void WhenIClickSignupButton()
    {
        SignupLoginPage.ClickSignupButton();
    }

    [When("I fill account details with title \"(.*)\", password \"(.*)\", day \"(.*)\", month \"(.*)\", year \"(.*)\"")]
    public static void WhenIFillAccountDetails(string title, string password, string day, string month, string year)
    {
        AccountInfoPage.FillAccountInfo(title, password, day, month, year);
    }

    [When("I select newsletter and offers")]
    public static void WhenISelectNewsletterAndOffers()
    {
        AccountInfoPage.SelectNewsletter();
        AccountInfoPage.SelectOffers();
    }

    [When(@"I fill address details with first name ""(.*)"", last name ""(.*)"", company ""(.*)"", address1 ""(.*)"", address2 ""(.*)"", country ""(.*)"", state ""(.*)"", city ""(.*)"", zipcode ""(.*)"", mobile ""(.*)""")]
    public static void WhenIFillAddressDetails(string firstName, string lastName, string company, string address1, string address2, string country, string state, string city, string zipcode, string mobile)
    {
        AccountInfoPage.FillAddressInfo(firstName, lastName, company, address1, address2, country, state, city, zipcode, mobile);
    }

    [When("I click Create Account button")]
    public static void WhenIClickCreateAccountButton()
    {
        AccountInfoPage.ClickCreateAccount();
    }

    [When("I click Continue after account creation")]
    public static void WhenIClickContinueAfterAccountCreation()
    {
        AccountCreatedPage.ClickContinue();
    }

    [When("I enter login email \"(.*)\" and password \"(.*)\"")]
    public static void WhenIEnterLoginEmailAndPassword(string email, string password)
    {
        SignupLoginPage.EnterLoginCredentials(email, password);
    }

    [When("I enter login email and password from db")]
    public void WhenIEnterLoginEmailAndPasswordFromDb()
    {
        // Use the user registered in this scenario if available
        if (_scenarioContext.TryGetValue("LoggedInUser", out var userObj) && userObj is UserCredential user)
        {
            SignupLoginPage.EnterLoginCredentials(user.Email, user.Password);
        }
        else
        {
            // fallback to random user if not found
            var fallbackUser = UserCredentialDb.GetRandomUser();
            SignupLoginPage.EnterLoginCredentials(fallbackUser.Email, fallbackUser.Password);
            _scenarioContext["LoggedInUser"] = fallbackUser;
        }
    }

    [When("I click login button")]
    public static void WhenIClickLoginButton()
    {
        SignupLoginPage.ClickLoginButton();
    }

    [When("I click Delete Account button")]
    public static void WhenIClickDeleteAccountButton()
    {
        UserProfilePage.ClickDeleteAccount();
    }

    [When("I click Continue after account deletion")]
    public static void WhenIClickContinueAfterAccountDeletion()
    {
        AccountDeletedPage.ClickContinue();
    }

    [When("I click on Contact Us button")]
    public static void WhenIClickOnContactUsButton()
    {
        ContactUsPage.GoToContactUs();
    }

    [When("I enter contact name, email, subject and message")]
    public static void WhenIEnterContactDetails()
    {
        ContactUsPage.FillContactForm("Test User", "testuser@example.com", "Test Subject", "Test message body");
    }

    [When("I upload a file")]
    public static void WhenIUploadAFile()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "dummy.txt");
        if (!File.Exists(filePath)) File.WriteAllText(filePath, "dummy content");
        ContactUsPage.UploadFile(filePath);
    }

    [When("I click Submit button")]
    public static void WhenIClickSubmitButton()
    {
        ContactUsPage.ClickSubmit();
    }

    [When("I accept the alert")]
    public static void WhenIAcceptTheAlert()
    {
        ContactUsPage.AcceptAlert();
    }

    [When("I click Home button")]
    public static void WhenIClickHomeButton()
    {
        ContactUsPage.ClickHome();
    }

    [When("I click View Product for any product on home page")]
    public static void WhenIClickViewProductForAnyProduct()
    {
        HomePage.ClickViewProductForAnyProduct();
    }

    [When("I click Add to cart button")]
    public static void WhenIClickAddToCartButton()
    {
        ProductDetailPage.ClickAddToCart();
    }

    [When("I click View Cart button")]
    public static void WhenIClickViewCartButton()
    {
        ProductDetailPage.ClickViewCart();
    }

    [When("I add products to cart")]
    public static void WhenIAddProductsToCart()
    {
        HomePage.AddProductToCart(0);
        Thread.Sleep(1000); // Short wait for UI update
        // Do not navigate to cart or assert cart here; let scenario steps handle it
    }

    [When("I click Cart button")]
    public static void WhenIClickCartButton()
    {
        HomePage.ClickCartButton();
    }

    [When("I click Proceed To Checkout button")]
    public static void WhenIClickProceedToCheckoutButton()
    {
        CartPage.ClickProceedToCheckout();
    }

    [When("I click Register Login button")]
    public static void WhenIClickRegisterLoginButton()
    {
        try
        {
            var driver = Utilities.WebDriverManager.Driver;
            try
            {
                var modalCloseButton = driver.FindElement(By.CssSelector("#checkoutModal .close-checkout-modal"));
                modalCloseButton.Click();
                Thread.Sleep(500);
            }
            catch (NoSuchElementException)
            {
            }
            HomePage.ClickSignupLogin();
        }
        catch (ElementClickInterceptedException)
        {
            var driver = Utilities.WebDriverManager.Driver;
            var signupLoginButton = driver.FindElement(By.XPath("//a[contains(text(),'Signup / Login')]"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", signupLoginButton);
        }
    }

    [When("I enter description in comment text area")]
    public static void WhenIEnterDescriptionInCommentTextArea()
    {
        CheckoutPage.EnterComment("This is a test order. Please do not process.");
    }

    [When("I click Place Order button")]
    public static void WhenIClickPlaceOrderButton()
    {
        CheckoutPage.ClickPlaceOrder();
    }

    [When("I enter payment details")]
    public static void WhenIEnterPaymentDetails()
    {
        var faker = new Faker("en");
        PaymentPage.EnterPaymentDetails(
            faker.Name.FullName(),
            faker.Finance.CreditCardNumber(),
            faker.Finance.CreditCardCvv(),
            faker.Date.Future().Month.ToString(),
            faker.Date.Future().Year.ToString()
        );
    }

    [When("I click Pay and Confirm Order button")]
    public static void WhenIClickPayAndConfirmOrderButton()
    {
        PaymentPage.ClickPayAndConfirmOrder();
    }

    [When("I register a new random user")]
    public void WhenIRegisterANewRandomUser()
    {
        var faker = new Bogus.Faker("en");
        var allowedCountries = new[] { "India", "United States", "Canada", "Australia", "Singapore" };
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
            State = faker.Address.StateAbbr(),
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
        
        SignupLoginPage.EnterNameAndEmail(username, email);
        SignupLoginPage.ClickSignupButton();
        
        AccountInfoPage.FillAccountInfo(account.Title, account.Password, account.Day, account.Month, account.Year);
        AccountInfoPage.SelectNewsletter();
        AccountInfoPage.SelectOffers();
        AccountInfoPage.FillAddressInfo(
            address.FirstName, address.LastName, address.Company, address.Address1, address.Address2,
            address.Country, address.State, address.City, address.ZipCode, address.Mobile
        );
        AccountInfoPage.ClickCreateAccount();
    }

    [When("I increase quantity to (.*)")]
    public void WhenIIncreaseQuantityTo(int quantity)
    {
        ProductDetailPage.SetQuantity(quantity);
        _scenarioContext["ExpectedQuantity"] = quantity;
    }

    [When("I fill all details in Signup and create account")]
    public void WhenIFillAllDetailsInSignupAndCreateAccount()
    {
        // Reuse the existing method for registering a new random user
        WhenIRegisterANewRandomUser();
    }
}
