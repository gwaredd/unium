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
    type    : "CON_CONNECTED",
    payload : state
  }
}

export function Send( id, q ) {
  return {
    type    : "CON_SEND",
    payload : {
      id    : id,
      q     : q
    }
  }
}
