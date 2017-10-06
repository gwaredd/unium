//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Tabs, Tab, Glyphicon } from 'react-bootstrap'

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
      curIndex  : 0,
      curId     : 0
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

  onEnterTab = (index, id) => {
    this.setState({
      curIndex  : index,
      curId     : id
    })
  }

  //-------------------------------------------------------------------------------

  title = (name,index) => {
    if( this.state.curIndex == index ) {
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
        onEnter = { () => { this.onEnterTab( index, id ) } }
        mountOnEnter
      >
        <AcGrid tabId={id} />
      </Tab>
    )
  }

  //-------------------------------------------------------------------------------

  componentWillMount() {
  }

  render() {
    return (
      <Tabs animation={true} id="tabs" className='acTabs'>
        { Object.keys( this.props.tabs.byId ).map( this.createTab ) }
        <Tab eventKey={999999} title="+" />
      </Tabs>      
    )
  }
}
