import React from 'react'
import { Navbar } from 'react-bootstrap'

// export default class NavBar extends React.Component {
export default function NavBar() {
	return (
    <Navbar fixedTop>
      <Navbar.Header>
        <Navbar.Brand>
          Unium Tutorial
        </Navbar.Brand>
      </Navbar.Header>
    </Navbar>
  )
}
