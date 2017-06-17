
UniumSocket = require './UniumSocketPromises'
log         = require './log'
options     = require 'optimist'

require( 'chai' ).should()

socket = new UniumSocket()


#--------------------------------------------------------------------------------

argv = options

  .usage "Run tutorial test script"

  .describe 'h',    'show usage'
  .alias    'h',    '?'
  .alias    'h',    'help'

  .describe 'l',    'watch game debug log'
  .alias    'l',    'log'

  .describe 'd',    'debug transport protocol'
  .alias    'd',    'debug'

  .argv

if argv.help?
  options.showHelp()
  process.exit 1


#--------------------------------------------------------------------------------

watchGlobalEvents = ->

  if argv.log?
    socket.bind 'debug', '/events.debug'
    socket.on   'debug', (m) -> log.http JSON.stringify m

  socket.bind 'scene',  '/events.sceneLoaded'


#--------------------------------------------------------------------------------

checkIsTutorial = ->

  log.info "checking tutorial scene"

  socket
    .get "/about"
    .then (d) ->
      d.Product.should.equal "unium"
      d.Unium.should.equal "1.0"
      d.Scene.should.equal "Tutorial"


#--------------------------------------------------------------------------------

reloadScene = ->
  socket.waitForThenGet 'scene', '/utils/scene/Tutorial', 10000

watchPickupEvent = ->
  # need to do this after the scene is reloaded!
  socket.bind 'pickup', '/scene/Game.Tutorial.OnPickupCollected'

#--------------------------------------------------------------------------------

collectPickups = ->

  socket

    # get position of first pickup in the scene

    .get "/q/scene/Game/Pickup[0].Transform.position"

    .then (d) ->

      # exit if  no pickups found

      throw new Error "no more pickups" if not d? or d.length == 0

      # otherwise ...
      #   first wait for a pickup message
      #   then call Move on player with position of pickup
        
      pos = JSON.stringify d[0]
      log.info "collect pickup at #{pos}"
      
      socket.waitForThenGet 'pickup', "/q/scene/Game/Player.Player.MoveTo(#{pos})", 10000



    # when we get the pickup message, then repeat (until none left)

    .then collectPickups


#--------------------------------------------------------------------------------

if argv.debug?
  socket.debug = (d,msg) -> log.info d, msg

socket
  .open 'ws://localhost:8342/ws'
  .then watchGlobalEvents
  .then checkIsTutorial
  .then reloadScene
  .then watchPickupEvent
  .then collectPickups
  .catch (e) -> log.error e; socket.close()
 
