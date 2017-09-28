import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'

import App from './app.jsx'
import Store from './store.jsx'

Store.dispatch( { type: 'OVERLORD_CONNECT' } )

ReactDOM.render(

  <Provider store={Store}>
    <App/>
  </Provider>,

  document.getElementById( 'root' )
)

