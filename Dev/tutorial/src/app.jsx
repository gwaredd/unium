//--------------------------------------------------------------------------------

import React from 'react'
import ReactDOM from 'react-dom'

import Bootstrap from 'bootstrap/dist/css/bootstrap.css';
import Theme from './resources/theme.css'
import Style from './resources/custom.css'

import Intro from './components/intro.jsx'
import NavBar from './components/navbar.jsx'
import Tutorial from './components/tutorial.jsx'

import 'brace/mode/csharp';
import 'brace/theme/textmate';
import 'brace/mode/javascript';

//--------------------------------------------------------------------------------

class App extends React.Component {

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
          { this.state.page==0 ? (<Intro setPage={this.setPage} />) : (<Tutorial page={this.state.page} setPage={this.setPage}/>) }
        </div>
      </div>
    )
  }
}

ReactDOM.render( <App/>, document.getElementById('app') )

