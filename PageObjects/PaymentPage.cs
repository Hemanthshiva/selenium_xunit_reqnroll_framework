using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class PaymentPage : BasePage
{
    // Locators
    private static readonly By NameOnCardInput = By.CssSelector("input[name='name_on_card']");
    private static readonly By CardNumberInput = By.CssSelector("input[name='card_number']");
    private static readonly By CvcInput = By.CssSelector("input[name='cvc']");
    private static readonly By ExpiryMonthInput = By.CssSelector("input[name='expiry_month']");
    private static readonly By ExpiryYearInput = By.CssSelector("input[name='expiry_year']");
    private static readonly By PayAndConfirmOrderButton = By.CssSelector("#submit");
    private static readonly By SuccessMessage = By.CssSelector("div.alert-success, p.alert-success, div.message-success, p.message-success, div.success-message, p.success-message");
    
    // Methods
    public static void EnterPaymentDetails(string nameOnCard, string cardNumber, string cvc, string expiryMonth, string expiryYear)
    {
        try
        {
            SetValue(NameOnCardInput, nameOnCard);
            SetValue(CardNumberInput, cardNumber);
            SetValue(CvcInput, cvc);
            SetValue(ExpiryMonthInput, expiryMonth);
            SetValue(ExpiryYearInput, expiryYear);
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
            Click(PayAndConfirmOrderButton);
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
            catch { /* Ignore */ }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
