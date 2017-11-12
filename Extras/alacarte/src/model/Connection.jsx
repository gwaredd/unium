//-------------------------------------------------------------------------------
// websocket connection to game as redux middleware

import * as App from '../actions/App.jsx'

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
    return () => store.dispatch( App.ConnectionState( true ) )
  }

  onClose = () => { 
    var store = this.store
    return () => store.dispatch( App.ConnectionState( false ) )
  }

  onMessage = () => {
    var store = this.store
    return (e) => {
      const msg = JSON.parse( e.data )
      store.dispatch({
        type    : "SOCK_" + msg.id.toUpperCase(),
        payload : msg.data
      })
    }
  }

  onError = () => {
    var store = this.store
    return (e) => {
      console.error( e )
      store.dispatch( App.ConnectionError( "Failed to connect to game" ) )
    }
  }


  connect() {

    this.disconnect()

    // get URL end-point

    const { api } = this.store.getState().app.config

    var url = api.replace( /^http?/i, "ws" )

    if( DEVSERVER ) {
      url = url.replace( /:\d+/, ':8342' )
    }

    // create websocket
   
    this.socket           = new WebSocket( url )
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
    socket.send( JSON.stringify( data ) )
  }
}


//-------------------------------------------------------------------------------
// redux middleware

export default (function(){ 

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

      case 'CON_ERROR':
        store.dispatch( App.Error( action.payload ) )
        break

      default:
        return next( action )
    }
  }

})()

