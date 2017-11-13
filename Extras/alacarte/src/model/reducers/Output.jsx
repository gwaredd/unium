//-------------------------------------------------------------------------------

export default function( state=[], action ) {

  if( action.type === 'APP_OUTPUT' ) {
      var output = state.slice()
      output.push( action.payload )
      return output
  }

  return state
}
  
