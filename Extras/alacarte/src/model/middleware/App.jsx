//-------------------------------------------------------------------------------
// websocket connection to game as redux middleware

import { toast } from 'react-toastify'


//-------------------------------------------------------------------------------
// redux middleware

export default (function(){ 

  return store => next => action => {
    
    switch( action.type ) {

      case 'APP_INFO':
        toast.info( action.payload )
        break

      case 'APP_SUCCESS':
        toast.success( action.payload )
        break

      case 'APP_WARNING':
        toast.warn( action.payload )
        break

      case 'APP_ERROR':
        toast.error( action.payload )
        break
        
      case 'SOCK_DEBUG':
        console.log( action )
        break

      case 'APP_CONNECTED':
        return next( action )
        
      default:
        return next( action )
    }
  }

})()

