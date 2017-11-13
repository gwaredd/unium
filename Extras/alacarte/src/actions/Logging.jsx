
export function Print( type, msg ) {
  return {
    type    : "APP_OUTPUT",
    payload : {
      type  : type,
      text  : msg
    }
  }
}
  
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

