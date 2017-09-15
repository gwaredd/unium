#--------------------------------------------------------------------------------

express = require 'express'
morgan  = require 'morgan'
config  = require './config'


#--------------------------------------------------------------------------------

app = express()

app.use morgan 'dev'
app.use express.static './root'


#--------------------------------------------------------------------------------

app.get '/api', (req, res) ->
   res.send "put goes here?"


#--------------------------------------------------------------------------------

server = app.listen config.port, ->
  address = server.address()
  console.log "Listening on #{ address.address }:#{ address.port }"

