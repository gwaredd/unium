//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'
import { Navbar, Nav, NavItem, NavDropdown, MenuItem, Glyphicon } from 'react-bootstrap'

import * as Actions from '../actions/App.jsx'
import * as ActionsTab from '../actions/Tabs.jsx'

import { connect } from 'react-redux'

//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcNavBar extends React.Component {

  //-------------------------------------------------------------------------------

  onScreenshot = () => {
    this.props.dispatch( Actions.Screenshot() )
  }

  onAddPanelConfirm = (p) => {
    //ActionsTab.PanelCreate()
    console.log( p )
  }

  onAddPanel = () => {
    this.props.dispatch( Actions.AddPanel( this.onAddPanelConfirm ) )
  }


  //-------------------------------------------------------------------------------

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
            <Glyphicon glyph='cog'/>
          </NavItem>
          <NavItem eventKey={5} onClick={this.onScreenshot}>
            <Glyphicon glyph='camera'/>
          </NavItem>
          <NavItem eventKey={6} onClick={this.onAddPanel}>
            <Glyphicon glyph='plus'/>
          </NavItem>
        </Nav>
      </Navbar>
    )
  }
}

