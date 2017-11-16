//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import RGL, { Responsive, WidthProvider } from 'react-grid-layout'
import GridStyle from 'react-grid-layout/css/styles.css'
import ResizeStyle from 'react-resizable/css/styles.css'
import _ from 'lodash'

import AcPanel from './AcPanel.jsx'

const ResponsiveReactGridLayout = WidthProvider( Responsive )
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

    const { panels, tabId }  = this.props

    const unlocked  = "edit" in panels.state ? panels.state.edit : 0
    const panelList = _.filter( panels.byId, (v) => v.tab == tabId )

    var contents = _.map( panelList, (p,i) =>
      <div key={i} data-grid={p.layout}>
        <AcPanel panel={p} isLocked={p.id != unlocked} />
      </div>
    )
    
    return (
      <ReactGridLayout
        breakpoints={{lg: 1200, md: 996, sm: 768, xs: 480, xxs: 0}}
        {...this.props}
      >
        { contents }
      </ReactGridLayout>
    )
  }
}
