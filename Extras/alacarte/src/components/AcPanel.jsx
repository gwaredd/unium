//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Glyphicon, Button, Panel } from 'react-bootstrap'


import * as Actions from '../actions/App.jsx'
import { PanelRemove } from '../actions/Tabs.jsx'

//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    panels: store.panels
  }
})
export default class AcPanel extends React.Component {

  //-------------------------------------------------------------------------------

  onRemovePanel = () => {

    var { panel, dispatch } = this.props

    dispatch( Actions.Confirm(
      'Remove Panel',
      "Are you sure you want to remove '" + panel.name + "'",
      () => dispatch( PanelRemove( panel.id ) )
    ))
  }

  //-------------------------------------------------------------------------------

  render() {

    const panel = this.props.panel

    var title = (
      <div>
        { panel.name }
        <div className='pull-right'>
          <Glyphicon className='acPanelIcon' glyph="remove" style={{fontSize:'10px'}} onClick={this.onRemovePanel}/>
        </div>
      </div>
    )

    return (
      <Panel className="acPanel" header={title}>
        Panel content
      </Panel>
    )
  }
}

