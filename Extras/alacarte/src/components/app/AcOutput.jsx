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

const EventListenerMode = { capture: true }


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app     : store.app,
    output  : store.output
  }
})
export default class AcOutput extends React.Component {

  constructor(...args) {
    
    super(...args)

    this.state = {
      locked: false
    }

    this.dragOrigin    = 0
    this.height        = 0
    this.dragTimestamp = 0
  }


  componentDidUpdate () {
    if( !this.state.locked ) {
      var el = this.refs.output
      el.scrollTop = el.scrollHeight
    }
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


  //-------------------------------------------------------------------------------
  // draggable height

  onToggle = (e) => {
    this.height = this.height > 10 ? 0 : 300
    this.refs.output.style.height = this.height + 'px'
  }

  onMouseDown = (e) => {
    document.body.style['pointer-events'] = 'none'
    document.addEventListener( 'mouseup', this.onMouseUp, EventListenerMode )
    document.addEventListener( 'mousemove', this.onMouseMove, EventListenerMode )
    e.preventDefault()
    e.stopPropagation()

    this.dragOrigin    = e.screenY
    this.dragTimestamp = Date.now()
  }
    
  onMouseMove = (e) => {

    e.stopPropagation()

    const delta = this.dragOrigin - e.screenY
    this.dragOrigin = e.screenY
    this.height = Math.max( this.height + delta, 0 )

    this.refs.output.style.height = this.height + 'px'
  }

  onMouseUp = (e) => {
    document.body.style['pointer-events'] = 'auto'
    document.removeEventListener( 'mouseup', this.onMouseUp, EventListenerMode )
    document.removeEventListener( 'mousemove', this.onMouseMove, EventListenerMode )
    e.stopPropagation()

    if( Date.now() - this.dragTimestamp < 200 ) {
      this.onToggle()
    }
  }

  //-------------------------------------------------------------------------------
  
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
        <div className='acOutputTitle'>
          <span style={{width: "110px"}}>
            <small>
              { isConnected ?
                <Label bsStyle="success" onClick={this.onDisconnect}>Connected</Label>
              :
                <Label bsStyle="warning" onClick={this.onConnect}>Not Connected</Label>
              }
            </small>
          </span>
          <span onMouseDown={this.onMouseDown} onClick={this.onToggle} style={{width:'100%'}}>
            Output
          </span>
        
          <div style={{width: '60px'}}>
            <FontAwesome name='trash' onClick={this.onClear} />
            &nbsp;
            <FontAwesome
              className={ this.state.locked ? 'text-danger' : 'text-info' }
              name={ this.state.locked ? 'lock' : 'unlock' }
              onClick={this.onLock}
            />
            &nbsp;          
          </div>
        </div>

        <div className='acOutputContent'>
          <div ref='output' style={{height: this.height + 'px'}}>
            { contents }
          </div>
        </div>
      </div>
   )
  }
}
