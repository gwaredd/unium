# Fixtures provide test setup and configuration
# https://docs.pytest.org/en/latest/fixture.html#fixtures

import pytest

@pytest.fixture( scope="session" )
def unium_endpoint():
  return "http://localhost:8342"

@pytest.fixture( scope="session" )
def unium_socket():
  return "ws://localhost:8342/ws"

