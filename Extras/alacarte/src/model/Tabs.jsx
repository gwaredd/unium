//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'


const initial_state = {
  1: {
    id    : 1,
    name  : 'Tab A'
  },
  2: {
    id    : 2,
    name  : 'Tab B'
  },
  3: {
    id    : 3,
    name  : 'Tab C'
  },
}

//-------------------------------------------------------------------------------

function reduceById( state=initial_state, action ) {

  switch( action.type ) {
    
    case 'TAB_CREATE': {

      const { payload } = action
      const { id }      = payload
      const entry       = { ...payload }

      return { ...state, [id] : entry }
    }

    case 'TAB_REMOVE': {

      const { payload } = action
      const { id }      = payload

      return _.omit( state, id )
    }
  }

  return state
}


//-------------------------------------------------------------------------------

function reduceState( state={ curTab: 1 }, action ) {

  switch( action.type ) {
    case 'TAB_SELECT': {
      const { payload } = action
      const { id }      = payload
      return {...state, curTab: id }
    }
  }

  return state
}
  

//-------------------------------------------------------------------------------

export default combineReducers({
  byId    : reduceById,
  state   : reduceState
})
