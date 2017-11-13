//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'


const initial_state = {
  1: {
    id      : 1,
    panel   : 1,
    name    : 'Widget 1-1',
    style   : 'primary'
  },
  2: {
    id      : 2,
    panel   : 1,
    name    : 'Widget 2-1',
    style   : 'warning'
  }

}

//-------------------------------------------------------------------------------

function reduceById( state=initial_state, action ) {

  switch( action.type ) {
    
    case 'WIDGET_CREATE': {

      const { payload } = action
      const { id }      = payload
      const widget      = { ...payload }

      return { ...state, [id] : widget }
    }

    case 'WIDGET_REMOVE': {

      const { payload } = action
      const { id }      = payload

      return _.omit( state, id )
    }
  }

  return state
}


//-------------------------------------------------------------------------------

export default combineReducers({
  byId    : reduceById
})
