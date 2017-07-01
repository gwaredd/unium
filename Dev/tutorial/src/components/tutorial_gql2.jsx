//--------------------------------------------------------------------------------

import React from 'react'
import { Pagination } from 'react-bootstrap'
import TryNow from './try_now.jsx'

//--------------------------------------------------------------------------------

function GQL_Actions() {
 return <div>
    <p>
      Queries can also change values or invoke functions on the selected objects.
    </p>
    <p>
      For example, to change the rotate speed of all the pick ups.
    </p>

    <TryNow url="http://localhost:8342/q/scene//Pickup.Pickup.RotateSpeed=5" message='Note the query returns the new values' />
  </div>
}

function GQL_SetVector3() {
 return <div>
    <p>
      You can also pass Vector3 values.
    </p>
    <p>
      To teleport the player to the origin ...
    </p>

    <TryNow url="http://localhost:8342/q/scene//Player.Transform.position={'x':0,'y':0,'z':0}" message='Note the query returns the new values' />
  </div>
}


function GQL_Functions() {
 return <div>
    <p>
      You can also call functions.
    </p>
    <p>
      To call the MoveTo() function on the player ...
    </p>

    <TryNow url="http://localhost:8342/q/scene//Player.Player.MoveTo({'x':12,'y':1,'z':-28})" message='Note the query returns the new values' />
  </div>
}

//--------------------------------------------------------------------------------

const pages = [
  null,
  React.createFactory( GQL_Actions ),
  React.createFactory( GQL_SetVector3 ),
  React.createFactory( GQL_Functions ),
]

//--------------------------------------------------------------------------------

export default class Tutorial extends React.Component {

  constructor( props ) {
    super( props )
    this.state = { activePage: 1 }
  }

  onSelect(n) {
    this.setState({activePage: n})
  }

  render() {

    return (
      <div>
        
        <Pagination
          items={3}
          next={true}
          prev={true}
          activePage={ this.state.activePage }
          onSelect={(n) => this.onSelect(n) }
          />

          { pages[ this.state.activePage ]() }

      </div>
    )
  }
}

