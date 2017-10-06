//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import RGL, { WidthProvider } from 'react-grid-layout'
import GridStyle from 'react-grid-layout/css/styles.css'
import ResizeStyle from 'react-resizable/css/styles.css'
import _ from 'lodash'

import AcPanel from './AcPanel.jsx'


const ReactGridLayout = WidthProvider( RGL )


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    panels: store.panels
  }
})
export default class AcGrid extends React.PureComponent {

  static defaultProps = {
    className : "layout",
    items     : 1,
    rowHeight : 150,
    cols      : 4
  }

  //-------------------------------------------------------------------------------

  render() {

    var tabId  = this.props.tabId
    var panels = _.filter( this.props.panels.byId, (v) => v.tab == tabId )

    var dom = _.map( panels, (p,i) =>
      <div key={i} data-grid={p.layout}>
        <AcPanel panel={p} />
      </div>
    )
    
    return (
      <ReactGridLayout {...this.props}>
        { dom }
      </ReactGridLayout>
    )
  }
}
