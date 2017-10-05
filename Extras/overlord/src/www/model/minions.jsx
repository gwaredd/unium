//-------------------------------------------------------------------------------
// reducer

import _ from 'underscore'


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
      state = {...state, minions: action.payload }
      break

    case 'MINION_UPDATE':
    case 'MINION_ADD':
      var minions = _.reject( state.minions, function(m) { return m.id == action.payload.id } )
      minions.push( action.payload )
      minions.sort( function(a,b) { return a.id - b.id } )
      state = {...state, minions: minions }
      break

    case 'MINION_REMOVE':
      var minions = _.reject( state.minions, function(m) { return m.id == action.payload.id } )
      state = {...state, minions: minions }
      break
  }

  return state
}

