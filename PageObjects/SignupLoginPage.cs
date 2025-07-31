// ...existing code...
// Page Object for Signup/Login Page
using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class SignupLoginPage : BasePage
    {
        private By newUserSignup = By.XPath("//h2[text()='New User Signup!']");
        private readonly By loginToYourAccount = By.XPath("//h2[text()='Login to your account']");

        private By nameInput = By.XPath("//input[@data-qa='signup-name']");
        private By emailInput = By.XPath("//input[@data-qa='signup-email']");
        private By signupButton = By.XPath("//button[@data-qa='signup-button']");
        private By loginHeader = By.XPath("//h2[text()='Login to your account']");
        private By loginEmailInput = By.XPath("//input[@data-qa='login-email']");
        private By loginPasswordInput = By.XPath("//input[@data-qa='login-password']");
        private By loginButton = By.XPath("//button[@data-qa='login-button']");

        public bool IsNewUserSignupVisible() => Driver.FindElement(newUserSignup).Displayed;

        public bool IsLoginToYourAccountTextVisible() => Driver.FindElement(loginToYourAccount).Displayed;

        public string GetLoginToYourAccountText() => GetText(loginToYourAccount);

        public void EnterNameAndEmail(string name, string email)
        {
            Driver.FindElement(nameInput).SendKeys(name);
            Driver.FindElement(emailInput).SendKeys(email);
        }
        public void ClickSignupButton() => Driver.FindElement(signupButton).Click();
        public bool IsLoginHeaderVisible() => Driver.FindElement(loginHeader).Displayed;
        public void EnterLoginCredentials(string email, string password)
        {
            Driver.FindElement(loginEmailInput).SendKeys(email);
            Driver.FindElement(loginPasswordInput).SendKeys(password);
        }
        public void ClickLoginButton() => Driver.FindElement(loginButton).Click();
    }
}
