//--------------------------------------------------------------------------------

import React from 'react'

import 'bootstrap/dist/css/bootstrap.min.css';
import './theme.css'

import Intro from './components/intro.jsx'
import NavBar from './components/navbar.jsx'
import Tutorial from './components/tutorial.jsx'

import 'brace/mode/csharp';
import 'brace/theme/textmate';
import 'brace/mode/javascript';

//--------------------------------------------------------------------------------

export default class App extends React.Component {

  constructor( props ) {
    super( props )
    this.state    = { page:0 }
    this.setPage  = this.setPage.bind( this )
  }

  setPage( page ) {
    this.setState( { page:page } )
  }

  render () {
    return (
      <div>
        <NavBar/>
        <div className='container-fluid'>
          { this.state.page===0 ? (<Intro setPage={this.setPage} />) : (<Tutorial page={this.state.page} setPage={this.setPage}/>) }
        </div>
      </div>
    )
  }
}


