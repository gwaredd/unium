//-------------------------------------------------------------------------------

export function TabCreate( id, name ) {
  return {
    type: "TAB_CREATE",
    payload: {
      id    : id,
      name  : name
    }
  }
}

export function TabRemove( id ) {
  return {
    type    : "TAB_REMOVE",
    payload : {
      id    : id
    }
  }
}

export function TabSelect( id ) {
  return {
    type: "TAB_SELECT",
    payload: {
      id: id
    }
  }
}

//-------------------------------------------------------------------------------

export function PanelCreate( d ) {
  return {
    type: "PANEL_CREATE",
    payload: {
      id    : d.id,
      type  : d.type,
      name  : d.name
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

