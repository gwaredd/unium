//-------------------------------------------------------------------------------

import React from 'react'
import { Button } from 'react-bootstrap'
import FontAwesome from 'react-fontawesome'
import Axios from 'axios'

import * as Actions from '../../actions/App.jsx'
import * as Log from '../../actions/Logging.jsx'
import { WidgetCreate, WidgetRemove } from '../../actions/Tabs.jsx'

//-------------------------------------------------------------------------------

export default class AcWidget extends React.Component {

  onClick = (e) => {

    const { widget, dispatch, appConfig, isLocked } = this.props

    if( !isLocked ) {
      return
    }

    if( !"query" in widget ) {
      dispatch( Log.Warning( "Widget '" + widget.name + "'has no query" ) )
      return
    }

    Axios.get( appConfig.api + widget.query )
      .then( (res) => {

        const { behaviour } = widget

        if( behaviour == 'Log' || behaviour == 'LogNotify' ) {
          dispatch( Log.Print( '[' + widget.name + ']' + JSON.stringify( res.data, null, 2 ) ) )
        }

        if( behaviour == 'Notify' || behaviour == 'LogNotify' ) {
          dispatch( Log.Success( widget.name + ' Success' ) )
        }

      })
      .catch( (err) => {
        dispatch( Log.Error( err.toString() ) )
      })

  }

  onEditWidgetConfirm = ( widget ) => {
    this.props.dispatch( WidgetCreate( widget ))
  }

  onEditWidget = (e) => {

    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    
    var { dispatch, widget } = this.props

    dispatch( Actions.AddWidget( this.onEditWidgetConfirm, widget ) )
  }
  
  onRemoveWidget = (e) => {

    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()

    var { dispatch, widget } = this.props

    dispatch( Actions.Confirm(
      'Remove Widget',
      "Are you sure you want to remove '" + widget.name + "'",
      () => dispatch( WidgetRemove( widget.id ) )
    ))

  }

  render() {

    const { widget } = this.props

    return (
      <Button bsStyle={ widget.style } block onClick={this.onClick}>
        <span>
          { widget.name }
        </span>
        { !this.props.isLocked && 
          <div className='pull-right'>
            <FontAwesome className='acPanelIcon' name='pencil' onClick={this.onEditWidget} />
            &nbsp;
            <FontAwesome className='acPanelIcon' name='times' onClick={this.onRemoveWidget} />
          </div>
        }
      </Button>
    )
  }
}

