import pytest
import json
import async_timeout

class WebsocketHelper:

  """A class to make the websocket interface more test friendly"""

  def __init__( self, ws ):
    """instantiate with the websocket to use"""
    self.ws = ws
    self.next_id = 1
    self._on = {}
    self._once = {}


  def _handle_message( self, msg ):
    """fire off any event handlers waiting on the given message"""

    if not "id" in msg:
      return

    mid = msg["id"]

    if mid in self._on:
      for fn in self._on[ mid ]:
        fn( msg )

    if mid in self._once:
      for fn in self._once[ mid ]:
        fn( msg )
      del self._once[ mid ]


  async def wait_for( self, event, timeout=2.0 ):
    """wait until we receive a message with a specific id (or timeout)"""

    async with async_timeout.timeout( timeout ):
      while True:

        msg = json.loads( await self.ws.recv() )
        self._handle_message( msg )

        if "id" in msg and msg["id"] == event:
          return msg


  async def send( self, uri, name="" ):
    """send a message, return message id"""

    if name == "":
      mid = f"m{self.next_id}"
      self.next_id += 1
    else:
      mid = name

    await self.ws.send( json.dumps( { 'id': mid, 'q' : uri } ))

    return mid


  async def get( self, uri, timeout=1.0 ):
    """send a message and return the response (or timeout)"""

    async with async_timeout.timeout( timeout ):

      mid = await self.send( uri )
      msg = await self.wait_for( mid, timeout )

      assert "id" in msg
      assert "data" in msg
      assert msg["id"] == mid

      return msg['data']


  async def bind( self, uri, name="" ):
    """
    register to receive events
    uses the event name from the URI as the identifier unless we've given given one explicitly
    """
    mid = name if name else uri.split(".")[-1]

    await self.send( f"/bind{uri}", mid )

    response = await self.wait_for( mid )
    assert "id" in response
    assert "info" in response

    return response


  def on( self, event, function ):
    """register an event handler - will be fired every time a message with a given id is received"""
    if not event in self._on:
      self._on[ event ] = []
    self._on[ event ].append( function )


  def once( self, event, function ):
    """register an event handler - will be fired once a message with a given id is received"""
    if not event in self._once:
      self._once[ event ] = []
    self._once[ event ].append( function )

