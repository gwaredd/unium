#--------------------------------------------------------------------------------

express     = require 'express'
http        = require 'http'
morgan      = require 'morgan'
WebSocket   = require 'ws'
options     = require 'optimist'
bodyParser  = require 'body-parser'
_           = require 'underscore'


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

logger.colours = !argv.nc?


#--------------------------------------------------------------------------------

minions   = []
clients   = []

broadcast = (id, data) ->
  msg = JSON.stringify id:id, data:data
  _.each clients, (client) -> client.send msg



#--------------------------------------------------------------------------------
# client commands

onClient = (msg) ->

  try

    msg = JSON.parse msg

    switch msg.id
      when "list"
        @send JSON.stringify id:'list', data: _.map minions, (socket) -> socket.minion.getInfo()
      else
        throw "Unknown overlord message type '#{msg.id}'"

  catch e

    @overlord.error e


#--------------------------------------------------------------------------------
# minion commands

onMinion = (msg) ->

  try

    msg = JSON.parse msg

    switch msg.id
      when "about"
        @minion.onAbout msg.data
      when "scene"
        @minion.onScene msg.data
      else
        throw "Unknown overlord message type '#{msg.id}'"

    broadcast 'update', this.minion.getInfo()

  catch e
  
    @minion.error e


#--------------------------------------------------------------------------------

app     = express()
server  = http.createServer app

app.use morgan '[:date[iso]] :remote-addr :status :method :url'
app.use bodyParser.raw type: '*/*'
app.use express.static config.root


app.post '/minion', (req, res) ->
  logger.log null, "Minion Registered: " + req.body.toString 'utf8'
  res.sendStatus 200


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
      broadcast 'remove', this.minion.getInfo()
      xs = this
      minions = _.reject minions, (sock) -> sock == xs

    socket.on 'message', onMinion
    minions.push socket

    broadcast 'add', socket.minion.getInfo()

  else

    error "Rejected websocket request for #{req.url} from #{ip}", req
    socket.close()


#--------------------------------------------------------------------------------

server.listen config.port, ->
  addr = server.address()
  logger.ip = addr.address
  logger.log null, "Listening on #{addr.address}:#{addr.port}"

