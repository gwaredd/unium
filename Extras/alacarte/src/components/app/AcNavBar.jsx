//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'
import FontAwesome from 'react-fontawesome'

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
    const id      = keys.length == 0 ? 1 : _.max( keys ) + 1    
    const payload = {...d, id: id, tab: tab }

    dispatch( ActionsTab.PanelCreate( payload ) )
  }

  onScreenshot  = () => this.props.dispatch( Actions.Screenshot() )
  onAddPanel    = () => this.props.dispatch( Actions.AddPanel( this.onAddPanelConfirm ) )
  onSave        = () => this.props.dispatch( Actions.Save() )


  //-------------------------------------------------------------------------------

  render() {

    return (
      <Navbar fixedTop>
        <Navbar.Header>
          <Navbar.Brand>
            Unium: À La Carte
          </Navbar.Brand>
          <Navbar.Toggle />
        </Navbar.Header>
        <Navbar.Collapse>
          <Nav pullRight>
            <NavItem eventKey={3} onClick={this.onSave}>
              <FontAwesome name='floppy-o' />
            </NavItem>
            <NavItem eventKey={3} onClick={this.onScreenshot}>
              <FontAwesome name='camera' />
            </NavItem>
            <NavItem eventKey={4} onClick={this.onAddPanel}>
              <FontAwesome name='plus' />
            </NavItem>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    )
  }
}

