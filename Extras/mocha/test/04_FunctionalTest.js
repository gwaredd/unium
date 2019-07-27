const { test }    = require( 'mocha' );
const config      = require( './config' );
const UniumHelper = require( './unium' );

context( 'An example functional test', () => {
 
  const u = new UniumHelper();

  test( 'Check we can run around the tutorial level and collect all the pickups', async function() {

    this.timeout( 20 * 1000 )

    await u.connect( config.ws );

    // check this is the unium executable
    const about = await u.get( '/about' );
    about.Company.should.equal( 'gwaredd' );
    about.Product.should.equal( 'unium' );
    
    // (re)load the tutorial scene (ensures we're in the start state)
    const res = await u.get( '/utils/scene/Tutorial' );
    res.scene.should.equal( 'Tutorial' );

    // get the positions of all pickups
    const pickups = await u.get( "/q/scene/Game/Pickup.transform.position" );

    // we should have 4 pickups
    pickups.length.should.equal( 4 );

    // register for OnPickupCollected events
    await u.bind( "/scene/Game.Tutorial.OnPickupCollected" )

    // move towards each pickup
    for( let pos of pickups ) {
      const p = JSON.stringify( pos );
      await u.get( `/q/scene/Game/Player.Player.MoveTo(${p})` );
      await u.wait_for( "OnPickupCollected", 10 );
    }

    // check there are no more pickups left in the level
    const pickups_remaining = await u.get( "/q/scene/Game/Pickup.name" );
    pickups_remaining.length.should.equal( 0 );
  });

  after( () => {
    u.disconnect();
  });

});
