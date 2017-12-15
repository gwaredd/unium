//-------------------------------------------------------------------------------

import React from 'react'
import FontAwesome from 'react-fontawesome'

import { Button } from 'react-bootstrap'


//-------------------------------------------------------------------------------

export default class WidgetLink extends React.Component {
  

  //-------------------------------------------------------------------------------

  onClick = () => {
    
    const { widget, dispatch, appConfig } = this.props
  
    if( !"query" in widget ) {
      dispatch( Log.Warning( "Widget '" + widget.name + "'has no query" ) )
      return
    }

    const url = widget.query.startsWith( "http" ) ? widget.query : appConfig.api + widget.query
    
    var win = window.open( url, '_blank' )
    win.focus();

  }
  
  //-------------------------------------------------------------------------------
  
  render() {

    const { widget, isEditing } = this.props

    return (
      <Button
        bsStyle="default"
        className="acWidget"
        style={{ backgroundColor: widget.colour, color: widget.textColour }}
        block
        onClick={this.onClick}>
        { widget.name }
        { !isEditing && (
            <div className='pull-right'>
              <FontAwesome name='external-link' />
            </div>
          )}
        
      </Button>
    )
  }
}

