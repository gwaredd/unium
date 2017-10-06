//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Tabs, Tab, Glyphicon } from 'react-bootstrap'
import _ from 'lodash'

import AcGrid from './AcGrid.jsx'
import * as Actions from '../model/Actions.jsx'


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
    var action = Actions.tabRemove( this.state.curId )
    this.props.dispatch( action )
  }

  onRemoveTab = () => {
    var action = Actions.appConfirm(
      'Remove Tab',
      'Are you sure you want to remmove the tab',
      this.onConfirmRemoveTab
    )

    this.props.dispatch( action )
  }

  onAddTabConfirm = () => {
    var id = _.max( _.keys( this.props.tabs.byId ) ) + 1
    const action = Actions.tabAdd( id, 'test' )
    return this.props.dispatch( action )
  }

  onEnterTab = (id) => {
    this.setState({ curId: id })
  }

  onSelectTab = (key) => {
    if( key == 999999 ) {
      const action = Actions.appDialogAdd( 'Add Tab', this.onAddTabConfirm )
      return this.props.dispatch( action )
    }
    this.setState({ curKey: key })
  }

  //-------------------------------------------------------------------------------

  title = (name,index) => {
    if( this.state.curKey == index ) {
      return (
        <span>
          { name } &nbsp; <Glyphicon glyph="remove" style={{fontSize:'10px'}} onClick={ this.onRemoveTab }/>
        </span>
      )
    } else {
      return <span>{name}</span>
    }
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
