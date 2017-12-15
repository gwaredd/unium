//-------------------------------------------------------------------------------

import _ from 'lodash'
import React from 'react'
import FontAwesome from 'react-fontawesome'

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
  HelpBlock,
  Table,
  Alert

} from 'react-bootstrap'


import Utils from '../../Utils.jsx'
import * as Log from '../../actions/Logging.jsx'


//-------------------------------------------------------------------------------

export default class WidgetTable extends React.Component {

  constructor( ...args ) {
    super( ...args )

    this.state = {
      error: null,
      data: null
    }
  }


  //-------------------------------------------------------------------------------

  processData = (data) => {

    try {

      const { widget } = this.props
      
      if( _.isObject( data ) ) {

        if( widget.options.filter != "" ) {

          const keys = widget.options.filter.split( /\s*,\s*/ )
          const keyMap = _.zipObject( keys, _.map( keys, () => true ) )

          if( widget.options.type == 'Include' ) {

            data = _.pickBy( data, (v,k) => k in keyMap )

          } else {

            data = _.omitBy( data, (v,k) => k in keyMap )

          }


          console.log( keyMap )
        }

        this.setState({data:data})

      } else {
        console.error( "TODO: handle arrays!")
      }
      
    }
    catch( e ) {
      console.error( "TODO: handle parse error" )
    }
  }


  //-------------------------------------------------------------------------------

  fetchData = () => {

    const { widget, dispatch, appConfig } = this.props

    if( !"query" in widget ) {
      dispatch( Log.Warning( "Widget '" + widget.name + "'has no query" ) )
      return
    }
  
    Utils.Fetch( widget.query, appConfig )
  
      .then( (res) => {
  
        if( 'log' in widget && widget.log ) {
          dispatch( Log.Print( '[' + widget.name + ']' + JSON.stringify( res.data, null, 2 ) ) )
        }
  
        if( 'notify' in widget && widget.notify ) {
          dispatch( Log.Success( widget.name + ' Success' ) )
        }

        this.processData( res.data )

      })
  
      .catch( (err) => {
        dispatch( Log.Error( err.toString() ) )
        this.setState({ error: err.toString(), data: null })
      })
    
  }


  //-------------------------------------------------------------------------------

  componentWillMount() {
  }
  
  
  //-------------------------------------------------------------------------------

  render() {

    const { widget, isEditing } = this.props

    return (
      <div className="panel">
        <div className="panel-heading panel-title" style={{backgroundColor: widget.colour, color: widget.textColour }}>
          { widget.name }
          { !isEditing && (
            <div className='pull-right'>
              <FontAwesome name='refresh' onClick={this.fetchData} />
            </div>
          )}
        </div>
        <div className="panel-body">

        { this.state.error != null && (
          <Alert bsStyle="danger">
            Failed to fetch data<br/>
            { this.state.error }
          </Alert>
        )}

        { this.state.error == null && this.state.data == null && (
          <Alert bsStyle="info">
            Data not fetched yet
          </Alert>
        )}

        { this.state.data != null && (
          <Table responsive> 
            <tbody>
            {
              _.map( this.state.data, (v,k) => {
                return (
                  <tr key={k}>
                    <td>{k}</td>
                    <td>{JSON.stringify(v)}</td>
                  </tr>
                )
              })
            }
            </tbody>
          </Table>
        )}

        </div>
      </div>
    )
  }

}
