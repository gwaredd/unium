#--------------------------------------------------------------------------------

config    = require './config'
assert    = require( 'chai' ).assert
WebSocket = require 'ws'

describe 'Sockets Sample Parameters', ->

  #--------------------------------------------------------------------------------
  it 'should be able to set frequency', (done) ->

    this.timeout 1000 * 2

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"fps", q: "/q/stats.FPS", repeat: freq:0
    ws.on 'close',  -> done()

    count = 0

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.is.equal 'fps'
      msg.should.not.have.property 'error'

      if count==0
        msg.should.have.property( 'info' ).and.is.equal 'repeating'
      else
        msg.should.have.property 'data'

      ws.close() if ++count > 30


  #--------------------------------------------------------------------------------
  it 'should be able to set number of samples', (done) ->

    this.timeout 1000 * 3

    count = 0

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"fps", q: "/q/stats.FPS", repeat: freq:0, samples:10
    ws.on 'close',  -> count.should.equal 10; done()

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.is.equal 'fps'
      msg.should.not.have.property 'error'

      if msg.info?
        if msg.info == 'finished'
          setTimeout ( -> ws.close() ), 2000 
        else
          msg.info.should.equal 'repeating'
      else
        msg.should.have.property 'data'
        count++


  #--------------------------------------------------------------------------------
  it 'should be able to skip samples', (done) ->

    this.timeout 1000 * 3

    count = 0

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"reset", q: "/q/scene/Game.Test.FrameCounter=0"
    ws.on 'close',  -> done()

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property 'id'
      msg.should.not.have.property 'error'

      if msg.id == 'reset'

        msg.should.have.property 'data'
        msg.data.should.eql [0]

        ws.send JSON.stringify id:'fc', q: '/q/scene/Game.Test.FrameCounter', repeat: freq:0, skip:10, samples:10

      else if msg.id == 'fc'

        if msg.info?
          ws.close() if msg.info == 'finished'
        else
          msg.should.have.property 'data'
          msg.data.should.be.an 'array'
          msg.data.length.should.equal 1
          msg.data[0].should.be.above( 10 ).and.below( 30 )

      else

        assert false, "unknown message"

  #--------------------------------------------------------------------------------
  it 'caching should be disabled by default', (done) ->

    this.timeout 1000 * 3

    # NB: hack using fact that unity vectors are replaced!

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify q: "/q/scene/Game/Sphere.Transform.position.y", repeat: freq:0.1
    ws.on 'close',  -> done()

    count = 0
    prev = 0.0

    ws.on 'message', (data) ->

      msg = JSON.parse data

      # info message

      if msg.info?
        count.should.equal 0
        ++count
        return

      msg.should.have.property 'data'
      msg.data.should.be.an 'array'
      msg.data.length.should.equal 1
      
      # NB: hack using fact that unity vectors are replaced!
      prev.should.not.equal msg.data[0] if count > 1
      prev = msg.data[0]

      ws.close() if count++ > 10

  #--------------------------------------------------------------------------------
  it 'should be able to cache objects', (done) ->

    this.timeout 1000 * 3

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify q: "/q/scene/Game/Sphere.Transform.position.y", repeat: freq:0.1, cache: true
    ws.on 'close',  -> done()

    count = 0
    prev = 0.0

    ws.on 'message', (data) ->

      msg = JSON.parse data

      # info message

      if msg.info?
        count.should.equal 0
        ++count
        return

      msg.should.have.property 'data'
      msg.data.should.be.an 'array'
      msg.data.length.should.equal 1
      
      # NB: hack using fact that unity vectors are replaced!
      prev.should.equal msg.data[0] if count > 1
      prev = msg.data[0]

      ws.close() if count++ > 10
