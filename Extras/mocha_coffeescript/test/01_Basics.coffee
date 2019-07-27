# https://mochajs.org/#getting-started

# a basic test

assert = require 'assert'

describe 'A basic test', () ->
  it 'Assert true', () ->
    # put your test code here
    assert true


# the chai library provides a nicer way to write assertions (BDD style)
# https://www.chaijs.com/

require( 'chai' ).should()

describe 'A test with the chai assertion library', () ->
  it 'An empty array should have length 0', () ->
    [].length.should.equal 0

