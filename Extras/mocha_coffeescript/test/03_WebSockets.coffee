# https://github.com/websockets/ws

{ test }  = require 'mocha'
chai      = require 'chai'
WebSocket = require 'ws'
config    = require './config'

context 'Using the websocket interface', () ->

  # NB: this is an example of using the 'raw' sockets
  # we simplify this wit the helper object in the final example

  ws = null

  before -> ws = new WebSocket config.ws

  test 'Check we are in the tutorial scene (web sockets)', (done) ->
    
    this.timeout 3000

    ws.on 'open', -> ws.send JSON.stringify { 'q': '/about' }
    ws.on 'error', chai.assert.fail

    ws.on 'message', (msg) ->

      json = JSON.parse msg
      json.should.have.property 'data'
      json.should.not.have.property 'error'

      { Product, Company, Scene } = json.data

      Product.should.equal 'unium'
      Company.should.equal 'gwaredd'
      Scene.should.equal 'Tutorial'

      done()

  after -> ws.close()

