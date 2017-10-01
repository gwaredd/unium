//-------------------------------------------------------------------------------
// reducer

export const initial_state = {
  screenshot: false
}

export function reducer( state=initial_state, action ) {

  switch( action.type ) {

    case 'APP_SCREENSHOT':
      state = {...state, screenshot: action.payload }
      break
  }

  return state
}

