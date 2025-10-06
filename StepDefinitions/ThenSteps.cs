using OpenQA.Selenium;
using Reqnroll;
using selenium_xunit_reqnroll_framework.PageObjects;
using selenium_xunit_reqnroll_framework.Utilities;

namespace selenium_xunit_reqnroll_framework.StepDefinitions
{
    [Binding]
    public class ThenSteps(ScenarioContext scenarioContext)
    {
        private readonly SignupLoginPage signupLoginPage = new();
        private readonly AccountInfoPage accountInfoPage = new();
        private readonly ContactUsPage contactUsPage = new();
        private readonly CartPage cartPage = new();
        private readonly CheckoutPage checkoutPage = new();
        private readonly PaymentPage paymentPage = new();
        private readonly ScenarioContext scenarioContext = scenarioContext;

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
        public static void ThenAccountCreatedShouldBeVisible() => Assert.True(AccountCreatedPage.IsAccountCreatedVisible());

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
        public static void ThenAccountDeletedShouldBeVisible() => Assert.True(AccountDeletedPage.IsAccountDeletedVisible());

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
        public static void ThenGetInTouchIsVisible() => Assert.True(ContactUsPage.IsGetInTouchVisible());

        [Then("Success message is visible")]
        public static void ThenSuccessMessageIsVisible() => Assert.True(ContactUsPage.IsSuccessMessageVisible());

        [Then("product detail page is opened")]
        public static void ThenProductDetailPageIsOpened() => Assert.True(ProductDetailPage.IsProductDetailPageVisible());

        [Then("product is displayed in cart page with exact quantity (.*)")]
        public static void ThenProductIsDisplayedInCartPageWithExactQuantity(int expectedQuantity)
        {
            Assert.True(CartPage.IsCartPageVisible(), "Cart page should be visible");
            Assert.True(CartPage.IsAnyProductDisplayedInCartWithQuantity(expectedQuantity),
                $"Product should be displayed in cart with quantity {expectedQuantity}");
        }

        // New steps for Place Order: Register while Checkout
        [Then("I should see the cart page")]
        public static void ThenIShouldSeeTheCartPage()
        {
            var driver = Utilities.WebDriverManager.Driver;
            try
            {
                var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(15));
                var cartTable = wait.Until(d =>
                {
                    var el = d.FindElement(By.Id("cart_info_table"));
                    return (el != null && el.Displayed) ? el : null;
                });
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'center'});", cartTable);
                Assert.True(cartTable.Displayed, "Cart page should be visible");
            }
            catch (Exception)
            {
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                var fileName = $"CartPage_NotVisible_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                screenshot.SaveAsFile(fileName);
                throw new Xunit.Sdk.XunitException("Cart page is not visible. Screenshot saved as " + fileName);
            }
        }

        [Then("I should see Address Details and Review Your Order")]
        public static void ThenIShouldSeeAddressDetailsAndReviewYourOrder()
        {
            Assert.True(CheckoutPage.IsAddressDetailsVisible(), "Address details should be visible");
            Assert.True(CheckoutPage.IsOrderReviewVisible(), "Order review should be visible");
        }

        [Then(@"I should see success message ""(.*)""")]
        public static void ThenIShouldSeeSuccessMessage(string expectedMessage)
        {
            // Take a screenshot for debugging
            var screenshot = ((ITakesScreenshot)Utilities.WebDriverManager.Driver).GetScreenshot();
            var fileName = $"success_message_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            screenshot.SaveAsFile(fileName);
            Console.WriteLine($"Screenshot saved as {fileName}");

            // Wait a bit to ensure the page has fully loaded
            Thread.Sleep(2000);

            // Check for the success message
            Assert.True(PaymentPage.IsSuccessMessageVisible(expectedMessage),
                $"Success message '{expectedMessage}' should be visible");
        }

        [Then("I am on the home page")]
        public static void ThenIAmOnTheHomePage()
        {
            Assert.True(HomePage.IsHomePageVisible(), "Home page should be visible");
        }
    }
}
