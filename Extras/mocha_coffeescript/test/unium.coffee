# a class to make the websocket interface a little more test friendly
# by providing a promise/async version of the websocket interface

chai      = require 'chai'
WebSocket = require 'ws'

module.exports = class UniumHelper

  constructor: () ->
    @ws      = null
    @next_id = 1
    @_on     = {}
    @_once   = {}


  # async connect

  connect: (uri) -> new Promise (resolve, reject) =>
    this.disconnect()
    @ws = new WebSocket uri
    @ws.on 'open', resolve
    @ws.on 'error', reject
    @ws.on 'message', (m) => this.onMessage m

  disconnect: ->
    return unless @ws
    @ws.close()
    @ws = null


  # when a message is received, invoke any handlers registered for this event

  onMessage: (msg) ->

    data = JSON.parse msg
    {id} = data

    return unless id

    if id of @_on
      @_on[ id ].forEach (fn) -> fn data

    if id of @_once
      @_once[ id ].forEach (fn) -> fn data
      delete @_once[ id ]


  # add event handlers

  on: (event, fn) ->
    @_on[ event ] = [] unless event of @_on
    @_on[ event ].push fn

  once: (event, fn) ->
    @_once[ event ] = [] unless event of @_once
    @_once[ event ].push fn

  # send message, return message id

  send: (uri, id) ->
    mid = id || "m#{@next_id++}"
    @ws.send JSON.stringify id: mid, q: uri
    return mid


  # wait until we receive a message with a given id

  wait_for: (id, timeout = 2) ->
    return new Promise (resolve,reject) =>
      timeoutHandle = setTimeout ( -> reject 'timeout' ), timeout * 1000
      this.once id, (data) ->
        clearTimeout timeoutHandle
        resolve data

  # send a query and result the result (like a HTTP get)

  get: (uri, timeout = 2) -> 

    id  = await this.send uri
    res = await this.wait_for id, timeout

    chai.expect( res ).to.not.be.null
    res.should.have.property 'id'
    res.id.should.equal id
    res.should.not.have.property 'error'
    res.should.have.property 'data'

    return res.data


  # convenient function to binding to events
  # use event name as id unless given on

  bind: (uri, name) ->

    id = name || uri.split'.'.pop()

    this.send "/bind#{uri}", id
    res = await this.wait_for id

    chai.expect( res ).to.not.be.null
    res.should.have.property 'id'
    res.should.not.have.property 'error'
    res.should.have.property 'info'

    return res
