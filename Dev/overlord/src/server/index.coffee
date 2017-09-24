#--------------------------------------------------------------------------------

express   = require 'express'
http      = require 'http'
morgan    = require 'morgan'
WebSocket = require 'ws'
_         = require 'underscore'

config    = require './config'
Minion    = require './minion'

#--------------------------------------------------------------------------------

print = (req, type, msg) ->
  ip = if req? then req.headers['x-forwarded-for'] || req.connection.remoteAddress else "-"
  timestamp = new Date().toISOString()
  console.log "[#{timestamp}] #{ip} [#{type}] #{msg}"

log   = (msg, req) -> print req, "INFO", msg
warn  = (msg, req) -> print req, "WARNING", msg
error = (msg, req) -> print req, "ERROR", msg


#--------------------------------------------------------------------------------

app     = express()
server  = http.createServer app

app.use morgan '[:date[iso]] :remote-addr :status :method :url'
app.use express.static './root'


#--------------------------------------------------------------------------------

minions   = []
clients   = []

onClient = (msg) ->
  switch msg.type
    when "list"
      console.log "list"
    else
      warn "Unknown overlord message type '#{msg.type}'"


#--------------------------------------------------------------------------------

onMinion = (msg) ->
  warn "TODO: minion api"
  this.close()


#--------------------------------------------------------------------------------
# handle web socket connections

wss = new WebSocket.Server { server }

wss.on 'connection', (socket, req) ->

  if req.url == "/overlord"
  
    log "Accepted websocket request for #{req.url}", req

    socket.isAlive = true
    socket.on 'pong',     -> this.isAlive = true
    socket.on 'close',    -> xs = this; clients = _.reject clients, (c) -> c == xs
    socket.on 'message',  onClient

    clients.push socket

  else if req.url == "/minion"

    socket.isAlive = true
    socket.on 'pong',     -> this.isAlive = true
    socket.on 'close',    -> xs = this; minions = _.reject minions, (m) -> m.socket == xs
    socket.on 'message',  onMinion

    minions.push new Minion socket

  else

    error "Rejected websocket request for #{req.url}", req
    socket.close()


#--------------------------------------------------------------------------------

server.listen config.port, ->
  addr = server.address()
  log "Listening on #{addr.address}:#{addr.port}"

