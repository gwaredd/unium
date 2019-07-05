import React from 'react'
import Axios from 'axios'
import { FormGroup, InputGroup, FormControl, Button } from 'react-bootstrap'


export default class TryNow extends React.Component {

  constructor( props ) {
    super( props )
    this.state = {
      output: ""
    }
  }

  onTryNow() {
    
    var tutorial = this

    Axios.get( this.props.url )
    .then( (res) => {
      if( res.data ) {
        tutorial.setState({output: tutorial.props.message + '\n\n' + JSON.stringify( res.data, null, 2 ) })
      } else {
        tutorial.setState({output: "Hmm, something's not right! This is what we got back ...\n\n" + JSON.stringify( res, null, 2 ) })
      }
    })
    .catch( (e) => {
      tutorial.setState({output: "Oops, something went wrong!\nIs the game running the tutorial scene?\n\n" + e })
    })
  }

  onOpen() {
    window.open( this.props.url, '_blank' );
  }

  render() {

    return (
        <div style={{ paddingTop: '20px', width:'80%', margin:'0 auto'}}>

          <FormGroup>
            <InputGroup>
              <FormControl value={this.props.url} readOnly />
              <InputGroup.Append>
                <Button variant='info' onClick={()=>this.onOpen()}>&gt;&gt;</Button>
              </InputGroup.Append>
            </InputGroup>
          </FormGroup>

          <Button variant="info" onClick={()=>this.onTryNow()}>Try Now</Button>

          <div style={{ marginTop: '20px' }}>
            <h4>Output</h4>
            <pre style={{ height: '24em' }}>
              { this.state.output }
            </pre>
          </div>

        </div>
    )
  }
}

