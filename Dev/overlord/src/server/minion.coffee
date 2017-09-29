#--------------------------------------------------------------------------------

logger = require './log'
nextID = 1

module.exports = (socket,ip) ->

  @id     = nextID++
  @ip     = ip
  @socket = socket

  @about  = null
  @scene  = ""

  @onClose = -> logger.log @ip, "Minion##{@id} disconnected"
  @onScene = (data) -> @scene = data

  @onAbout = (data) ->
    @about = data
    @scene = data.Scene

    if data.Product? and data.Version? and data.Platform?
      logger.log @ip, "Minion##{@id} running #{data.Product} v#{data.Version}, #{data.Platform}"
    else
      @socket.close()


  @getInfo = ->
    id      : @id
    ip      : @ip
    product : if @about? then @about.Product else "Unknown"
    version : if @about? then @about.Version else "Unknown"
    editor  : if @about? and @about.IsEditor then true else false
    scene   : @scene


  logger.log ip, "Minion##{@id} connected"

  return this

