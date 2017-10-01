
export function appScreenshot( state ) {
  return {
    type: "APP_SCREENSHOT",
    payload: state
  }
}


export function appConfirm( type ) {

  var payload = null

  if( type != null ) {
    payload = {
      type: type
    }
  }

  return {
    type: "APP_CONFIRM",
    payload: payload
  }
}


export function appCancelDialog() {
  return {
    type: "APP_CANCEL_DIALOG"
  }
}
