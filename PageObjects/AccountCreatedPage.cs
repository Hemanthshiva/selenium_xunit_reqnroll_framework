// ...existing code...
// Page Object for Account Created and Deletion
// Ignore Spelling: reqnroll xunit

using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class AccountCreatedPage : BasePage
    {
        private By accountCreatedText = By.XPath("//b[text()='Account Created!']");
        private By continueButton = By.XPath("//a[text()='Continue']");
        public bool IsAccountCreatedVisible() => Driver.FindElement(accountCreatedText).Displayed;
        public void ClickContinue() => Driver.FindElement(continueButton).Click();
    }
    public class AccountDeletedPage : BasePage
    {
        private By accountDeletedText = By.CssSelector("[data-qa='account-deleted']");
        private By continueButton = By.XPath("//a[text()='Continue']");
        public bool IsAccountDeletedVisible() => Driver.FindElement(accountDeletedText).Displayed;
        public void ClickContinue() => Driver.FindElement(continueButton).Click();
    }
}
