//-------------------------------------------------------------------------------

// https://youtu.be/nrg7zhgJd4w

import { combineReducers, applyMiddleware, createStore } from 'redux'
import thunk from 'redux-thunk'
import { createLogger } from 'redux-logger'
import * as Minions from './minions.jsx'

const reducers = combineReducers({
  minions: Minions.reducer
})

const initial_state = {
  minions: Minions.initial_state
}

const middleware = applyMiddleware( thunk, createLogger() )

const store = createStore( reducers, initial_state, middleware );

export default store

