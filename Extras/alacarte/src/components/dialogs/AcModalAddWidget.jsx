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
  Checkbox,
  Panel
} from 'react-bootstrap'


const initialState = {
  name      : 'Widget Name',
  style     : 'Default',
  behaviour : 'Silent',
  query     : '/about',
}


//-------------------------------------------------------------------------------

export default class AcModalAddWidget extends React.Component {

  constructor( props ) {
    super( props )
    const { dialog } = props
    this.state = { ...initialState, ...dialog.widget }
  }

  onChangeName      = (e) => { this.setState({ name: e.target.value }) }
  onChangeStyle     = (e) => { this.setState({ style: e }) }
  onChangeBehaviour = (e) => { this.setState({ behaviour: e }) }
  onChangeQuery     = (e) => { this.setState({ query: e.target.value }) }
  onChangeLogOutput = (e) => { this.setState({ logOutput: e.target.checked }) }
  onChangeNotify    = (e) => { this.setState({ notify: e.target.checked }) }
  

  //-------------------------------------------------------------------------------

  render() {

    const { dialog, onCancel } = this.props

    const onOK = (e) => {

      e.preventDefault()

      if( this.state.name != '' ) {
        dialog.callback( { ...this.state, style: this.state.style.toLowerCase() } )
      }

      this.state = { ...initialState }
      onCancel()
    }

    return (
      <Modal show={true} onHide={onCancel} bsSize="large">
          <Modal.Header closeButton>
            <Modal.Title>Add Widget</Modal.Title>
          </Modal.Header>
          <Modal.Body>

          <Form horizontal onSubmit={onOK}>

            <FormGroup>
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

            <FormGroup>
              <Col componentClass={ControlLabel} sm={2}>
                Query
              </Col>
              <Col sm={10}>
                <FormControl
                  type="text"
                  value={this.state.query}
                  onChange={this.onChangeQuery}
                />
              </Col>
            </FormGroup>

            <FormGroup controlId="formType">
              <Col componentClass={ControlLabel} sm={2}>
                Style
              </Col>
              
              <Col sm={10}>
                <DropdownButton
                    componentClass={InputGroup.Button}
                    title={this.state.style}
                    id='formStyle'
                    >
                    <MenuItem key="1" onSelect={()=>this.onChangeStyle('Default')}>
                      <Panel header="Default"/>
                    </MenuItem>
                    <MenuItem key="2" onSelect={()=>this.onChangeStyle('Primary')}>
                      <Panel header="Primary" bsStyle="primary"/>
                    </MenuItem>
                    <MenuItem key="3" onSelect={()=>this.onChangeStyle('Success')}>
                      <Panel header="Success" bsStyle="success"/>
                    </MenuItem>
                    <MenuItem key="4" onSelect={()=>this.onChangeStyle('Info')}>
                      <Panel header="Info" bsStyle="info"/>
                    </MenuItem>
                    <MenuItem key="5" onSelect={()=>this.onChangeStyle('Warning')}>
                      <Panel header="Warning" bsStyle="warning"/>
                    </MenuItem>
                    <MenuItem key="6" onSelect={()=>this.onChangeStyle('Danger')}>
                      <Panel header="Danger" bsStyle="danger"/>
                    </MenuItem>
                  </DropdownButton>
              </Col>
            </FormGroup>

            <FormGroup>
              <Col componentClass={ControlLabel} sm={2}>
                Behaviour
              </Col>
              <Col sm={10}>
                <DropdownButton
                    componentClass={InputGroup.Button}
                    title={this.state.behaviour}
                    id='formBehaviour'
                  >
                    <MenuItem key="1" onSelect={()=>this.onChangeBehaviour('Silent')}>
                      Silent
                    </MenuItem>
                    <MenuItem key="2" onSelect={()=>this.onChangeBehaviour('Log')}>
                      Log Output
                    </MenuItem>
                    <MenuItem key="3" onSelect={()=>this.onChangeBehaviour('Notify')}>
                      Notify
                    </MenuItem>
                    <MenuItem key="4" onSelect={()=>this.onChangeBehaviour('LogNotify')}>
                      Log &amp; Notify
                    </MenuItem>
                  </DropdownButton>
              </Col>
            </FormGroup>

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
