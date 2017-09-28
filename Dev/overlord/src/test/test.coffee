

WebSocket = require 'ws'

ws = new WebSocket 'ws://localhost:8080/minion'

ws.on 'open', ->
  ws.send 'something'

ws.on 'message', (data) ->
  console.log data

# perfectSquares = ->
#   num = 0
#   loop
#     num += 1
#     yield num * num
#   return

# window.ps or= perfectSquares()
