//-------------------------------------------------------------------------------

import React from 'react'

import {
  Alert,
  Modal,
  Button
} from 'react-bootstrap'


//-------------------------------------------------------------------------------

export default class AcModalScreenshot extends React.Component {

  constructor( props ) {
    super( props )
    this.state = {
      loaded  : false,
      error   : null,
      data    : null
    }
  }

  onSuccess = (e) => { this.setState({ loaded: true, error: null }) }
  onError   = (e) => { this.setState({ loaded: false, error: "Failed to load image" }) }

  render() {

    const { dialog, appConfig, onCancel } = this.props

    const src = appConfig.api + '/utils/screenshot'
    
    return (
      <Modal show={true} bsSize="large" onHide={onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>Screenshot</Modal.Title>
          </Modal.Header>
          <Modal.Body>

            { !this.state.loaded && this.state.error == null &&
              <Alert bsStyle="info">
                <h4>Taking Screenshot</h4>
                <p>Please wait ...</p>
              </Alert>
            }

            { !this.state.loaded && this.state.error != null &&
              <Alert bsStyle="danger">
                <h4>Failed</h4>
                <p>{ this.state.error }</p>
              </Alert>
            }

            <center>
              <img
                src={src}
                onLoad={this.onSuccess}
                onError={this.onError}
              />
            </center>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="success" onClick={onCancel}>Close</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}

