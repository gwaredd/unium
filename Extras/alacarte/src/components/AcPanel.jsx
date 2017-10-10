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

  onRemovePanelConfirm = ( id ) => {

    console.log( id )

    //this.props.dispatch( PanelRemove( id ) )
  }

  onRemovePanel = ( id ) => {

    var panel = this.props.panels.byId[ id ]

    var action = Actions.Confirm(
      'Remove Panel',
      "Are you sure you want to remove '" + panel.name + "'",
      this.onRemovePanelConfirm,
      id
    )

    this.props.dispatch( action )
  }

  //-------------------------------------------------------------------------------

  render() {

    const panel = this.props.panel

    // var panel = _.find( this.props.panels.byId, (p) => p.id == panelId )

    var title = (
      <div>
        { panel.name }
        <div className='pull-right'>
          <Glyphicon className='acPanelIcon' glyph="remove" style={{fontSize:'10px'}} onClick={()=>this.onRemovePanel(panel.id)}/>
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

