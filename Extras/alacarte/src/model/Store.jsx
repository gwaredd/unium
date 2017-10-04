//-------------------------------------------------------------------------------

import { combineReducers, applyMiddleware, createStore } from 'redux'
import thunk from 'redux-thunk'
import { createLogger } from 'redux-logger'

import * as App from './App.jsx'
import Tabs from './Tabs.jsx'
import * as Panels from './Panels.jsx'
import * as Widgets from './Widgets.jsx'

//-------------------------------------------------------------------------------

const reducers = combineReducers({
  app     : App.reducer,
  tabs    : Tabs,
  panels  : Panels.reducer,
  widgets : Widgets.reducer,
})

const middleware = applyMiddleware(
  thunk,
  createLogger()
)

//-------------------------------------------------------------------------------

export default createStore( reducers, {}, middleware )

