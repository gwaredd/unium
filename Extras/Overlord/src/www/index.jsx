//-------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'
import { Provider } from 'react-redux'
import { Panel } from 'react-bootstrap';

import Store from './model/store.jsx'
import * as actions from './model/actions.jsx'

import Menu from './menu.jsx'
import Minions from './minions.jsx'


//-------------------------------------------------------------------------------

Store.dispatch( actions.ovConnect() )

ReactDOM.render(

  <Provider store={Store}>
    <div>
      <Menu />
      <Panel header="Minions" bsStyle="primary">
        <Minions/>
      </Panel>
    </div>
  </Provider>,

  document.getElementById( 'root' )
)

