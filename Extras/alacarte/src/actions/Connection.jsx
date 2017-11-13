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

export function SetState( state ) {
  return {
    type    : "APP_CONNECTED",
    payload : state
  }
}

export function Send( id, data ) {
  return {
    type    : "CON_SEND",
    payload : {
      id    : id,
      data  : data
    }
  }
}

export function Error( msg ) {
  return {
    type    : "CON_ERROR",
    payload : msg
  }
}

