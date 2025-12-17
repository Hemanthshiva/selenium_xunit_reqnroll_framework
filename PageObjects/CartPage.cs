using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class CartPage : BasePage
{
    // Locators
    private static readonly By CartInfoTable = By.Id("cart_info_table");
    private static readonly By CartItems = By.CssSelector("#cart_info_table tbody tr");
    private static readonly By ProductNameInCart = By.CssSelector("td.cart_description h4 a");
    private static readonly By ProductQuantityInCart = By.CssSelector("td.cart_quantity button.disabled");
    private static readonly By DeleteProductButton = By.CssSelector("td.cart_delete a.cart_quantity_delete");
    private static readonly By ProceedToCheckoutButton = By.CssSelector(".btn.btn-default.check_out");

    // Methods
    public static bool IsCartPageVisible()
    {
        try
        {
            var element = WaitForVisible(CartInfoTable);
            ScrollIntoView(element);
            return element.Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            CaptureCartPageScreenshot();
            return false;
        }
        catch (NoSuchElementException)
        {
            CaptureCartPageScreenshot();
            return false;
        }
    }

    private static void CaptureCartPageScreenshot()
    {
        try
        {
            if (Driver is ITakesScreenshot screenshotDriver)
            {
                var screenshot = screenshotDriver.GetScreenshot();
                var fileName = $"CartPage_NotVisible_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                var filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                screenshot.SaveAsFile(filePath);
            }
        }
        catch { /* Ignore screenshot errors */ }
    }

    public static int GetProductQuantityInCart(string? productName = null)
    {
        try
        {
            WaitForVisible(CartInfoTable);

            var cartItems = Driver.FindElements(CartItems);
            
            if (cartItems.Count == 0)
            {
                return 0;
            }

            // If no specific product name is provided, get quantity of first product
            if (string.IsNullOrEmpty(productName))
            {
                var quantityElement = cartItems[0].FindElement(ProductQuantityInCart);
                return int.Parse(quantityElement.Text.Trim());
            }

            // Find specific product by name
            foreach (var item in cartItems)
            {
                try
                {
                    var nameElement = item.FindElement(ProductNameInCart);
                    if (nameElement.Text.Contains(productName, StringComparison.OrdinalIgnoreCase))
                    {
                        var quantityElement = item.FindElement(ProductQuantityInCart);
                        return int.Parse(quantityElement.Text.Trim());
                    }
                }
                catch (NoSuchElementException)
                {
                    continue;
                }
            }

            return 0;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public static bool IsAnyProductDisplayedInCartWithQuantity(int quantity)
    {
        try
        {
            WaitForVisible(CartInfoTable);
            var cartItems = Driver.FindElements(CartItems);
            
            foreach (var item in cartItems)
            {
                try
                {
                    var quantityElement = item.FindElement(ProductQuantityInCart);
                    if (int.Parse(quantityElement.Text.Trim()) == quantity)
                    {
                        return true;
                    }
                }
                catch { /* continue checking */ }
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static void ClickProceedToCheckout()
    {
        try
        {
            Click(ProceedToCheckoutButton);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Proceed To Checkout button not found or not clickable");
        }
    }

    public static void ClickRegisterLogin()
    {
        // This button usually appears inside the modal when checking out as guest
        var registerLoginButton = By.XPath("//u[text()='Register / Login']");
        try
        {
            Click(registerLoginButton);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Register / Login button in checkout modal not found");
        }
    }
}
