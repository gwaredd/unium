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

    case 'MINION_LIST':
      break

    case 'MINION_ADD':
      break

    case 'MINION_REMOVE':
      break

    case 'MINION_UPDATE':
      break
  }

  return state
}

