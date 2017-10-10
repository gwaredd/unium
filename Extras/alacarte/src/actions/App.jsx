//-------------------------------------------------------------------------------

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

