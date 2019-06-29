
import React from 'react'

import {
  Form,
  FormGroup,
  Col,
  FormControl,
  Dropdown,
  DropdownButton,
  InputGroup
} from 'react-bootstrap'

const initialState = {
  filter:         "",
  type:           "Include",
  fetchOnStartup: true,
}


export default class OptionsTable extends React.Component {

  constructor( ...args ) {
    super( ...args )
    const { options } = this.props
    this.state = { ...initialState, ...options }
  }

  onChangeFilter     = (v) => this.setState( { filter: v.target.value } )
  onChangeFilterType = (v) => this.setState( { type:   v } )
  onChangeFetch      = (v) => this.setState( { fetchOnStartup: v.target.checked } )
  
  
  //-------------------------------------------------------------------------------
  
  render() {

    return (

      <div>

        <FormGroup controlId="formFilter">
          <Col componentClass={Form.Label} sm={2}>Filter</Col>
          <Col sm={10}>
            <InputGroup>
              <FormControl
                type="text"
                value={this.state.filter}
                onChange={this.onChangeFilter}
                autoFocus={true}
                />
              <DropdownButton componentClass={InputGroup.Button} id="formFilterType" title={this.state.type}>
                <Dropdown.Item key="1" onSelect={()=>this.onChangeFilterType('Include')}>Include</Dropdown.Item>
                <Dropdown.Item key="2" onSelect={()=>this.onChangeFilterType('Exclude')}>Exclude</Dropdown.Item>
              </DropdownButton>
            </InputGroup>
            <Form.Text className="text-muted">
              Comma separated list of key names (or leave blank to ignore)
            </Form.Text>
          </Col>
        </FormGroup>

        <FormGroup>
          <Col smOffset={2} sm={10}>
            <Form.Check checked={this.state.fetchOnStartup} onChange={this.onChangeFetch} >
              Fetch on start up
            </Form.Check>
          </Col>
        </FormGroup>

      </div>
    )
  }

}
