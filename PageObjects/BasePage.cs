// Base class for all Page Objects
using OpenQA.Selenium;
using selenium_xunit_reqnroll_framework.Utilities;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public abstract class BasePage
    {
        // Correctly access the Driver property from the Utilities namespace  
        // Mark Driver as static since it does not access instance data  
        protected static IWebDriver Driver => selenium_xunit_reqnroll_framework.Utilities.WebDriverManager.Driver;

        // Mark BaseUrl as static since it does not access instance data  
        protected static string BaseUrl => ConfigManager.BaseUrl;

        //get text from an element
        protected static string GetText(By by)
        {
            try
            {
                return Driver.FindElement(by).Text;
            }
            catch (NoSuchElementException)
            {
                return string.Empty; // Return empty string if element is not found
            }
        }
    }
}
