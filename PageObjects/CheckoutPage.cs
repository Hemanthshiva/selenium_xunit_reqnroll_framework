using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class CheckoutPage : BasePage
{
    // Locators
    private static readonly By AddressDetailsSection = By.CssSelector("#address_delivery");
    private static readonly By OrderReviewSection = By.CssSelector(".cart_info");
    private static readonly By CommentTextArea = By.CssSelector("textarea.form-control");
    private static readonly By PlaceOrderButton = By.CssSelector("a.check_out");
    
    // Methods
    public static bool IsAddressDetailsVisible()
    {
        try
        {
            return WaitForVisible(AddressDetailsSection).Displayed;
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
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            
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
            SetValue(CommentTextArea, comment);
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
            Click(PlaceOrderButton);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Place Order button not found or not clickable");
        }
    }
}
