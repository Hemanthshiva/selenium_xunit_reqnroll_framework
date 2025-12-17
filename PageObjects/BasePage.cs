// Base class for all Page Objects
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using selenium_xunit_reqnroll_framework.Utilities;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public abstract class BasePage
{
    // Correctly access the Driver property from the Utilities namespace  
    protected static IWebDriver Driver => selenium_xunit_reqnroll_framework.Utilities.WebDriverManager.Driver;

    protected static string BaseUrl => ConfigManager.BaseUrl;

    // Get text from an element
    protected static string GetText(By by)
    {
        try
        {
            return Driver.FindElement(by).Text;
        }
        catch (NoSuchElementException)
        {
            return string.Empty;
        }
    }

    // Click an element, with wait and scroll
    protected static void Click(By by, int timeoutSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
        var element = wait.Until(d => d.FindElement(by));
        ScrollIntoView(element);
        try
        {
            element.Click();
        }
        catch (ElementClickInterceptedException)
        {
            // Fallback to JS click
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }

    // Wait for element to be visible
    protected static IWebElement WaitForVisible(By by, int timeoutSeconds = 10)
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
        return wait.Until(d =>
        {
            var el = d.FindElement(by);
            return (el != null && el.Displayed) ? el : null;
        });
    }

    // Set value in input field
    protected static void SetValue(By by, string value, int timeoutSeconds = 10)
    {
        var element = WaitForVisible(by, timeoutSeconds);
        element.Clear();
        element.SendKeys(value);
    }

    // Scroll element into view
    protected static void ScrollIntoView(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({block: 'center', inline: 'center'});", element);
    }
}
