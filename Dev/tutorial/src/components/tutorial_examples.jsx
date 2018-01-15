import React from 'react'
import { ListGroup, ListGroupItem, Panel } from 'react-bootstrap'

export default class Tutorial extends React.Component {

  render() {
    return (
      <div>
        <p>
          Here are some simplfied examples that put it all together
        </p>
        <ul>
          <li><a href='debug_output.html' target='_blank'>Debug Output</a></li>
          <li><a href='fps_graph.html' target='_blank'>FPS Graph</a></li>
          <li><a href='automated_play.html' target='_blank'>Automated Playthrough</a></li>
          <li><a href='sockets.html' target='_blank'>Socket Explorer</a></li>
          <li><a href='../index.html' target='_blank'>Debug Menu</a></li>
        </ul>
      </div>
    )
  }
}

