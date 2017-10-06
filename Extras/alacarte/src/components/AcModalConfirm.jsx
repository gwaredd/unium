//-------------------------------------------------------------------------------

import React from 'react'
import { Modal, Button } from 'react-bootstrap'
import { connect } from 'react-redux'

import { appCancelDialog } from '../model/Actions.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcModalConfirm extends React.Component {

  onCancel = () => {
    this.props.dispatch( appCancelDialog() )
  }

  onOK = () => {
    this.props.app.confirm.callback()
    this.onCancel()
  }

  render() {

    var app = this.props.app
    var { confirm } = app

    if( confirm == null ) {
      return <div/>
    }

    return (
      <Modal show={true} bsSize="large" onHide={this.onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>{ confirm.title }</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <h4>{ confirm.question }</h4>
          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={this.onCancel}>Cancel</Button>
            <Button bsStyle="danger" onClick={this.onOK}>OK</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
