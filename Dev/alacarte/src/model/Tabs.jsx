//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'


//-------------------------------------------------------------------------------

function reduceById( state={}, action ) {

  switch( action.type ) {
    
    case 'TAB_ADD': {

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

function reduceAllIds( state=[], action ) {  

  switch( action.type ) {

    case 'TAB_ADD': {

      const { payload } = action
      const { id }      = payload

      return state.concat( id )
    }

    case 'TAB_REMOVE': {

      const { payload } = action
      const { id }      = payload

      return state.filter( (item) => item !== id )
    }
  }

  return state
}


//-------------------------------------------------------------------------------

export default combineReducers({
  byId    : reduceById,
  allIds  : reduceAllIds
})
