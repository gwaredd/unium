
app = angular.module "app", []

app.controller 'Root', ($scope) ->

  ws = null

  send = (msg) ->
    return unless ws?
    msg = JSON.stringify msg
    $scope.output += "send: " + msg + "\n"
    ws.send msg
    $scope.$apply() unless $scope.$$phase

  #--------------------------------------------------------------------------------

  $scope.connected  = false
  $scope.msg        = ""
  $scope.output     = ""

  $scope.ToggleConnect = ->

    if $scope.connected
      ws.close()
      $scope.connected = false

    else

      ws = new WebSocket 'ws://localhost:8342/ws' #

      ws.onerror  = (err) ->
        $scope.output += "[ERROR] #{JSON.stringify err}\n"
        $scope.$apply() unless $scope.$$phase

      ws.onopen = ->
        send id: "about", q: "/about"

      ws.onclose = ->
        $scope.output += "socket closed\n"
        $scope.connected = false
        $scope.$apply() unless $scope.$$phase

      ws.onmessage = (evt) ->
        $scope.output += "recv: " + evt.data + "\n"
        $scope.$apply() unless $scope.$$phase

      $scope.connected = true

  #--------------------------------------------------------------------------------

  $scope.OnSend = -> send JSON.parse $scope.msg
  $scope.OnClear = -> $scope.output = ""; $scope.$apply() unless $scope.$$phase

  #--------------------------------------------------------------------------------

  debugMessage =
    debug:
      id  : "dbg"
      q   : "/bind/events.debug"
    unbind:
      q   : "/socket.unbind(dbg)"
    fps   :
      id  : "watch"
      q   : "/q/stats.FPS"
      repeat: freq: 0.25
    fps   :
      id  : "watch"
      q   : "/q/stats.FPS"
      repeat: freq: 0.25
    sphere:
      id  : "watch"
      q   : "/q/scene/Game/Sphere.Transform.position"
      repeat: freq: 1.0
    stop  : 
      q   : "/socket.stop(watch)"

  $scope.Msg = (id) -> $scope.msg = JSON.stringify debugMessage[id], null, 2


