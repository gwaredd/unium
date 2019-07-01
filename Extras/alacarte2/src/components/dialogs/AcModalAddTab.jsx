//-------------------------------------------------------------------------------

import React from 'react'

import {
  Modal,
  Button,
  Form,
  Col,
  Row
} from 'react-bootstrap'


//-------------------------------------------------------------------------------

export default class AcModalAddTab extends React.Component {

  constructor( props ) {
    super( props )
    this.state = {
      name  : ''
    }
  }

  onChangeName = (e) => { this.setState({ name: e.target.value }); }


  //-------------------------------------------------------------------------------

  render() {

    const { dialog, onCancel } = this.props
    
    const onOK = (e) => {

      e.preventDefault()

      if( this.state.name !== '' ) {
        dialog.callback( this.state )
      }

      this.setState({
        name: ''
      })

      onCancel()
    }
        
    return (
      <Modal show={true} onHide={onCancel}>
        <Modal.Header closeButton>
          <Modal.Title>Create a New Tab</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          <Form onSubmit={onOK} >
            <Form.Group as={Row} controlId="formName">
              <Form.Label column sm='2'>
                Name
              </Form.Label>
              <Col sm={10}>
                <Form.Control
                  type="text"
                  value={this.state.name}
                  onChange={this.onChangeName}
                  autoFocus={true}
                />
              </Col>
            </Form.Group>
          </Form>
        </Modal.Body>
        <Modal.Footer>
          <Button variant="default" onClick={onCancel}>Cancel</Button>
          <Button variant="success" onClick={onOK}>Create Tab</Button>
        </Modal.Footer>          
      </Modal>
    )
  }
}
