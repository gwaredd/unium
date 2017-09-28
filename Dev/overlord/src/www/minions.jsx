//-------------------------------------------------------------------------------
// reducer


export const initial_state = {
  connected : false,
  minions   : []
}

export function reducer( state=initial_state, action ) {

  switch( action.type ) {

    case 'CONNECTION_STATE':
      state = {...state, connected: action.payload.state }
      break
  }

  return state
}

