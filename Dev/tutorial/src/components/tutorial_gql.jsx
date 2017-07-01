import React from 'react'
import { Pagination } from 'react-bootstrap'
import TryNow from './try_now.jsx'

export default class Tutorial extends React.Component {

  render() {

    return (
      <div>
        <p>
          GQL
        </p>

        <Pagination
          items={10}
          />
          {/*activePage={this.state.activePage}
          bsSize="large"
          onSelect={this.handleSelect}*/}
          {/*<li><a href="#">&raquo;</a></li>*/}

        <TryNow url="http://localhost:8342/about" message='It works! The game returned us some data ...' />

      </div>
    )
  }
}

