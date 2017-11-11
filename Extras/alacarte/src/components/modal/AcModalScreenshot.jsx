//-------------------------------------------------------------------------------

import React from 'react'
import { Modal, Button } from 'react-bootstrap'


//-------------------------------------------------------------------------------

export default class AcModalScreenshot extends React.Component {

  render() {

    const { dialog, onCancel } = this.props
    
    return (
      <Modal show={true} bsSize="large" onHide={onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>Take Screenshot</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            TODO: do screenshot
          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={onCancel}>Save</Button>
            <Button bsStyle="success" onClick={onCancel}>Close</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}

