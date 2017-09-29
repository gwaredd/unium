#--------------------------------------------------------------------------------

express   = require 'express'
http      = require 'http'
morgan    = require 'morgan'
WebSocket = require 'ws'
options   = require 'optimist'
_         = require 'underscore'


config    = require './config'
logger    = require './log'

Minion    = require './minion'
Client    = require './client'

#--------------------------------------------------------------------------------

argv = options

  .usage    "Unium Overlord Server"

  .describe 'h',    'show usage'
  .alias    'h',    '?'
  .alias    'h',    'help'

  .describe 'nc',   'turn colour outupt off'
  .alias    'nc',   'no-colours'

  .argv

if argv.help?
  options.showHelp()
  process.exit 0


#--------------------------------------------------------------------------------

minions   = []
clients   = []

logger.colours = !argv.nc?

broadcast = (msg) ->
  msg = JSON.stringify msg
  _.each clients, (client) -> client.send msg



#--------------------------------------------------------------------------------
# client commands

onClient = (msg) ->

  try

    msg = JSON.parse msg

    if msg.id == 'list'
      this.send JSON.stringify id:'list', data: _.map minions, (m) -> m.getInfo()
    else
      this.overlord.error "Unknown overlord message type '#{msg.id}'"

  catch e
    this.overlord.error e


#--------------------------------------------------------------------------------
# minion commands

onMinion = (msg) ->

  try

    msg = JSON.parse msg

    switch msg.id
      when "about"
        this.minion.onAbout msg.data
      when "scene"
        this.minion.onScene msg.data
      else
        throw "Unknown overlord message type '#{msg.id}'"

    broadcast id:'update', data: this.minion.getInfo()

  catch e
    error "" + e


#--------------------------------------------------------------------------------

app     = express()
server  = http.createServer app

app.use morgan '[:date[iso]] :remote-addr :status :method :url'
app.use express.static config.root



#--------------------------------------------------------------------------------
# handle web socket connections

wss = new WebSocket.Server { server }

wss.on 'connection', (socket, req) ->

  ip = req.headers['x-forwarded-for'] || req.connection.remoteAddress


  if req.url == "/overlord"

    socket.overlord = new Client socket, ip

    socket.on 'close', ->
      this.overlord.onClose()
      xs = this
      clients = _.reject clients, (sock) -> sock == xs

    socket.on 'message', onClient
    clients.push socket
  

  else if req.url == "/minion"

    socket.minion = new Minion socket, ip

    socket.on 'close', ->
      this.minion.onClose()
      broadcast id:'remove', data: this.minion.getInfo()
      xs = this
      minions = _.reject minions, (sock) -> sock == xs

    socket.on 'message', onMinion
    minions.push socket

    broadcast id:'add', data: socket.minion.getInfo()

  else

    error "Rejected websocket request for #{req.url} from #{ip}", req
    socket.close()


#--------------------------------------------------------------------------------

server.listen config.port, ->
  addr = server.address()
  logger.ip = addr.address
  logger.log null, "Listening on #{addr.address}:#{addr.port}"

