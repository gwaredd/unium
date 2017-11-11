//-------------------------------------------------------------------------------

import React from 'react'
import Axios from 'axios'

import {
  Alert,
  Modal,
  Button
} from 'react-bootstrap'


//-------------------------------------------------------------------------------

export default class AcModalScreenshot extends React.Component {

  render() {

    const { dialog, onCancel } = this.props

    const onSuccess = (res) => console.log( res )
    const onError = (err) => console.log( err )
    
    Axios.get( 'http://localhost:8342/utils/screenshot' )
      .then( onSuccess )
      .catch( onError )
  
    
    return (
      <Modal show={true} bsSize="large" onHide={onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>Screenshot</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Alert bsStyle="info">
              <h4>Taking Screenshot</h4>
              <p>Please wait ...</p>
            </Alert>
          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={onCancel}>Save</Button>
            <Button bsStyle="success" onClick={onCancel}>Close</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}

