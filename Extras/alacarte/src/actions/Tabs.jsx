
//-------------------------------------------------------------------------------

export function TabRemove( id ) {
  return {
    type    : "TAB_REMOVE",
    payload : {
      id    : id
    }
  }
}


export function TabCreate( id, name ) {
  return {
    type: "TAB_CREATE",
    payload: {
      id    : id,
      name  : name
    }
  }
}

export function PanelCreate( id, name, type ) {
  return {
    type: "PANEL_CREATE",
    payload: {
      id    : id,
      type  : type,
      name  : name
    }
  }

}

export function PanelRemove( id ) {
  return {
    type    : "PANEL_REMOVE",
    payload : {
      id    : id
    }
  }  
}

