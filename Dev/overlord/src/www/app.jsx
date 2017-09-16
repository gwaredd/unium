import React from 'react';
import { Alert } from 'react-bootstrap';

const alertInstance = (
  <Alert bsStyle="warning">
    <strong>Holy guacamole!</strong> Best check yo self, you're not looking too good.
  </Alert>
);

export default class App extends React.Component {
  render() {
    return (
     <div style={{textAlign: 'center'}}>
        <h1>Hello World</h1>
        { alertInstance }
      </div>
    );
  }
}
