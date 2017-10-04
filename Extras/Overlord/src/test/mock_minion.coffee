#--------------------------------------------------------------------------------

chalk     = require 'chalk'
options   = require 'optimist'
WebSocket = require 'ws'

#--------------------------------------------------------------------------------

argv = options

  .usage    "Fake a minion client"

  .describe 'h',    'show usage'
  .alias    'h',    '?'
  .alias    'h',    'help'

  .describe 'n',    'number of minions'
  .alias    'n',    'num'
  .default  'n',    1

  .describe 'url',  'overlord endpoint'
  .default  'url',  'ws://localhost:8080/minion'

  .argv

if argv.help?
  options.showHelp()
  process.exit 1


#--------------------------------------------------------------------------------

print = (type, msg) ->
  timestamp = new Date().toISOString()
  type = chalk.red "[#{type}]" if type == "ERROR"
  type = chalk.magenta "[#{type}]" if type == "WARN "
  type = chalk.cyan "[#{type}]" if type == "INFO "
  
  console.log "[#{timestamp}] #{type} #{msg}"

log   = (msg) -> print "INFO ", msg
warn  = (msg) -> print "WARN ", msg
error = (msg) -> print "ERROR", msg


#--------------------------------------------------------------------------------

nextID = 0

Minion = ->

  @id   = ++nextID
  @itr  = null

  @log    = (m) -> log "[#{@id}] #{m}"
  @error  = (m) -> error "[#{@id}] #{m}"

  #--------------------------------------------------------------------------------

  @connect = ->

    minion = this
    itr = this.itr

    minion.log "Connecting ..."

    ws = new WebSocket argv.url

    ws.on 'open',    ( ) -> itr.next ws
    ws.on 'close',   ( ) -> itr.throw "Connection closed"
    ws.on 'error',   (e) -> minion.error "#{e}"
    ws.on 'message', (m) -> minion.log "Received: #{m}"

    yield "waiting for connection"


  #--------------------------------------------------------------------------------

  @logic = ->

    try

      ws = yield from this.connect()
      @log "Connected to #{ argv.url }"

      # send about

      @log "Sending about"

      about = 
        Unium       : "1.0"
        Unity       : "2007.1"
        Mono        : "2.0"
        IsEditor    : true
        Product     : "Mock Minion"
        Company     : "gwaredd"
        Version     : "1.0"
        IPAddress   : "127.0.0.1"
        FPS         : 60.0
        RunningTime : 12.0
        Scene       : "Scene1"
        Platform    : "Android"
        Port        : 8342

      ws.send JSON.stringify id:"about", data:about


      minion = this

      changeScene = ->

        n = 1 + Math.floor Math.random() * 5;
        scene = "Scene#{n}"

        minion.log "Changine to #{scene}"

        ws.send JSON.stringify id:"scene", data: scene
        minion.itr.next()

      
      sceneChanges = 10000
      for i in [0..sceneChanges]
        setTimeout changeScene, 2000 + Math.floor Math.random() * 3000;
        yield

      ws.close()
      yield

    catch e
      @log "#{e}"

  #--------------------------------------------------------------------------------

  @run = ->
    @itr = @logic()
    @itr.next()

  @run()    
  return this


#--------------------------------------------------------------------------------

startMinion = -> new Minion()
setTimeout startMinion, Math.floor Math.random() * 5000 for i in [ 0 .. argv.num-1 ]

