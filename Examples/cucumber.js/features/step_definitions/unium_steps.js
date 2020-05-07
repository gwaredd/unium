//--------------------------------------------------------------------------------
// steps for unium tests

const axios       = require( 'axios' );
const WebSocket   = require( 'ws' );
const config      = require( './config' );
const UniumHelper = require( './unium' );

const { Given, When, Then, Before, After } = require( 'cucumber' );
const { expect } = require( 'chai' );


//--------------------------------------------------------------------------------
// test setup and teardown

// clear any data or websocket at the start of every scenario

Before( function() {
  this.data = null;
  this.u    = null;
})

// ensure any websocket connection (if used) is closed after scenario

After( function() {
  if( this.u ) {
    this.u.disconnect();
    this.u = null;
  }
})


//--------------------------------------------------------------------------------
// general steps


// check the product

Given( 'the product is {string}', async function(product) {
  const res = await axios.get( `${config.uri}/about` );
  res.status.should.equal( 200 );
  res.should.have.property( 'data' );

  const {data} = res;
  data.should.have.property( 'Product' );
  data.Product.should.equal( product );
  this.data = data;
});


// set the current data to the results of a given query

Given( 'we have fetched {string}', async function( path ) {
  const res = await axios.get( `${config.uri}${path}` );
  res.status.should.equal( 200 );
  res.should.have.property( 'data' );
  this.data = res.data;
});


// check the type of the current data

Then( 'the data should be a {string}', function( type ) {
  ( typeof this.data ).should.equal( type );
});

// check a property value of the current data

Then( 'the {string} property should be {string}', function( property, value ) {
  expect( this.data ).to.not.be.null;
  this.data.should.have.property( property );
  this.data[ property ].should.equal( value );
});


//--------------------------------------------------------------------------------
// web sockets

// ensure we have a websocket connection to the game

Given( 'we have created a websocket connection', async function() {
  this.u = new UniumHelper();
  await this.u.connect( config.ws );
});


// send a query using the current websocket connection
// remember the id of the message

When( 'we send {string}', (uri) => {
  expect( this.u ).to.not.be.null;
  this.mid = this.u.send( uri );
});

// wait for a response to the current query and set the current data accordingly

Then( 'we should receive a response', async function() {
  expect( this.u ).to.not.be.null;
  this.data = await this.u.wait_for( this.mid );
});


//--------------------------------------------------------------------------------
// misc

// reload level

Given( 'we have reloaded the {string} level', async function(level) {
  expect( this.u ).to.not.be.null;
  const res = await this.u.get( `/utils/scene/${level}` );
  res.should.have.property( 'scene' );
  res.scene.should.equal( level );
});

// check number of results

Then( 'sending {string} should return {string} results', async function(query, num) {
  expect( this.u ).to.not.be.null;
  const res = await this.u.get( query );
  res.should.be.an( 'array' );
  res.length.should.equal( parseInt( num ) );
  this.data = res;
});
