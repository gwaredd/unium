//-------------------------------------------------------------------------------

import { combineReducers, applyMiddleware, createStore } from 'redux'
import thunk from 'redux-thunk'
import { createLogger } from 'redux-logger'

import * as ReducerApp from './ReducerApp.jsx'

//-------------------------------------------------------------------------------

const reducers = combineReducers({
  app: ReducerApp.reducer
})

const initial_state = {
  app: ReducerApp.initial_state
}

const middleware = applyMiddleware(
  thunk,
  createLogger()
)

//-------------------------------------------------------------------------------

export default createStore( reducers, initial_state, middleware )

