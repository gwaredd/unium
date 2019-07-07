//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import FontAwesome from 'react-fontawesome'

import { DndProvider } from 'react-dnd'
import HTML5Backend from 'react-dnd-html5-backend'

import Widget from '../widgets/Widget'

import _ from 'lodash';
import * as Actions from '../../actions/App'
import * as Tabs from '../../actions/Tabs'

//-------------------------------------------------------------------------------

class AcPanel extends React.Component {

  //-------------------------------------------------------------------------------

  onRemovePanel = () => {

    const { panel, dispatch } = this.props

    dispatch( Actions.Confirm(
      'Remove Panel',
      "Are you sure you want to remove '" + panel.name + "'",
      () => dispatch( Tabs.PanelRemove( panel.id ) )
    ))
  }

  onAddWidgetConfirm = ( v ) => {
    const { widgets, panel, dispatch } = this.props
    const keys  = _.map( _.keys( widgets.byId ), (k) => parseInt(k) )
    const id    = keys.length === 0 ? 1 : _.max( keys ) + 1
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

    const { widgets, app, panel, isEditing } = this.props

    let menu;

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

    const panelWidgets = _.values( widgets.byId ).filter( (w) => w.panel === panel.id );

    return (
      <DndProvider backend={HTML5Backend}>
        <div className="acPanel card">
          <div className="card-header card-title" style={{backgroundColor: panel.colour, color: panel.textColour }}>
            { panel.name }
            <div className='pull-right'>
              { menu }
            </div>s
          </div>
          <div className="card-body">
            { panelWidgets.map( (widget,i) => {
              return (
                <Widget
                  key={`wid${widget.id}`}
                  id={widget.id}
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
      </DndProvider>
    )
  }
}

const mapStateToProps = ( state, ownProps ) => {
  return {
    widgets : state.widgets,
    app     : state.app
  }
}

export default connect( mapStateToProps )( AcPanel )
