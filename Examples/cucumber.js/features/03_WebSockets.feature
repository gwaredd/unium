Feature: The about endpoint using WebSockets 

  We can also connect through a web socket and send queries that way.

  Scenario: Check the tutorial level is running using a websocket

    Given we have created a websocket connection
    When  we send "/about"
    Then  we should receive a response
    And   the "Product" property should be "unium"
    And   the "Company" property should be "gwaredd"
    And   the "Scene" property should be "Tutorial"
