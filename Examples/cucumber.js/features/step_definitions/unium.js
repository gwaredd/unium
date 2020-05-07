// a class to make the websocket interface a little more test friendly
// by providing a promise/async version of the websocket interface

const chai      = require( 'chai' );
const WebSocket = require( 'ws' );

const { expect } = chai;

module.exports = class UniumHelper {

  constructor() {
    this.ws      = null;
    this.next_id = 1;
    this._on     = {};
    this._once   = {};
  }

  // async connect

  connect( uri ) {
    return new Promise( (resolve, reject) => {
      this.disconnect();
      this.ws = new WebSocket( uri );
      this.ws.on( 'open', resolve );
      this.ws.on( 'error', reject );
      this.ws.on( 'message', (m) => this.onMessage(m) );
    });
  }

  disconnect() {
    if( this.ws ) {
      this.ws.close();
      this.ws = null;
    }
  }


  // when a message is received, invoke any handlers registered for this event

  onMessage( msg ) {

    const data = JSON.parse( msg );
    const {id} = data;

    if(!id) {
      return;
    }

    if( id in this._on ) {
      this._on[ id ].forEach( fn => fn( data ) );
    }

    if( id in this._once ) {
      this._once[ id ].forEach( fn => fn( data ) );
      delete this._once[ id ];
    }
  }

  // add event handlers

  on( event, fn ) {
    if( event in this._on ) {
      this._on[ event ].push( fn );
    } else {
      this._on[ event ] = [ fn ];
    }
  }

  once( event, fn ) {
    if( event in this._once ) {
      this._once[ event ].push( fn );
    } else {
      this._once[ event ] = [ fn ];
    }
  }

  // send message, return message id

  send( uri, id ) {
    const msg = {
      id: id || `m${this.next_id++}`,
      q: uri
    };
    this.ws.send( JSON.stringify( msg ) )
    return msg.id;
  }

  // wait until we receive a message with a given id
  
  wait_for( id, timeout = 2 ) {
    return new Promise( (resolve,reject) => {
      const timeoutHandle = setTimeout( () => reject( 'timeout out' ), timeout * 1000 );
      this.once( id, (data) => {
        clearTimeout( timeoutHandle );
        resolve( data );
      });
    });
  }

  // send a query and result the result (like a HTTP get)

  async get( uri, timeout = 2 ) {

    const id  = await this.send( uri );
    const res = await this.wait_for( id, timeout );

    expect( res ).to.not.be.null;
    res.should.have.property( 'id' );
    res.id.should.equal( id );
    res.should.not.have.property( 'error' );
    res.should.have.property( 'data' );

    return res.data;
  }
  
  // convenient function to binding to events
  // use event name as id unless given on

  async bind( uri, name ) {

    const id = name || uri.split('.').pop();

    await this.send( `/bind${uri}`, id );
    const res = await this.wait_for( id );

    expect( res ).to.not.be.null;
    res.should.have.property( 'id' );
    res.should.not.have.property( 'error' );
    res.should.have.property( 'info' );

    return res;
  }
}
