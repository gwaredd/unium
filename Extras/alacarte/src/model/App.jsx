//-------------------------------------------------------------------------------

import { toast } from 'react-toastify'


//-------------------------------------------------------------------------------
// reducer

const initial_state = {
  dialog    : null,
  connected : false,
  config    : {
    api     : document.location.protocol + '//' + document.location.host
  }
}

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
  }

  return state
}

