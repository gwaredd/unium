#--------------------------------------------------------------------------------

express   = require 'express'
http      = require 'http'
morgan    = require 'morgan'
WebSocket = require 'ws'

config    = require './config'


#--------------------------------------------------------------------------------

app = express()
srv = http.createServer app


app.use morgan '[:date[iso]] :remote-addr :status :method :url'
app.use express.static './root'


#--------------------------------------------------------------------------------
# web sockets

wss = new WebSocket.Server { server }

wss.on 'connection', (ws, req) ->
  ip = req.connection.remoteAddress

  # const location = url.parse(req.url, true);
  # // You might use location.query.access_token to authenticate or share sessions
  # // or req.headers.cookie (see http://stackoverflow.com/a/16395220/151312)

  # ws.on('message', function incoming(message) {
  #   console.log('received: %s', message);
  # });

  # ws.send('something');


# const wss = new WebSocket.Server({ port: 8080 });


#--------------------------------------------------------------------------------
# api

app.get '/overlord', (req, res) ->
   res.send "overload api"

app.get '/minion', (req, res) ->
   res.send "minion api"


#--------------------------------------------------------------------------------

srv.listen config.port, ->
  addr = srv.address()
  console.log "Listening on #{addr.address}:#{addr.port}"

