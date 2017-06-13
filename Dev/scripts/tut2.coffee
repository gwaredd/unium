
UniumSocket = require './unium'
log         = require './log'

require( 'chai' ).should()

socket = new UniumSocket()

#--------------------------------------------------------------------------------

bindEvents = ->

  socket.bind 'debug', '/events.debug'
  socket.on   'debug', (m) -> log.http JSON.stringify m

  socket.bind 'pickups', '/scene/Game.Tutorial.OnPickupCollected'


#--------------------------------------------------------------------------------

checkIsTutorial = ->

  log.info "checking tutorial scene"

  socket
    .get "/about"
    .then (d) ->
      d.Product.should.equal "unium"
      d.Unium.should.equal "1.0"
      d.Scene.should.equal "Tutorial"
      log.info "scene ok"


#--------------------------------------------------------------------------------

collectPickups = ->

  socket
    .get "/q/scene/Game/Pickup[0].Transform.position"
    .then (d) ->

      if not d? or d.length == 0
        log.info "no more pickups"
        socket.close()
        return
        
      pos = JSON.stringify d[0]
      log.info "collect pickup at #{pos}"
      socket
        .get "/q/scene/Game/Player.Player.MoveTo(#{pos})"

        .then ->
          socket
            .waitFor 'pickup', 10000
            .then collectPickups


#--------------------------------------------------------------------------------

socket.debug = (d,msg) -> log.info d, msg

socket
  .open 'ws://localhost:8342/ws'
  .then bindEvents
  .then checkIsTutorial
  .then collectPickups
  .catch (e) -> log.error e; g.close()
 
