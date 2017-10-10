//-------------------------------------------------------------------------------

import React from 'react'
import { Modal, Button } from 'react-bootstrap'
import { connect } from 'react-redux'

import { CancelDialog } from '../actions/App.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcModalConfirm extends React.Component {

  onCancel = () => {
    this.props.dispatch( CancelDialog() )
  }

  onOK = () => {
    this.props.app.dialog.callback( this.props.app.dialog.data )
    this.onCancel()
  }

  render() {

    var { app } = this.props
    var { dialog } = app

    if( dialog == null || dialog.modal != "confirm" ) {
      return null
    }

    return (
      <Modal show={true} bsSize="large" onHide={this.onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>{ dialog.title }</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <h4>{ dialog.question }</h4>
          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={this.onCancel}>Cancel</Button>
            <Button bsStyle="danger" onClick={this.onOK}>Confirm</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
