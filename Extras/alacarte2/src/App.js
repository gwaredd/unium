//-------------------------------------------------------------------------------

import React from 'react'
import { Provider } from 'react-redux'
import { ToastContainer } from 'react-toastify'

import AcNavBar from './components/app/AcNavBar'
import AcTabs from './components/app/AcTabs'
import AcDialogs from './components/app/AcDialogs'
import AcOutput from './components/app/AcOutput'

import 'react-toastify/dist/ReactToastify.min.css'

import Store from './model/Store'

//-------------------------------------------------------------------------------

export default class App extends React.Component {
  render() {
    return (
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
      </Provider>
    );
  }
}

Store.dispatch( { type: 'APP_LOAD' } )
