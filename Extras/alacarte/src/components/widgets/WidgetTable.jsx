
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
  Panel,
  HelpBlock

} from 'react-bootstrap'

export default class WidgetTable extends React.Component {
    
  render() {

    const { widget } = this.props

    return (
      <span>{ widget.name }</span>
    )
  }

}
