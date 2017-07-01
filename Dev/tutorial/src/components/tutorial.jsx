import React from 'react'
import { Button, ListGroup, ListGroupItem, Panel } from 'react-bootstrap'

import TutorialServer from './tutorial_server.jsx'
import TutorialRoutes from './tutorial_routes.jsx'
import TutorialGQL from './tutorial_gql.jsx'
import TutorialGQL2 from './tutorial_gql2.jsx'
import TutorialSockets from './tutorial_sockets.jsx'
import TutorialExamples from './tutorial_examples.jsx'

const pages = [
  { name: 'Intro',        component: null },
  { name: 'Web Server',   component: React.createFactory( TutorialServer ) },
  { name: 'Routes',       component: React.createFactory( TutorialRoutes ) },
  { name: 'GQL Queries',  component: React.createFactory( TutorialGQL ) },
  { name: 'GQL Actions',  component: React.createFactory( TutorialGQL2 ) },
  { name: 'WebSockets',   component: React.createFactory( TutorialSockets ) },
  { name: 'Examples',     component: React.createFactory( TutorialExamples ) },
]

export default class Tutorial extends React.Component {

  constructor( props ) {
    super( props )
  }

  MenuItem(name,index) {
    return <ListGroupItem key={index} onClick={()=>this.props.setPage(index)} active={index==0} bsStyle={index==this.props.page ? 'success' : null}>{name}</ListGroupItem>
  }

  Next() {
    this.props.setPage( this.props.page + 1 )
  }
  
  render() {

    const page = pages[ this.props.page ];

    return (
      <div className='container'>
        <div className="row">
          <div className="col-sm-2">
            <ListGroup>
              { pages.map( (p,i) => this.MenuItem( p.name, i ) ) }
            </ListGroup>
          </div>
          <div className="col-sm-10">
            <Panel header={ page.name } bsStyle="primary">
              { page.component( this.props ) }
              { this.props.page != pages.length - 1 && (
                <div>
                  <hr/>
                  <Button className="pull-right" bsStyle="success" onClick={()=>this.Next()}>Next</Button>
                </div>
              )}
            </Panel>
          </div>
        </div>
      </div>
    )
  }
}

