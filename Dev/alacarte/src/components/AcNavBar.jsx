//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'
import { Navbar, Nav, NavItem, NavDropdown, MenuItem, Glyphicon } from 'react-bootstrap'

export default class AcNavBar extends React.Component {

  render() {
    return (
      <Navbar fixedTop inverse>
        <Navbar.Header>
          <Navbar.Brand>
            Unium: À La Carte
          </Navbar.Brand>
        </Navbar.Header>
        <Nav pullRight>
          <NavItem eventKey={4} href="#">
            <Glyphicon glyph='lock'/>
          </NavItem>
          <NavItem eventKey={5} href="#">
            <Glyphicon glyph='camera'/>
          </NavItem>
          <NavItem eventKey={6} href="#">
            <Glyphicon glyph='plus'/>
          </NavItem>
        </Nav>
      </Navbar>
    )
  }
}

