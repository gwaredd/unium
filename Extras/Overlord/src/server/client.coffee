#--------------------------------------------------------------------------------

logger = require './log'
nextID = 1

module.exports = (socket,ip) ->

  @id     = nextID++
  @ip     = ip
  @socket = socket

  @log    = (m) -> logger.log @ip, "Client##{@id} #{m}"
  @error  = (m) -> logger.error @ip, "Client##{@id} #{m}"

  @onClose = -> @log "disconnected"

  this.log "connected"

  return this

