// Page Object for Account Created and Deletion
// Ignore Spelling: reqnroll xunit

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class AccountCreatedPage : BasePage
    {
        // Locators
        private static By AccountCreatedText => By.XPath("//b[text()='Account Created!']");
        private static By ContinueButton => By.XPath("//a[text()='Continue']");
        
        // Methods
        public static bool IsAccountCreatedVisible()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(d => d.FindElement(AccountCreatedText));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        
        public static void ClickContinue()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var button = wait.Until(d => d.FindElement(ContinueButton));
                
                // Scroll to the button to ensure it's visible
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
                Thread.Sleep(500);
                
                try
                {
                    button.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", button);
                }
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Continue button not found or not clickable");
            }
        }
    }
    
    public class AccountDeletedPage : BasePage
    {
        // Locators
        private static By AccountDeletedText => By.CssSelector("[data-qa='account-deleted']");
        private static By AccountDeletedHeader => By.CssSelector("h2.title");
        private static By ContinueButton => By.XPath("//a[text()='Continue']");
        
        // Methods
        public static bool IsAccountDeletedVisible()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                
                // Try the primary locator first
                try
                {
                    var element = wait.Until(d => d.FindElement(AccountDeletedText));
                    return element.Displayed;
                }
                catch (WebDriverTimeoutException)
                {
                    // Fall back to the header locator
                    var header = wait.Until(d => d.FindElement(AccountDeletedHeader));
                    return header.Displayed && header.Text.Contains("ACCOUNT DELETED", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
        
        public static void ClickContinue()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var button = wait.Until(d => d.FindElement(ContinueButton));
                
                // Scroll to the button to ensure it's visible
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
                Thread.Sleep(500);
                
                try
                {
                    button.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", button);
                }
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Continue button not found or not clickable");
            }
        }
    }
}
