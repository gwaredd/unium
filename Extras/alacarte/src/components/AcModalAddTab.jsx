//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'

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


import { CancelDialog } from '../actions/App.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcModalAddTab extends React.Component {

  onCancel = () => {
    this.props.dispatch( CancelDialog() )
  }

  onOK = () => {
    if( this.state.name != '' ) {
      this.props.app.dialog.callback( this.state )
      this.state = {name:''}
    }
    this.onCancel()
  }


  //-------------------------------------------------------------------------------

  constructor( props ) {
    super( props )
    this.state = {
      name  : ''
    }
  }

  onChangeName = (e) => {
    this.setState({ name: e.target.value });
  }


  //-------------------------------------------------------------------------------

  render() {

    var { app } = this.props
    var { dialog } = app

    if( dialog == null || dialog.modal != "addTab" ) {
      return null
    }
    
    return (
      <Modal show={true} onHide={this.onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>Create a New Tab</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <Form horizontal>
            <FormGroup controlId="formName">
              <Col componentClass={ControlLabel} sm={2}>
                Name
              </Col>
              <Col sm={10}>
                <FormControl
                  type="text"
                  value={this.state.name}
                  onChange={this.onChangeName}
                />
              </Col>
            </FormGroup>

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={this.onCancel}>Cancel</Button>
            <Button bsStyle="success" onClick={this.onOK}>Create Tab</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
