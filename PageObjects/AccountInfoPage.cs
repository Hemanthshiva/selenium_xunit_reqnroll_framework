// Page Object for Account Information Page
using OpenQA.Selenium;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class AccountInfoPage : BasePage
    {
        private By enterAccountInfo = By.XPath("//b[text()='Enter Account Information']");
        private By titleMr = By.Id("id_gender1");
        private By titleMrs = By.Id("id_gender2");
        private By passwordInput = By.Id("password");
        private By dayDropdown = By.Id("days");
        private By monthDropdown = By.Id("months");
        private By yearDropdown = By.Id("years");
        private By newsletterCheckbox = By.Id("newsletter");
        private By offersCheckbox = By.Id("optin");
        private By firstNameInput = By.Id("first_name");
        private By lastNameInput = By.Id("last_name");
        private By companyInput = By.Id("company");
        private By address1Input = By.Id("address1");
        private By address2Input = By.Id("address2");
        private By countryDropdown = By.Id("country");
        private By stateInput = By.Id("state");
        private By cityInput = By.Id("city");
        private By zipcodeInput = By.Id("zipcode");
        private By mobileNumberInput = By.Id("mobile_number");
        private By createAccountButton = By.XPath("//button[text()='Create Account']");
        public bool IsEnterAccountInfoVisible() => Driver.FindElement(enterAccountInfo).Displayed;
        public void FillAccountInfo(string title, string password, string day, string month, string year)
        {
            if (title == "Mr") Driver.FindElement(titleMr).Click();
            else Driver.FindElement(titleMrs).Click();
            Driver.FindElement(passwordInput).SendKeys(password);
            new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(dayDropdown)).SelectByText(day);
            new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(monthDropdown)).SelectByText(month);
            new OpenQA.Selenium.Support.UI.SelectElement(Driver.FindElement(yearDropdown)).SelectByText(year);
        }
        public void SelectNewsletter()
        {
            var element = Driver.FindElement(newsletterCheckbox);
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
            }
        }
        public void SelectOffers()
        {
            var element = Driver.FindElement(offersCheckbox);
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
            }
        }
        public void FillAddressInfo(string firstName, string lastName, string company, string address1, string address2, string country, string state, string city, string zipcode, string mobile)
        {
            Driver.FindElement(firstNameInput).SendKeys(firstName);
            Driver.FindElement(lastNameInput).SendKeys(lastName);
            Driver.FindElement(companyInput).SendKeys(company);
            Driver.FindElement(address1Input).SendKeys(address1);
            Driver.FindElement(address2Input).SendKeys(address2);
            var countryElement = Driver.FindElement(countryDropdown);
            try
            {
                new OpenQA.Selenium.Support.UI.SelectElement(countryElement).SelectByText(country);
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView(true);", countryElement);
                try
                {
                    countryElement.Click();
                }
                catch (ElementClickInterceptedException)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", countryElement);
                }
                new OpenQA.Selenium.Support.UI.SelectElement(countryElement).SelectByText(country);
            }
            Driver.FindElement(stateInput).SendKeys(state);
            Driver.FindElement(cityInput).SendKeys(city);
            Driver.FindElement(zipcodeInput).SendKeys(zipcode);
            Driver.FindElement(mobileNumberInput).SendKeys(mobile);
        }
        public void ClickCreateAccount()
        {
            var element = Driver.FindElement(createAccountButton);
            try
            {
                element.Click();
            }
            catch (ElementClickInterceptedException)
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
            }
        }
    }
}
