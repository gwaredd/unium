
UniumSocket = require './UniumSocket'
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

  .describe 'd',    'watch game debug output'
  .alias    'd',    'debug'

  .describe 'm',    'enable message debugging'
  .alias    'm',    'message'

  .argv

if argv.help?
  options.showHelp()
  process.exit 1




#--------------------------------------------------------------------------------

bindEvents = ->

  if argv.debug?
    socket.bind 'debug', '/events.debug'
    socket.on   'debug', (m) -> log.http JSON.stringify m

  socket.bind 'pickup', '/scene/Game.Tutorial.OnPickupCollected'


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
  # TODO: reloadScene


#--------------------------------------------------------------------------------

collectPickups = ->

  socket
    .get "/q/scene/Game/Pickup[0].Transform.position"
    .then (d) ->

      throw new Error "no more pickups" if not d? or d.length == 0
        
      pos = JSON.stringify d[0]
      log.info "collect pickup at #{pos}"

      socket.get "/q/scene/Game/Player.Player.MoveTo(#{pos})"

    .then (d) -> socket.waitFor 'pickup', 10000
    .then collectPickups


#--------------------------------------------------------------------------------

if argv.verbose?
  socket.debug = (d,msg) -> log.info d, msg

socket
  .open 'ws://localhost:8342/ws'
  .then checkIsTutorial
  .then bindEvents
  .then collectPickups
  .catch (e) -> log.error e; socket.close()
 
