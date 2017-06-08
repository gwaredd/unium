#--------------------------------------------------------------------------------

config    = require './config'
assert    = require( 'chai' ).assert
WebSocket = require 'ws'

describe 'Sockets GQL', ->

  #--------------------------------------------------------------------------------
  it 'should be able query stats', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify id:"stats", q: "/q/stats"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->
      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'stats'
      msg.should.have.property 'data'
      msg.should.not.have.property 'error'

      results = msg.data
      results.should.be.an "array"
      results.should.be.length 1
      
      results[0].should.have.property 'FPS'
      results[0].should.have.property 'RunningTime'
      results[0].should.have.property 'Scene'

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should be able query the scene graph', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify id:"q", q: "/q/scene"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->
      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'q'
      msg.should.have.property 'data'
      msg.should.not.have.property 'error'

      results = msg.data
      results.should.be.an "array"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should handle not found queries', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify id:"notfound", q: "/q/notfound"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->
      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'notfound'
      msg.should.have.property 'data'
      msg.should.not.have.property 'error'

      results = msg.data
      results.should.be.an "array"
      results.should.be.length 0

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should be able send a complex query', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify id:"q", q: "/q/scene/Game/Cu*.Transform.position[x>=0].x"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->
      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'q'
      msg.should.have.property 'data'
      msg.should.not.have.property 'error'

      results = msg.data
      results.should.be.an "array"
      results.should.have.length 3
      results.should.include 0
      results.should.include 5

      ws.close()

