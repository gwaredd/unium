#--------------------------------------------------------------------------------

UniumSocket = require './UniumSocketCoroutines'
log         = require './log'
options     = require 'optimist'
chalk       = require 'chalk'

require( 'chai' ).should()


# command line options

argv = options

  .usage "Run tutorial test script"

  .describe 'h',    'show usage'
  .alias    'h',    '?'
  .alias    'h',    'help'

  .describe 'l',    'watch game debug log'
  .alias    'l',    'log'

  .describe 'd',    'debug transport protocol'
  .alias    'd',    'debug'

  .describe 'ip',   'specificy the ip address'

  .argv

if argv.help?
  options.showHelp()
  process.exit 1

if argv.debug?
  socket.debug = (d,msg) -> log.info d, msg


#--------------------------------------------------------------------------------

socket = new UniumSocket()


runCoroutine = (fn) ->
  
  proxy =
    next  : (d) -> itr.next d
    throw : (e) -> itr.throw e

  itr = fn proxy
  itr.next()


#--------------------------------------------------------------------------------

collectPickups = (itr) ->

  # register for pickup events

  yield from socket.bind itr, map:'/scene/Game.Tutorial.OnPickupCollected', to:'pickup'

  # collect all the pickups

  while true

    # get position of first pickup in the scene
    
    pos = yield from socket.get itr, "/q/scene/Game/Pickup[0].Transform.position"

    # if none left then exit

    if not pos? or pos.length == 0
      log.info "No more pickups"
      return

    # otherwise, collect pickup

    pos = JSON.stringify pos[0]
    log.info "Move to " + chalk.cyan pos

    yield from socket.getAndWait itr, get:"/q/scene/Game/Player.Player.MoveTo(#{pos})", waitFor:'pickup', timeout:10000


#--------------------------------------------------------------------------------

start = (itr) ->

  url = if argv.ip? then "ws://#{argv.ip}:8342/ws" else 'ws://localhost:8342/ws'

  try

    # connect to game

    yield from socket.open itr, url
    log.info "Connected to " + chalk.cyan socket.ws.url

    # check this is the right scene

    log.info "Checking tutorial scene"
    about = yield from socket.get itr, "/about"
    about.Product.should.equal "unium"
    about.Unium.should.equal "1.0"
    about.Scene.should.equal "Tutorial"

    # bind to global events

    if argv.log?
      yield from socket.bind itr, map:'/events.debug', to:'debug'
      socket.on 'debug', (m) -> log.http JSON.stringify m

    # reload scene

    log.info "Reloading scene"
    yield from socket.bind itr, map:'/events.sceneLoaded', to:'scene'
    yield from socket.getAndWait itr, get:'/utils/scene/Tutorial', waitFor:'scene', timeout:10000

    log.info "Collecting pickups"
    yield from collectPickups itr


    socket.close()

  catch e
    log.error e



runCoroutine start  

