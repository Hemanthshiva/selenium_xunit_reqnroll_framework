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
	When I click Delete Account button
	Then Account Deleted should be visible