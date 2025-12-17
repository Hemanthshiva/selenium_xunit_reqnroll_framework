// Page Object for User Profile actions (delete account)
using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class UserProfilePage : BasePage
{
    private static readonly By DeleteAccountButton = By.LinkText("Delete Account");

    public static void ClickDeleteAccount() => Click(DeleteAccountButton);
}
