//-------------------------------------------------------------------------------

export function TabCreate( id, name ) {
  return {
    type: "TAB_CREATE",
    payload: {
      id      : id,
      name    : name,
      layout  : {}
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

export function TabLayout( id, layout ) {
  return {
    type: "TAB_LAYOUT",
    payload: {
      id: id,
      layout: layout
    }
  }
}

//-------------------------------------------------------------------------------

export function PanelCreate( panel ) {
  return {
    type: "PANEL_CREATE",
    payload: {...panel, tab: parseInt( panel.tab )}
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


export function WidgetRemove( id, panel ) {
  return {
    type    : "WIDGET_REMOVE",
    payload : {
      id    : id,
      panel : panel,
    }
  }  
}

