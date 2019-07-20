import pytest
import json
import websockets

@pytest.mark.asyncio
async def test_webSocket( unium_socket ):
  """simple test using the WebSocket interface"""

  async with websockets.connect( unium_socket ) as ws:

    msg = json.dumps( { "q": "/about" } )
    await ws.send( msg )
    
    response = json.loads( await ws.recv() )
    assert "data" in response

    data = response["data"]
    assert data["Product"] == 'unium'
    assert data["Company"] == 'gwaredd'
    assert data["Scene"] == 'Tutorial'
