#--------------------------------------------------------------------------------

log       = require 'npmlog'
util      = require 'util'
moment    = require 'moment'
WebSocket = require 'ws'


#--------------------------------------------------------------------------------

# override log write to augment with dynamic text
log.__write = log.write
log.write = (msg, style) -> this.__write ( if util.isFunction msg then msg() else msg ), style

log.prefixStyle = fg: 'white'
log.heading = -> "[" + moment().format( "YYYY-MM-DD HH:MM:SS" ) + "]"

log.style.warn  = fg: 'yellow', bg: 'black'
log.disp.info   = ''
log.disp.warn   = ' [WARNING]'
log.disp.error  = ' [ERROR]'

#--------------------------------------------------------------------------------

ws = new WebSocket 'ws://localhost:8342/ws'

ws.on 'open', -> ws.send JSON.stringify id: "dbg", q: "/bind/scene/Game.Test.TickEvent"
ws.on 'error', (err) -> log.error err

ws.on 'message', (msg) ->

  msg = JSON.parse msg

  if msg.data?

    log.info JSON.stringify msg.data

  else if msg.info?

    if msg.info.startsWith 'bind'
      d = msg.info.split ' '
      log.info "bound to #{d[1]} event"

  else if msg.error?
    log.error msg.error
    ws.close()

