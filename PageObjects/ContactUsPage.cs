using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects;

public class ContactUsPage : BasePage
{
    private static readonly By ContactUsButton = By.XPath("//a[contains(text(),'Contact us') or contains(text(),'Contact Us')]");
    private static readonly By GetInTouchHeader = By.XPath("//h2[text()='Get In Touch']");
    private static readonly By NameInput = By.Name("name");
    private static readonly By EmailInput = By.Name("email");
    private static readonly By SubjectInput = By.Name("subject");
    private static readonly By MessageInput = By.Name("message");
    private static readonly By UploadInput = By.Name("upload_file");
    private static readonly By SubmitButton = By.XPath("//input[@type='submit' and @name='submit']");
    private static readonly By SuccessMessage = By.XPath("//div[contains(text(),'Success! Your details have been submitted successfully.')]");
    private static readonly By HomeButton = By.XPath("//a[contains(text(),'Home')]");

    public static void GoToContactUs()
    {
        // Handle any cookie popup first
        HomePage.HandleCookiePopup();

        // Try clicking with JavaScript if regular click fails due to overlay
        // The BasePage.Click helper already handles this logic (WaitForVisible + Click + JS fallback)
        Click(ContactUsButton);
    }

    public static bool IsGetInTouchVisible() => Driver.FindElement(GetInTouchHeader).Displayed;

    public static void FillContactForm(string name, string email, string subject, string message)
    {
        SetValue(NameInput, name);
        SetValue(EmailInput, email);
        SetValue(SubjectInput, subject);
        SetValue(MessageInput, message);
    }

    public static void UploadFile(string filePath)
    {
        Driver.FindElement(UploadInput).SendKeys(filePath);
    }

    public static void ClickSubmit()
    {
        Click(SubmitButton);
    }

    public static void AcceptAlert()
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
        wait.Until(d =>
        {
            try { d.SwitchTo().Alert().Accept(); return true; } catch { return false; }
        });
    }

    public static bool IsSuccessMessageVisible() => WaitForVisible(SuccessMessage).Displayed;

    public static void ClickHome()
    {
        Click(HomeButton);
    }

    public static bool IsHomePageVisible() => HomePage.IsHomePageVisible();
}
