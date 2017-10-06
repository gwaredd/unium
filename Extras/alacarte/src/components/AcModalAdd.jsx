//-------------------------------------------------------------------------------

import React from 'react'
import { Modal, Button, Form, FormGroup, Col, FormControl, ControlLabel, DropdownButton, InputGroup, MenuItem, Panel } from 'react-bootstrap'
import { connect } from 'react-redux'

import { appCancelDialog } from '../model/Actions.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcModalAdd extends React.Component {

  onCancel = () => {
    this.props.dispatch( appCancelDialog() )
  }

  onOK = () => {
    console.log( this.state )
    this.props.app.dlgAdd.callback()
    this.onCancel()
  }

  render() {

    var app = this.props.app
    var { dlgAdd } = app

    if( dlgAdd == null ) {
      return <div/>
    }

    return (
      <Modal show={true} onHide={this.onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>{ dlgAdd.title }</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <Form horizontal>
            <FormGroup controlId="formName">
              <Col componentClass={ControlLabel} sm={2}>
                Name
              </Col>
              <Col sm={10}>
                <FormControl type="text" placeholder="..." />
              </Col>
            </FormGroup>

            <FormGroup controlId="formType">
              <Col smOffset={2} sm={10}>
              <DropdownButton
                  componentClass={InputGroup.Button}
                  id="formTypeDropDown"
                  title="Type"
                >
                  <MenuItem key="1">
                    <Panel header="Default"/>
                  </MenuItem>
                  <MenuItem key="2">
                    <Panel header="Primary" bsStyle="primary"/>
                  </MenuItem>
                  <MenuItem key="3">
                    <Panel header="Success" bsStyle="success"/>
                  </MenuItem>
                  <MenuItem key="4">
                    <Panel header="Info" bsStyle="info"/>
                  </MenuItem>
                  <MenuItem key="5">
                    <Panel header="Warning" bsStyle="warning"/>
                  </MenuItem>
                  <MenuItem key="6">
                    <Panel header="Danger" bsStyle="danger"/>
                  </MenuItem>
                </DropdownButton>
              </Col>
            </FormGroup>

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={this.onCancel}>Cancel</Button>
            <Button bsStyle="danger" onClick={this.onOK}>OK</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
