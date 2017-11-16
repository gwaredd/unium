
export function Clear() {
  return {
    type    : "LOG_CLEAR"
  }
}

export function Print( msg ) {
  return {
    type    : "LOG",
    payload : {
      type  : 'default',
      text  : msg
    }
  }
}
  
export function Info( msg ) {
  return {
    type    : "LOG",
    payload : {
      type  : 'info',
      text  : msg
    }
  }
}

export function Success( msg ) {
  return {
    type    : "LOG",
    payload : {
      type  : 'success',
      text  : msg
    }
  }
}

export function Warning( msg ) {
  return {
    type    : "LOG",
    payload : {
      type  : 'warning',
      text  : msg
    }
  }
}

export function Error( msg ) {
  return {
    type    : "LOG",
    payload : {
      type  : 'danger',
      text  : msg
    }
  }
}
