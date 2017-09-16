import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'

import App from './app.jsx'
import Store from './store.jsx'

const app = document.getElementById( 'root' )

ReactDOM.render(
  <Provider store={Store}>
    <App/>
  </Provider>, app )

