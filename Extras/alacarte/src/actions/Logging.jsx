
export function Print( type, msg ) {
  return {
    type    : "LOG_OUTPUT",
    payload : {
      type  : type,
      text  : msg
    }
  }
}
  
export function Info( msg ) {
  return {
    type    : "LOG_INFO",
    payload : msg
  }
}

export function Success( msg ) {
  return {
    type    : "LOG_SUCCESS",
    payload : msg
  }
}

export function Warning( msg ) {
  return {
    type    : "LOG_WARNING",
    payload : msg
  }
}

export function Error( msg ) {
  return {
    type    : "LOG_ERROR",
    payload : msg
  }
}

