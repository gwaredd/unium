import React from 'react'
import Axios from 'axios'
import { FormGroup, InputGroup, FormControl, Glyphicon, Button } from 'react-bootstrap'

export default class TryNow extends React.Component {

  constructor( props ) {
    super( props )
    this.state = {
      output: ""
    }
  }

  fetch() {
    
    var tutorial = this

    Axios.get( this.props.url )
    .then( (res) => {
      if( res.data ) {
        tutorial.setState({output:tutorial.props.message + '\n\n' + JSON.stringify( res.data, null, 2 ) })
      } else {
        tutorial.setState({output:"Hmm, something's not right! This is what we got back ...\n\n" + JSON.stringify( res, null, 2 ) })
      }
    })
    .catch( (e) => {
      tutorial.setState({output:"Oops, something went wrong!\nIs the game running the tutorial scene?\n\n" + e })
    })
  }

  render() {

    return (
        <div style={{ paddingTop: '20px', width:'80%', margin:'0 auto'}}>
          <FormGroup>
            <InputGroup>
              <FormControl type="text" value={this.props.url} readOnly />
              <InputGroup.Addon>
                <a href={this.props.url} target='_blank'>
                  <Glyphicon glyph="share-alt" />
                </a>
              </InputGroup.Addon>
            </InputGroup>
          </FormGroup>
          <Button bsStyle="info" onClick={()=>this.fetch()}>Try Now</Button>

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

