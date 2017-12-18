//-------------------------------------------------------------------------------

var server = document.location.host

if( DEVSERVER ) {
  server = server.replace( /:\d+/, ':8342' )
}

const initial_state = {
  dialog      : null,
  connected   : false,
  config      : {
    api       : document.location.protocol + '//' + server,
    ws        : "ws://" + server + "/ws"
  },
}


//-------------------------------------------------------------------------------

export default function App( state=initial_state, action ) {

  switch( action.type ) {
    case 'CONFIG_IMPORT':     return { ...state }
    case 'APP_CONFIG':        return { ...state }
    case 'CON_CONNECTED':     return { ...state, connected: action.payload }
    case 'APP_DAILOG_SHOW':   return { ...state, dialog: action.payload }
    case 'APP_DIALOG_CANCEL': return { ...state, dialog: null }
  }

  return state
}

