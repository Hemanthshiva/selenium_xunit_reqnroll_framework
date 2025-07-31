# Allure Reporting Setup

This project has been configured to use Allure for test reporting instead of ExtentReports.

## Prerequisites

1. Install Allure CLI:
   ```bash
   # Using npm
   npm install -g allure-commandline --save-dev
   
   # Or using Scoop (Windows)
   scoop install allure
   
   # Or download from https://github.com/allure-framework/allure2/releases
   ```

## Running Tests and Generating Reports

1. **Run your tests** (this will generate allure-results):
   ```bash
   dotnet test
   ```

2. **Generate and serve Allure report**:
   ```bash
   # Generate and open report in browser
   allure serve allure-results
   
   # Or generate static report
   allure generate allure-results -o allure-report --clean
   ```

3. **View the report**:
   - If using `allure serve`: Report opens automatically in browser
   - If using `allure generate`: Open `allure-report/index.html` in browser

## Features

- **Test Results**: Pass/Fail status for each scenario
- **Step Details**: Individual step execution results
- **Screenshots**: Automatic screenshot capture on failures
- **Timeline**: Test execution timeline
- **Categories**: Test categorization and filtering
- **History**: Test execution history tracking

## Configuration

- Allure results are stored in `allure-results/` directory
- Configuration can be modified in `allureConfig.json`
- Screenshots are automatically attached to failed steps

## Cleanup

To clean up old results:
```bash
# Remove old results
rmdir /s allure-results
rmdir /s allure-report
```