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
  Checkbox,
  Panel
} from 'react-bootstrap'


import * as App from '../../actions/App.jsx'


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    app: store.app
  }
})
export default class AcModalSettings extends React.Component {

  constructor( props ) {
    super( props )
    this.state = { ...props.app.settings }
    this.config = {...props.app.config }
  }

  onChangeStorage = (e) => { this.setState({ useLocalStorage: e.target.checked }) }

  onOK = (e) => {
    e.preventDefault()
    const { dispatch, onCancel } = this.props
    dispatch( App.ChangeConfig( this.state ) )
    onCancel()
  }

  //-------------------------------------------------------------------------------

  render() {

    const { dialog, onCancel } = this.props

    return (
      <Modal show={true} onHide={onCancel} bsSize="large">
          <Modal.Header closeButton>
            <Modal.Title>Settings</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <Form horizontal onSubmit={this.onOK}>

            <FormGroup controlId="formName">
              <Col componentClass={ControlLabel} sm={2}>
                End Point
              </Col>
              <Col sm={10}>
                <FormControl
                  type="text"
                  value={this.config.api}
                  disabled
                />
              </Col>
            </FormGroup>


            <FormGroup>
              <Col componentClass={ControlLabel} sm={2}>
                Local Storage
              </Col>
              <Col sm={10}>
                <Checkbox checked={this.state.useLocalStorage} onChange={this.onChangeStorage} />
              </Col>
            </FormGroup>

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={onCancel}>Cancel</Button>
            <Button bsStyle="success" onClick={this.onOK}>Save</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
