import pytest

# basic test

def test_basic():
  assert True


# test with configration parameters (see conftest.py)

def test_withFixture( unium_endpoint ):
  assert isinstance( unium_endpoint, str )

