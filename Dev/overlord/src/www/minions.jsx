//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Table } from 'react-bootstrap';
import * as actions from './model/actions.jsx'

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
    return 
      <Table striped bordered condensed hover>
        <thead>
          <tr>
            <th>ID</th>
            <th>IP</th>
            <th>Application</th>
            <th>Version</th>
            <th>Scene</th>
          </tr>
        </thead>
      </Table>
  }
}

