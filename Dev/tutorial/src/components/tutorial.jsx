import React from 'react'
import { ListGroup, ListGroupItem, Panel } from 'react-bootstrap'

import TutorialServer from './tutorial_server.jsx'

const pages = [
  { name: 'Intro',      component: React.createFactory( TutorialServer ) },
  { name: 'Web Server', component: React.createFactory( TutorialServer ) },
  { name: 'Routes',     component: React.createFactory( TutorialServer ) },
  { name: 'GQL',        component: React.createFactory( TutorialServer ) },
  { name: 'WebSockets', component: React.createFactory( TutorialServer ) },
]

export default class Tutorial extends React.Component {

  constructor( props ) {
    super( props )
  }

  MenuItem(name,index) {
    return <ListGroupItem key={index} onClick={()=>this.props.setPage(index)} active={index==0} bsStyle={index==this.props.page ? 'success' : null}>{name}</ListGroupItem>
  }
  
  render() {
    return (
      <div className="row">
        <div className="col-md-2">
          <ListGroup>
            { pages.map( (p,i) => this.MenuItem( p.name, i ) ) }
          </ListGroup>
        </div>
        <div className="col-md-10">
          <Panel header={ pages[ this.props.page ].name } bsStyle="primary">
            { pages[ this.props.page ].component() }
          </Panel>
        </div>
      </div>
    )
  }
}

