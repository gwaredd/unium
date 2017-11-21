//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Tabs, Tab, Nav, NavItem, Glyphicon, Row, Col } from 'react-bootstrap'
import FontAwesome from 'react-fontawesome'
import _ from 'lodash'

import AcGrid from './AcGrid.jsx'

import * as App from '../../actions/App.jsx'
import * as Actions from '../../actions/Tabs.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    tabs: store.tabs
  }
})
export default class AcTabs extends React.Component {

  onRemoveTab = () => {
    
    var { dispatch, tabs } = this.props
    var curTab = tabs.state.curTab
    var tab = tabs.byId[ curTab ]

    dispatch( App.Confirm(
      'Remove Tab',
      "Are you sure you want to remove '" + tab.name + "'",
      (id) => dispatch( Actions.TabRemove( id ) ),
      curTab
    ))
  }

  //-------------------------------------------------------------------------------

  onAddTabConfirm = ( s ) => {
    
    var { dispatch, tabs } = this.props
    
    var keys  = _.map( _.keys( tabs.byId ), (k) => parseInt(k) )
    var id    = keys.length == 0 ? 1 : _.max( keys ) + 1
        
    dispatch( Actions.TabCreate( id, s.name ) )
  }
    
  onSelectTab = (id) => {

    var { dispatch } = this.props

    if( id == -1 ) {
      dispatch( App.AddTab( this.onAddTabConfirm ) )
    } else {
      dispatch( Actions.TabSelect( id ) )
    }
  }


  //-------------------------------------------------------------------------------

  createTab = ( id ) => {

    var { tabs }    = this.props
    var { curTab }  = tabs.state
    var tab         = tabs.byId[ id ]

    var title = <span>{ tab.name } &nbsp; { id == curTab &&
      <FontAwesome name='times' style={{fontSize:'10px'}} onClick={ this.onRemoveTab }/>
    }
    </span>

    return (
      <Tab
        key     = { id }
        eventKey= { id }
        title   = { title }
        mountOnEnter
      >
        <AcGrid tabId={id} layout={tab.layout} />
      </Tab>
    )
  }


  //-------------------------------------------------------------------------------

  render() {

    var { tabs } = this.props
    var { curTab } = tabs.state

    return (
      <Tabs
        id        = "tabs"
        className = 'acTabs'
        animation = { true }
        onSelect  = { this.onSelectTab }
      >
        { Object.keys( tabs.byId ).map( this.createTab ) }
        <Tab eventKey={-1} title="+" />
      </Tabs>      
    )
  }
}

