import React from 'react'
import { ListGroup, ListGroupItem, Panel } from 'react-bootstrap'

export default class Tutorial extends React.Component {

  render() {
    return (
      <div>
        Click the links below to see some simple examples
        <hr/>
        <ul>
          <li><a href='#'>Debug Output</a></li>
          <li><a href='#'>FPS Graph</a></li>
          <li><a href='#'>Automated Playthrough</a></li>
        </ul>
      </div>
    )
  }
}

