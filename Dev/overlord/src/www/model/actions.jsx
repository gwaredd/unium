
export function setConnectedState( state ) {
  return {
    type: "CONNECTION_STATE",
    payload: {
      state: state
    }
  }
}

export function ovConnect() {
  return { type: 'OVERLORD_CONNECT' }
}

export function ovDisconnect() {
  return { type: 'OVERLORD_DISCONNECT' }
}

export function listMinions() {
  return {
    type: "OVERLORD_SEND",
    payload: {
      id: 'list'
    }
  }
}

