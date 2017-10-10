//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'

import Store from './model/Store.jsx'

import AcNavBar from './components/AcNavBar.jsx'
import AcTabs from './components/AcTabs.jsx'
import AcModalScreenshot from './components/AcModalScreenshot.jsx'
import AcModalConfirm from './components/AcModalConfirm.jsx'
import AcModalAddTab from './components/AcModalAddTab.jsx'
import AcModalAddPanel from './components/AcModalAddPanel.jsx'
import AcOutput from './components/AcOutput.jsx'

import Style from './style.css'


//-------------------------------------------------------------------------------

ReactDOM.render(

  <Provider store={Store}>
    <div>
      <AcNavBar/>
      <AcTabs/>
      <AcOutput/>
      
      <AcModalScreenshot/>
      <AcModalConfirm/>
      <AcModalAddTab/>
      <AcModalAddPanel/>
    </div>
  </Provider>,

  document.getElementById( 'root' )
)
