//-------------------------------------------------------------------------------
// websocket connection to game as redux middleware

import * as Log from '../../actions/Logging.jsx'
import * as Action from '../../actions/Connection.jsx'


//-------------------------------------------------------------------------------

class Connection
{
  socket  = null
  store   = null

  constructor( store ) {
    this.store = store
  }

  onOpen = () => { 
    var store = this.store
    return () => store.dispatch( Action.SetState( true ) )
  }

  onClose = () => { 
    var store = this.store
    return () => store.dispatch( Action.SetState( false ) )
  }

  onMessage = () => {
    var store = this.store
    return (e) => {
      const msg = JSON.parse( e.data )
      store.dispatch({
        type    : "SOCK_" + msg.id.toUpperCase(),
        payload : msg
      })
    }
  }

  onError = () => {
    var store = this.store
    return (e) => {
      store.dispatch( Log.Error( "Failed to connect to game" ) )
    }
  }


  connect() {

    this.disconnect()

    // get URL end-point

    const { config } = this.store.getState().app

    // create websocket
   
    this.socket           = new WebSocket( config.ws )
    this.socket.onopen    = this.onOpen()
    this.socket.onclose   = this.onClose()
    this.socket.onerror   = this.onError()
    this.socket.onmessage = this.onMessage()
  }

  disconnect() {
    if( this.socket !== null ) {
      this.socket.close()
    }

    this.socket = null
  }

  send( data ) {
    this.socket.send( JSON.stringify( data ) )
  }
}


//-------------------------------------------------------------------------------

function ConnectionMiddleware() { 

  var gConnection = null

  return store => next => action => {
    
    switch( action.type ) {

      case 'CON_CONNECT':
        if( gConnection === null ) {
          gConnection = new Connection( store )
        }
        gConnection.connect()
        break

      case 'CON_DISCONNECT':
        if( gConnection !== null ) {
          gConnection.disconnect()
        }
        break

      case 'CON_SEND':
        if( gConnection !== null ) {
          gConnection.send( action.payload )
        }
        break

      default:
        return next( action )
    }
  }
}


//-------------------------------------------------------------------------------

export default ConnectionMiddleware()

