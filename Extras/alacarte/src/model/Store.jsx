//-------------------------------------------------------------------------------

import { combineReducers, applyMiddleware, createStore } from 'redux'
import thunk from 'redux-thunk'
import { createLogger } from 'redux-logger'

import App from './App.jsx'
import Tabs from './Tabs.jsx'
import Panels from './Panels.jsx'
import Widgets from './Widgets.jsx'

//-------------------------------------------------------------------------------

const reducers = combineReducers({
  app     : App,
  tabs    : Tabs,
  panels  : Panels,
  widgets : Widgets,
})

const middleware = applyMiddleware(
  thunk,
  createLogger()
)

//-------------------------------------------------------------------------------

export default createStore( reducers, {}, middleware )

