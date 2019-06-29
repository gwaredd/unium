//-------------------------------------------------------------------------------

import _ from 'lodash'
import React from 'react'
import { GithubPicker } from 'react-color'
import ItemTypes from '../../Utils'

import {
  Modal,
  Button,
  Form,
  Row,
  Col,
  InputGroup,
} from 'react-bootstrap'


const initial_state = {
  name:        '',
  colour:      '#004dcf',
  textColour:  'white',
  showColours: false
}

//-------------------------------------------------------------------------------

export default class AcModalAddPanel extends React.Component {

  constructor( props ) {
    super( props )
    this.state = {...initial_state}
  }

  onChangeName    = (e) => { this.setState({ name: e.target.value }); }
  onChangeColour  = (c) => {
    const text = c.hex in ItemTypes.DARK_COLOURS ? 'white' : 'black'
    this.setState({ colour: c.hex, textColour: text, showColours: false })
  }

  onShowColours     = (e) => { this.setState({ showColours: !this.state.showColours }) }
  

  //-------------------------------------------------------------------------------

  render() {

    const { dialog, onCancel } = this.props
    
    const onOK = (e) => {

      e.preventDefault()
      
      if( this.state.name !== '' ) {
        const panel = _.omit( {...this.state}, 'showColours' )
        dialog.callback( panel )
      }

      onCancel()
    }

    
    return (
      <Modal show={true} onHide={onCancel}>
          <Modal.Header closeButton>
            <Modal.Title>Add Panel</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <Form onSubmit={onOK}>

            <Form.Group as={Row} controlId="formType">
              <Form.Label>
                Name
              </Form.Label>
              <Col sm={10}>
                <InputGroup>

                  <InputGroup.Prepend>
                    <InputGroup.Text
                      style={{
                        backgroundColor: this.state.colour,
                      }}
                      onClick={this.onShowColours}
                    >
                      &nbsp;
                    </InputGroup.Text>
                  </InputGroup.Prepend>

                  <Form.Control
                    type="text"
                    value={this.state.name}
                    onChange={this.onChangeName}
                    autoFocus={true}
                  />
                </InputGroup>
              </Col>
            </Form.Group>

              { this.state.showColours && 
                <Form.Group>
                  <Col sm={{span:10, offset:1}}>
                    <GithubPicker width='215px' onChangeComplete={ this.onChangeColour }/>
                  </Col>
                </Form.Group>
              }

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button variant="default" onClick={onCancel}>Cancel</Button>
            <Button variant="success" onClick={onOK}>Create Panel</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
