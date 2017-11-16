//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { ToastContainer } from 'react-toastify'

import Store from './model/Store.jsx'

import AcNavBar from './components/app/AcNavBar.jsx'
import AcTabs from './components/app/AcTabs.jsx'
import AcDialogs from './components/app/AcDialogs.jsx'
import AcOutput from './components/app/AcOutput.jsx'

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

Store.dispatch( { type: 'APP_LOAD' } )

