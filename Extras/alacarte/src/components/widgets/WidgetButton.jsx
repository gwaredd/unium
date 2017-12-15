//-------------------------------------------------------------------------------

import React from 'react'

import { Button } from 'react-bootstrap'
import Axios from 'axios'

import * as Log from '../../actions/Logging.jsx'


//-------------------------------------------------------------------------------

function onClickHander() {
  
  const { id, widgets, dispatch, appConfig } = this.props
  const widget = widgets.byId[ id ]

  if( !"query" in widget ) {
    dispatch( Log.Warning( "Widget '" + widget.name + "'has no query" ) )
    return
  }

  Axios
    .get( appConfig.api + widget.query )

    .then( (res) => {

      if( 'log' in widget && widget.log ) {
        dispatch( Log.Print( '[' + widget.name + ']' + JSON.stringify( res.data, null, 2 ) ) )
      }

      if( 'notify' in widget && widget.notify ) {
        dispatch( Log.Success( widget.name + ' Success' ) )
      }

    })

    .catch( (err) => {
      dispatch( Log.Error( err.toString() ) )
    })

}

//-------------------------------------------------------------------------------

export default class WidgetButton {


  //-------------------------------------------------------------------------------

  componentWillMount() {
    this.onClick = () => onClickHander.call( this )
  }

  //-------------------------------------------------------------------------------
  
  render() {

    const { widget } = this.props

    return (
      <Button
        bsStyle="default"
        className="acWidget"
        style={{ backgroundColor: widget.colour, color: widget.textColour }}
        block
        onClick={this.onClick}>
        { widget.name }
      </Button>
    )
  }

  //-------------------------------------------------------------------------------

  options() {
    return null
  }
}

