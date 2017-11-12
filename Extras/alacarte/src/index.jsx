//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { ToastContainer } from 'react-toastify'

import Store from './model/Store.jsx'

import AcNavBar from './components/AcNavBar.jsx'
import AcTabs from './components/AcTabs.jsx'
import AcDialogs from './components/AcDialogs.jsx'
import AcOutput from './components/AcOutput.jsx'

import 'react-toastify/dist/ReactToastify.min.css'
import Style from './style.css'


//-------------------------------------------------------------------------------

ReactDOM.render(

  <Provider store={Store}>
    <div>
      <AcNavBar/>
      <AcTabs/>
      <AcOutput/>
      <AcDialogs/>
      <ToastContainer 
          position="bottom-right"
          autoClose={3000}
          hideProgressBar={true}
          newestOnTop={false}
          closeOnClick
          pauseOnHover
      />      
    </div>
  </Provider>,

  document.getElementById( 'root' )
)
