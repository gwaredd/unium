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

export function Save() {
  return {
    type    : "APP_SAVE"
  }
}

export function Delete() {
  return {
    type    : "APP_DELETE"
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

export function EditPanel( id ) {
  return {
    type    : "APP_PANEL_STATE",
    payload : {
      edit : id
    }
  }
}

export function MoveWidget( id, dragIndex, hoverIndex ) {
  return {
    type    : "WIDGET_MOVE",
    payload : {
      id:         id,
      dragIndex:  dragIndex,
      hoverIndex: hoverIndex
    }
  }
}

export function AddWidget( callback, widget ) {
  return {
    type    : "APP_DAILOG_SHOW",
    payload : {
      modal     : 'addWidget',
      callback  : callback,
      widget    : widget
    }
  }
}

export function CancelDialog() {
  return {
    type: "APP_DIALOG_CANCEL"
  }
}

export function ChangeConfig( settings ) {
  return {
    type: "APP_CONFIG",
    payload: settings
  }
}
