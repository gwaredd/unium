//-------------------------------------------------------------------------------
// misc

export function Info( msg ) {
  return {
    type    : "APP_INFO",
    payload : msg
  }
}

export function Success( msg ) {
  return {
    type    : "APP_SUCCESS",
    payload : msg
  }
}

export function Warning( msg ) {
  return {
    type    : "APP_WARNING",
    payload : msg
  }
}

export function Error( msg ) {
  return {
    type    : "APP_ERROR",
    payload : msg
  }
}



//-------------------------------------------------------------------------------
// websocket connection

export function Connect() {
  return {
    type: "CON_CONNECT",
  }
}

export function Disconnect() {
  return {
    type: "CON_DISCONNECT",
  }
}

export function ConnectionState( state ) {
  return {
    type    : "APP_CONNECTED",
    payload : state
  }
}

export function ConnectionSend( id, data ) {
  return {
    type    : "CON_SEND",
    payload : {
      id    : id,
      data  : data
    }
  }
}

export function ConnectionError( msg ) {
  return {
    type    : "CON_ERROR",
    payload : msg
  }
}


//-------------------------------------------------------------------------------
// dialogs

export function Screenshot() {
  return {
    type    : "APP_DAILOG_SHOW",
    payload : {
      modal     : 'screenshot',
    }
  }
}


export function Confirm( title, question, callback, data ) {
  return {
    type    : "APP_DAILOG_SHOW",
    payload : {
      modal     : 'confirm',
      title     : title,
      question  : question,
      callback  : callback,
      data      : data
    }
  }
}

export function AddTab( callback ) {
  return {
    type    : "APP_DAILOG_SHOW",
    payload : {
      modal     : 'addTab',
      callback  : callback
    }
  }
}

export function AddPanel( callback ) {
  return {
    type    : "APP_DAILOG_SHOW",
    payload : {
      modal     : 'addPanel',
      callback  : callback
    }
  }
}

export function AddWidget( callback ) {
  return {
    type    : "APP_DAILOG_SHOW",
    payload : {
      modal     : 'addWidget',
      callback  : callback
    }
  }
}

export function CancelDialog() {
  return {
    type: "APP_DIALOG_CANCEL"
  }
}

