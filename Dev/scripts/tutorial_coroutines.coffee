#--------------------------------------------------------------------------------

UniumSocket = require './UniumSocketCoroutines'
log         = require './log'
options     = require 'optimist'
chalk       = require 'chalk'

require( 'chai' ).should()

socket = new UniumSocket()


#--------------------------------------------------------------------------------
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

  .argv

if argv.help?
  options.showHelp()
  process.exit 1

if argv.debug?
  socket.debug = (d,msg) -> log.info d, msg


#--------------------------------------------------------------------------------

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
    yield from socket.getAndWait itr, get:"/q/scene/Game/Player.Player.MoveTo(#{pos})", waitFor:'pickup', timeout:10000


#--------------------------------------------------------------------------------

start = (itr) ->

  # try

    # connect to game

    yield from socket.open itr, 'ws://localhost:8342/ws'
    log.info "Connected to " + chalk.cyan socket.ws.url

    # check this is the right scene

    log.info "checking tutorial scene"
    about = yield from socket.get itr, "/about"
    about.Product.should.equal "unium"
    about.Unium.should.equal "1.0"
    about.Scene.should.equal "Tutorial"

    # bind to global events

    if argv.log?
      yield from socket.bind itr, map:'/events.debug', to:'debug'
      socket.on 'debug', (m) -> log.http JSON.stringify m

    yield from socket.bind itr, map:'/events.sceneLoaded', to:'scene'
    

    # reload scene

    yield from socket.getAndWait itr, get:'/utils/scene/Tutorial', waitFor:'scene', timeout:10000

    yield from collectPickups itr


    socket.close()

  # catch e
  #   log.error e

  # process.exit 0


runCoroutine start  

