import React from 'react'
import { connect } from 'react-redux'
import { Alert, Nav, Navbar, NavItem, MenuItem, NavDropdown, Panel } from 'react-bootstrap';

// https://youtu.be/nrg7zhgJd4w
// redux-websocket? - https://github.com/giantmachines/redux-websocket

// connect store values ...
@connect( (store) => {
  return {
    user: store.user
  }
})
export default class App extends React.Component {

  componentWillMount() {
    // dispatch?
    //this.props.dispatch()
  }

  render() {
    console.log( this.props.user )
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


