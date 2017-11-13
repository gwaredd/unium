//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Button, Glyphicon } from 'react-bootstrap'

import * as Actions from '../../actions/App.jsx'
import { WidgetRemove } from '../../actions/Tabs.jsx'

//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    widgets: store.widgets
  }
})
export default class AcWidget extends React.Component {

  
  onRemoveWidget = () => {

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
      <Button bsStyle={ widget.style } block>
        { widget.name }
        <div className='pull-right'>
          <Glyphicon className='acPanelIcon' glyph="remove" onClick={this.onRemoveWidget}/>
        </div>
      </Button>
    )
  }
}

