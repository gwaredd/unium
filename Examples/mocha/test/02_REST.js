// https://github.com/axios/axios

const { test } = require( 'mocha' );
const axios    = require( 'axios' );
const config   = require( './config' );

context( 'Asynchronous HTTP calls ', () => {

  test( 'Check we are in the tutorial scene', async () => {

    // get data from unium
    const res = await axios.get( `${config.url}/about` );

    // check web request was OK
    res.status.should.equal( 200 );

    // check data returned is as expected
    with( res.data ) {
      Product.should.equal( 'unium' );
      Company.should.equal( 'gwaredd' );
      Scene.should.equal( 'Tutorial' );
    }
  });

});

