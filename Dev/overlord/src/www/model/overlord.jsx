//-------------------------------------------------------------------------------
// middleware for handling websocket connections to server

import * as actions from './actions.jsx'

export default (function(){ 

  //-------------------------------------------------------------------------------

  var socket = null
  
  const onOpen = (ws,store,token) => evt => {
    store.dispatch( actions.setConnectedState( true ) )
    store.dispatch( actions.listMinions() )
  }

  const onClose = (ws,store) => evt => {
    store.dispatch( actions.setConnectedState( false ) )
  }

  const onMessage = (ws,store) => evt => {
    var msg = JSON.parse( evt.data )
    store.dispatch({
      type    : "MINION_" + msg.id.toUpperCase(),
      payload : msg.data
    })
  }


  //-------------------------------------------------------------------------------
  // redux

  return store => next => action => {

    switch( action.type ) {

      case 'OVERLORD_CONNECT':

        if(socket != null) {
          socket.close()
        }

        var url = document.location.href.replace( /^http?/i, "ws" ) + 'overlord'

        socket            = new WebSocket( url )
        socket.onmessage  = onMessage( socket, store )
        socket.onclose    = onClose( socket, store )
        socket.onopen     = onOpen( socket, store, action.token )

        break


      case 'OVERLORD_DISCONNECT':

        if(socket != null) {
          socket.close()
        }

        socket = null

        break


      case 'OVERLORD_SEND':
        socket.send( JSON.stringify( action.payload ) )
        break

      default:
        return next( action )
    }
  }
})()

