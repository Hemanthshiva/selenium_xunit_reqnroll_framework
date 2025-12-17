using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class ProductDetailPage : BasePage
{
    // Locators
    private static readonly By ProductDetailSection = By.CssSelector(".product-information");
    private static readonly By ProductName = By.CssSelector(".product-information h2");
    private static readonly By QuantityInput = By.Id("quantity");
    private static readonly By AddToCartButton = By.CssSelector(".btn.btn-default.cart");
    private static readonly By ViewCartButton = By.XPath("//u[text()='View Cart']");
    private static readonly By ContinueShoppingButton = By.CssSelector(".btn.btn-success.close-modal");
    private static readonly By ProductPrice = By.CssSelector(".product-information span span");

    // Methods
    public static bool IsProductDetailPageVisible()
    {
        try
        {
            return WaitForVisible(ProductDetailSection).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public static string GetProductName()
    {
        try
        {
            return GetText(ProductName);
        }
        catch (WebDriverTimeoutException)
        {
            return string.Empty;
        }
    }

    public static void SetQuantity(int quantity)
    {
        try
        {
            SetValue(QuantityInput, quantity.ToString());
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Quantity input field not found or not interactable");
        }
    }

    public static void ClickAddToCart()
    {
        try
        {
            Click(AddToCartButton);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Add to Cart button not found or not clickable");
        }
    }

    public static void ClickViewCart()
    {
        try
        {
            Click(ViewCartButton);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("View Cart button not found or not clickable");
        }
    }

    public static void ClickContinueShopping()
    {
        try
        {
            Click(ContinueShoppingButton, 5);
        }
        catch (WebDriverTimeoutException)
        {
            // Continue shopping button might not appear, which is fine
        }
    }

    public static string GetProductPrice()
    {
        try
        {
            return GetText(ProductPrice);
        }
        catch (WebDriverTimeoutException)
        {
            return string.Empty;
        }
    }
}
