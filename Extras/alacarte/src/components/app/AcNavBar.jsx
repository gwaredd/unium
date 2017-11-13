//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'

import * as Connection from '../../actions/Connection.jsx'
import * as Actions from '../../actions/App.jsx'
import * as ActionsTab from '../../actions/Tabs.jsx'

import { connect } from 'react-redux'

import { 
  Navbar,
  Nav,
  NavItem,
  NavDropdown,
  MenuItem,
  Glyphicon,
  Label
} from 'react-bootstrap'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app    : store.app,
    panels : store.panels,
    tabs   : store.tabs
  }
})
export default class AcNavBar extends React.Component {

  //-------------------------------------------------------------------------------

  onAddPanelConfirm = ( d ) => {

    var { dispatch, panels, tabs } = this.props
    var tab = tabs.state.curTab

    const keys    = _.map( _.keys( panels.byId ), (k) => parseInt(k) )
    const id      = _.max( keys ) + 1
    const payload = {...d, id: id, tab: tab }

    dispatch( ActionsTab.PanelCreate( payload ) )
  }

  onScreenshot  = () => this.props.dispatch( Actions.Screenshot() )
  onAddPanel    = () => this.props.dispatch( Actions.AddPanel( this.onAddPanelConfirm ) )
  onConnect     = () => this.props.dispatch( Connection.Connect() )
  onDisconnect  = () => this.props.dispatch( Connection.Disconnect() )


  //-------------------------------------------------------------------------------

  render() {

    const isConnected = this.props.app.connected

    return (
      <Navbar fixedTop>
        <Navbar.Header>
          <Navbar.Brand>
            Unium: À La Carte
          </Navbar.Brand>
        </Navbar.Header>
        <Nav pullRight>
          { isConnected ?
            <NavItem eventKey={1} onClick={this.onDisconnect}>
              <Label bsStyle="success">Connected</Label>
              </NavItem>
          :
            <NavItem eventKey={2} onClick={this.onConnect}>
              <Label bsStyle="warning">Not Connected</Label>
            </NavItem>
          }
          <NavItem eventKey={3} onClick={this.onScreenshot}>
            <Glyphicon glyph='camera'/>
          </NavItem>
          <NavItem eventKey={4} onClick={this.onAddPanel}>
            <Glyphicon glyph='plus'/>
          </NavItem>
        </Nav>
      </Navbar>
    )
  }
}

