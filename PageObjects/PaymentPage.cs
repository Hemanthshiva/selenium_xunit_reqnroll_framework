using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class PaymentPage : BasePage
    {
        // Locators
        private static By NameOnCardInput => By.CssSelector("input[name='name_on_card']");
        private static By CardNumberInput => By.CssSelector("input[name='card_number']");
        private static By CvcInput => By.CssSelector("input[name='cvc']");
        private static By ExpiryMonthInput => By.CssSelector("input[name='expiry_month']");
        private static By ExpiryYearInput => By.CssSelector("input[name='expiry_year']");
        private static By PayAndConfirmOrderButton => By.CssSelector("#submit");
        private static By SuccessMessage => By.CssSelector("div.alert-success, p.alert-success, div.message-success, p.message-success, div.success-message, p.success-message");
        
        // Methods
        public static void EnterPaymentDetails(string nameOnCard, string cardNumber, string cvc, string expiryMonth, string expiryYear)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                
                var nameInput = wait.Until(d => d.FindElement(NameOnCardInput));
                nameInput.Clear();
                nameInput.SendKeys(nameOnCard);
                
                var cardNumberInput = Driver.FindElement(CardNumberInput);
                cardNumberInput.Clear();
                cardNumberInput.SendKeys(cardNumber);
                
                var cvcInput = Driver.FindElement(CvcInput);
                cvcInput.Clear();
                cvcInput.SendKeys(cvc);
                
                var expiryMonthInput = Driver.FindElement(ExpiryMonthInput);
                expiryMonthInput.Clear();
                expiryMonthInput.SendKeys(expiryMonth);
                
                var expiryYearInput = Driver.FindElement(ExpiryYearInput);
                expiryYearInput.Clear();
                expiryYearInput.SendKeys(expiryYear);
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Payment form fields not found or not interactable");
            }
        }

        public static void ClickPayAndConfirmOrder()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var button = wait.Until(d => d.FindElement(PayAndConfirmOrderButton));
                
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
                throw new Exception("Pay and Confirm Order button not found or not clickable");
            }
        }

        public static bool IsSuccessMessageVisible(string expectedMessage)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                
                // First try with the specific selector
                try
                {
                    var message = wait.Until(d => d.FindElement(SuccessMessage));
                    if (message.Displayed && message.Text.Contains(expectedMessage))
                    {
                        return true;
                    }
                }
                catch (WebDriverTimeoutException)
                {
                    // Continue to try other methods
                }
                
                // Try to find any element containing the success message text
                try
                {
                    var elements = Driver.FindElements(By.XPath($"//*[contains(text(), '{expectedMessage}')]"));
                    if (elements.Count > 0 && elements[0].Displayed)
                    {
                        return true;
                    }
                }
                catch
                {
                    // Continue to try other methods
                }
                
                // Try to find any element with a success class that might contain our message
                try
                {
                    var successElements = Driver.FindElements(By.CssSelector("[class*='success']"));
                    foreach (var element in successElements)
                    {
                        if (element.Displayed && element.Text.Contains(expectedMessage))
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    // Continue to try other methods
                }
                
                // As a last resort, check if the page contains our text anywhere
                return Driver.PageSource.Contains(expectedMessage);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}