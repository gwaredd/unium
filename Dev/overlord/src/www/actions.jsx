
export function setConnectedState( state ) {
  return {
    type: "CONNECTION_STATE",
    payload: {
      state: state
    }
  }
}

export function ovSendCommand( cmd ) {
  return {
    type: "OVERLORD_SEND",
    payload: {
      type: cmd
    }
  }
}

