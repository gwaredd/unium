//-------------------------------------------------------------------------------
// reducer


export const initial_state = {
  connected : false,
  minions   : []
}

export function reducer( state=initial_state, action ) {

  switch( action.type ) {
    case 'OVERLORD_CONNECTION': {
      state = {...state, connected: action.payload.state }
    }
  }

  return state
}

