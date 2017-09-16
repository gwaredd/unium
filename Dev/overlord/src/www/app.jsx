import React from 'react';
import { Alert, Nav, Navbar, NavItem, MenuItem, NavDropdown, Panel } from 'react-bootstrap';

import { combineReducers, applyMiddleware, createStore } from 'redux';
import logger from 'redux-logger';
import thunk from 'redux-thunk'

const middleware = applyMiddleware( thunk, logger() )

const reducerOne = function( state={test:1}, action ) {

  if( action.type === "INC" ) {
    // state = {...state,test:'ok'} // state immutable!
  }


  return state
}

const reducerUser = function( state={name:""}, action ) {
}

const reducers = combineReducers({
  one: reducerOne,
  user: reducerUser
})

const store = createStore( reducers, 0, middleware );

store.subscribe( () => {
  console.log( "store changed", store.getState() );
})

store.dispatch( {type:"INC", payload: 1 })

store.dispatch( (dispatch) => {
  // do async here ... then ...
  dispatch( {type:"ASYNC", payload: 1 })
})


export default class App extends React.Component {
  render() {
    return (
      <div>
        <Navbar>
          <Navbar.Header>
            <Navbar.Brand>
              <a href="#">Unium: Overlord</a>
            </Navbar.Brand>
          </Navbar.Header>
        </Navbar>
        <Panel header="Minions" bsStyle="primary">
          Panel content
        </Panel>
      </div>
    );
  }
}


