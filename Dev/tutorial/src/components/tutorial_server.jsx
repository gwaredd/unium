import React from 'react'
import TryNow from './try_now.jsx'

export default class Tutorial extends React.Component {

  render() {

    return (
      <div>
        <p>
          Essentially, Unium is a webserver that you embed into your game. This gives you a remote API that you can target for automated tests or debug tools.
        </p>
        <p>
          The 
          

Appium is built on the idea that testing native apps shouldn't require including an SDK or recompiling your app. And that 

Meaning you are free to use your preferred test practices, frameworks, and tools.
        </p>
        <p>
          Check the tutorial scene is running by clicking <b><i>Try Now</i></b> below to fetch some basic information about the game.
        </p>

        <TryNow url="http://localhost:8342/about" message='It works! The game returned us some data ...' />

      </div>
    )
  }
}

