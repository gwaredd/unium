//-------------------------------------------------------------------------------

import React from 'react'
import { Glyphicon, Button, Panel } from 'react-bootstrap'

export default class AcPanel extends React.Component {

  render() {

    var title = (
      <div>
        Ac Panel
        <div className='pull-right'>
          <Glyphicon className='acPanelIcon' glyph="remove" style={{fontSize:'10px'}} onClick={ () => {alert('ok')}}/>
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

