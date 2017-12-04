//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Panel } from 'react-bootstrap'
import FontAwesome from 'react-fontawesome'

import { DragDropContext } from 'react-dnd'
import HTML5Backend from 'react-dnd-html5-backend'

import AcWidget from './AcWidget.jsx'
import * as Actions from '../../actions/App.jsx'
import { WidgetCreate, PanelRemove } from '../../actions/Tabs.jsx'

//-------------------------------------------------------------------------------

@DragDropContext( HTML5Backend )
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
    var id    = keys.length == 0 ? 1 : _.max( keys ) + 1
    dispatch( WidgetCreate( { ...v, id: id, panel: panel.id, tab: panel.tab } ))
  }

  onAddWidget = () => {
    this.props.dispatch( Actions.AddWidget( this.onAddWidgetConfirm ) )
  }

  onToggleLock = () => {
    const { panel, isLocked, dispatch } = this.props
    dispatch( Actions.EditPanel( isLocked ? panel.id : 0 ) )
  }

  moveWidget = ( dragIndex, hoverIndex ) => {
    const { panel, dispatch } = this.props
    dispatch( Actions.MoveWidget( panel.id, dragIndex, hoverIndex ) )
  }
    

  //-------------------------------------------------------------------------------

  render() {

    const { widgets, dispatch, app, panel, isLocked } = this.props

    var title;

    if( isLocked ) {

      title = (
        <div className="panel-title">
          { panel.name }
          <div className='pull-right'>
            <FontAwesome className='acPanelIcon' name='lock'  onClick={this.onToggleLock} /> &nbsp;
          </div>
        </div>
      )
      
    } else {

      title = (
        <div className="panel-title">
          { panel.name }
          <div className='pull-right'>
            <FontAwesome className='acPanelIcon' name='plus' onClick={this.onAddWidget} /> &nbsp;
            <FontAwesome className='acPanelIcon' name='trash-o' onClick={this.onRemovePanel} /> &nbsp;
            <FontAwesome className='acPanelIcon' name='unlock' onClick={this.onToggleLock}  /> &nbsp;
          </div>
        </div>
      )

    }

    return (
      <div className="acPanel panel">
        <div className="panel-heading" style={{backgroundColor: panel.colour, color: panel.textColour }}>
          {title}
        </div>
        <div>
          <div className="panel-body">
            { _.map( panel.widgets, (wid,i) =>
              <AcWidget
                key={wid}
                id={wid}
                index={i}
                appConfig={app.config}
                isLocked={isLocked}
                moveWidget={this.moveWidget}
              />
            )}
          </div>
        </div>
      </div>
    )
  }
}

