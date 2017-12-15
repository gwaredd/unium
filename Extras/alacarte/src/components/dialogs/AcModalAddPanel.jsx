//-------------------------------------------------------------------------------

import _ from 'lodash'
import React from 'react'
import { GithubPicker } from 'react-color'
import ItemTypes from '../../Utils.jsx'

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
      
      if( this.state.name != '' ) {
        var panel = {...this.state}
        panel = _.omit( panel, 'showColours' )

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

          <Form horizontal onSubmit={onOK}>

            <FormGroup controlId="formType">
                <Col componentClass={ControlLabel} sm={2}>Name</Col>
                <Col sm={10}>
                  <InputGroup>
                    <InputGroup.Addon style={{padding: '0px'}}>
                      <div style={{
                        backgroundColor: this.state.colour,
                        width:           '25px',
                        height:          '36px',
                      }}
                      onClick={this.onShowColours}
                      >
                        <span style={{backgroundColor: this.state.colour, height:'100%', width:'100%'}} />
                      </div>
                    </InputGroup.Addon>                
                    <FormControl
                      type="text"
                      value={this.state.name}
                      onChange={this.onChangeName}
                      autoFocus={true}
                      />
                  </InputGroup>
                </Col>
              </FormGroup>

              { this.state.showColours && 
                <FormGroup>
                  <Col smOffset={2} sm={10}>
                    <GithubPicker width='215px' onChangeComplete={ this.onChangeColour }/>
                  </Col>
                </FormGroup>
              }

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={onCancel}>Cancel</Button>
            <Button bsStyle="success" onClick={onOK}>Create Panel</Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
