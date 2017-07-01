import React from 'react'
import Axios from 'axios'
import AceEditor from 'react-ace'

import { FormGroup, InputGroup, FormControl, Glyphicon, Button, Alert } from 'react-bootstrap'


export default class Tutorial extends React.Component {

  static url = "http://localhost:8342/utils/screenshot"

  static code = `// add a thing
RoutesHTTP.Add( "/about", HandlerUtils.HandlerAbout );
  
// define handler

public static void HandlerAbout( RequestAdapter req, string path )
{
    req.Respond( JsonReflector.Reflect( new {
        Unium = Unium.Version.ToString( 2 ),
        Unity = Application.unityVersion,
        Mono  = Environment.Version.ToString(),
        ...
    }));
}
`

  constructor( props ) {
    super( props )
    this.state = {
      hasImage  : false,
      error     : null,
      imageData : null,
    }
  }

  screenshot() {

    var tutorial = this

    Axios.get( Tutorial.url, { responseType:"blob" })
    .then( (res) => {
        var reader = new window.FileReader()
        reader.readAsDataURL( res.data )
        reader.onload = () => {
          tutorial.setState({
            hasImage  : true,
            imageData : reader.result,
            error     : null
          })
        }
        reader.onerror = (e) => {
          tutorial.setState({ error: e.toString() })
        }
    })
    .catch( (e) => {
      tutorial.setState({ error: e.toString() })
    })
  }

  render() {
    return (
      <div>
        <p>
          some explanation of routes
        </p>

        <p>
          screenshot
        </p>

         <AceEditor
            mode="csharp"
            theme="textmate"
            editorProps={{$blockScrolling: true}}
            readOnly
            value={Tutorial.code}
            fontSize={14}
            height='320px'
            width='80%'
            showPrintMargin={false}
            showGutter={true}
            highlightActiveLine={true}
          />

        <div style={{ padding: '3em'}}>
          <FormGroup>
            <InputGroup>
              <FormControl type="text" value={Tutorial.url} readOnly />
              <InputGroup.Addon>
                <a href={Tutorial.url} target='_blank'>
                  <Glyphicon glyph="share-alt" />
                </a>
              </InputGroup.Addon>
            </InputGroup>
          </FormGroup>
          <Button bsStyle="info" onClick={()=>this.screenshot()}>Take screenshot</Button>

          { this.state.hasImage && (
            <p>
              <img src={this.state.imageData} width="80%" />
            </p>
          )}
          { this.state.error && (
            <Alert bsStyle="danger">
              <h4>Oh snap! You got an error!</h4>
              <p>
                {this.state.error}
              </p>
            </Alert>
          )}
          
        </div>
        
      </div>
    )
  }
}

