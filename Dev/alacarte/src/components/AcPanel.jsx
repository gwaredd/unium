//-------------------------------------------------------------------------------

import React from 'react'
import { Glyphicon, Button, Panel } from 'react-bootstrap'

export default class AcPanel extends React.Component {

  render() {

    var title = (
      <span className='clearfix'>
        <span>
          Ac Panel
        </span>
        <Button className='pull-right' bsStyle="primary" bsSize="small" onClick={() => {alert('do stuff')}}>
          <Glyphicon glyph="remove" />
        </Button>
      </span>
    )

    return (
      <Panel className="acPanel" header={title} bsStyle="primary">
        Panel content
      </Panel>
    )
  }
}

