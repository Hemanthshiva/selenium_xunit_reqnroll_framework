Feature: Product Quantity in Cart
  As a user
  I want to add products to cart with specific quantity
  So that I can verify the correct quantity is displayed in cart

  Scenario: Verify Product quantity in Cart
    Given I am on the home page
    Then the page title should contain "Automation Exercise"
    When I click View Product for any product on home page
    Then product detail page is opened
    When I increase quantity to 4
    And I click Add to cart button
    And I click View Cart button
    Then product is displayed in cart page with exact quantity 4