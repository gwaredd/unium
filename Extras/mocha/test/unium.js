// a class to make the websocket interface a little more test friendly

const chai      = require('chai');
const WebSocket = require( 'ws' );

const { expect } = chai;

module.exports = class UniumHelper {

  constructor() {
    this.ws      = null;
    this.next_id = 1;
    this._on     = {};
    this._once   = {};
  }

  connect( uri ) {
    return new Promise( (resolve,reject) => {
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
  
  wait_for( msg, timeout = 2 ) {
    return new Promise( (resolve,reject) => {
      const timeoutHandle = setTimeout( () => reject( 'timeout out' ), timeout * 1000 );
      this.once( msg, (data) => {
        clearTimeout( timeoutHandle );
        resolve( data );
      });
    });
  }

  async send( uri, name ) {

    const msg = {
      id: name || `m${this.next_id++}`,
      q: uri
    };

    this.ws.send( JSON.stringify( msg ) )

    return msg.id;
  }

  async get( uri, timeout ) {

    const mid = await this.send( uri );
    const msg = await this.wait_for( mid, timeout );

    expect( msg ).to.not.be.null;

    msg.should.have.property( 'id' );
    msg.should.not.have.property( 'error' );
    msg.should.have.property( 'data' );
    msg.id.should.equal( mid );

    return msg.data;
  }

  async bind( uri, name ) {

    const mid = name || uri.split('.').pop();

    await this.send( `/bind${uri}`, mid );
    const res = await this.wait_for( mid );

    expect( res ).to.not.be.null;
    res.should.have.property( 'id' );
    res.should.not.have.property( 'error' );
    res.should.have.property( 'info' );

    return res;
  }

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
}
