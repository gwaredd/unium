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

      // if( widget.panel in state ) {

      // }

      //widget.panel

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
      
      return _.reject( state, (p) => p.panel == id )
      
    }
    
    case 'TAB_REMOVE': {

      const { payload } = action
      const { id }      = payload
      
      return _.reject( state, (p) => p.tab == id )
      
    }
  }

  return state
}
  

//-------------------------------------------------------------------------------

export default combineReducers({
  byId    : reduceById
})

