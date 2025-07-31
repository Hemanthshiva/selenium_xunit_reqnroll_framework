# Selenium xUnit Reqnroll Automation Framework

A comprehensive C# test automation framework built with **Selenium WebDriver**, **xUnit**, and **Reqnroll** (formerly SpecFlow) for behavior-driven development (BDD) testing. This framework provides a robust foundation for automated web testing with integrated reporting, database management, and API testing capabilities.

## 🚀 Features

- **BDD Testing**: Gherkin-based test scenarios using Reqnroll
- **Cross-Browser Support**: Chrome WebDriver with extensible architecture
- **Page Object Model**: Clean, maintainable page object implementation
- **Allure Reporting**: Rich test reports with screenshots and detailed execution logs
- **Database Integration**: SQLite database for test data management
- **API Testing**: REST API integration for user registration
- **Configuration Management**: JSON-based configuration system
- **Parallel Execution**: xUnit test runner with parallel execution support
- **Cookie Handling**: Automatic cookie consent dialog management
- **Screenshot Capture**: Automatic screenshot capture on test failures

## 🏗️ Architecture

### Project Structure
```
selenium-xunit-reqnroll-framework/
├── Features/                    # Gherkin feature files
│   ├── ContactUs.feature
│   ├── LoginUser.feature
│   └── RegisterUser.feature
├── StepDefinitions/            # Step definition implementations
│   ├── GivenSteps.cs
│   ├── WhenSteps.cs
│   └── ThenSteps.cs
├── PageObjects/                # Page Object Model classes
│   ├── BasePage.cs
│   ├── HomePage.cs
│   ├── ContactUsPage.cs
│   ├── SignupLoginPage.cs
│   ├── AccountInfoPage.cs
│   ├── AccountCreatedPage.cs
│   └── UserProfilePage.cs
├── Utilities/                  # Utility classes
│   ├── WebDriverManager.cs
│   ├── ConfigManager.cs
│   ├── UserCredentialDb.cs
│   ├── ApiUserRegistrationClient.cs
│   └── ReportManager.cs
├── Hooks/                      # Test hooks and lifecycle management
│   └── TestHooks.cs
├── appsettings.json           # Configuration file
├── allureConfig.json          # Allure reporting configuration
└── README.md
```

### Key Components

#### 1. **WebDriver Management**
- Singleton pattern implementation for WebDriver lifecycle
- Automatic driver setup and teardown
- Configurable timeouts and browser settings

#### 2. **Page Object Model**
- Base page class with common functionality
- Static methods for better performance
- Robust element interaction with error handling

#### 3. **Configuration Management**
- JSON-based configuration system
- Environment-specific settings
- Centralized configuration access

#### 4. **Database Integration**
- SQLite database for test data storage
- User credential management
- Automated database initialization

#### 5. **Reporting System**
- Allure framework integration
- Automatic screenshot capture on failures
- Step-by-step execution tracking
- Rich HTML reports with timeline and history

## 🛠️ Prerequisites

- **.NET 8.0 SDK** or later
- **Visual Studio 2022** or **Visual Studio Code**
- **Chrome Browser** (latest version)
- **Allure CLI** (for report generation)

### Installing Allure CLI
```bash
# Using npm
npm install -g allure-commandline --save-dev

# Using Scoop (Windows)
scoop install allure

# Or download from https://github.com/allure-framework/allure2/releases
```

## 📦 Dependencies

The framework uses the following NuGet packages:

```xml
<PackageReference Include="Selenium.WebDriver" Version="4.33.0" />
<PackageReference Include="Selenium.Support" Version="4.33.0" />
<PackageReference Include="Selenium.WebDriver.ChromeDriver" Version="137.0.7151.11900" />
<PackageReference Include="WebDriverManager" Version="2.17.5" />
<PackageReference Include="Reqnroll" Version="2.4.1" />
<PackageReference Include="Reqnroll.xUnit" Version="2.4.1" />
<PackageReference Include="xunit" Version="2.9.3" />
<PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.14.1" />
<PackageReference Include="Allure.Net.Commons" Version="2.12.1" />
<PackageReference Include="Microsoft.Data.Sqlite" Version="9.0.6" />
<PackageReference Include="RestSharp" Version="112.1.0" />
<PackageReference Include="Bogus" Version="35.6.3" />
```

## ⚙️ Configuration

### appsettings.json
```json
{
  "BaseUrl": "https://automationexercise.com",
  "Browser": "Chrome",
  "Timeout": 15
}
```

### allureConfig.json
```json
{
  "allure": {
    "directory": "allure-results",
    "links": [
      "https://example.org/mylink"
    ]
  }
}
```

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd selenium-xunit-reqnroll-framework
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Build the Project
```bash
dotnet build
```

### 4. Run Tests
```bash
# Run all tests
dotnet test

# Run specific test
dotnet test --filter "ContactUsForm"

# Run with verbose output
dotnet test --verbosity normal
```

### 5. Generate Allure Reports
```bash
# Generate and serve report (opens in browser)
allure serve allure-results

# Generate static report
allure generate allure-results -o allure-report --clean
```

## 📝 Test Scenarios

### Contact Us Feature
```gherkin
Feature: Contact Us
  As a user
  I want to contact Automation Exercise
  So that I can submit my query

Scenario: Contact Us Form
  Given I am on the home page
  Then the page title should contain "Automation Exercise"
  When I click on Contact Us button
  Then GET IN TOUCH is visible
  When I enter contact name, email, subject and message
  And I upload a file
  And I click Submit button
  And I accept the alert
  Then Success message is visible
  When I click Home button
  Then I am on the home page
```

### User Registration Feature
```gherkin
Feature: Register User
  As a new user
  I want to register on Automation Exercise
  So that I can use the site features

Scenario: Register User
  Given I am on the home page
  When I register a new random user
  Then Account Created should be visible
  When I click Continue after account creation
  Then Logged in as username should be visible
  When I click Delete Account button
  Then Account Deleted should be visible
```

### User Login Feature
```gherkin
Feature: Login User
  As a registered user
  I want to login to Automation Exercise
  So that I can access my account

Background:
  Given 1 users are registered via API

Scenario: Login User with correct email and password
  Given I am on the home page
  Then the page title should contain "Automation Exercise"
  When I click on Signup/Login button
  Then "Login to your account" text should be visible
  When I enter login email and password from db
  And I click login button
  Then Logged in as username should be visible
```

## 🔧 Key Features Explained

### 1. **Cookie Consent Handling**
The framework automatically handles cookie consent dialogs:
```csharp
public static void HandleCookiePopup()
{
    try
    {
        var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
        var cookieButton = wait.Until(driver =>
        {
            var elements = driver.FindElements(CookieAcceptButton);
            return elements.Count > 0 && elements[0].Displayed ? elements[0] : null;
        });
        cookieButton?.Click();
    }
    catch (WebDriverTimeoutException) { /* Popup not present, ignore */ }
}
```

### 2. **Robust Element Interaction**
Elements are clicked with fallback to JavaScript execution:
```csharp
try
{
    btn.Click();
}
catch (ElementClickInterceptedException)
{
    // Use JavaScript click to bypass overlay issues
    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", btn);
}
```

### 3. **Database Integration**
SQLite database for storing test user credentials:
```csharp
public static void InsertUser(UserCredential user)
{
    using var connection = new SqliteConnection($"Data Source={DbPath}");
    connection.Open();
    using var insertCmd = connection.CreateCommand();
    insertCmd.CommandText = "INSERT INTO Users (Username, Email, Password) VALUES ($username, $email, $password);";
    insertCmd.Parameters.AddWithValue("$username", user.Username);
    insertCmd.Parameters.AddWithValue("$email", user.Email);
    insertCmd.Parameters.AddWithValue("$password", user.Password);
    insertCmd.ExecuteNonQuery();
}
```

### 4. **API Integration**
REST API calls for user registration:
```csharp
[Given(@"(.*) users are registered via API")]
public static async Task Given5UsersAreRegisteredViaAPI(int count)
{
    var faker = new Faker("en");
    var client = new RestClient("https://automationexercise.com/api/");
    
    for (int i = 0; i < count; i++)
    {
        var user = new UserRegistrationModel { /* ... */ };
        var request = new RestRequest("createAccount", Method.Post);
        // Add parameters and execute request
    }
}
```

## 📊 Reporting

The framework uses **Allure** for comprehensive test reporting:

- **Test Results**: Pass/Fail status for each scenario
- **Step Details**: Individual step execution results with timing
- **Screenshots**: Automatic screenshot capture on failures
- **Timeline**: Test execution timeline visualization
- **Categories**: Test categorization and filtering
- **History**: Test execution history tracking
- **Attachments**: File uploads and error logs

### Sample Report Features:
- Overview dashboard with test statistics
- Detailed test case execution logs
- Failed test analysis with screenshots
- Execution timeline and duration metrics
- Historical trend analysis

## 🧪 Best Practices Implemented

1. **Page Object Model**: Clean separation of test logic and page interactions
2. **Static Methods**: Performance optimization for page object methods
3. **Singleton Pattern**: Efficient WebDriver management
4. **Configuration Management**: Centralized settings management
5. **Error Handling**: Robust exception handling throughout the framework
6. **Wait Strategies**: Explicit waits for reliable element interactions
7. **Screenshot Capture**: Automatic failure documentation
8. **Database Management**: Proper connection handling and cleanup
9. **Test Data Management**: Faker library for realistic test data generation
10. **Parallel Execution**: xUnit parallel test execution support

## 🔍 Troubleshooting

### Common Issues and Solutions

1. **WebDriver Issues**
   - Ensure Chrome browser is installed and up-to-date
   - WebDriverManager automatically handles driver downloads

2. **Element Not Found**
   - Check if cookie consent dialog is blocking elements
   - Verify element locators are correct
   - Ensure proper wait strategies are implemented

3. **Database Connection Issues**
   - Check if SQLite database file permissions are correct
   - Ensure database initialization is called before operations

4. **Allure Report Generation**
   - Verify Allure CLI is installed correctly
   - Check if allure-results directory exists and contains test results

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For questions and support:
- Create an issue in the repository
- Review the troubleshooting section
- Check the Allure documentation for reporting issues

## 🔄 Version History

- **v1.0.0**: Initial release with basic BDD framework
- **v1.1.0**: Added Allure reporting integration
- **v1.2.0**: Enhanced cookie handling and element interaction
- **v1.3.0**: Added API testing capabilities and database integration

---

**Built with ❤️ using C#, Selenium, xUnit, and Reqnroll**