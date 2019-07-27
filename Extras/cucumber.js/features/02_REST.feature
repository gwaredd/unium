Feature: The about endpoint 

  Unium provides an '/about' end point that gives some high-level information
  about the application

  Scenario: Check the tutorial level is running

    Given we have fetched "/about"
    Then  the data should be a "object"
    And   the "Product" property should be "unium"
    And   the "Company" property should be "gwaredd"
    And   the "Scene" property should be "Tutorial"
