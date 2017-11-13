//-------------------------------------------------------------------------------

import React from 'react'
import _ from 'lodash'

import {
  Navbar,
  Glyphicon,
  PanelGroup,
  Button,
  Panel
} from 'react-bootstrap'

import * as App from '../../actions/App.jsx'

import { connect } from 'react-redux'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    output  : store.output
  }
})
export default class AcOutput extends React.Component {

  render() {

    const { output } = this.props

    var contents = null

    if( output.length > 0 ) {
      contents = _.map( output, (o,i) =>  <p key={i} className={'text-' + o.type}>{ o.text }</p> )
    } else {
      contents = "No output"
    }    

   var title = (
      <div className='clearfix'>
        Output
        <div className='pull-right'>
          <Glyphicon glyph="chevron-up" />
          <Glyphicon glyph="chevron-down" />
        </div>
      </div>
    )

    return (
      <div className='debugOutput'>
        <Panel collapsible header={title} eventKey="1" style={{margin:'0px'}}>
          { contents }
        </Panel>
      </div>
   )
  }
}
