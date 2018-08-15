import React from 'react'
import AceEditor from 'react-ace'
import { Button, Pagination } from 'react-bootstrap'
import TryNow from './try_now.jsx'


//--------------------------------------------------------------------------------

class SocketsDemo extends React.Component {

  Log( txt ) {
    var div = document.getElementById( 'output' )
    if( div ) {
      div.innerHTML += txt + '\n'
    }
  }

  constructor( props ) {
    super( props )
    this.state = {
      ws: null
    }
  }

  componentDidMount() {
    var div = document.getElementById( 'output' )
    if( div ) {
      div.innerHTML = ''
    }
  }

  componentWillUnmount() {
    if( this.state.ws ) {
      this.state.ws.close()
    }
  }

  send(msg) {
    msg = JSON.stringify(msg)
    this.Log( "Sending : " + msg )
    this.state.ws.send(msg)
  }

  connect() {

    var demo = this
    var ws = new WebSocket( "ws://localhost:8342/ws" )

    ws.onerror    = ()  => { demo.Log( "Error connecting to game - is it running?" ) }
    ws.onclose    = ()  => { demo.Log( "Connection closed" ) }
    ws.onmessage  = (m) => { demo.Log( "Received: " + m.data ) }

    this.state.ws = ws

    return ws
  }

  render() {
   return (
     <div>
      <Button bsStyle="info" onClick={()=>this.Run()}>Try Now</Button>
      <br/><br/>
      <pre id='output'>
      </pre>
    </div>
   )
  }
}

//--------------------------------------------------------------------------------

class SocketsBasicDemo extends SocketsDemo {

  Run() {

    var demo = this
    var ws = this.connect()

    ws.onopen = () => {
      demo.Log( "Connected to game" )
      demo.send({ id:'some_id', q:'/about' })
    };

    ws.onmessage = (m) => { demo.Log( "Received: " + m.data ); ws.close() }
  }
}

function Sockets_Basic() {
 return <div>
    <p>
      The RESTful interface is convenient but there are a couple of things it is not suitable for. Specifically, watching variables change over time and getting notifications about in-game events. This is where the web socket interface comes in.
    </p>
    <p>
      Unium has a simple JSON message format. Queries are passed along with an (optional) id in this format:
    </p>
    <pre>
        &#123; id: "&lt;message id&gt;", q: "&lt;/query&gt;" &#125;
    </pre>
    <p>
      A reply is of the form
    </p>
    <pre>
      &#123;  id: "&lt;message id&gt;", data: ... &#125;
    </pre>
    <p>
      Or
    </p>
    <pre>
      &#123; id: "&lt;message id&gt;", error: ... &#125;
    </pre>
    <p style={{marginTop:"50px"}}>
      To see the about query in action click the button below.
    </p>
    <SocketsBasicDemo/>
  </div>
}

//--------------------------------------------------------------------------------

class SocketsWatcherDemo extends SocketsDemo {

  Run() {

    var demo = this
    var ws = this.connect()

    ws.onopen = () => {
      demo.Log( "Connected to game" )
      demo.send({ id:'fps', q:'/q/stats.FPS', repeat: { freq: 1 } })
    };
  }
}

function Sockets_Watcher() {
 return <div>
    <p>
      Watching variables is done with "repeaters". Repeaters repeat the query at a given period to “sample” the results.
    </p>
    <p>
      For example, to sample the framerate every second we send ...
    </p>
    <pre>
      &#123; id: "fps", q:"/q/stats.FPS", repeat:&#123; freq: 1 &#125; &#125;
    </pre>
    <SocketsWatcherDemo/>
  </div>
}

//--------------------------------------------------------------------------------

class SocketsBindDemo extends SocketsDemo {

  Run() {

    var demo = this
    var ws = this.connect()

    ws.onopen = () => {
      demo.Log( "Connected to game" )
      demo.send({ id:'debug', q:'/bind/events.debug' })
    };

    ws.onmessage = (m) => {
      demo.Log( "Recieved: " + m.data );
      var msg = JSON.parse( m.data )

      if( msg.info ) {
        demo.send({ id:'hello', q:'/q/scene/Game.Tutorial.SayHello()' })
      } else if( msg.data && msg.id == 'hello' ) {
        ws.close()
      }
    }
  }
}


function Sockets_Bind() {
 return (
    <div>
      <p>
        Unium can “bind” to any C# event with a delegate type of Action&lt;object&gt; that is accessible in the GQL tree using the /bind route.
      </p>
      <p>
        Any events that fire will have the same message id as the bound event. If the query successfully bound to an event it will return an 'info' message of the form below before any events are sent (otherwise an error message).
      </p>
      <pre>
        &#123; id: "&lt;message id&gt;", info:"bound" &#125;
      </pre>
      <p>
        For example, to get the debug output ...
      </p>
      <SocketsBindDemo/>
    </div>
  )
}


//--------------------------------------------------------------------------------

const pages = [
  null,
  React.createFactory( Sockets_Basic ),
  React.createFactory( Sockets_Watcher ),
  React.createFactory( Sockets_Bind ),
]


//--------------------------------------------------------------------------------


export default class Tutorial extends React.Component {

  constructor( props ) {
    super( props )
    this.state = { activePage: 1 }
  }

  onSelect(n) {
    this.setState({activePage: n})
  }

  render() {
   return (
      <div>
        <Pagination
          items={3}
          next={true}
          prev={true}
          activePage={ this.state.activePage }
          onSelect={(n) => this.onSelect(n) }
          />
          { pages[ this.state.activePage ]() }
      </div>
    )
  }
}

