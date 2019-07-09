#--------------------------------------------------------------------------------

_         = require 'underscore'
config    = require './config'
req       = require 'requestify'
assert    = require( 'chai' ).assert
WebSocket = require 'ws'


describe 'Sockets Bind Events', ->

  #--------------------------------------------------------------------------------
  it 'should be able to bind to custom tick event', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"tick", q: "/bind/scene/Game.Test.TickEvent"
    ws.on 'close',  -> done()

    count = 0

    ws.on 'message', (data) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.equal "tick"
      msg.should.not.have.property 'error'

      if msg.info?
        return if msg.info == "bind TickEvent"
        return ws.close() == "unbind TickEvent"
        assert false, "unknown info message"

      msg.should.have.property 'data'
      msg.data.should.have.property 'levelTime'
      count.should.be.at.most 3

      ws.send JSON.stringify id:"tick", q:"/socket.unbind(tick)" if ++count == 3


  #--------------------------------------------------------------------------------
  it 'should not be able to bind to custom tick event as a function', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"tick", q: "/bind/scene/Game.Test.TickEvent()"
    ws.on 'close',  -> done()

    count = 0

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.equal "tick"
      msg.should.have.property 'error'
      msg.should.not.have.property 'data'
      msg.should.not.have.property 'info'

      msg.error.should.equal "Bad Request"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should not be able to bind to custom tick event as a variable', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"tick", q: "/bind/scene/Game.Test.TickEvent=123"
    ws.on 'close',  -> done()

    count = 0

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.equal "tick"
      msg.should.have.property 'error'
      msg.should.not.have.property 'data'
      msg.should.not.have.property 'info'

      msg.error.should.equal "Bad Request"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should not be able to bind to fields', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"field", q: "/bind/scene/Game.Test.FrameCounter"
    ws.on 'close',  -> done()

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.equal "field"
      msg.should.have.property 'error'
      msg.should.not.have.property 'data'
      msg.should.not.have.property 'info'

      msg.error.should.equal "Not Found"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should not be able to bind to functions', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"func", q: "/bind/scene/Game.Test.RandomDebugMessage"
    ws.on 'close',  -> done()

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.equal "func"
      msg.should.have.property 'error'
      msg.should.not.have.property 'data'
      msg.should.not.have.property 'info'

      msg.error.should.equal "Not Found"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should not be able to bind to not found fields', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"notfound", q: "/bind/scene/Game.Test.NotFound"
    ws.on 'close',  -> done()

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.equal "notfound"
      msg.should.have.property 'error'
      msg.should.not.have.property 'data'
      msg.should.not.have.property 'info'

      msg.error.should.equal "Not Found"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should not be able to bind to not found objects', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"notfound", q: "/bind/scene/NotFound.Test.TickEvent"
    ws.on 'close',  -> done()

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.equal "notfound"
      msg.should.have.property 'error'
      msg.should.not.have.property 'data'
      msg.should.not.have.property 'info'

      msg.error.should.equal "Not Found"

      ws.close()


  #--------------------------------------------------------------------------------
  it 'should be able to bind to multiple events', (done) ->

    this.timeout 1000 * 6

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"multi", q: "/bind/scene/Game/Cube.TestTicker.TickEvent"
    ws.on 'close',  -> done()


    count = 0
    open = 0
    unbind = false

    received = {
      1: false
      2: false
      3: false
      4: false
    }

    ws.on 'message', (data) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.equal "multi"
      msg.should.not.have.property 'error'

      # handle info

      if msg.info?
        if msg.info == "bound"
          ++count
          return ++open
        if msg.info == "unbound"
          --open
          setTimeout (-> ws.close()), 2000 if open==0
          return
        assert false, "unknown info message"

      # handle data

      msg.should.have.property 'data'
      msg.data.should.have.property 'id'
      msg.data.should.have.property 'levelTime'

      id = msg.data.id
      id.should.be.within 1,4

      received[ id ] = true

      if not unbind
        unbind = Object.values( received ).reduce ((a,b) -> a && b), true
        ws.send JSON.stringify id:"tick", q:"/socket.unbind(multi)" if unbind
