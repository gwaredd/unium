//-------------------------------------------------------------------------------

import React from 'react'
import PropTypes from 'prop-types'
import _ from 'underscore'
import RGL, { WidthProvider } from 'react-grid-layout'

import AcPanel from './AcPanel.jsx'

const ReactGridLayout = WidthProvider(RGL)

import GridStyle from 'react-grid-layout/css/styles.css'
import ResizeStyle from 'react-resizable/css/styles.css'

export default class AcGrid extends React.PureComponent {

  static propTypes = {
    onLayoutChange: PropTypes.func.isRequired
  }

  static defaultProps = {
    className: "layout",
    items: 1,
    rowHeight: 150,
    onLayoutChange: function() {},
    cols: 4
  }

  constructor(props) {
    super(props)

    const layout = this.generateLayout()
    this.state = { layout }
  }

  generateDOM() {
    return _.map(_.range(this.props.items), function(i) {
      return (
        <div key={i}>
          <AcPanel/>
        </div>
      )
    })
  }

  generateLayout() {
    const p = this.props
    return _.map(new Array(p.items), function(item, i) {
      const y = _.result(p, 'y') || Math.ceil(Math.random() * 4) + 1
      return {x: 2 * i % 4, y: Math.floor(i / 6) * y, w: 2, h: 2, i: i.toString()}
    })
  }

  onLayoutChange(layout) {
    this.props.onLayoutChange(layout)
  }

  render() {
    return (
      <ReactGridLayout layout={this.state.layout} onLayoutChange={this.onLayoutChange}
          {...this.props}>
        {this.generateDOM()}
      </ReactGridLayout>
    )
  }
}
