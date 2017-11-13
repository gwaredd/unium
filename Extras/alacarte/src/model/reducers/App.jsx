//-------------------------------------------------------------------------------

var server = document.location.host

if( DEVSERVER ) {
  server = server.replace( /:\d+/, ':8342' )
}

const initial_state = {
  dialog    : null,
  connected : false,
  config    : {
    api     : document.location.protocol + '//' + server,
    ws      : "ws://" + server + "/ws"
  }
}


//-------------------------------------------------------------------------------

export default function App( state=initial_state, action ) {

  switch( action.type ) {

    case 'APP_CONNECTED':
      state = {...state, connected: action.payload }
      break

    case 'APP_DAILOG_SHOW':
      state = {...state, dialog: action.payload }
      break

    case 'APP_DIALOG_CANCEL':
      state = {...state, dialog: null }
      break
  }

  return state
}

