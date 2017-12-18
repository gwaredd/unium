//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'


//-------------------------------------------------------------------------------

function reduceById( state={}, action ) {

  switch( action.type ) {

    case 'CONFIG_IMPORT': {
      return { ...action.payload.panels.byId }
    }
    

    //-------------------------------------------------------------------------------

    case 'PANEL_CREATE': {

      const { payload } = action
      const { id }      = payload
      const panel       = { ...payload, widgets: [] }

      return { ...state, [id] : panel }
    }


    case 'PANEL_REMOVE': {

      const { payload } = action
      const { id }      = payload

      return _.omit( state, id )
    }

    case 'TAB_REMOVE': {

      const { payload } = action
      const { id }      = payload
      
      return _.omitBy( state, (t) => t.tab == id )
    }


    //-------------------------------------------------------------------------------

    case 'WIDGET_CREATE': {

      const widget = action.payload
      const panelID = widget.panel
      
      if( panelID in state ) {

        const panel = state[ panelID ]
        var widgets = [ ...panel.widgets ]

        if( _.find( widgets, (w) => w == widget.id ) == null ) {
          widgets.push( widget.id )
        }

        var newPanel = { ...panel, widgets: widgets }
  
        return { ...state, [ panelID ]: newPanel }

      } else {
        console.warn( "Can't find panel " + widget.panel )
      }

      break
    }


    case 'WIDGET_REMOVE': {

      const { id, panel } = action.payload

      if( panel in state ) {

        const oldPanel = state[ panel ]
        const widgets = _.reject( oldPanel.widgets, (wid) => wid == id )

        return { ...state, [ panel ]: { ...oldPanel, widgets: widgets } }

     } else {
        console.warn( "Can't find panel " + widget.panel )
        break
      }

      // return _.omit( state, id )
    }

    case 'WIDGET_MOVE': {

      const { id, dragIndex, hoverIndex } = action.payload

      const panel   = state[ id ]
      const dragged = panel.widgets[ dragIndex ]

      var widgets = [ ...panel.widgets.slice( 0, dragIndex ), ...panel.widgets.slice( dragIndex + 1 ) ]

      widgets.splice( hoverIndex, 0, dragged )

      return { ...state, [ id ]: { ...panel, widgets: widgets } }
    }
          
  }
  
  return state
}


//-------------------------------------------------------------------------------


function reduceState( state={}, action ) {

  switch( action.type ) {

    case "APP_PANEL_STATE":
      return { ...state, ...action.payload }
      break
  }

  return state
}


//-------------------------------------------------------------------------------

export default combineReducers({
  byId    : reduceById,
  state   : reduceState
})

