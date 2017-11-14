//-------------------------------------------------------------------------------

import React from 'react'
import { Button, Glyphicon } from 'react-bootstrap'
import Axios from 'axios'

import * as Actions from '../../actions/App.jsx'
import * as Log from '../../actions/Logging.jsx'
import { WidgetRemove } from '../../actions/Tabs.jsx'

//-------------------------------------------------------------------------------

export default class AcWidget extends React.Component {

  onClick = (e) => {

    const { widget, dispatch, appConfig } = this.props

    console.log( widget.behaviour )

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
        { widget.name }
        <div className='pull-right'>
          <Glyphicon className='acPanelIcon' glyph="remove" onClick={this.onRemoveWidget}/>
        </div>
      </Button>
    )
  }
}

