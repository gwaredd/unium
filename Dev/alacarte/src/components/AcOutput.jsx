//-------------------------------------------------------------------------------

import React from 'react'
import { Navbar, Glyphicon, PanelGroup, Button, Panel } from 'react-bootstrap'

export default class AcPanel extends React.Component {

  render() {

    var title = (
      <div className='clearfix'>
        Debug Output
        <div className='pull-right'>
          <Glyphicon glyph="chevron-up" />
          <Glyphicon glyph="chevron-down" />
        </div>
      </div>
    )

    return (
      <Navbar fixedBottom>
        <Panel collapsible header={title} eventKey="1" style={{margin:'0px'}}>Panel 1 content</Panel>
      </Navbar>
   )
  }
}
