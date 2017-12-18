//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'

//-------------------------------------------------------------------------------

function reduceById( state={}, action ) {

  switch( action.type ) {
    
    case 'CONFIG_IMPORT': {
      return { ...action.payload.widgets.byId }
    }

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


    case 'PANEL_REMOVE': {

      const { payload } = action
      const { id }      = payload
      
      return _.omitBy( state, (w) => w.panel == id )
      
    }
    
    case 'TAB_REMOVE': {

      const { payload } = action
      const { id }      = payload
      
      return _.omitBy( state, (w) => w.tab == id )
      
    }
  }

  return state
}
  

//-------------------------------------------------------------------------------

export default combineReducers({
  byId    : reduceById
})

