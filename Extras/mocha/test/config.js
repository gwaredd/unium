// global before

before( () => {
  // ensure all tests have access to 'should'
  require( 'chai' ).should();
})

// test config parameters

module.exports = {
  url: 'http://localhost:8342',
  ws: 'ws://localhost:8342/ws'
}

