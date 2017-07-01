import React from 'react'
import TryNow from './try_now.jsx'

export default class Tutorial extends React.Component {

  render() {

    return (
      <div>
        <p>
          Unium is an experimental library for the purposes of facilitating automated testing and tool development for your Unity games.
        </p>
        <p>
          The main idea is twofold. Firstly, embed a web server into your project to provide an interface onto the game. Secondly, implement a query language that to some degree takes care of the tedious bits.
        </p>
        <p>
          The advantages of a web server is that HTTP provides a technology agnostic protocol that places no restrictions on whatever tools and frameworks you wish to use. It also means it will work whether the game is running in editor, on device or on some headless server in the clouds.
        </p>
        <p>
          Check the tutorial scene is running by clicking <b><i>Try Now</i></b> below to fetch some basic information about the game, or alternatively copy the url into your browser.
        </p>

        <TryNow url="http://localhost:8342/about" message='It works! The game returned us some data ...' />

      </div>
    )
  }
}

