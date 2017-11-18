//-------------------------------------------------------------------------------

import React from 'react'

import {
  Modal,
  Button,
  Form,
  FormGroup,
  Col,
  FormControl,
  ControlLabel,
  DropdownButton,
  InputGroup,
  MenuItem,
  Panel,
  HelpBlock
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

      if( this.state.name != '' ) {
        dialog.callback( this.state )
      }

      this.state = { name: '' }
      onCancel()
    }
        
    return (
      <Modal show={true} onHide={onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>Create a New Tab</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <Form horizontal onSubmit={onOK} >
            <FormGroup controlId="formName">
              <Col componentClass={ControlLabel} sm={2}>
                Name
              </Col>
              <Col sm={10}>
                <FormControl
                  type="text"
                  value={this.state.name}
                  onChange={this.onChangeName}
                  autoFocus={true}
                />
              </Col>
            </FormGroup>

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={onCancel}>Cancel</Button>
            <Button bsStyle="success" onClick={onOK}>Create Tab</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
