#--------------------------------------------------------------------------------

logger = require './log'
nextID = 1

module.exports = (socket,ip) ->

  @id     = nextID++
  @ip     = ip
  @socket = socket

  @about  = null
  @scene  = ""

  @log    = (m) -> logger.log @ip, "Minion##{@id} #{m}"
  @error  = (m) -> logger.error @ip, "Minion##{@id} #{m}"

  @onClose = -> this.log "disconnected"
  @onScene = (data) -> @scene = data

  @onAbout = (data) ->
    @about = data
    @scene = data.Scene

    if data.Product? and data.Version? and data.Platform?
      @log "running #{data.Product} v#{data.Version} on #{data.Platform}"
    else
      @socket.close()


  @getInfo = ->
    id      : @id
    ip      : if @about? then "#{@about.IPAddress}:#{@about.Port}" else @ip
    product : if @about? then @about.Product else "Unknown"
    version : if @about? then @about.Version else "Unknown"
    editor  : if @about? and @about.IsEditor then true else false
    platform: if @about? then @about.Platform else "Unknown"
    scene   : @scene


  this.log "connected"

  return this

