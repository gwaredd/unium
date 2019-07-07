//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'

// drag and drop
import { findDOMNode } from 'react-dom'
import { DragSource, DropTarget } from 'react-dnd'
import ItemTypes from '../../Utils'

// widgets
import FontAwesome from 'react-fontawesome'
import { Controls } from '../app/AcWidgets'

// actions
import * as Actions from '../../actions/App'
import * as Tabs from '../../actions/Tabs'


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

    if (dragIndex  === hoverIndex) {
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

class Widget extends React.Component {
  

  //-------------------------------------------------------------------------------
  //  edit

  onEditWidgetConfirm = ( widget ) => {
    this.props.dispatch( Tabs.WidgetCreate( widget ))
  }

  onEditWidget = (e) => {

    e.stopPropagation()
    
    const { dispatch, id, widgets } = this.props
    const widget = widgets.byId[ id ]
    
    dispatch( Actions.AddWidget( this.onEditWidgetConfirm, widget ) )
  }
  

  //-------------------------------------------------------------------------------
  // remove

  onRemoveWidget = (e) => {

    e.stopPropagation()
    e.nativeEvent.stopImmediatePropagation()

    const { dispatch, id, widgets } = this.props
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
    
    const component = Controls[ widget.type.toLowerCase() ]
    const element = React.createElement( component, {
      widget: widget,
      dispatch: this.props.dispatch,
      appConfig: this.props.appConfig,
      isEditing: isEditing
    })

    if( !isEditing ) {
      return element;
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
        {element}
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

const mapStateToProps = ( state, ownProps ) => {
  return {
    widgets : state.widgets
  }
}

const WidgetDropTarget = DropTarget( ItemTypes.WIDGET, cardTarget, connect => ({
    connectDropTarget: connect.dropTarget(),
  }))( Widget );

const WidgetDragSource = DragSource( ItemTypes.WIDGET, cardSource, (connect, monitor) => ({
    connectDragSource: connect.dragSource(),
    isDragging:        monitor.isDragging(),
  }))( WidgetDropTarget )

const WidgetStore = connect( mapStateToProps )( WidgetDragSource );

export default WidgetStore;

