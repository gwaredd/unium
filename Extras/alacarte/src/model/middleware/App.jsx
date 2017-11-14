//-------------------------------------------------------------------------------
// websocket connection to game as redux middleware

import { toast } from 'react-toastify'

import * as Connection from '../../actions/Connection.jsx'
import * as Log from '../../actions/Logging.jsx'

//-------------------------------------------------------------------------------
// redux middleware

export default (function(){ 

  return store => next => action => {

    switch( action.type ) {

      case 'LOG':

        const { type, text } = action.payload

        switch( type ) {
          case 'info':    toast.info( text ); break
          case 'success': toast.success( text ); break
          case 'warning': toast.warn( text ); break
          case 'danger':  toast.error( text ); break
        }
        
        break

      case 'CON_CONNECTED':
        if( action.payload ) {
          store.dispatch( Log.Print( "Connected" ) )
          store.dispatch( Connection.Send( 'debug', '/bind/events.debug' ) )
        } else {
          store.dispatch( Log.Print( "Disconnected" ) )
        }
        break

      case 'SOCK_DEBUG':

        var msg = action.payload

        if( "info" in msg ) {
          store.dispatch( Log.Info( "Debug messages bound" ) )
        } else if( "error" in msg ) {
          store.dispatch( Log.Error( msg.error ) )
        } else {
          store.dispatch( Log.Print( msg.data.message ) )
        }

        break
    }

    return next( action )
  }

})()

