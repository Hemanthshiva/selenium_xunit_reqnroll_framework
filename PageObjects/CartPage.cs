using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class CartPage : BasePage
    {
        // Locators
        private static By CartInfoTable => By.Id("cart_info_table");
        private static By CartItems => By.CssSelector("#cart_info_table tbody tr");
        private static By ProductNameInCart => By.CssSelector("td.cart_description h4 a");
        private static By ProductPriceInCart => By.CssSelector("td.cart_price p");
        private static By ProductQuantityInCart => By.CssSelector("td.cart_quantity button.disabled");
        private static By ProductQuantityInCartAlt => By.CssSelector("td.cart_quantity .cart_quantity_button");
        private static By ProductTotalInCart => By.CssSelector("td.cart_total p.cart_total_price");
        private static By DeleteProductButton => By.CssSelector("td.cart_delete a.cart_quantity_delete");
        private static By EmptyCartMessage => By.CssSelector("#empty_cart");
        private static By ProceedToCheckoutButton => By.CssSelector(".btn.btn-default.check_out");

        // Methods
        public static bool IsCartPageVisible()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var element = wait.Until(d =>
                {
                    var el = d.FindElement(CartInfoTable);
                    return (el != null && el.Displayed) ? el : null;
                });
                // Scroll into view for Firefox reliability
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'center'});", element);
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
                var screenshotDriver = Driver as ITakesScreenshot;
                if (screenshotDriver != null)
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
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

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

        public static string GetProductNameInCart(int index = 0)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

                var cartItems = Driver.FindElements(CartItems);
                
                if (cartItems.Count > index)
                {
                    var nameElement = cartItems[index].FindElement(ProductNameInCart);
                    return nameElement.Text.Trim();
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetProductPriceInCart(int index = 0)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

                var cartItems = Driver.FindElements(CartItems);
                
                if (cartItems.Count > index)
                {
                    var priceElement = cartItems[index].FindElement(ProductPriceInCart);
                    return priceElement.Text.Trim();
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetProductTotalInCart(int index = 0)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

                var cartItems = Driver.FindElements(CartItems);
                
                if (cartItems.Count > index)
                {
                    var totalElement = cartItems[index].FindElement(ProductTotalInCart);
                    return totalElement.Text.Trim();
                }

                return string.Empty;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static int GetCartItemsCount()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

                var cartItems = Driver.FindElements(CartItems);
                return cartItems.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static bool IsCartEmpty()
        {
            try
            {
                var emptyMessage = Driver.FindElement(EmptyCartMessage);
                return emptyMessage.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public static void DeleteProduct(int index = 0)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

                var cartItems = Driver.FindElements(CartItems);
                
                if (cartItems.Count > index)
                {
                    var deleteButton = cartItems[index].FindElement(DeleteProductButton);
                    deleteButton.Click();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete product at index {index}: {ex.Message}");
            }
        }

        public static void ClickProceedToCheckout()
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                var checkoutButton = wait.Until(d => d.FindElement(ProceedToCheckoutButton));
                checkoutButton.Click();
            }
            catch (WebDriverTimeoutException)
            {
                throw new Exception("Proceed to Checkout button not found or not clickable");
            }
        }

        public static bool IsProductDisplayedInCart(string productName, int expectedQuantity)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

                var cartItems = Driver.FindElements(CartItems);
                
                foreach (var item in cartItems)
                {
                    try
                    {
                        var nameElement = item.FindElement(ProductNameInCart);
                        var quantityElement = item.FindElement(ProductQuantityInCart);
                        
                        if (nameElement.Text.Contains(productName, StringComparison.OrdinalIgnoreCase))
                        {
                            int actualQuantity = int.Parse(quantityElement.Text.Trim());
                            return actualQuantity == expectedQuantity;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool IsAnyProductDisplayedInCartWithQuantity(int expectedQuantity)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
                wait.Until(d => d.FindElement(CartInfoTable));

                var cartItems = Driver.FindElements(CartItems);
                
                if (cartItems.Count > 0)
                {
                    var firstItem = cartItems[0];
                    
                    // Try primary locator first
                    try
                    {
                        var quantityElement = firstItem.FindElement(ProductQuantityInCart);
                        int actualQuantity = int.Parse(quantityElement.Text.Trim());
                        return actualQuantity == expectedQuantity;
                    }
                    catch (NoSuchElementException)
                    {
                        // Try alternative locator
                        try
                        {
                            var quantityElement = firstItem.FindElement(ProductQuantityInCartAlt);
                            int actualQuantity = int.Parse(quantityElement.Text.Trim());
                            return actualQuantity == expectedQuantity;
                        }
                        catch (NoSuchElementException)
                        {
                            // Try to find any element with quantity information
                            var allQuantityElements = firstItem.FindElements(By.CssSelector("td.cart_quantity *"));
                            
                            foreach (var element in allQuantityElements)
                            {
                                var text = element.Text.Trim();
                                if (int.TryParse(text, out int quantity))
                                {
                                    return quantity == expectedQuantity;
                                }
                            }
                        }
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}