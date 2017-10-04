//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Nav, Navbar, NavItem, Glyphicon } from 'react-bootstrap'
import * as actions from './model/actions.jsx'

//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    minions: store.minions
  }
})
export default class Menu extends React.Component {

  onConnect = () => {
    this.props.dispatch( actions.ovConnect() )
  }

  onDisconnect = () => {
    this.props.dispatch( actions.ovDisconnect() )
  }

  render() {

    var minions = this.props.minions

    return (

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
                    Connected &nbsp
                    <Glyphicon glyph='ok-sign'/>
                  </NavItem>
                ):(
                  <NavItem onClick={this.onConnect}>
                    Not connected &nbsp
                    <Glyphicon glyph='remove-sign'/>
                  </NavItem>
                )}
            </Nav>
          </Navbar.Collapse>
        </Navbar>
    )
  }
}
