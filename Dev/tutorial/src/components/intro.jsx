import React from 'react'
import ReactDOM from 'react-dom'
import { Jumbotron, Button } from 'react-bootstrap'

export default class Intro extends React.Component {

  render() {
    return (
      <div className='container'>
        <Jumbotron>
          <h1>Unium Tutorial</h1>
          <p>
            Welcome to the Unium Tutorial. This will introduce you to key ideas and show how you might be able to start using it on your own projects.
          </p>
          <p>
            This is an interactive tutorial, please ensure the tutorial scene is running in the editor on the same computer.
          </p>
          <p>
            GL &amp; HF, Gw
          </p>
          <p>
            <Button bsStyle="primary" onClick={ ()=>this.props.setPage(1) }>Begin</Button>
          </p>
        </Jumbotron>
      </div>
    )
  }
}

