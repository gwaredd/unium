#--------------------------------------------------------------------------------

log       = require './log'
WebSocket = require 'ws'


#--------------------------------------------------------------------------------

ws = new WebSocket 'ws://localhost:8342/ws'

ws.on 'open', -> ws.send JSON.stringify id: "dbg", q: "/bind/events.debug"
ws.on 'error', (err) -> log.error err

ws.on 'message', (msg) ->

  msg = JSON.parse msg

  if msg.data?

    switch msg.data.type
      when 'Log' then log.info msg.data.message
      when 'Warning' then log.warn msg.data.message
      when 'Error' then log.error msg.data.message

  else if msg.info?

    if msg.info.startsWith 'bind'
      d = msg.info.split ' '
      log.info "bound to #{d[1]} event"

  else if msg.error?
    log.error msg.error
    ws.close()

