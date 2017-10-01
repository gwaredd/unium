//-------------------------------------------------------------------------------

import React from 'react'
import { Tabs, Tab } from 'react-bootstrap'

import AcGrid from './AcGrid.jsx'

export default class AcTabs extends React.Component {

  render() {
    return (
      <Tabs animation={true}>
        <Tab eventKey={1} title="Tab 1" mountOnEnter>
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
