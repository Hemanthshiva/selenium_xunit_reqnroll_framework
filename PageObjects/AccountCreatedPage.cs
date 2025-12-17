// Page Object for Account Created and Deletion
// Ignore Spelling: reqnroll xunit

using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class AccountCreatedPage : BasePage
{
    // Locators
    private static readonly By AccountCreatedText = By.XPath("//b[text()='Account Created!']");
    private static readonly By ContinueButton = By.XPath("//a[text()='Continue']");
    
    // Methods
    public static bool IsAccountCreatedVisible()
    {
        try
        {
            return WaitForVisible(AccountCreatedText).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
    
    public static void ClickContinue()
    {
        try
        {
            Click(ContinueButton);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Continue button not found or not clickable");
        }
    }
}

public class AccountDeletedPage : BasePage
{
    // Locators
    private static readonly By AccountDeletedText = By.CssSelector("[data-qa='account-deleted']");
    private static readonly By AccountDeletedHeader = By.CssSelector("h2.title");
    private static readonly By ContinueButton = By.XPath("//a[text()='Continue']");
    
    // Methods
    public static bool IsAccountDeletedVisible()
    {
        try
        {
            // Try the primary locator first
            try
            {
                return WaitForVisible(AccountDeletedText).Displayed;
            }
            catch (WebDriverTimeoutException)
            {
                // Fall back to the header locator
                var header = WaitForVisible(AccountDeletedHeader);
                return header.Displayed && header.Text.Contains("ACCOUNT DELETED", StringComparison.OrdinalIgnoreCase);
            }
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
    
    public static void ClickContinue()
    {
        try
        {
            Click(ContinueButton);
        }
        catch (WebDriverTimeoutException)
        {
            throw new Exception("Continue button not found or not clickable");
        }
    }
}
