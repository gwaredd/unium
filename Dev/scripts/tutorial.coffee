#--------------------------------------------------------------------------------

_         = require 'underscore'
chalk     = require 'chalk'
q         = require 'q'
WebSocket = require 'ws'
options   = require 'optimist'
log       = require './log'

require( 'chai' ).should()

#--------------------------------------------------------------------------------

argv = options

  .usage "Run tutorial test script"

  .describe 'h',    'show usage'
  .alias    'h',    '?'
  .alias    'h',    'help'

  .describe 'd',    'enable debug output'
  .alias    'd',    'debug'

  .argv

if argv.help?
  options.showHelp()
  process.exit 1


#--------------------------------------------------------------------------------

Game = ->

  nextID      = 0
  ws          = null

  watches     = {}
  msgPromise  = {}

  @onMessage = (msg) ->

    log.info "recv: #{msg}" if argv.debug?

    msg = JSON.parse msg

    return unless msg.id?

    if msg.id of watches
      return log.error  "#{msg.id} - #{msg.error}" if msg.error?
      return log.notice "#{msg.id} - #{msg.info}" if msg.info?
      watches[ msg.id ].resolve msg.data

    if msg.id of msgPromise
      d = msgPromise[ msg.id ]
      delete msgPromise[ msg.id ]
      
      return d.resolve msg.data if msg.data?
      return d.reject msg.error if msg.error?


  #--------------------------------------------------------------------------------

  @get = (url) ->
    d = q.defer()
    mid = "m" + ++nextID
    msgPromise[ mid ] = d
    msg = JSON.stringify id: mid, q: url
    log.info "send: #{msg}" if argv.debug?
    ws.send msg
    d.promise

  @watch = (id, url) ->
    watches[ id ] = []
    msg = JSON.stringify id: id, q: url
    log.info "send: #{msg}" if argv.debug?
    ws.send msg

  @waitFor = (id, timeout) ->
    d = q.defer()
    watches[ id ] = d
    setTimeout ( -> d.reject 'timed out' ), timeout
    d.promise

  @close = ->
    log.info "closing"
    ws.close()


  @connect  = (url) ->
    log.info "connecting to " + chalk.cyan url

    d = q.defer()
    ws = new WebSocket url
    ws.on 'open',    -> log.info "connected to game"; d.resolve()
    ws.on 'error',   (e) -> d.reject e
    ws.on 'message', @onMessage
    d.promise

  return this


#--------------------------------------------------------------------------------

checkIsTutorial = (g) ->

  log.info "checking tutorial scene"

  g
    .get "/about"
    .then (d) ->
      d.Product.should.equal "unium"
      d.Unium.should.equal "1.0"
      d.Scene.should.equal "Tutorial"

#--------------------------------------------------------------------------------

collectNextPickup = (g) ->

  g
    .get "/q/scene/Game/Pickup[0].Transform.position"
    .then (d) ->

      if not d? or d.length == 0
        log.info "no more pickups"
        g.close()
        return
        
      pos = JSON.stringify d[0]
      log.info "collect pickup at #{pos}"
      g
        .get "/q/scene/Game/Player.Player.MoveTo(#{pos})"
        .then -> g.waitFor 'pickup', 10000
        .then -> collectNextPickup g


#--------------------------------------------------------------------------------

watchPickups = (g) ->
  g.watch "pickup", "/bind/scene/Game.Tutorial.OnPickupCollected"


#--------------------------------------------------------------------------------

g = new Game()

g
  .connect 'ws://localhost:8342/ws'
  .then -> checkIsTutorial g
  .then -> watchPickups g
  .then -> collectNextPickup g
  .catch (e) -> log.error e; g.close()
 
