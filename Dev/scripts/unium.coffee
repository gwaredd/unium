_         = require 'underscore'
q         = require 'q'
WebSocket = require 'ws'


#--------------------------------------------------------------------------------

WatchList = ->

  nextID  = 0
  watches = {}

  @contains = (id) -> id of watches


  @add = (mid, fn, count) ->

    count = -1 unless count?
    wid   = "w" + ++nextID

    watches[ mid ] = [] unless mid of watches
    watches[ mid ].push mid:mid, wid:wid, fn:fn, count:count

    return "#{mid}/#{wid}"


  @remove = (id) ->
    [mid,wid] = id.split ',', 2
    return unless mid of watches
    watches[ mid ] = _.reject watches[ mid ], (w) -> w.wid == wid


  @removeAll = ->
    watches = {}


  @onMessage = (msg) ->
    return unless msg.id? and msg.id of watches
    w.fn msg for w in watches[ msg.id ]

  return


#--------------------------------------------------------------------------------

PromiseLedger = ->

  promises  = {}

  @add = (id,d,timer) ->

    if id of promises
      clearTimeout promises[ id ].timeout if promises[ id ].timeout?
      promises[ id ].promise.reject "promise replaced"

    ledger = this

    promises[ id ] =
      promise : d
      timeout : if timer? then setTimeout ( -> ledger.reject id, "timed out" ), timer else null


  @resolve = (id, data) ->
    return unless id of promises

    promise = promises[ id ]
    delete promises[ id ]

    clearTimeout promise.timeout if promise.timeout?
    promise.promise.resolve "promise replaced"


  @reject = (id, e) ->
    return unless id of promises

    promise = promises[ id ]
    delete promises[ id ]

    clearTimeout promise.timeout if promise.timeout?
    promise.promise.reject e


  @rejectAll = ->
    @reject k, "rejecting all promises" for k,v of promises


  @makeGood = (msg) ->
    return false unless msg.id? and msg.id of promises
    if msg.error? then  @reject   msg.id, msg.error 
    if msg.data?  then  @resolve  msg.id, msg.data
    if msg.info?  then  @resolve  msg.id, msg.info 
    return true

  return



#--------------------------------------------------------------------------------

UniumSocket = ->

  @ws     = null
  @debug  = null

  #--------------------------------------------------------------------------------

  nextID      = 0
  promises    = new PromiseLedger()
  watchlist   = new WatchList()

  onMessage = (msg) ->
    @debug "recv", msg if @debug?
    msg = JSON.parse msg
    promises.makeGood msg
    watchlist.onMessage msg


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
    promises.rejectAll()
    watchlist.removeAll()
    boundEvents = {}
    @ws.close()

  @send = (msg) ->
    @debug "send", msg if @debug?
    @ws.send msg

  #--------------------------------------------------------------------------------
  # do thing

  @get = ( url, id )->

    d = q.defer()

    id = "m" + ++nextID unless id?
    msg = JSON.stringify id:id, q:url

    promises.add id, d
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
    wid = watchlist.add id, fn
    return cancel: -> watchlist.remove wid


  #--------------------------------------------------------------------------------
  # wait for a thing

  @waitFor = (id, timer) ->

    d = q.defer()

    watcher   = null
    timeout   = null

    if timer?
      watcher   = watchlist.add id, (msg) -> watcher.cancel(); clearTimeout timeout; d.resolve msg
      timeout   = setTimeout ( -> watcher.cancel(); d.reject "timed out" ), timer
    else
      watcher = watchlist.add id, (msg) -> watcher.cancel(); d.resolve msg

    d.promise


  return

module.exports = UniumSocket

