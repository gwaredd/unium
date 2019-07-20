import pytest
import websockets
import unium

@pytest.mark.asyncio
async def test_webSocket( unium_socket ):

  """
  a functional test
  run around the tutorial level and checks we can collect all the pickups
  """

  # connect to the game
  async with websockets.connect( unium_socket ) as ws:

    u = unium.WebsocketHelper( ws )

    # check we have the unium project 
    about = await u.get( "/about" )
    assert about[ "Product" ] == "unium"

    # (re)load the tutorial scene (ensures it's in the start state)
    res = await u.get( "/utils/scene/Tutorial" )
    assert res[ "scene" ] == "Tutorial"

    # get positions of all pickups
    pickups = await u.get( "/q/scene/Game/Pickup.transform.position" )

    # we should have 4 to start with
    assert len( pickups ) == 4

    # register for OnPickupCollected events
    await u.bind( "/scene/Game.Tutorial.OnPickupCollected" )

    # move to each pickup
    for pos in pickups:
      await u.get( f"/q/scene/Game/Player.Player.MoveTo({pos})" )
      await u.wait_for( "OnPickupCollected", timeout=10.0 )

    # check there are no more pickups left in the level
    pickups = await u.get( "/q/scene/Game/Pickup.name" )
    assert len( pickups ) == 0
