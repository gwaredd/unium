import React from 'react'
import Axios from 'axios'
import AceEditor from 'react-ace'

import { FormGroup, InputGroup, FormControl, Glyphicon, Button, Alert } from 'react-bootstrap'


export default class Tutorial extends React.Component {

  static url = "http://localhost:8342/utils/screenshot"

  static code = `// register the about handler route
RoutesHTTP.Add( "/about", HandlerUtils.HandlerAbout );
  
// The handler is just a function that responds with some data.
// In this case it reflects some JSON data about the game.

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
Routes map incoming requests to associated handling routines by their URL (or partial URL). You can register your own routes for custom actions.
        </p>
        <p>
The default routes along with the other configuration can be found in the Unium.cs file. The route handler for the 'about' route from the first example looks like this.
        </p>
        <center>
          <AceEditor
              mode="csharp"
              theme="textmate"
              editorProps={{$blockScrolling: true}}
              readOnly
              value={Tutorial.code}
              fontSize={14}
              height='280px'
              width='80%'
              showPrintMargin={false}
              showGutter={true}
              highlightActiveLine={true}
            />
          </center>

        <hr/>
        <p style={{marginTop:'20px'}}>
          Handlers can return any kind of data, for example they can be used to take a screenshot.
        </p>

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
            <center style={{marginTop:'3em'}}>
              <img src={this.state.imageData} width="80%" />
            </center>
          )}
          { this.state.error && (
            <Alert bsStyle="danger">
              <h4>Oops! Something went wrong :/</h4>
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

