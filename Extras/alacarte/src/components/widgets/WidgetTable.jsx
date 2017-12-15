
import React from 'react'
import Widget from './Widget.jsx'

export default class WidgetTable {


  componentWillMount() {
  }

  //-------------------------------------------------------------------------------
  
  render() {

    const { widget } = this.props

    return (
      <span>{ widget.name }</span>
    )
  }

  options() {
    return (
      <span>table options goes here</span>
    )
  }

}
