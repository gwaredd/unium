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

      case 'LOG_INFO':
        toast.info( action.payload )
        break

      case 'LOG_SUCCESS':
        toast.success( action.payload )
        break

      case 'LOG_WARNING':
        toast.warn( action.payload )
        break

      case 'CON_ERROR':
      case 'LOG_ERROR':
        toast.error( action.payload )
        break
        
      case 'SOCK_DEBUG':

        var msg = action.payload

        if( "info" in msg ) {
          store.dispatch( Log.Info( "Now receiving debug messages" ) )
        } else if( "error" in msg ) {
          store.dispatch( Log.Error( msg.error ) )
        } else {
          console.log( msg.data )
        }

        break

      case 'CON_CONNECTED':
        if( action.payload ) {
          store.dispatch( Connection.Send( 'debug', '/bind/events.debug' ) )
        }
        return next( action )
        
      default:
        return next( action )
    }
  }

})()

