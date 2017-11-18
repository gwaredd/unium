//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'


//-------------------------------------------------------------------------------

function reduceById( state={}, action ) {

  switch( action.type ) {

    case 'CONFIG_IMPORT': {
      return { ...action.payload.tabs.byId }
    }
    
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

    case 'TAB_LAYOUT': {

      const { payload }     = action
      const { id, layout }  = payload

      if( id in state ) {
        const tab = {...state[id], layout: layout }
        return { ...state, [id] : tab }
      }
    }
  }

  return state
}


//-------------------------------------------------------------------------------

function reduceState( state={ curTab: 1 }, action ) {

  switch( action.type ) {

    case 'CONFIG_IMPORT': {
      return { ...action.payload.tabs.state }
    }
    
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
