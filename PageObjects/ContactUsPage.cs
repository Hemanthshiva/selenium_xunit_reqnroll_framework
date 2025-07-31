using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace selenium_xunit_reqnroll_framework.PageObjects
{
    public class ContactUsPage : BasePage
    {
        private static By ContactUsButton => By.XPath("//a[contains(text(),'Contact us') or contains(text(),'Contact Us')]");
        private static By GetInTouchHeader => By.XPath("//h2[text()='Get In Touch']");
        private static By NameInput => By.Name("name");
        private static By EmailInput => By.Name("email");
        private static By SubjectInput => By.Name("subject");
        private static By MessageInput => By.Name("message");
        private static By UploadInput => By.Name("upload_file");
        private static By SubmitButton => By.XPath("//input[@type='submit' and @name='submit']");
        private static By SuccessMessage => By.XPath("//div[contains(text(),'Success! Your details have been submitted successfully.')]");
        private static By HomeButton => By.XPath("//a[contains(text(),'Home')]");

        public static void GoToContactUs()
        {
            // Handle any cookie popup first
            HomePage.HandleCookiePopup();

            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            var btn = wait.Until(d => d.FindElement(ContactUsButton));

            // Try clicking with JavaScript if regular click fails due to overlay
            try
            {
                btn.Click();
            }
            catch (ElementClickInterceptedException)
            {
                // Use JavaScript click to bypass overlay issues
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", btn);
            }
        }
        public static bool IsGetInTouchVisible() => Driver.FindElement(GetInTouchHeader).Displayed;
        public static void FillContactForm(string name, string email, string subject, string message)
        {
            Driver.FindElement(NameInput).SendKeys(name);
            Driver.FindElement(EmailInput).SendKeys(email);
            Driver.FindElement(SubjectInput).SendKeys(subject);
            Driver.FindElement(MessageInput).SendKeys(message);
        }
        public static void UploadFile(string filePath)
        {
            Driver.FindElement(UploadInput).SendKeys(filePath);
        }
        public static void ClickSubmit()
        {
            Driver.FindElement(SubmitButton).Click();
        }
        public static void AcceptAlert()
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
            wait.Until(d =>
            {
                try { d.SwitchTo().Alert().Accept(); return true; } catch { return false; }
            });
        }
        public static bool IsSuccessMessageVisible() => Driver.FindElement(SuccessMessage).Displayed;
        public static void ClickHome()
        {
            Driver.FindElement(HomeButton).Click();
        }
        public static bool IsHomePageVisible() => HomePage.IsHomePageVisible();
    }
}
