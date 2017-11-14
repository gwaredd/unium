//-------------------------------------------------------------------------------

import React from 'react'
import _ from 'lodash'

import {
  Navbar,
  Glyphicon,
  PanelGroup,
  Button,
  Panel,
  Collapse
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

  constructor(...args) {
    super(...args);

    this.state = {
      size: 0,
      locked: false
    }
  }


  componentDidUpdate () {
    if( !this.state.locked ) {
      var el = this.refs.output;
      el.scrollTop = el.scrollHeight;
    }
  }

  onExpand = () => {
    this.setState({size: this.state.size + 1})
  }

  onCollapse = () => {
    this.setState({size: this.state.size - 1})
  }

  onToggle = () => {
    this.setState({size: this.state.size <= 0 ? 1 : this.state.size - 1 })
  }

  onLock = () => {
    this.setState({locked: !this.state.locked })
  }

    
  render() {

    const { output } = this.props

    var contents = null

    if( output.length > 0 ) {
      contents = _.map( output, (o,i) =>  <p key={i} className={'text-' + o.type}>[{o.timestamp}] { o.text }</p> )
    } else {
      contents = "No output"
    }    

    const locked = this.state.locked ? 'danger' : 'info'

    return (

      <div className='acOutput'>

        <div className='acOutputH'>
          <Glyphicon className={'text-' + locked} glyph="lock" onClick={this.onLock} />
          &nbsp;
          <span onClick={this.onToggle}>
            Output
          </span>
          <div className='pull-right'>
            { this.state.size < 2 && 
              <Glyphicon glyph="chevron-up" onClick={this.onExpand} />
            }
            { this.state.size > 0 && 
              <Glyphicon glyph="chevron-down" onClick={this.onCollapse} />
            }
          </div>
        </div>

        <div ref='output' className={'acOutputX acOutput' + this.state.size}>
          { contents }
        </div>
      </div>
   )
  }
}
