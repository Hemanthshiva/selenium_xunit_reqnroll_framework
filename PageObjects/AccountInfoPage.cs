// Page Object for Account Information Page
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class AccountInfoPage : BasePage
{
    private static readonly By EnterAccountInfo = By.XPath("//b[text()='Enter Account Information']");
    private static readonly By TitleMr = By.Id("id_gender1");
    private static readonly By TitleMrs = By.Id("id_gender2");
    private static readonly By PasswordInput = By.Id("password");
    private static readonly By DayDropdown = By.Id("days");
    private static readonly By MonthDropdown = By.Id("months");
    private static readonly By YearDropdown = By.Id("years");
    private static readonly By NewsletterCheckbox = By.Id("newsletter");
    private static readonly By OffersCheckbox = By.Id("optin");
    private static readonly By FirstNameInput = By.Id("first_name");
    private static readonly By LastNameInput = By.Id("last_name");
    private static readonly By CompanyInput = By.Id("company");
    private static readonly By Address1Input = By.Id("address1");
    private static readonly By Address2Input = By.Id("address2");
    private static readonly By CountryDropdown = By.Id("country");
    private static readonly By StateInput = By.Id("state");
    private static readonly By CityInput = By.Id("city");
    private static readonly By ZipCodeInput = By.Id("zipcode");
    private static readonly By MobileNumberInput = By.Id("mobile_number");
    private static readonly By CreateAccountButton = By.XPath("//button[text()='Create Account']");

    public static bool IsEnterAccountInfoVisible()
    {
        try
        {
            return WaitForVisible(EnterAccountInfo).Displayed;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    public static void FillAccountInfo(string title, string password, string day, string month, string year)
    {
        Click(title == "Mr" ? TitleMr : TitleMrs);
        SetValue(PasswordInput, password);
        SelectByText(DayDropdown, day);
        SelectByText(MonthDropdown, month);
        SelectByText(YearDropdown, year);
    }

    public static void SelectNewsletter()
    {
        Click(NewsletterCheckbox);
    }

    public static void SelectOffers()
    {
        Click(OffersCheckbox);
    }

    public static void FillAddressInfo(string firstName, string lastName, string company, string address1, string address2, string country, string state, string city, string zipcode, string mobile)
    {
        SetValue(FirstNameInput, firstName);
        SetValue(LastNameInput, lastName);
        SetValue(CompanyInput, company);
        SetValue(Address1Input, address1);
        SetValue(Address2Input, address2);
        
        // Country dropdown might need scrolling or special handling, but let's try standard approach first
        // If it fails, we can add the specific retry logic from original code back if needed, 
        // but Click() in BasePage already handles ElementClickInterceptedException.
        // However, SelectByText uses native SelectElement which might not trigger the BasePage.Click logic.
        // Let's implement a safe Select helper or just use the logic here.
        SelectByText(CountryDropdown, country);

        SetValue(StateInput, state);
        SetValue(CityInput, city);
        SetValue(ZipCodeInput, zipcode);
        SetValue(MobileNumberInput, mobile);
    }

    public static void ClickCreateAccount()
    {
        Click(CreateAccountButton);
    }

    private static void SelectByText(By by, string text)
    {
        var element = WaitForVisible(by);
        // Ensure element is scrolled into view before interacting
        ScrollIntoView(element); 
        new SelectElement(element).SelectByText(text);
    }
}
