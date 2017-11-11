//-------------------------------------------------------------------------------
// reducer

const initial_state = {
  dialog  : null,
  config  : {
    api: 'http://localhost:8342/' // TODO: get automatically
  }
}


export default function App( state=initial_state, action ) {

  switch( action.type ) {

    case 'APP_DAILOG_SHOW':
      state = {...state, dialog: action.payload }
      break

    case 'APP_DIALOG_CANCEL':
      state = {...state, dialog: null }
      break
  }

  return state
}

