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
      It is available on a special route called <b>/q</b> and operates on a custom tree data structure. The most important node in this tree is the scene.
    </p>
    <p>
      The query selects all the objects in the tree that match that path and returns an array of the results (in JSON format). This may be an empty array if no objects match or there is an error executing the query.
    </p>
    <p>
      Use the numbers above to step through different GQL examples.
    </p>

    <TryNow url="http://localhost:8342/q/scene/Game/Player" message='The game returned the player game object ...' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_PlayerPosition() {
  return <div>
    <p>
      Properties can be selected with dot notation. To get the player position ...
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Player.Transform.position" message='The player position ...' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_MultipleMatches() {
  return <div>
    <p>
      Paths can matches multiple objects. To get all the position of all the pickups ...
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup.Transform.position" message='The positions of all game objects called Pickup' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WildCards() {
  return <div>
    <p>
      Wildcards match partial object names. To get all the names of all child objects of Game starting with P ...
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/P*.name" message='All objects with names matching P*' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WhereClauses_1() {
  return <div>
    <p>
      Square brackets [] are used to filter objects based on an expression. If the expression evaluates to an integer it will be treated as an index into a list, otherwise it will filter the list based on the “truth” of the expression.
    </p>
    <p>
      Find all the pickups and return the 3rd one from the list ...
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup[2].Transform.position" message='The position of the 3rd pickup' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WhereClauses_2() {
  return <div>
    <p>
      Or filter the results. Find all the pickups whose x position is greater than or equal to 0 and return the position:
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup.Transform.position[x>=0]" message='The positions of all the pickups with x >= 0' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_WhereClauses_3() {
  return <div>
    <p>
      Find all the pickups whose x position is greater than or equal to 0 and return the RotateSpeed.
    </p>
    <TryNow url="http://localhost:8342/q/scene/Game/Pickup[Transform.position.x>=0].Pickup.RotateSpeed" message='Rotate speeds of all pickups with x >= 0 ...' />
  </div>
}


//--------------------------------------------------------------------------------

function GQL_Find_1() {
  return <div>
    <p>
      You can use // to recursively search all child nodes (from the current node)
    </p>
    <p>
      Find all the pickups in the scene (find by name)
    </p>
    <TryNow url="http://localhost:8342/q/scene//Pickup" message='All the Pickups in the scene' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_Find_2() {
  return <div>
    <p>
      Or, find all the game objects with the Pickup tag
    </p>
    <TryNow url="http://localhost:8342/q/scene//[tag='Pickup']" message='All the objects tagged as a Pickup' />
  </div>
}

//--------------------------------------------------------------------------------

function GQL_Find_3() {
  return <div>
    <p>
      Or, by combining the recursive search with a wildcard you can find objects with specific components.
    </p>
    <TryNow url="http://localhost:8342/q/scene//*.Pickup.RotateSpeed" message='The rotate speeds of all the Pickup components' />
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
  React.createFactory( GQL_Find_1 ),
  React.createFactory( GQL_Find_2 ),
  React.createFactory( GQL_Find_3 ),
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

