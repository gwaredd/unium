#--------------------------------------------------------------------------------

config    = require './config'
assert    = require( 'chai' ).assert
WebSocket = require 'ws'

describe 'Sockets Sample Variables', ->

  #--------------------------------------------------------------------------------
  it 'should be able to sample variables', (done) ->

    this.timeout 1000 * 10

    pass = false

    # repeat sphere position

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"repeat", q: "/q/scene/Game/Sphere.Transform.position", repeat:{}
    ws.on 'close',  -> assert pass, "Failed to complete all messages sucessfully"; done()

    expecting = "repeat"
    count     = 0
    prev      = 0

    ws.on 'message', (data) ->

      msg = JSON.parse data

      switch expecting

        when "repeat"
          msg.should.have.property( 'id' ).and.is.equal 'repeat'
          msg.should.have.property( 'info' ).and.is.equal 'repeating'
          expecting = "data"

        when "data"

          count++

          msg.should.have.property( 'id' ).and.is.equal 'repeat'

          msg.should.not.have.property 'message'
          msg.should.have.property 'data'
          msg.data.should.be.an 'array'
          msg.data.should.have.length 1
          msg.data[0].should.have.property 'x'
          msg.data[0].should.have.property 'y'
          msg.data[0].should.have.property 'z'

          if count == 1
            prev = msg.data[0].y
          else
            assert msg.data[0].y != prev, "y position did not change"


          if count==5
            ws.send JSON.stringify id:"repeat", q: "/socket.stop(repeat)"
            expecting = "close"


        when "close"

          msg.should.have.property( 'id' ).and.is.equal 'repeat'
          msg.should.have.property( 'info' ).and.is.equal 'stopped'

          expecting = "none"
          pass = true

          setTimeout ( -> ws.close() ), 2000

        else
          assert false, "unexpected message"


  #--------------------------------------------------------------------------------
  it 'should gracefully handle sampling not found variables', (done) ->

    this.timeout 1000 * 3
    pass = false

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"notfound", q: "/q/stats.NotFound", repeat: {}
    ws.on 'close',  -> assert pass, "No message returned"; done()

    expect = "repeating"

    ws.on 'message', (data) ->

      msg = JSON.parse data

      msg.should.have.property( 'id' ).and.is.equal 'notfound'
      msg.should.have.property( 'info' ).and.is.equal expect
      msg.should.not.have.property 'data'
      msg.should.not.have.property 'error'

      if expect == "sampling"
        expect = "stopped"
      else
        pass = true
        ws.close()


  #--------------------------------------------------------------------------------
  it 'should be able to sample multiple variables', (done) ->

    this.timeout 1000 * 5

    count = 0

    ws  = new WebSocket config.sockets
    ws.on 'open',   -> ws.send JSON.stringify id:"multiple", q: "/q/scene/Game/*.TestBehaviour.SomeID", repeat: {}
    ws.on 'close',  -> assert count==3, "Does not have 3 messages"; done()

    expect = "sampling"

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.is.equal 'multiple'
      msg.should.not.have.property 'error'

      if expect == "sampling"
        msg.should.have.property( 'info' ).and.is.equal 'repeating'
        expect = "data"
      else
        msg.should.have.property 'data'
        msg.data.should.be.an "array"
        msg.data.should.have.length 5
     
      ws.close() if ++count == 3


  #--------------------------------------------------------------------------------
  it 'should be able to sample functions', (done) ->

    this.timeout 1000 * 5

    count = 0

    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify id:"fun", q: "/q/scene/Game/Cube.TestBehaviour.CallThisFunction()", repeat: {}
    ws.on 'close',  -> assert count==3, "Does not have 3 messages"; done()

    expect = "sampling"

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.is.equal 'fun'
      msg.should.not.have.property 'error'

      if expect == "sampling"
        msg.should.have.property( 'info' ).and.is.equal 'repeating'
        expect = "data"
      else
        msg.should.have.property 'data'
        msg.data.should.eql [1,2,3,4]

      ws.close() if ++count == 3


  #--------------------------------------------------------------------------------
  it 'should be able to sample setting values', (done) ->

    this.timeout 1000 * 5

    count = 0

    ws  = new WebSocket config.sockets

    ws.on 'open',   -> ws.send JSON.stringify id:"fun", q: "/q/scene/Game/Cube.TestBehaviour.RandomValue=1000", repeat: {}
    ws.on 'close',  -> assert count==3, "Does not have 3 messages"; done()

    expect = "sampling"

    ws.on 'message', (data) ->

      msg = JSON.parse data
      msg.should.have.property( 'id' ).and.is.equal 'fun'
      msg.should.not.have.property 'error'

      if expect == "sampling"
        msg.should.have.property( 'info' ).and.is.equal 'repeating'
        expect = "data"
      else
        msg.should.have.property 'data'
        msg.data.should.eql [ 1000, 1000, 1000, 1000 ]

      ws.close() if ++count == 3

