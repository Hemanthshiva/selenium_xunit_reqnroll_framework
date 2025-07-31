// ...existing code...
// Page Object for User Profile actions (delete account)
using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class UserProfilePage : BasePage
    {
        private By deleteAccountButton = By.LinkText("Delete Account");
        public void ClickDeleteAccount() => Driver.FindElement(deleteAccountButton).Click();
    }
}
