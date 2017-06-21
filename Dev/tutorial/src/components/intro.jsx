import React from 'react'
import ReactDOM from 'react-dom'
import { Jumbotron, Button } from 'react-bootstrap'

export default class Intro extends React.Component {

  render() {
    return (
      <div className='container'>
        <Jumbotron>
          <h1>Unium Tutorial</h1>
          <p>This is a simple hero unit, a simple jumbotron-style component for calling extra attention to featured content or information.</p>
          <p><Button bsStyle="primary" onClick={ ()=>this.props.setPage(1) }>Learn more</Button></p>
        </Jumbotron>
      </div>
    )
  }
}

