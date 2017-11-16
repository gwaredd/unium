//-------------------------------------------------------------------------------
// websocket connection to game as redux middleware

import Axios from 'axios'
import { toast } from 'react-toastify'

import * as Connection from '../../actions/Connection.jsx'
import * as Log from '../../actions/Logging.jsx'

const customSave = '/file/persistent/alacarte.json'
const staticSave = '/file/streaming/alacarte.json'


//-------------------------------------------------------------------------------
// redux middleware

export default (function(){ 

  return store => next => action => {

    switch( action.type ) {

      case 'APP_LOAD': {

        const { api } = store.getState().app.config

        Axios.get( api + customSave )
          .then( (res) => {
            store.dispatch( { type: 'CONFIG_IMPORT', payload: res.data } )
            store.dispatch( Log.Success( 'Custom config loaded' ) )
          })
          .catch( (err) => {
            store.dispatch( Log.Warning( 'No custom config found' ) )
          })

        return
      }

      case 'APP_SAVE': {
        var state = { ...store.getState() }
        const { api } = state.app.config


        delete state['output']
        

        Axios.post( api + customSave, JSON.stringify( state ) )
          .then( (res) => {
            store.dispatch( Log.Success( 'Config saved' ) )
          })
          .catch( (err) => {
            store.dispatch( Log.Error( 'Failed to save config: ' + err.toString() ) )
          })
  
        return
      }
          
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

