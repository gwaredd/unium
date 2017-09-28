//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Alert, Nav, Navbar, NavItem, MenuItem, NavDropdown, Panel, Glyphicon } from 'react-bootstrap';


// connect store values ...
@connect( (store) => {
  return {
    minions: store.minions
  }
})
export default class App extends React.Component {

  onConnect = () => {
    this.props.dispatch({type:'OVERLORD_CONNECT'})
  }

  onDisconnect = () => {
    this.props.dispatch({type:'OVERLORD_DISCONNECT'})
  }

  render() {

    var minions = this.props.minions

    return (
      <div>

        <Navbar>
          <Navbar.Header>
            <Navbar.Brand>
              <a href="#">Unium: Overlord</a>
            </Navbar.Brand>
          </Navbar.Header>
          <Navbar.Collapse style={{marginRight: '10px'}}>
            <Nav pullRight>
                { minions.connected ? ( 
                  <NavItem onClick={this.onDisconnect}>
                    Connected &nbsp;
                    <Glyphicon glyph='ok-sign'/>
                  </NavItem>
                ):(
                  <NavItem onClick={this.onConnect}>
                    Not connected &nbsp;
                    <Glyphicon glyph='remove-sign'/>
                  </NavItem>
                )}
            </Nav>
          </Navbar.Collapse>
        </Navbar>

        <Panel header="Minions" bsStyle="primary">
          Panel content
        </Panel>

      </div>
    );
  }
}


