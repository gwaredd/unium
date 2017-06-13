#================================================================================

_         = require 'underscore'
q         = require 'q'
WebSocket = require 'ws'


#================================================================================

MessageHandlers = ->

  handlers  = {}
  nextID    = 0


  #--------------------------------------------------------------------------------

  _cancel = ( id, pid ) ->

    return unless id of handlers

    handlers[ id ] =_.reject handlers[ id ], (h) ->
      return false if h.pid != pid

      clearTimeout h.timeout if h.timeout?
      h.callback.reject "timed out" if h.callback.promise?
      return true


  #--------------------------------------------------------------------------------

  @add = ( id, callback, timer ) ->

    ledger = this

    pid = ++nextID
    cancel  = -> _cancel id, pid
    timeout = if not timer? then null else setTimeout cancel, timer

    handlers[ id ] = [] unless id of handlers
    handlers[ id ].push pid:pid, callback:callback, timeout:timeout
  
    return cancel


  #--------------------------------------------------------------------------------

  @removeAll = ->

    for id,list of handlers
      _.each list, (h) ->
        clearTimeout h.timeout if h.timeout?
        h.callback.reject "removing handler" if h.callback.promise?

    handlers = {}

  #--------------------------------------------------------------------------------

  @onMessage = (msg) ->

    return unless msg.id? and msg.id of handlers

    # for each handler

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

    # remove entry if emtpy

    delete handlers[ msg.id ] if handlers[ msg.id ].length == 0


  return


#================================================================================

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
  # do thing

  @get = ( url, id, timer )->

    d = q.defer()

    id = "m" + ++nextID unless id?
    msg = JSON.stringify id:id, q:url
    handlers.add id, d, timer
    @send msg
  
    d.promise


  #--------------------------------------------------------------------------------
  # watch a thing

  @bind = (id, url) ->
    url = url.subsstring 2 if url.startsWith "/q/" 
    @get "/bind#{url}", id
    
  @unbind = (id) ->
    @get "/socket.unbind(#{id})", id

  @on = (id, fn) ->
    handlers.add id, fn


  #--------------------------------------------------------------------------------
  # wait for a thing

  @waitFor = (id, timer) ->

    d = q.defer()
    handlers.add id, d, timer
    d.promise

  return

#================================================================================

module.exports = UniumSocket
