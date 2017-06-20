#================================================================================

_         = require 'underscore'
WebSocket = require 'ws'


#================================================================================
# functions (or priomises) attached to messages with an optional timeout

MessageHandlers = ->

  handlers  = {}
  nextID    = 0


  #--------------------------------------------------------------------------------
  # cancel a handler given a message id and it's unique id

  _cancel = ( mid, uid ) ->

    return unless mid of handlers

    handlers[ mid ] = _.reject handlers[ mid ], (h) ->

      return false if h.uid != uid

      clearTimeout h.timeout if h.timeout?
      h.callback.throw "timed out" if h.callback.throw?

      return true


  #--------------------------------------------------------------------------------
  # add a handler for a given message id

  @add = ( mid, callback, timer ) ->

    uid = ++nextID # unique id
    cancel  = -> _cancel mid, uid
    timeout = if not timer? then null else setTimeout cancel, timer

    handlers[ mid ] = [] unless mid of handlers
    handlers[ mid ].push uid:uid, callback:callback, timeout:timeout
  
    return cancel


  #--------------------------------------------------------------------------------
  # remove all handlers, cancel any timeouts and throw errors

  @removeAll = ->

    console.log handlers

    _.mapObject handlers, (list) -> _.each list, (h) ->
        clearTimeout h.timeout if h.timeout?
        h.callback.throw "removing handler" if h.callback.throw?

    handlers = {}

  #--------------------------------------------------------------------------------
  # pass message contents along to al

  @onMessage = (msg) ->

    return unless msg.id? and msg.id of handlers

    coroutines  = _.filter handlers[ msg.id ], (h) -> h.callback.next?
    callbacks   = _.filter handlers[ msg.id ], (h) -> not h.callback.next?

    if callbacks.length > 0
      _.each callbacks, (callback) -> callback msg
      handlers[ msg.id ] = callbacks
    else
      delete handlers[ msg.id ] 

    _.each coroutines, (h) ->
      clearTimeout h.timeout if h.timeout?
      if msg.error? then  h.callback.throw  msg.error 
      if msg.data?  then  h.callback.next   msg.data
      if msg.info?  then  h.callback.next   info: msg.info
    

  return


#================================================================================
# convenient interface for unium WebSockets

UniumSocket = ->

  @ws     = null
  @debug  = null

  #--------------------------------------------------------------------------------

  nextID      = 0
  handlers    = new MessageHandlers()

  onMessage = (msg) ->
    @debug "recv", msg if @debug?
    handlers.onMessage JSON.parse msg


  #--------------------------------------------------------------------------------

  @open = (itr, url) ->
    sock = this
    @ws = new WebSocket url
    @ws.on 'open',    itr.next
    @ws.on 'error',   itr.throw
    @ws.on 'message', (m) -> onMessage.call sock, m
    yield "connecting"

  @close = ->
    handlers.removeAll()
    @ws.close()

  @send = (msg) ->
    @debug "send", msg if @debug?
    @ws.send msg


  #--------------------------------------------------------------------------------
  # do a thing


  # send a message and wait for the response
  # itr, url, as:id, timeout:1000

  @get = ( itr, url, cfg )->
    id      = if cfg? and cfg.as? then cfg.as else "m" + ++nextID
    timeout = if cfg? then cfg.timeout else null

    handlers.add id, itr, timeout
    @send JSON.stringify id:id, q:url
    yield


  # set up listener for event first before invoking get
  # itr, waitFor:event, timeout:1000, get:url

  @getAndWait = ( itr, cfg )->

    timeout = if cfg? then cfg.timeout else null
    handlers.add cfg.waitFor, itr, timeout

    id = "m" + ++nextID
    @send JSON.stringify id:id, q:cfg.get
    
    yield


  #--------------------------------------------------------------------------------
  # watch a thing

  # bind and unbind to game events
  # itr, map:url to:id

  @bind = (itr, cfg) ->
    url = cfg.map
    url = url.subsstring 2 if url.startsWith "/q/" 
    url = url.subsstring 5 if url.startsWith "/bind/"
    yield from @get itr, "/bind#{url}", as:cfg.to

  @unbind = (itr, id) ->
    yield from @get itr, "/socket.unbind(#{id})", as:id


  #--------------------------------------------------------------------------------
  # wait for a thing


  # whenever message of type 'id' is received, call the callback function
  # returns a cancel function

  @on = (id, callback) ->
    handlers.add id, callback


  # wait until the next message of type 'id' and then continue

  @waitFor = (itr, id, timer) ->
    handlers.add id, itr, timer
    yield

  return

# #================================================================================

module.exports = UniumSocket
