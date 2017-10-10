//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'


const initial_state = {
  1: {
    id      : 1,
    tab     : 1,
    name    : 'Panel 1a',
    layout  : { x: 0, y: 0, w: 1, h: 2 }
  },
  2: {
    id      : 2,
    tab     : 1,
    name    : 'Panel 2a',
    layout  : { x: 1, y: 0, w: 1, h: 2 }
  },
  3: {
    id      : 3,
    tab     : 2,
    name    : 'Panel 3b',
    layout  : { x: 0, y: 0, w: 1, h: 2 }
  },
  4: {
    id      : 4,
    tab     : 3,
    name    : 'Panel 4c',
    layout  : { x: 0, y: 0, w: 1, h: 2 }
  },
}

//-------------------------------------------------------------------------------

function reduceById( state=initial_state, action ) {

  switch( action.type ) {
    
    case 'PANEL_CREATE': {

      // const { payload } = action
      // const { id }      = payload
      // const entry       = { ...payload }

      // return { ...state, [id] : entry }
    }

    case 'PANEL_CREATE': {

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

