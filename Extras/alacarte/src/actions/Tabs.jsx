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
      name  : d.name,
      tab   : parseInt( d.tab )
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

//-------------------------------------------------------------------------------

export function WidgetCreate( v ) {
  return {
    type    : "WIDGET_CREATE",
    payload : v
  }  
}


export function WidgetRemove( id ) {
  return {
    type    : "WIDGET_REMOVE",
    payload : {
      id    : id
    }
  }  
}

