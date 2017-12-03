//-------------------------------------------------------------------------------

import React from 'react'
import FontAwesome from 'react-fontawesome'

import * as Connection from '../../actions/Connection.jsx'
import * as Log from '../../actions/Logging.jsx'

import _ from 'lodash'

import {
  Navbar,
  Glyphicon,
  PanelGroup,
  Button,
  Panel,
  Collapse,
  Label
} from 'react-bootstrap'

import * as App from '../../actions/App.jsx'

import { connect } from 'react-redux'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app     : store.app,
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

  onExpand = (e) => {
    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    this.setState({size: this.state.size + 1})
  }

  onCollapse = (e) => {
    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    this.setState({size: this.state.size - 1})
  }

  onToggle = (e) => {
    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    this.setState({size: this.state.size <= 0 ? 1 : this.state.size - 1 })
  }

  onLock = (e) => {
    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    this.setState({locked: !this.state.locked })
  }

  onClear = (e) => {
    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    this.props.dispatch( Log.Clear() )
  }

  onConnect = (e) => {
    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    this.props.dispatch( Connection.Connect() )
  }

  onDisconnect = () => {
    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()
    this.props.dispatch( Connection.Disconnect() )
  }
    
  render() {

    const { output, app } = this.props
    const isConnected = app.connected
    

    var contents = null

    if( output.length > 0 ) {
      contents = _.map( output, (o,i) =>  <pre key={i} className={'text-' + o.type}>[{o.timestamp}] { o.text }</pre> )
    } else {
      contents = "No output"
    }    

    return (

      <div className='acOutput'>

        <div className='acOutputTitle' onClick={this.onToggle}>
          <small>
            { isConnected ?
              <Label bsStyle="success" onClick={this.onDisconnect}>Connected</Label>
            :
              <Label bsStyle="warning" onClick={this.onConnect}>Not Connected</Label>
            }
          </small>
          &nbsp;
          Output
        
          <div className='pull-right'>
            <FontAwesome name='trash' onClick={this.onClear} />
            &nbsp;
            <FontAwesome
              className={ this.state.locked ? 'text-danger' : 'text-info' }
              name={ this.state.locked ? 'lock' : 'unlock' }
              onClick={this.onLock}
            />
            &nbsp;
          
            { this.state.size < 2 && 
              <Glyphicon glyph="chevron-up" onClick={this.onExpand} />
            }
            { this.state.size > 0 && 
              <Glyphicon glyph="chevron-down" onClick={this.onCollapse} />
            }
          </div>
        </div>

        <div className={'acOutputContent acOutputSize' + this.state.size}>
          <div ref='output'>
            { contents }
          </div>
          {/* <div>
            <input type="text" />
          </div> */}
        </div>
      </div>
   )
  }
}
