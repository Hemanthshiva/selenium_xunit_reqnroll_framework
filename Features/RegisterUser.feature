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
    When I click Continue after account deletion
    Then I am on the home page
