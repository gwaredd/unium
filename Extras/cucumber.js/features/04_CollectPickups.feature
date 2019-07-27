Feature: The unium tutorial level

  Scenario: We can run around the tutorial level and collect all the pickups

    Given the product is "unium"
    And   we have created a websocket connection
    And   we have reloaded the "Tutorial" level
    Then  sending "/q/scene/Game/Pickup.name" should return "4" results
    When  after we collect the pickups
    Then  sending "/q/scene/Game/Pickup.name" should return "0" results
