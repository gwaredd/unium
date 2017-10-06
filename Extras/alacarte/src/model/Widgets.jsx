//-------------------------------------------------------------------------------

import { combineReducers } from 'redux'
import _ from 'lodash'


const initial_state = {}

//-------------------------------------------------------------------------------

function reduceById( state=initial_state, action ) {

  return state
}


//-------------------------------------------------------------------------------

function reduceAllIds( state=[], action ) {  

  return state
}


//-------------------------------------------------------------------------------

export default combineReducers({
  byId    : reduceById
  //allIds  : reduceAllIds
})
