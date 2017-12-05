//-------------------------------------------------------------------------------

import moment from 'moment'

export default function( state=[], action ) {

  switch( action.type ) {

    case 'LOG_CLEAR': {
      return []
    }
    
    case 'LOG': {

      const { payload } = action

      const log = {
        ...payload,
        timestamp : moment().format( 'HH:mm:ss.SS' )
      }
      
      var output = state.slice()
      output.push( log )

      return output
    }
  } 

  return state
}

