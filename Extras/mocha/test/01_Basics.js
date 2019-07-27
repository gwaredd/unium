// https://mochajs.org/#getting-started

//
// a basic test
//

assert = require( 'assert' );

describe( 'A basic test', () => {
  it( 'Assert true', () => {
    // put your test code here
    assert( true );
  });
});


//
// the chai library provides a nicer way to write assertions (BDD style)
// https://www.chaijs.com/
//

require( 'chai' ).should();

describe( 'A test with the chai assertion library', () => {
  it( 'An empty array should have length 0', () => {
    [].length.should.equal( 0 );
  });
});


//
// tests with setup and teardown code
// NB: 'context' and 'test' are synonyms for 'describe' and 'it'
//

const { test } = require( 'mocha' );

context( 'A test with setup and teardown', () => {

  before( () => {
    // this is run at the start of this test suite before any tests
  });

  beforeEach( () => {
    // this is run before each test
  });

  // test cases

  test( 'Test number 1', () => {
  });

  test( 'Test number 2', () => {
  });

  test( 'Test number 3', () => {
  });

  afterEach( () => {
    // this is run after each test
  });
  
  after( () => {
    // this is run at the end of this test suite after all tests have run
  });

});

