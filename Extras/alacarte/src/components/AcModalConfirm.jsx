//-------------------------------------------------------------------------------

import React from 'react'
import { Modal, Button } from 'react-bootstrap'
import { connect } from 'react-redux'

import * as Actions from '../model/Actions.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcModalConfirm extends React.Component {

  onClose = () => {
    this.props.dispatch( Actions.appCancelDialog() )
  }

  render() {

    var app = this.props.app

    return (
      <Modal show={app.confirm!=null} bsSize="large" onHide={this.onClose}>
          <Modal.Header closeButton>
            <Modal.Title>Confirm ...</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <h4>Text in a modal</h4>
          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={this.onClose}>Cancel</Button>
            <Button bsStyle="danger">OK</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}

