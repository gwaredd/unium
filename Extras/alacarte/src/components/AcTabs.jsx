//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Tabs, Tab, Glyphicon } from 'react-bootstrap'
import _ from 'lodash'

import AcGrid from './AcGrid.jsx'

import * as App from '../actions/App.jsx'
import * as Actions from '../actions/Tabs.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    tabs: store.tabs
  }
})
export default class AcTabs extends React.Component {

  constructor( props ) {
    super( props )
    this.state = {
      curKey  : 0,
      curId   : 0
    }
  }

  //-------------------------------------------------------------------------------

  onConfirmRemoveTab = () => {
    this.props.dispatch( Actions.TabRemove( this.state.curId ) )
  }

  onRemoveTab = () => {
    
    var id = this.state.curId
    var tab = this.props.tabs.byId[ id ]

    var action = App.Confirm(
      'Remove Tab',
      "Are you sure you want to remove '" + tab.name + "'",
      this.onConfirmRemoveTab
    )

    this.props.dispatch( action )
  }

  onAddTabConfirm = ( s ) => {
    var keys = _.keys( this.props.tabs.byId )
    var id = _.max( keys ) + 1
    this.props.dispatch( Actions.TabCreate( id, s.name ) )
  }

  onEnterTab = (id) => {
    this.setState({ curId: id })
  }

  onSelectTab = (key) => {
    if( key == 999999 ) {
      const action = App.AddTab( this.onAddTabConfirm )
      return this.props.dispatch( action )
    }
    this.setState({ curKey: key })
  }

  //-------------------------------------------------------------------------------

  title = (name,index) => {
    if( this.state.curKey != index ) {
      return <span>{name}</span>
    }

    return (
      <span>
        { name } &nbsp; <Glyphicon glyph="remove" style={{fontSize:'10px'}} onClick={ this.onRemoveTab }/>
      </span>
    )
  }

  createTab = (id, index) => {

    var tab = this.props.tabs.byId[ id ]

    return (
      <Tab
        key     = { index }
        eventKey= { index }
        title   = { this.title( tab.name, index ) }
        onEnter = { () => { this.onEnterTab( id ) } }
        mountOnEnter
      >
        <AcGrid tabId={id} />
      </Tab>
    )
  }

  //-------------------------------------------------------------------------------

  render() {
    return (
      <Tabs
        id        = "tabs"
        className = 'acTabs'
        animation = {true}
        onSelect  = {this.onSelectTab}
        activeKey = {this.state.curKey}
      >
        { Object.keys( this.props.tabs.byId ).map( this.createTab ) }
        <Tab eventKey={999999} title="+" />
      </Tabs>      
    )
  }
}
