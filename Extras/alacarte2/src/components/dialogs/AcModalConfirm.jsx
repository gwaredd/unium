//-------------------------------------------------------------------------------

import React from 'react'
import { Modal, Button } from 'react-bootstrap'


//-------------------------------------------------------------------------------

export default class AcModalConfirm extends React.Component {

  render() {

    const { dialog, onCancel } = this.props

    const onOK = () => {
      dialog.callback( dialog.data )
      onCancel()
    }
  
    return (
      <Modal show={true} bsSize="large" onHide={onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>{ dialog.title }</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <h4>{ dialog.question }</h4>
          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={ onCancel }>Cancel</Button>
            <Button bsStyle="danger" onClick={ onOK }>Confirm</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
