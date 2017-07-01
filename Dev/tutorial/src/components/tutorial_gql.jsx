//--------------------------------------------------------------------------------

import React from 'react'
import { Pagination } from 'react-bootstrap'
import TryNow from './try_now.jsx'

//--------------------------------------------------------------------------------

function GQL_Intro() {
 return <div>
    <p>
      GQL (Game Query Language) is an XPath-like query language that lets you retrieve data from the game, set variables and invoke functions.
    </p>
    <p>
      It is available on a special route called /q and operates on a custom tree data structure. The most important node in this tree is the scene.
    </p>
    <p>
      The query selects all the objects in the tree that match that path and returns an array of the results (in JSON format). This may be an empty array if no objects match or there is an error executing the query.
    </p>

    <TryNow url="http://localhost:8342/q/scene/Game/Player" message='The game returned the player game object ...' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_PlayerPosition() {
  return <div>
    <p>
      Properties on objects can be selected with dot notation, so to get the player position ...
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Player.Transform.position" message='Just the player position ...' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_MultipleMatches() {
  return <div>
    <p>
      If the path matches multiple objects they will all be added to results. To get all the position of all the pickups:
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup.Transform.position" message='All game objects called Pickup' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WildCards() {
  return <div>
    <p>
      You can use wildcards in names to match partial names. To get all the names of all child objects of Game starting with P:
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/P*.name" message='All objects matching P*' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WhereClauses_1() {
  return <div>
    <p>
      Square brackets [] can be used to filter objects based on an expression. Variables within the brackets refer to properties of the current node. If the expression evaluates to an integer number it will be treated as an index into a list, otherwise it will filter the list based on the “truth” of the expression.
    </p>
    <p>
      Find all the pickups and return the 3rd one from the list (0 based index naturally):
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup[2].Transform.position" message='The 3rd pickup position' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WhereClauses_2() {
  return <div>
    <p>
      Or filter the results. Find all the pickups whose x position is greater than or equal to 0 and return the position:
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup.Transform.position[x>=0]" message='Position with x >= 0' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WhereClauses_3() {
  return <div>
    <p>
      Find all the pickups whose x position is greater than or equal to 0 and return the RotateSpeed value from from the attached Pickup component.
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup[Transform.position.x>=0].Pickup.RotateSpeed" message='Rotate speeds ...' />
  </div>
}


//--------------------------------------------------------------------------------

function GQL_Find_1() {
  return <div>
    <p>
      
      Find all the pickups whose x position is greater than or equal to 0 and return the RotateSpeed value from from the attached Pickup component.
    </p>
    <TryNow url="http://localhost:8342/q/scene//Pickup" message='Rotate speeds ...' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_Find_2() {
  return <div>
    <p>
      /q/scene//[tag='Pickup']
      Find all the pickups whose x position is greater than or equal to 0 and return the RotateSpeed value from from the attached Pickup component.
    </p>
    <TryNow url="http://localhost:8342/q/scene//Pickup" message='Rotate speeds ...' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_Find_3() {
  return <div>
    <p>
      /q/scene//*.Pickup.RotateSpeed
      Find all the pickups whose x position is greater than or equal to 0 and return the RotateSpeed value from from the attached Pickup component.
    </p>
    <TryNow url="http://localhost:8342/q/scene//Pickup" message='Rotate speeds ...' />
  </div>
}


//--------------------------------------------------------------------------------

const pages = [
  null,
  React.createFactory( GQL_Intro ),
  React.createFactory( GQL_PlayerPosition ),
  React.createFactory( GQL_MultipleMatches ),
  React.createFactory( GQL_WildCards ),
  React.createFactory( GQL_WhereClauses_1 ),
  React.createFactory( GQL_WhereClauses_2 ),
  React.createFactory( GQL_WhereClauses_3 ),
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
          items={10}
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

