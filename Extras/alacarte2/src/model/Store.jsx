//-------------------------------------------------------------------------------

import { combineReducers, applyMiddleware, createStore } from 'redux'
import thunk from 'redux-thunk'
import { createLogger } from 'redux-logger'

import App from './reducers/App'
import Tabs from './reducers/Tabs'
import Panels from './reducers/Panels'
import Widgets from './reducers/Widgets'
import Output from './reducers/Output'

import AppMiddleware from './middleware/App'
import ConnectionMiddleware from './middleware/Connection'


//-------------------------------------------------------------------------------

const reducers = {
  app     : App,
  tabs    : Tabs,
  panels  : Panels,
  widgets : Widgets,
  output  : Output,
}

const middleware = [
  thunk,
  AppMiddleware,
  ConnectionMiddleware
]

middleware.push( createLogger() )


//-------------------------------------------------------------------------------

export default createStore(
  combineReducers( reducers ),
  {},
  applyMiddleware( ...middleware )
)

