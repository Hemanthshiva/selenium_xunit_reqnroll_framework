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
        private static By ProductsSection => By.CssSelector(".features_items");
        private static By ProductItems => By.CssSelector(".features_items .col-sm-4");
        private static By ViewProductButtons => By.XPath("//a[contains(text(),'View Product')]");
        private static By FirstViewProductButton => By.XPath("(//a[contains(text(),'View Product')])[1]");
        private static By AddToCartButtons => By.CssSelector(".add-to-cart");
        private static By FirstAddToCartButton => By.XPath("(//a[contains(@class,'add-to-cart')])[1]");
        private static By ContinueShoppingButton => By.CssSelector(".btn-success");
        private static By CartButton => By.XPath("//a[contains(text(),'Cart')]");

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

        public static bool IsLoggedInAsVisible(string username)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(d =>
                    {
                        var el = d.FindElement(LoggedInAs);
                        return (el.Displayed && el.Text.Contains(username)) ? el : null;
                    });
                return element != null;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static void ClickViewProductForAnyProduct()
        {
            HandleCookiePopup();
            
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
                
                // Wait for products section to be visible
                wait.Until(d => d.FindElement(ProductsSection));
                
                // Scroll down to make sure products are visible
                ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 800);");
                
                // Wait a bit for any animations to complete
                Thread.Sleep(1000);
                
                // Find the first "View Product" button
                var viewProductButton = wait.Until(d => d.FindElement(FirstViewProductButton));
                
                // Scroll to the button to ensure it's visible
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", viewProductButton);
                
                // Wait a bit more
                Thread.Sleep(500);
                
                // Try clicking with JavaScript if regular click fails
                try
                {
                    viewProductButton.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", viewProductButton);
                }
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("View Product button not found or not clickable");
            }
        }

        public static bool AreProductsVisible()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var productsSection = wait.Until(d => d.FindElement(ProductsSection));
                return productsSection.Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        public static int GetProductsCount()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(ProductsSection));
                var products = Driver.FindElements(ProductItems);
                return products.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static void AddProductToCart(int productIndex = 0)
        {
            HandleCookiePopup();
            
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
                
                // Wait for products section to be visible
                wait.Until(d => d.FindElement(ProductsSection));
                
                // Scroll down to make sure products are visible
                ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 800);");
                
                // Wait a bit for any animations to complete
                Thread.Sleep(1000);
                
                // Find all add to cart buttons
                var addToCartButtons = Driver.FindElements(AddToCartButtons);
                
                if (addToCartButtons.Count > productIndex)
                {
                    var button = addToCartButtons[productIndex];
                    
                    // Scroll to the button to ensure it's visible
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", button);
                    Thread.Sleep(500);
                    
                    // Try clicking with JavaScript if regular click fails
                    try
                    {
                        button.Click();
                    }
                    catch (ElementClickInterceptedException)
                    {
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", button);
                    }
                    
                    // Wait for the "Continue Shopping" button to appear
                    var continueButton = wait.Until(d => d.FindElement(ContinueShoppingButton));
                    continueButton.Click();
                }
                else
                {
                    throw new Exception($"Product at index {productIndex} not found");
                }
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new Exception($"Failed to add product to cart: {ex.Message}");
            }
        }

        public static void ClickCartButton()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var button = wait.Until(d => d.FindElement(CartButton));
                button.Click();
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Cart button not found or not clickable");
            }
        }
    }
}
