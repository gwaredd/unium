//-------------------------------------------------------------------------------

import React from 'react'
import { Tabs, Tab, Glyphicon } from 'react-bootstrap'

import AcGrid from './AcGrid.jsx'

import { connect } from 'react-redux'

import * as Actions from '../model/Actions.jsx'


@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcTabs extends React.Component {

  constructor( props ) {
    super( props )
    this.state = {
      active: 1
    }
  }

  onRemoveTab = () => {
    this.props.dispatch( Actions.appConfirm( 'remove_tab' ) )
  }

  onEnterTab = (index) => {
    this.setState({active:index})
  }

  title = (name,index) => {
    if( this.state.active == index ) {
      return (
        <span>
          {name} &nbsp; <Glyphicon glyph="remove" style={{fontSize:'10px'}} onClick={this.onRemoveTab}/>
        </span>
      )
    } else {
      return <span>{name}</span>
    }
  }

  render() {

    return (

      <Tabs animation={true} id="tabs" className='acTabs'>
        <Tab eventKey={1} title={this.title('Tab 1',1)} mountOnEnter onEnter={()=>{this.onEnterTab(1)}}>
          <AcGrid/>
        </Tab>
        <Tab eventKey={2} title={this.title('Tab 2',2)} mountOnEnter onEnter={()=>{this.onEnterTab(2)}}>
          <AcGrid items={20}/>
        </Tab>
        <Tab eventKey={3} title={this.title('Tab 3',3)} mountOnEnter onEnter={()=>{this.onEnterTab(3)}}>
          <AcGrid/>
        </Tab>
        <Tab eventKey={4} title="+">
        </Tab>
      </Tabs>      
    )
  }
}
