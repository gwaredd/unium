
nextID = 1

module.exports = (socket) ->

  @id     = nextID++
  @socket = socket

  return this

