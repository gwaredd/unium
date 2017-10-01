//-------------------------------------------------------------------------------

import React from 'react'
import { Tabs, Tab, Glyphicon } from 'react-bootstrap'

import AcGrid from './AcGrid.jsx'

export default class AcTabs extends React.Component {

  render() {

    var title = (
      <div>
        Tab &nbsp; <Glyphicon glyph="remove" style={{fontSize:'10px'}} onClick={ () => {alert('ok')}}/>
      </div>
    )
    
    return (
      <Tabs animation={true} id="tabs" >
        <Tab eventKey={1} title={title} mountOnEnter>
          <AcGrid/>
        </Tab>
        <Tab eventKey={2} title="Tab 2" mountOnEnter>
          <AcGrid/>          
        </Tab>
        <Tab eventKey={3} title="Tab 3" mountOnEnter>
          <AcGrid/>
        </Tab>
        <Tab eventKey={4} title="+">
        </Tab>
      </Tabs>      
    )
  }
}
