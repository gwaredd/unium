// https://github.com/websockets/ws

const { test }  = require( 'mocha' );
const chai      = require( 'chai' );
const WebSocket = require( 'ws' );
const config    = require( './config' );

context( 'Using the websocket interface', () => {

    // NB: this is an example of using the 'raw' sockets
    // we simplify this wit the helper object in the final example

  let ws;

  before( () => {
    ws = new WebSocket( config.ws );
  });

  test( 'Check we are in the tutorial scene (web sockets)', function( done ) {
    
    this.timeout( 3000 );

    ws.on( 'open', () => ws.send( JSON.stringify( { 'q': '/about' }) ) );
    ws.on( 'error', (e) => chai.assert.fail( e ) );

    ws.on( 'message', (msg) => {

      const json = JSON.parse( msg );
      json.should.have.property( 'data' );
      json.should.not.have.property( 'error' );

      with( json.data ) {
        Product.should.equal( 'unium' );
        Company.should.equal( 'gwaredd' );
        Scene.should.equal( 'Tutorial' );
      }

      done();
    });
  });

  after( () => {
    ws.close();
  });

});

