//-------------------------------------------------------------------------------

import React from 'react'

import { Button } from 'react-bootstrap'

import Utils from '../../Utils.jsx'
import * as Log from '../../actions/Logging.jsx'


//-------------------------------------------------------------------------------

export default class WidgetButton extends React.Component {
  

  //-------------------------------------------------------------------------------

  onClick = () => {
    
    const { widget, dispatch, appConfig } = this.props
  
    if( !"query" in widget ) {
      dispatch( Log.Warning( "Widget '" + widget.name + "'has no query" ) )
      return
    }

    Utils.Fetch( widget.query, appConfig )
    
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
}

