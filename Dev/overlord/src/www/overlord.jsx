
import * as actions from './actions.jsx'

export default class Overlord {

  constructor( store ) {
    this.store = store
    this.connect()
  }

  connect() {

    var url = document.location.href.replace( /^http?/i, "ws" ) + 'overlord'
    var socket = new WebSocket( url )

    var store = this.store
    
    socket.onopen = function() {
      store.dispatch( actions.onConnection( true ) )
      socket.send( JSON.stringify({type:'list'}) )
    }

    socket.onclose = function() {
      store.dispatch( actions.onConnection( false ) )
    }

    socket.onerror = function( err ) {
      console.error( err )
    }
    
    socket.onmessage = function( msg ) {

    }
  }
}

