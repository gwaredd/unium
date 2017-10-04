//-------------------------------------------------------------------------------

import { combineReducers, applyMiddleware, createStore } from 'redux'
import thunk from 'redux-thunk'
import { createLogger } from 'redux-logger'
import Overlord from './overlord.jsx'
import * as Minions from './minions.jsx'

const reducers = combineReducers({
  minions: Minions.reducer
})

const initial_state = {
  minions: Minions.initial_state
}

const middleware = applyMiddleware( thunk, Overlord, createLogger() )

export default createStore( reducers, initial_state, middleware );

