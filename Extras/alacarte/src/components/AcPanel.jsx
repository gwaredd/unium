//-------------------------------------------------------------------------------

import React from 'react'
import { Glyphicon, Button, Panel } from 'react-bootstrap'

import { connect } from 'react-redux'

import * as Actions from '../model/Actions.jsx'


@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcPanel extends React.Component {

  onRemovePanel = () => {
    this.props.dispatch( Actions.appConfirm( 'remove_panel' ) )
  }

  render() {

    var title = (
      <div>
        Ac Panel
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

