//-------------------------------------------------------------------------------

import _ from 'lodash'
import React from 'react'
import { GithubPicker } from 'react-color'
import ItemTypes from '../../ItemTypes.jsx'
import Widgets from '../app/AcWidgets.jsx'

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

  onChangeName      = (e) => { this.setState({ name: e.target.value }) }
  onChangeQuery     = (e) => { this.setState({ query: e.target.value }) }
  onChangeLog       = (e) => { this.setState({ log: e.target.checked }) }
  onChangeNotify    = (e) => { this.setState({ notify: e.target.checked }) }
  onChangeType      = (e) => { this.setState({ type: e }) }
  onShowColours     = (e) => { this.setState({ showColours: !this.state.showColours }) }

  onChangeColour    = (c) => {
    const text = c.hex in ItemTypes.DARK_COLOURS ? 'white' : 'black'
    this.setState({ colour: c.hex, textColour: text, showColours: false })
  }

  
  //-------------------------------------------------------------------------------

  render() {

    const { dialog, onCancel } = this.props

    const onOK = (e) => {

      e.preventDefault()

      if( this.state.name != '' ) {

        var widget = { ...this.state }
        widget = _.omit( widget, 'showColours' )

        dialog.callback( widget )
      }

      onCancel()
    }

    const type = this.state.type.toLowerCase()
    const options = new Widgets[ type ]()
    const $options = options.options.call( this )

    return (
      <Modal show={true} onHide={onCancel} bsSize="large">
          <Modal.Header closeButton>
            <Modal.Title>Add Widget</Modal.Title>
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
                  <DropdownButton componentClass={InputGroup.Button} id="formType" title={this.state.type}>
                    <MenuItem key="1" onSelect={()=>this.onChangeType('Button')}>Button</MenuItem>
                    <MenuItem key="2" onSelect={()=>this.onChangeType('Table')}>Table</MenuItem>
                  </DropdownButton>
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

            <FormGroup>
              <Col componentClass={ControlLabel} sm={2}>Query</Col>
              <Col sm={10}>
                <FormControl type="text" value={this.state.query} onChange={this.onChangeQuery}/>
              </Col>
            </FormGroup>

            <FormGroup>
              <Col componentClass={ControlLabel} sm={2}>
                Output
              </Col>
              <Col sm={10}>
                <Checkbox checked={this.state.log} onChange={this.onChangeLog} >
                  Copy results to output window
                </Checkbox>
              </Col>
            </FormGroup>

            <FormGroup>
              <Col smOffset={2} sm={10}>
                <Checkbox checked={this.state.notify} onChange={this.onChangeNotify} >
                  Display successful notification toast message
                </Checkbox>
              </Col>
            </FormGroup>

            { $options != null && (
              <div>
                <hr/>
                { $options }
              </div>
            )}

          </Form>

          </Modal.Body>
          <Modal.Footer>
            <Button bsStyle="default" onClick={onCancel}>Cancel</Button>
            <Button bsStyle="success" onClick={onOK}>
            { this.props.dialog.widget ? "Update Widget" : "Create Widget" }
            </Button>
          </Modal.Footer>          
      </Modal>
    )
  }
}
