const chai     = require( 'chai' );
const { When } = require( 'cucumber' );

// collect pickup script for the tutorial scene

When( 'after we collect the pickups', {timeout: 20 * 1000}, async function() {

  const {u} = this;
  chai.expect(u).to.not.be.undefined;

  // register for OnPickupCollected events
  await u.bind( "/scene/Game.Tutorial.OnPickupCollected" );

  // collect pickups
  const pickups = await u.get( '/q/scene/Game/Pickup.Transform.position' );

  for( let pos of pickups ) {
    const p = JSON.stringify( pos );
    await u.get( `/q/scene/Game/Player.Player.MoveTo(${p})` );
    await u.wait_for( "OnPickupCollected", 10 );
  }

});

