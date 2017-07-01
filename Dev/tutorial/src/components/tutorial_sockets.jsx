import React from 'react'
import AceEditor from 'react-ace'
import { Pagination } from 'react-bootstrap'
import TryNow from './try_now.jsx'


export default class Tutorial extends React.Component {
  
  static code = `
  // Set variables
  var myBankBalance = 0;
  var output = "";

  // The loop
  do {
    output += "My bank balance is now $" + myBankBalance + "<br>";
    myBankBalance ++;
  }
  while (myBankBalance <= 10);

  // Output results to the above HTML element
  document.getElementById("msg").innerHTML = output;
`

  render() {

    return (
      <div>
        <p>
          Sockets
        </p>

        <ul>
          <li>have own routes - may or may not be same</li>
          <li>example</li>
          <li>repeating</li>
          <li>event based</li>
        </ul>

         <AceEditor
            mode="javascript"
            theme="textmate"
            editorProps={{$blockScrolling: true}}
            readOnly
            value={Tutorial.code}
            fontSize={14}
            height='320px'
            width='80%'
            showPrintMargin={false}
            showGutter={true}
            highlightActiveLine={true}
          />

        <p>
          next examples - putting it all together
        </p>

       <Pagination
          items={4}
          />
          {/*activePage={this.state.activePage}
          bsSize="large"
          onSelect={this.handleSelect}*/}
          {/*<li><a href="#">&raquo;</a></li>*/}

        <TryNow url="http://localhost:8342/about" message='It works! The game returned us some data ...' />

      </div>
    )
  }
}

