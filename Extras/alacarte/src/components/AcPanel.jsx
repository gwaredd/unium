//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Glyphicon, Button, Panel } from 'react-bootstrap'


import * as Actions from '../model/Actions.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    panels: store.panels
  }
})
export default class AcPanel extends React.Component {

  //-------------------------------------------------------------------------------

  onRemovePanel = () => {
    this.props.dispatch( Actions.appConfirm( 'Remove Panel', 'Are you sure?', null ) )
  }

  //-------------------------------------------------------------------------------

  render() {

    const panel = this.props.panel

    // var panel = _.find( this.props.panels.byId, (p) => p.id == panelId )

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

