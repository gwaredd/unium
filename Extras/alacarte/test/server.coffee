#-------------------------------------------------------------------------------

path    = require 'path'
fs      = require 'fs'
express = require 'express'
morgan  = require 'morgan'


#-------------------------------------------------------------------------------


# download body of request (express doesn't buffer by default)

body = (req, res, next) ->

  data = ""
  req.setEncoding 'utf8'
  req.on 'data', (chunk) -> data += chunk
  req.on 'end', -> req.body = data; next()


# add cors headers

cors = (req, res, next) ->
  res.header 'Access-Control-Allow-Origin', '*'
  res.header 'Access-Control-Allow-Methods', 'GET,POST,DELETE'
  res.header 'Access-Control-Allow-Headers', 'Content-Type'

  next()


#-------------------------------------------------------------------------------

# save file

fileSave = (req, res) ->

  filename = path.join './test', req.url.substr 1
  fs.writeFileSync filename, req.body


  res.status( 200 ).send "ok"


#-------------------------------------------------------------------------------
# quick and dirty test server

app = express()

app.use cors
app.use morgan "tiny"
app.use body
app.post '/file/*', fileSave
app.use express.static "./test"


app.listen 8342, -> console.log "listening on 8342"

