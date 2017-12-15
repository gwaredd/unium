
import React from 'react'
import Widget from './Widget.jsx'

import {
  Modal,
  Form,
  FormGroup,
  Col,
  FormControl,
  ControlLabel,
  DropdownButton,
  InputGroup,
  MenuItem,
  HelpBlock

} from 'react-bootstrap'

const initialState = {
  filter: "",
  type:   "Include",
}


export default class OptionsTable extends React.Component {

  constructor( ...args ) {
    super( ...args )
    const { options } = this.props
    this.state = { ...initialState, ...options }
  }

  onChangeFilter     = (v) => this.setState( { filter: v.target.value } )
  onChangeFilterType = (v) => this.setState( { type:   v } )
  
  
  //-------------------------------------------------------------------------------
  
  render() {

    return (

      <FormGroup controlId="formFilter">
        <Col componentClass={ControlLabel} sm={2}>Filter</Col>
        <Col sm={10}>
          <InputGroup>
            <FormControl
              type="text"
              value={this.state.filter}
              onChange={this.onChangeFilter}
              autoFocus={true}
              />
            <DropdownButton componentClass={InputGroup.Button} id="formFilterType" title={this.state.type}>
              <MenuItem key="1" onSelect={()=>this.onChangeFilterType('Include')}>Include</MenuItem>
              <MenuItem key="2" onSelect={()=>this.onChangeFilterType('Exclude')}>Exclude</MenuItem>
            </DropdownButton>
          </InputGroup>
          <HelpBlock>Comma separated list of key names (or leave blank to ignore)</HelpBlock>
        </Col>
      </FormGroup>
    )
  }

}
