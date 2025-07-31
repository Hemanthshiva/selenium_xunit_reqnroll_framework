// Page Object for Home Page
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class HomePage : BasePage
    {
        private static By SignupLoginButton => By.XPath("//a[contains(text(),'Signup / Login')]");
        private static By LoggedInAs => By.XPath("//li[contains(.,'Logged in as')]");
        private static By CookieAcceptButton => By.CssSelector("button[aria-label=Consent]");

        public static void GoTo() => Driver.Navigate().GoToUrl(BaseUrl);
        public static bool IsHomePageVisible() => Driver.FindElement(SignupLoginButton).Displayed;

        public static void HandleCookiePopup()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                var cookieButton = wait.Until(driver =>
                {
                    var elements = driver.FindElements(CookieAcceptButton);
                    return elements.Count > 0 && elements[0].Displayed ? elements[0] : null;
                });
                cookieButton?.Click();
            }
            catch (WebDriverTimeoutException) { /* Popup not present, ignore */ }
            catch { /* Ignore any other errors for robustness */ }
        }

        public static void ClickSignupLogin()
        {
            HandleCookiePopup();
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var button = wait.Until(driver => driver.FindElement(SignupLoginButton));
            button.Click();
        }

        public static bool IsLoggedInAsVisible(string username) => Driver.FindElement(LoggedInAs).Text.Contains(username);
    }
}
