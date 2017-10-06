//-------------------------------------------------------------------------------

export function appScreenshot( state ) {
  return {
    type: "APP_SCREENSHOT",
    payload: state
  }
}


export function appConfirm( title, question, callback ) {
  return {
    type    : "APP_CONFIRM",
    payload : {
      title     : title,
      question  : question,
      callback  : callback
    }
  }
}

export function appDialogAdd( title, callback ) {
  return {
    type: "APP_DIALOG_ADD",
    payload : {
      title     : title,
      callback  : callback
    }
  }
}

export function appCancelDialog() {
  return {
    type: "APP_CANCEL_DIALOG"
  }
}


//-------------------------------------------------------------------------------

export function tabRemove( id ) {
  return {
    type    : "TAB_REMOVE",
    payload : {
      id    : id
    }
  }
}


export function tabAdd( id, name ) {
  return {
    type: "TAB_ADD",
    payload: {
      id    : id,
      name  : name
    }
  }
}

