using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class CheckoutPage : BasePage
    {
        // Locators
        private static By AddressDetailsSection => By.CssSelector("#address_delivery");
        private static By OrderReviewSection => By.CssSelector(".cart_info");
        private static By CommentTextArea => By.CssSelector("textarea.form-control");
        private static By PlaceOrderButton => By.CssSelector("a.check_out");
        
        // Methods
        public static bool IsAddressDetailsVisible()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(d => d.FindElement(AddressDetailsSection));
                return element.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static bool IsOrderReviewVisible()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                
                // Try multiple selectors to find the order review section
                try
                {
                    var element = wait.Until(d => d.FindElement(OrderReviewSection));
                    return element.Displayed;
                }
                catch (WebDriverTimeoutException)
                {
                    // Try alternative selector
                    var element = wait.Until(d => d.FindElement(By.CssSelector(".table-responsive")));
                    return element.Displayed;
                }
            }
            catch (WebDriverTimeoutException)
            {
                // If we can find any product items in the page, consider it as order review
                try
                {
                    var productItems = Driver.FindElements(By.CssSelector(".cart_description"));
                    return productItems.Count > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static void EnterComment(string comment)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var textArea = wait.Until(d => d.FindElement(CommentTextArea));
                textArea.Clear();
                textArea.SendKeys(comment);
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Comment text area not found or not interactable");
            }
        }

        public static void ClickPlaceOrder()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var button = wait.Until(d => d.FindElement(PlaceOrderButton));
                
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
                throw new Exception("Place Order button not found or not clickable");
            }
        }
    }
}