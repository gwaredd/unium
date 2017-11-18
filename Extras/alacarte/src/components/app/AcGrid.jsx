//-------------------------------------------------------------------------------

import React from 'react'
import { connect } from 'react-redux'
import { Responsive, WidthProvider } from 'react-grid-layout'
import GridStyle from 'react-grid-layout/css/styles.css'
import ResizeStyle from 'react-resizable/css/styles.css'
import _ from 'lodash'

import AcPanel from './AcPanel.jsx'

const ResponsiveReactGridLayout = WidthProvider( Responsive )


//-------------------------------------------------------------------------------

@connect( (store) => {
  return {
    panels: store.panels
  }
})
export default class AcGrid extends React.PureComponent {

  static defaultProps = {
    className   : "layout",
    items       : 1,
    rowHeight   : 150,
    breakpoints : { lg: 1200, md: 996, sm: 768, xs: 480, xxs: 0 },
    cols        : { lg: 6, md: 4, sm: 2, xs: 1, xxs: 1 },
    //cols      : 4
  }

  //-------------------------------------------------------------------------------

  render() {

    const { panels, tabId }  = this.props

    const unlocked  = "edit" in panels.state ? panels.state.edit : 0
    const panelList = _.filter( panels.byId, (v) => v.tab == tabId )

    var contents = _.map( panelList, (p,i) =>
      <div key={i}>
        <AcPanel panel={p} isLocked={p.id != unlocked} />
      </div>
    )

    // //...
// render() {
//   // {lg: layout1, md: layout2, ...}
//   var layouts = getLayoutsFromSomewhere();
//   return (
//     <ResponsiveReactGridLayout className="layout" layouts={layouts}
//       breakpoints={{lg: 1200, md: 996, sm: 768, xs: 480, xxs: 0}}
//       cols={{lg: 12, md: 10, sm: 6, xs: 4, xxs: 2}}>
//       <div key="1">1</div>
//       <div key="2">2</div>
//       <div key="3">3</div>
//     </ResponsiveReactGridLayout>
//   )
// }

    
    return (
      <ResponsiveReactGridLayout {...this.props}>
        { contents }
      </ResponsiveReactGridLayout>
    )
  }
}
