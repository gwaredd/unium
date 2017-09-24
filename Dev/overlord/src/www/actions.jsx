
export function onConnection( state ) {
  return {
    type: "OVERLORD_CONNECTION",
    payload: {
      state: state
    }
  }
}

