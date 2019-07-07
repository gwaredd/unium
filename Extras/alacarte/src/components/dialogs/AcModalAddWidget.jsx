//-------------------------------------------------------------------------------

import _ from 'lodash'
import React from 'react'
import { GithubPicker } from 'react-color'
import ItemTypes from '../../Utils'
import { Options } from '../app/AcWidgets'

import {
  Modal,
  Button,
  Form,
  Col,
  Row,
  Dropdown,
  DropdownButton,
  InputGroup
} from 'react-bootstrap'


const initialState = {
  name:        'Widget Name',
  query:       '/about',
  style:       'Default',
  colour:      '#004dcf',
  textColour:  'white',
  log:         true,
  notify:      true,
  type:        'Button',
  options:    {},
 
  showColours: false,
}


//-------------------------------------------------------------------------------

export default class AcModalAddWidget extends React.Component {

  constructor( props ) {
    super( props )
    const { dialog } = props
    this.state = { ...initialState, ...dialog.widget }
  }

  onChangeName      = (e) => this.setState( { name: e.target.value } )
  onChangeQuery     = (e) => this.setState( { query: e.target.value } )
  onChangeLog       = (e) => this.setState( { log: e.target.checked } )
  onChangeNotify    = (e) => this.setState( { notify: e.target.checked } )
  onShowColours     = (e) => this.setState( { showColours: !this.state.showColours } )
  onChangeType      = (e) => this.setState( { type: e } )
  
  onChangeColour    = (c) => {
    const text = c.hex in ItemTypes.DARK_COLOURS ? 'white' : 'black'
    this.setState({ colour: c.hex, textColour: text, showColours: false })
  }




  //-------------------------------------------------------------------------------

  onOK = (e) => {

    const { dialog, onCancel } = this.props

    e.preventDefault()

    if( this.state.name !== '' ) {

      let options = {}

      if( this.refs != null && this.refs.options != null ) {
        options = this.refs.options.state
      }

      const widget = _.omit( { ...this.state, options: options }, 'showColours' )
      dialog.callback( widget )
    }

    onCancel()
  }

    
  //-------------------------------------------------------------------------------

  render() {

    const { onCancel } = this.props

    const options = Options[ this.state.type.toLowerCase() ]
    const element = options && React.createElement( options, {
      ref: 'options',
      options: this.state.options
    })

    return (
      <Modal show={true} onHide={onCancel} size="lg">
          <Modal.Header closeButton>
            <Modal.Title>Add { this.state.type }</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <Form onSubmit={this.onOK}>

            <Form.Group as={Row} controlId="formType">
              <Form.Label column sm='2'>
                Name
              </Form.Label>
              <Col sm={10}>
                <InputGroup>
                  <InputGroup.Prepend style={{padding: '0px'}}>
                    <div style={{
                      backgroundColor: this.state.colour,
                      width:           '25px',
                      height:          '36px',
                    }}
                    onClick={this.onShowColours}
                    >
                      <span style={{backgroundColor: this.state.colour, height:'100%', width:'100%'}} />
                    </div>
                  </InputGroup.Prepend>                
                  <Form.Control
                    type="text"
                    value={this.state.name}
                    onChange={this.onChangeName}
                    autoFocus={true}
                    />
                  <DropdownButton id="formType" title={this.state.type}>
                    <Dropdown.Item key="1" onSelect={()=>this.onChangeType('Button')}>Button</Dropdown.Item>
                    <Dropdown.Item key="2" onSelect={()=>this.onChangeType('Link')}>Link</Dropdown.Item>
                    <Dropdown.Item key="3" onSelect={()=>this.onChangeType('Table')}>Table</Dropdown.Item>
                  </DropdownButton>
                </InputGroup>
              </Col>
            </Form.Group>

            { this.state.showColours && 
              <Form.Group>
                <Col sm={{offset:2, span:10}}>
                  <GithubPicker width='215px' onChangeComplete={ this.onChangeColour }/>
                </Col>
              </Form.Group>
            }

            <Form.Group as={Row}>
              <Form.Label column sm='2'>
                Query
              </Form.Label>
              <Col sm={10}>
                <Form.Control type="text" value={this.state.query} onChange={this.onChangeQuery}/>
              </Col>
            </Form.Group>

            <Form.Group as={Row}>
              <Form.Label column sm='2'>
                Output
              </Form.Label>
              <Col sm={10}>
                <Form.Check
                  type='checkbox'
                  label='Copy results to output window'
                  checked={this.state.log}
                  onChange={this.onChangeLog} />
              </Col>
            </Form.Group>

            <Form.Group as={Row}>
              <Col sm={{offset:2, span:10}}>
                <Form.Check
                  type='checkbox'
                  label='Display successful notification toast message'
                  checked={this.state.notify}
                  onChange={this.onChangeNotify} 
                />
              </Col>
            </Form.Group>

            {element}

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button variant="default" onClick={this.props.onCancel}>Cancel</Button>
            <Button variant="success" onClick={this.onOK}>
            { this.props.dialog.widget ? "Update Widget" : "Create Widget" }
            </Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
