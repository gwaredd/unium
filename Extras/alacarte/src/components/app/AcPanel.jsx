//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Panel } from 'react-bootstrap'
import FontAwesome from 'react-fontawesome'

import { DragDropContext } from 'react-dnd'
import HTML5Backend from 'react-dnd-html5-backend'

import Widget from '../widgets/Widget.jsx'

import * as Actions from '../../actions/App.jsx'
import * as Tabs from '../../actions/Tabs.jsx'

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
      () => dispatch( Tabs.PanelRemove( panel.id ) )
    ))
  }

  onAddWidgetConfirm = ( v ) => {
    var { widgets, panel, dispatch } = this.props
    var keys  = _.map( _.keys( widgets.byId ), (k) => parseInt(k) )
    var id    = keys.length == 0 ? 1 : _.max( keys ) + 1
    dispatch( Tabs.WidgetCreate( { ...v, id: id, panel: panel.id, tab: panel.tab } ))
  }

  onAddWidget = () => {
    this.props.dispatch( Actions.AddWidget( this.onAddWidgetConfirm ) )
  }

  onToggleLock = () => {
    const { panel, isEditing, dispatch } = this.props
    dispatch( Actions.EditPanel( isEditing ? 0 : panel.id ) )
  }

  moveWidget = ( dragIndex, hoverIndex ) => {
    const { panel, dispatch } = this.props
    dispatch( Actions.MoveWidget( panel.id, dragIndex, hoverIndex ) )
  }
    

  //-------------------------------------------------------------------------------

  render() {

    const { widgets, dispatch, app, panel, isEditing } = this.props

    var menu;

    if( !isEditing ) {

      menu = <FontAwesome className='acPanelIcon' name='lock'  onClick={this.onToggleLock} />

    } else {

      menu = (
        <span>
          <FontAwesome className='acPanelIcon' name='plus' onClick={this.onAddWidget} /> &nbsp;
          <FontAwesome className='acPanelIcon' name='trash-o' onClick={this.onRemovePanel} /> &nbsp;
          <FontAwesome className='acPanelIcon' name='unlock' onClick={this.onToggleLock}  />
        </span>
      )
    }

    return (
      <div className="acPanel panel">
        <div className="panel-heading panel-title" style={{backgroundColor: panel.colour, color: panel.textColour }}>
          { panel.name }
          <div className='pull-right'>
            { menu }
          </div>
        </div>
        <div className="panel-body">
          { _.map( panel.widgets, (wid,i) => {

            const widget = widgets.byId[ wid ]

            if( !widget ) {
              return null
            }
            
            return (
              <Widget
                key={wid}
                id={wid}
                index={i}
                appConfig={app.config}
                widget={widget}
                isEditing={isEditing}
                moveWidget={this.moveWidget}
              />
            )
          })}
        </div>
      </div>
    )
  }
}

