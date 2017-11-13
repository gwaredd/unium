//-------------------------------------------------------------------------------

import { combineReducers, applyMiddleware, createStore } from 'redux'
import thunk from 'redux-thunk'
import { createLogger } from 'redux-logger'

import App from './reducers/App.jsx'
import Tabs from './reducers/Tabs.jsx'
import Panels from './reducers/Panels.jsx'
import Widgets from './reducers/Widgets.jsx'
import Output from './reducers/Output.jsx'

import AppMiddleware from './middleware/App.jsx'
import ConnectionMiddleware from './middleware/Connection.jsx'


//-------------------------------------------------------------------------------

const reducers = {
  app     : App,
  tabs    : Tabs,
  panels  : Panels,
  widgets : Widgets,
  output  : Output,
}

var middleware = [
  thunk,
  AppMiddleware,
  ConnectionMiddleware
]

if( DEVSERVER ) {
  middleware.push( createLogger() )
}


//-------------------------------------------------------------------------------

export default createStore(
  combineReducers( reducers ),
  {},
  applyMiddleware( ...middleware )
)

