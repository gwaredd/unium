Feature: The about endpoint using WebSockets 

  We can also connect through a web socket and send queries that way.

  Scenario: Check the tutorial level is running using a websocket

    Given we have created a websocket connection
    When  we send "/about"
    Then  we should receive a response
    And   the "Product" message data should be "unium"
    And   the "Company" message data should be "gwaredd"
    And   the "Scene" message data should be "Tutorial"
