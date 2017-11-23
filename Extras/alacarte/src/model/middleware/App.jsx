//-------------------------------------------------------------------------------
// websocket connection to game as redux middleware

import Axios from 'axios'
import { toast } from 'react-toastify'
import _ from 'lodash'

import * as Connection from '../../actions/Connection.jsx'
import * as Log from '../../actions/Logging.jsx'

const customSave = '/file/persistent/alacarte.json'
const staticSave = '/file/streaming/alacarte.json'
const localStorageKey = 'unium'

const emptyConfig = {
  app: {
    settings: {
      useLocalStorage: true
    }
  },
  tabs: { byId: {}},
  panels: { byId: {}},
  widgets: { byId: {}},
}


//-------------------------------------------------------------------------------
// redux middleware

export default (function(){ 

  return store => next => action => {

    switch( action.type ) {

      //-------------------------------------------------------------------------------
      // load app settings

      case 'APP_LOAD': {

        const state = store.getState()
        const { api } = state.app.config
        const { useLocalStorage } = state.app.settings

        if( global.localStorage ) {
          const data = global.localStorage.getItem( localStorageKey )
          if( data != null ) {
            try {
              store.dispatch( { type: 'CONFIG_IMPORT', payload: JSON.parse( data ) } )
              store.dispatch( Log.Success( 'Custom config loaded from local storage' ) )
            } catch( e ) {
              console.error( e )
            }
            return
          }
        }

        Axios.get( api + customSave )
          .then( (res) => {
            store.dispatch( { type: 'CONFIG_IMPORT', payload: res.data } )
            store.dispatch( Log.Success( 'Custom config loaded from game' ) )
          })
          .catch( (err) => {
            store.dispatch( Log.Warning( 'No custom config found' ) )
          })

        return
      }


      //-------------------------------------------------------------------------------

      case 'APP_SAVE': {

        var data = { ...store.getState() }
        const { api } = data.app.config
        const { useLocalStorage } = data.app.settings

        
        delete data[ 'output' ]
        data.app = _.pick( data.app, 'settings' )
        data = JSON.stringify( data )


        if( useLocalStorage ) {

          if( !global.localStorage ) {
            store.dispatch( Log.Error( 'Failed to save config - no local storage' ) )
            return
          }

          global.localStorage.setItem( localStorageKey, data )
          store.dispatch( Log.Success( 'Config saved to local storage' ) )
          
        } else {

          // save to game

          Axios.post( api + customSave, data )
          .then( (res) => {
            store.dispatch( Log.Success( 'Config saved to persistent data' ) )
          })
          .catch( (err) => {
            store.dispatch( Log.Error( 'Failed to save config: ' + err.toString() ) )
          })

        }
  
        return
      }

      //-------------------------------------------------------------------------------

      case 'APP_DELETE': {

        var data = store.getState()
        const { api } = data.app.config
        const { useLocalStorage } = data.app.settings

        if( useLocalStorage ) {
          
          if( !global.localStorage ) {
            store.dispatch( Log.Error( 'Failed to save config - no local storage' ) )
            return
          }

          global.localStorage.removeItem( localStorageKey )

          store.dispatch( { type: 'CONFIG_IMPORT', payload: emptyConfig } )
          store.dispatch( Log.Success( 'Config removed' ) )
          
        } else {

          console.log( "delete on device" )
        }
          
        return
      }
      
      
      //-------------------------------------------------------------------------------
      // toast important app log messages

      case 'LOG': {

        const { type, text } = action.payload

        switch( type ) {
          case 'info':    toast.info( text ); break
          case 'success': toast.success( text ); break
          case 'warning': toast.warn( text ); break
          case 'danger':  toast.error( text ); break
        }
        
        break
      }


      //-------------------------------------------------------------------------------
      // handle connected to game state

      case 'CON_CONNECTED': {

        if( action.payload ) {
          store.dispatch( Log.Print( "Connected" ) )
          store.dispatch( Connection.Send( 'debug', '/bind/events.debug' ) ) // bind to debug messages
        } else {
          store.dispatch( Log.Print( "Disconnected" ) )
        }
        break
      }


      //-------------------------------------------------------------------------------
      // handle debug output from game

      case 'SOCK_DEBUG': {

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
    }

    return next( action )
  }

})()

