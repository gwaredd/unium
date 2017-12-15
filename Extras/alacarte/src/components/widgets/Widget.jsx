//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'

// drag and drop
import { findDOMNode } from 'react-dom'
import { DragSource, DropTarget } from 'react-dnd'
import ItemTypes from '../../Utils.jsx'

// widgets
import FontAwesome from 'react-fontawesome'
import { Controls } from '../app/AcWidgets.jsx'

// actions
import * as Actions from '../../actions/App.jsx'
import * as Log from '../../actions/Logging.jsx'
import * as Tabs from '../../actions/Tabs.jsx'


//-------------------------------------------------------------------------------
// dragging

const cardSource = {
  beginDrag(props) {
    return {
      id:    props.id,
      index: props.index,
    }
  },
}

const cardTarget = {

  hover( props, monitor, component ) {

    const dragIndex = monitor.getItem().index
    const hoverIndex = props.index

    // Don't replace items with themselves

    if (dragIndex === hoverIndex) {
      return
    }


    // Determine rectangle on screen
    const hoverBoundingRect = findDOMNode( component ).getBoundingClientRect()

    // Get vertical middle
    const hoverMiddleY = ( hoverBoundingRect.bottom - hoverBoundingRect.top ) / 2

    // Determine mouse position
    const clientOffset = monitor.getClientOffset()

    // Get pixels to the top
    const hoverClientY = clientOffset.y - hoverBoundingRect.top

    // Only perform the move when the mouse has crossed half of the items height
    // When dragging downwards, only move when the cursor is below 50%
    // When dragging upwards, only move when the cursor is above 50%

    // Dragging downwards
    if( dragIndex < hoverIndex && hoverClientY < hoverMiddleY ) {
      return
    }

    // Dragging upwards
    if( dragIndex > hoverIndex && hoverClientY > hoverMiddleY ) {
      return
    }

    // Time to actually perform the action
    props.moveWidget( dragIndex, hoverIndex )

    // Note: we're mutating the monitor item here!
    // Generally it's better to avoid mutations,
    // but it's good here for the sake of performance
    // to avoid expensive index searches.
    
    monitor.getItem().index = hoverIndex
  },
}

//-------------------------------------------------------------------------------

@DropTarget( ItemTypes.WIDGET, cardTarget, connect => ({
  connectDropTarget: connect.dropTarget(),
}))
@DragSource( ItemTypes.WIDGET, cardSource, (connect, monitor) => ({
  connectDragSource: connect.dragSource(),
  isDragging:        monitor.isDragging(),
}))
@connect( (store) => {
  return {
    widgets : store.widgets
  }
})
export default class Widget extends React.Component {
  

  //-------------------------------------------------------------------------------
  //  edit

  onEditWidgetConfirm = ( widget ) => {
    this.props.dispatch( Tabs.WidgetCreate( widget ))
  }

  onEditWidget = (e) => {

    e.stopPropagation()
    
    var { dispatch, id, widgets } = this.props
    const widget = widgets.byId[ id ]
    
    dispatch( Actions.AddWidget( this.onEditWidgetConfirm, widget ) )
  }
  

  //-------------------------------------------------------------------------------
  // remove

  onRemoveWidget = (e) => {

    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()

    var { dispatch, id, widgets } = this.props
    const widget = widgets.byId[ id ]
    
    dispatch( Actions.Confirm(
      'Remove Widget',
      "Are you sure you want to remove '" + widget.name + "'",
      () => dispatch( Tabs.WidgetRemove( widget.id, widget.panel ) )
    ))

  }

  //-------------------------------------------------------------------------------
  // render

  render() {

    const { isEditing, widget } = this.props

    
    const $control = Controls[ widget.type.toLowerCase() ]


    if( !isEditing ) {
      return <$control widget={widget} dispatch={this.props.dispatch} appConfig={this.props.appConfig} isEditing={false}/>
    }

    // editing ...

    const { connectDragSource, connectDropTarget, isDragging } = this.props

    const style = {
      paddingBottom: '0px',
      marginBottom:  '5px',
      opacity:       isDragging ? 0: 1,
      position:     'relative'
    }

    
    const html = (
      <div style={style} onMouseDown={(e)=>{e.stopPropagation()}}>
        <$control widget={widget} dispatch={this.props.dispatch} appConfig={this.props.appConfig} isEditing={true}/>
        <div className='acEdit' style={{color: widget.textColour}}>
            <FontAwesome className='acPanelIcon' name='pencil' onClick={this.onEditWidget} />
            &nbsp;
            <FontAwesome className='acPanelIcon' name='times' onClick={this.onRemoveWidget} />
          </div>
      </div>
    )

    return connectDragSource( connectDropTarget( html ) )
  }
}

