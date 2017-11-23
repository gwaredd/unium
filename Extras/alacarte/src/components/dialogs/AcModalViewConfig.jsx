//-------------------------------------------------------------------------------

import React from 'react'
import FontAwesome from 'react-fontawesome'
import Store from '../../model/Store.jsx'

import {
  Modal,
  Button,
} from 'react-bootstrap'


import * as App from '../../actions/App.jsx'


//-------------------------------------------------------------------------------

export default class AcModalViewConfig extends React.Component {

  render() {

    const { dialog, onCancel } = this.props

    var data = { ...Store.getState() }
    
    delete data[ 'output' ]
    data.app = _.pick( data.app, 'settings' )

    return (
      <Modal show={true} onHide={onCancel} bsSize="large">
          <Modal.Header closeButton>
            <Modal.Title>Settings</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <pre>
            { JSON.stringify( data, null, 2 ) }
          </pre>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="success" onClick={onCancel}>OK</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
