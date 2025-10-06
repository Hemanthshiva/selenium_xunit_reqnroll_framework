# Allure Reporting Setup

This project has been configured to use Allure for test reporting with Reqnroll integration.

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
   # Navigate to the bin directory where results are generated
   cd bin\Debug\net8.0
   
   # Generate and open report in browser
   allure serve allure-results
   
   # Or generate static report
   allure generate allure-results -o allure-report --clean
   ```

   **Alternative - From project root:**
   ```bash
   # Generate and serve from project root (using full path)
   allure serve bin\Debug\net8.0\allure-results
   
   # Or generate static report from project root
   allure generate bin\Debug\net8.0\allure-results -o allure-report --clean
   ```

3. **View the report**:
   - If using `allure serve`: Report opens automatically in browser
   - If using `allure generate`: Open `allure-report/index.html` in browser

## Features

- **Test Results**: Pass/Fail status for each scenario
- **Step Details**: Individual step execution results with Gherkin steps
- **Screenshots**: Automatic screenshot capture on failures
- **Timeline**: Test execution timeline
- **Categories**: Test categorization and filtering
- **History**: Test execution history tracking
- **BDD Integration**: Full Reqnroll/Gherkin scenario reporting

## Configuration

- Allure results are stored in `bin\Debug\net8.0\allure-results\` directory
- Configuration can be modified in `allureConfig.json`
- Screenshots are automatically attached to failed steps
- Reqnroll plugin automatically handles scenario and step reporting

## Package Dependencies

The following packages are required for Allure integration:
- `Allure.Net.Commons` (2.12.1)
- `Allure.Reqnroll` (2.12.1)

## Cleanup

To clean up old results:
```bash
# Remove old results from bin directory
rmdir /s bin\Debug\net8.0\allure-results
rmdir /s allure-report

# Or clean all build outputs
dotnet clean
```

## Troubleshooting

If Allure results are not being generated:
1. Ensure the `Allure.Reqnroll` package is installed
2. Check that the Reqnroll plugin is loaded (look for "Loading plugin Allure.ReqnrollPlugin.dll" in test output)
3. Verify the `reqnroll.json` includes the Allure plugin configuration
4. Make sure tests are actually running (not just building)