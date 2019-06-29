//-------------------------------------------------------------------------------

import React from 'react'
import FontAwesome from 'react-fontawesome'
import _ from 'lodash'

import * as Actions from '../../actions/App'
import * as ActionsTab from '../../actions/Tabs'

import { connect } from 'react-redux'

import { 
  Navbar,
  Nav
} from 'react-bootstrap'


//-------------------------------------------------------------------------------

class AcNavBar extends React.Component {

  onAddPanelConfirm = ( d ) => {

    const { dispatch, panels, tabs } = this.props
    const tab = tabs.state.curTab

    const keys    = _.map( _.keys( panels.byId ), (k) => parseInt(k) )
    const id      = keys.length === 0 ? 1 : _.max( keys ) + 1    
    const payload = {...d, id: id, tab: tab }

    dispatch( ActionsTab.PanelCreate( payload ) )
  }

  onScreenshot  = () => this.props.dispatch( Actions.Screenshot() )
  onAddPanel    = () => this.props.dispatch( Actions.AddPanel( this.onAddPanelConfirm ) )
  onSave        = () => this.props.dispatch( Actions.Save() )


  render() {

    return (
      <Navbar>
        <Navbar.Brand>
          Unium: À La Carte
        </Navbar.Brand>
        <Navbar.Toggle />
        <Navbar.Collapse>
          <Nav  className="justify-content-end">
            <Nav.Link eventKey={3} onClick={this.onSave}>
              <FontAwesome name='floppy-o' />
            </Nav.Link>
            <Nav.Link eventKey={3} onClick={this.onScreenshot}>
              <FontAwesome name='camera' />
            </Nav.Link>
            <Nav.Link eventKey={4} onClick={this.onAddPanel}>
              <FontAwesome name='plus' />
            </Nav.Link>
          </Nav>
        </Navbar.Collapse>
      </Navbar>
    )
  }
}

const mapStateToProps = ( state, ownProps ) => {
  return {
    panels : state.panels,
    tabs   : state.tabs
  }
}

export default connect( mapStateToProps )( AcNavBar );

