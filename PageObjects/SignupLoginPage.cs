// Page Object for Signup/Login Page
using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class SignupLoginPage : BasePage
{
    private static readonly By NewUserSignup = By.XPath("//h2[text()='New User Signup!']");
    private static readonly By LoginToYourAccount = By.XPath("//h2[text()='Login to your account']");
    private static readonly By NameInput = By.XPath("//input[@data-qa='signup-name']");
    private static readonly By EmailInput = By.XPath("//input[@data-qa='signup-email']");
    private static readonly By SignupButton = By.XPath("//button[@data-qa='signup-button']");
    private static readonly By LoginHeader = By.XPath("//h2[text()='Login to your account']");
    private static readonly By LoginEmailInput = By.XPath("//input[@data-qa='login-email']");
    private static readonly By LoginPasswordInput = By.XPath("//input[@data-qa='login-password']");
    private static readonly By LoginButton = By.XPath("//button[@data-qa='login-button']");

    public static bool IsNewUserSignupVisible() => WaitForVisible(NewUserSignup).Displayed;

    public static bool IsLoginToYourAccountTextVisible() => WaitForVisible(LoginToYourAccount).Displayed;

    public static string GetLoginToYourAccountText() => GetText(LoginToYourAccount);

    public static void EnterNameAndEmail(string name, string email)
    {
        SetValue(NameInput, name);
        SetValue(EmailInput, email);
    }

    public static void ClickSignupButton() => Click(SignupButton);

    public static bool IsLoginHeaderVisible() => WaitForVisible(LoginHeader).Displayed;

    public static void EnterLoginCredentials(string email, string password)
    {
        SetValue(LoginEmailInput, email);
        SetValue(LoginPasswordInput, password);
    }

    public static void ClickLoginButton() => Click(LoginButton);
}
