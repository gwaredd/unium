#-------------------------------------------------------------------------------
# emulate game

cors = (req, res, next) ->
  res.header 'Access-Control-Allow-Origin', '*'
  res.header 'Access-Control-Allow-Methods', 'GET,POST,DELETE'
  res.header 'Access-Control-Allow-Headers', 'Content-Type'

  next()


#-------------------------------------------------------------------------------

fileSave = (req, res) ->
  res.send "hello world"


#-------------------------------------------------------------------------------
# quick and dirty test server

express = require 'express'
morgan  = require 'morgan'

app = express()

app.use cors
app.use morgan "tiny"
app.use express.static "./test"

app.post '/file/persistent', fileSave

app.listen 8342, -> console.log "listening on 8342"

