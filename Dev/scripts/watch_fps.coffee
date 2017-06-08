WebSocket = require 'ws'

ws = new WebSocket 'ws://localhost:8342/ws'

ws.on 'open', ->
  ws.send JSON.stringify {
    q       : "/q/stats.FPS"
    repeat  :
      freq  : 0.25
  }

ws.on 'message', (msg) ->
  msg = JSON.parse msg
  console.log msg.data[0] if msg.data?

