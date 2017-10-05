//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Table } from 'react-bootstrap';
import * as actions from './model/actions.jsx'


//-------------------------------------------------------------------------------

function MinionList( props ) {

  var minions = props.minions
  
  var list = minions.map( (m) => {
    var url = "http://" + m.ip + "/"
    return (
      <tr key={m.id.toString()}>
        <td>{ m.id }</td>
        <td><a href={url} target="_blank">{ url }</a></td>
        <td>{ m.product }</td>
        <td>{ m.version }</td>
        <td>{ m.platform }</td>
        <td>{ m.editor ? "Yes" : "No" }</td>
        <td>{ m.scene }</td>
      </tr>
    )
  })

  return <tbody>{list}</tbody>
}

//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    minions: store.minions
  }
})
export default class Minions extends React.Component {

  render() {

    var minions = this.props.minions.minions

    if( minions.length == 0 ) {
      return <span> No minions connected </span>
    }

    return (
      <Table striped bordered condensed hover>
        <thead>
          <tr>
            <th>ID</th>
            <th>URL</th>
            <th>Product</th>
            <th>Version</th>
            <th>Platform</th>
            <th>Editor</th>
            <th>Scene</th>
          </tr>
        </thead>
        <MinionList minions={minions} />
      </Table>
    )
  }
}

