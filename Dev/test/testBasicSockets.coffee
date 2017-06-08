#--------------------------------------------------------------------------------

config    = require './config'
assert    = require( 'chai' ).assert
WebSocket = require 'ws'


describe 'Basic Sockets', ->

  #--------------------------------------------------------------------------------
  it 'should be able to connect to the http:// interface', (done) ->

    msg = null
    ws  = new WebSocket "#{ config.url }/ws"

    ws.on 'open',   -> ws.send JSON.stringify id:"msg", q: "/about"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->
      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'msg'
      msg.should.have.property 'data'
      msg.should.not.have.property 'error'
      
      msg.data.should.have.property 'FPS'
      msg.data.should.have.property 'RunningTime'
      msg.data.should.have.property 'Scene'

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should be able to connect to the ws:// interface', (done) ->

    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.close()
    ws.on 'close',  -> done()


  #--------------------------------------------------------------------------------
  it 'should not require an id on the message', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify q: "/about"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->
      msg = JSON.parse data

      msg.should.not.have.property 'id'
      msg.should.have.property 'data'
      msg.should.not.have.property 'status'
      msg.should.not.have.property 'error'

      msg.data.should.have.property 'FPS'
      msg.data.should.have.property 'RunningTime'
      msg.data.should.have.property 'Scene'

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should handle not found requests', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify id: "notfound", q: "/notfound"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'notfound'
      msg.should.not.have.property 'data'
      msg.should.have.property 'error'

      msg.error.should.equal "Not Found"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should handle malformed requests', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send "some invalid json {}[]':"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'socket'
      msg.should.not.have.property 'data'
      msg.should.have.property 'error'

      msg.error.should.equal "Bad Request"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should handle invalid fields', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify notQ: "/notfound"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'socket'
      msg.should.not.have.property 'data'
      msg.should.have.property 'error'

      msg.error.should.equal "Bad Request"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should not handle binary data', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send [1..10]
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'socket'
      msg.should.not.have.property 'data'
      msg.should.have.property 'error'

      msg.error.should.equal "Bad Request"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should be able to call socket commands', (done) ->

    msg = null
    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify q:"/socket.ping()"
    ws.on 'close',  -> assert msg?, "Failed to receive message"; done()

    ws.on 'message', (data, flags) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'socket'
      msg.should.have.property 'data'
      msg.should.not.have.property 'error'

      msg.data.should.equal "pong"

      ws.close()

