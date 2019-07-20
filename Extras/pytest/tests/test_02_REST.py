import pytest
import requests

def test_isTutorialScene( unium_endpoint ):
  """basic test using the REST interface"""

  response = requests.get( f"{unium_endpoint}/about" )
  assert response.status_code == 200

  data = response.json()
  assert data["Product"] == 'unium'
  assert data["Company"] == 'gwaredd'
  assert data["Scene"] == 'Tutorial'

