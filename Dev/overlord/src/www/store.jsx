//-------------------------------------------------------------------------------

import websocket from '@giantmachines/redux-websocket'

import { combineReducers, applyMiddleware, createStore } from 'redux'
import { createLogger } from 'redux-logger'
import thunk from 'redux-thunk'

const middleware = applyMiddleware( thunk, createLogger() )

// const app = combineReducers(reducers)
// const store = createStore(
//   app,
//   applyMiddleware(
//     websocket,
//     ...
//   )
// )

const reducerOne = function( state={test:1}, action ) {

  if( action.type === "INC" ) {
    // state = {...state,test:'ok'} // state immutable!
  }


  return state
}

const reducerUser = function( state={name:"fish"}, action ) {
  return state
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

export default store

