#================================================================================

_         = require 'underscore'
q         = require 'q'
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
      h.callback.reject "timed out" if h.callback.promise?

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
  # remove all handlers, cancel any timeouts and reject promises

  @removeAll = ->

    _.mapObject handlers, (list) -> _.each list, (h) ->
        clearTimeout h.timeout if h.timeout?
        h.callback.reject "removing handler" if h.callback.promise?

    handlers = {}

  #--------------------------------------------------------------------------------
  # pass message contents along to al

  @onMessage = (msg) ->

    return unless msg.id? and msg.id of handlers

    # invoke each handler (removing promises)

    handlers[ msg.id ] = _.filter handlers[ msg.id ], (h) ->

      if h.callback.promise?

        clearTimeout h.timeout if h.timeout?

        if msg.error? then  h.callback.reject   msg.error 
        if msg.data?  then  h.callback.resolve  msg.data
        if msg.info?  then  h.callback.resolve  msg.info

        return false

      else
        h.callback msg
        return true


    # remove an empty handler list

    delete handlers[ msg.id ] if handlers[ msg.id ].length == 0


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

  @open = (url) ->
    sock = this
    d = q.defer()
    @ws = new WebSocket url
    @ws.on 'open',    ()  -> d.resolve()
    @ws.on 'error',   (e) -> d.reject e
    @ws.on 'message', (m) -> onMessage.call sock, m
    d.promise

  @close = ->
    handlers.removeAll()
    @ws.close()

  @send = (msg) ->
    @debug "send", msg if @debug?
    @ws.send msg


  #--------------------------------------------------------------------------------
  # do a thing


  # send a message and wait for the response

  @get = ( url, timer, id )->

    future = q.defer()

    id = "m" + ++nextID unless id?
    handlers.add id, future, timer
    @send JSON.stringify id:id, q:url
  
    future.promise


  # set up listener for event first before invoking get

  @waitForThenGet = ( mid, url, timer )->

    # add handler for watched message first

    future = q.defer()
    handlers.add mid, future, timer

    # then get url and pass on any failures

    @get( url ).catch (e) -> future.priomise.reject e

    future.promise


  #--------------------------------------------------------------------------------
  # watch a thing


  # bind and unbind to game events

  @bind = (id, url) ->
    url = url.subsstring 2 if url.startsWith "/q/" 
    url = url.subsstring 5 if url.startsWith "/bind/" 
    @get "/bind#{url}", null, id
    
  @unbind = (id) ->
    @get "/socket.unbind(#{id})", null, id


  #--------------------------------------------------------------------------------
  # wait for a thing


  # whenever message of type 'id' is received, call the callback function
  # returns a cancel function

  @on = (id, callback) ->
    handlers.add id, callback


  # wait until the next message of type 'id' and then fulfill the future

  @waitFor = (id, timer) ->

    future = q.defer()
    handlers.add id, future, timer
    future.promise

  return

#================================================================================

module.exports = UniumSocket
