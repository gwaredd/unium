import React, { Component } from 'react'
import { Navbar } from 'react-bootstrap'

export default class NavBar extends Component {
 render() {
	return (
    <Navbar bg="light" expand="lg">
      <Navbar.Brand>
        Unium Tutorial
      </Navbar.Brand>    
    </Navbar>
    )
  }
}
