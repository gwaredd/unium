//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Glyphicon, Button, Panel } from 'react-bootstrap'


import AcWidget from './AcWidget.jsx'
import * as Actions from '../../actions/App.jsx'
import { WidgetCreate, PanelRemove } from '../../actions/Tabs.jsx'

//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    widgets : store.widgets,
    app     : store.app
  }
})
export default class AcPanel extends React.Component {

  //-------------------------------------------------------------------------------

  onRemovePanel = () => {

    var { panel, dispatch } = this.props

    dispatch( Actions.Confirm(
      'Remove Panel',
      "Are you sure you want to remove '" + panel.name + "'",
      () => dispatch( PanelRemove( panel.id ) )
    ))
  }

  onAddWidgetConfirm = ( v ) => {
    var { widgets, panel, dispatch } = this.props
    var keys  = _.map( _.keys( widgets.byId ), (k) => parseInt(k) )
    var id    = _.max( keys ) + 1    
    dispatch( WidgetCreate( { ...v, id: id, panel: panel.id } ))
  }

  onAddWidget = () => {
    this.props.dispatch( Actions.AddWidget( this.onAddWidgetConfirm ) )
  }

  //-------------------------------------------------------------------------------

  render() {

    const { widgets, dispatch, app, panel } = this.props
    const panelWidgets = _.filter( widgets.byId, (p) => p.panel == panel.id )

    var title = (
      <div>
        { panel.name }
        <div className='pull-right'>
          <Glyphicon className='acPanelIcon' glyph="plus" onClick={this.onAddWidget}/> &nbsp;
          <Glyphicon className='acPanelIcon' glyph="remove" onClick={this.onRemovePanel}/>
        </div>
      </div>
    )

    return (
      <Panel className="acPanel" header={title} bsStyle={ panel.type }>
        { _.map( panelWidgets, (w) => <AcWidget key={w.id} widget={w} dispatch={dispatch} appConfig={app.config}/> ) }
      </Panel>
    )
  }
}

