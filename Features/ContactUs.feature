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