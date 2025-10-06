using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class ProductDetailPage : BasePage
    {
        // Locators
        private static By ProductDetailSection => By.CssSelector(".product-information");
        private static By ProductName => By.CssSelector(".product-information h2");
        private static By QuantityInput => By.Id("quantity");
        private static By AddToCartButton => By.CssSelector(".btn.btn-default.cart");
        private static By ViewCartButton => By.XPath("//u[text()='View Cart']");
        private static By ContinueShoppingButton => By.CssSelector(".btn.btn-success.close-modal");
        private static By ProductPrice => By.CssSelector(".product-information span span");
        private static By ProductCategory => By.CssSelector(".product-information p:first-child");
        private static By ProductAvailability => By.CssSelector(".product-information p:nth-child(2)");
        private static By ProductCondition => By.CssSelector(".product-information p:nth-child(3)");
        private static By ProductBrand => By.CssSelector(".product-information p:nth-child(4)");

        // Methods
        public static bool IsProductDetailPageVisible()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(d => d.FindElement(ProductDetailSection));
                return element.Displayed;
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
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(d => d.FindElement(ProductName));
                return element.Text;
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
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var quantityField = wait.Until(d => d.FindElement(QuantityInput));
                
                // Clear the existing value and enter new quantity
                quantityField.Clear();
                quantityField.SendKeys(quantity.ToString());
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
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var addToCartBtn = wait.Until(d => d.FindElement(AddToCartButton));
                
                // Scroll to the button to ensure it's visible
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", addToCartBtn);
                
                // Try regular click first, fallback to JavaScript click if needed
                try
                {
                    addToCartBtn.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", addToCartBtn);
                }
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
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var viewCartBtn = wait.Until(d => d.FindElement(ViewCartButton));

                // Scroll into view using JavaScript
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'center'});", viewCartBtn);

                // Wait for element to be visible and enabled
                wait.Until(d => viewCartBtn.Displayed && viewCartBtn.Enabled);

                // Try regular click first, fallback to JavaScript click if needed
                try
                {
                    viewCartBtn.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", viewCartBtn);
                }
                catch (ElementNotInteractableException)
                {
                    // As a last resort, force click with JS
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", viewCartBtn);
                }
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
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
                var continueBtn = wait.Until(d => d.FindElement(ContinueShoppingButton));
                continueBtn.Click();
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
            catch
            {
                return string.Empty;
            }
        }

        public static string GetProductCategory()
        {
            try
            {
                return GetText(ProductCategory);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetProductAvailability()
        {
            try
            {
                return GetText(ProductAvailability);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetProductCondition()
        {
            try
            {
                return GetText(ProductCondition);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetProductBrand()
        {
            try
            {
                return GetText(ProductBrand);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}