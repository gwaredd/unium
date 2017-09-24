//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Alert, Nav, Navbar, NavItem, MenuItem, NavDropdown, Panel } from 'react-bootstrap';


// connect store values ...
@connect( (store) => {
  return {
    minions: store.minions
  }
})
export default class App extends React.Component {

  componentWillMount() {
    // dispatch?
    //this.props.dispatch()
  }

  render() {
    return (
      <div>
        <Navbar>
          <Navbar.Header>
            <Navbar.Brand>
              <a href="#">Unium: Overlord</a>
            </Navbar.Brand>
          </Navbar.Header>
        </Navbar>
        <Panel header="Minions" bsStyle="primary">
          Panel content
        </Panel>
      </div>
    );
  }
}


