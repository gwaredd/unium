#--------------------------------------------------------------------------------

config  = require './config'
assert  = require( 'chai' ).assert
req     = require 'requestify'


describe 'GQL Positive Tests', ->

  #--------------------------------------------------------------------------------
  it 'should be able to query stats', ->

    req.get( "#{ config.url }/q/stats" ).then (res) ->
      res.code.should.equal 200

      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 1

      body[0].should.have.property 'RunningTime'
      body[0].should.have.property 'FPS'
      body[0].should.have.property 'Scene'
      

  #--------------------------------------------------------------------------------
  it 'should be able to query the scene graph', ->

    req.get( "#{ config.url }/q/scene" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"


  #--------------------------------------------------------------------------------
  it 'should be able to get the game objects', ->

    req.get( "#{ config.url }/q/scene/Game/*" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length.above 1


  #--------------------------------------------------------------------------------
  it 'should be able to get the cubes', ->

    req.get( "#{ config.url }/q/scene/Game/Cube" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 4


  #--------------------------------------------------------------------------------
  it 'should be able to get the cube position with where clauses', ->

    req.get( "#{ config.url }/q/scene/Game/Cu*.Transform.position[x>=0].x" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 3
      body.should.include 0
      body.should.include 5


  #--------------------------------------------------------------------------------
  it 'should be able to call functions', ->

    req.get( "#{ config.url }/q/scene/Game/Cube[0].TestBehaviour.CallThisFunction()" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 1
      body.should.include 1


  #--------------------------------------------------------------------------------
  it 'should be able to call functions with parameters', ->

    req.get( "#{ config.url }/q/scene/Game/Cube[0].TestBehaviour.CallThisFunctionWithParams(3,4)" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 1
      body.should.include 7


  #--------------------------------------------------------------------------------
  it 'should be able to call multiple functions with parameters', ->

    req.get( "#{ config.url }/q/scene/Game/Cube.TestBehaviour.CallThisFunctionWithParams(3,4)" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 4
      body.should.eql [7,7,7,7]


  #--------------------------------------------------------------------------------
  it 'should be able to set variables', ->

    req.get( "#{ config.url }/q/scene/Game/Cube[0].TestBehaviour.RandomValue=7" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 1
      body.should.eql [ 7 ]


  #--------------------------------------------------------------------------------
  it 'should be able to set multiple variables', ->

    # set values

    req.get "#{ config.url }/q/scene/Game/Cube.TestBehaviour.RandomValue=89"
    .then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 4
      body.should.eql [ 89, 89, 89, 89 ]

    # check they were actually set

    .then -> req.get "#{ config.url }/q/scene/Game/Cube[0].TestBehaviour.RandomValue" 
    .then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 1
      body.should.eql [ 89 ]


  #--------------------------------------------------------------------------------
  it 'should not be able to call socket commands',  ->
    req
      .get "#{ config.url }/socket/cmd.ping()"
      .catch (res) -> res.code.should.equal 404


#################################################################################

describe 'GQL Negative Tests', ->

  #--------------------------------------------------------------------------------
  it 'should be able to handle malformed queries', ->
    req.get( "#{ config.url }/q/@£$!@$!$£" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0


  #--------------------------------------------------------------------------------
  it 'should be able to handle functions that do not exist', ->
    req.get( "#{ config.url }/q/scene/Game/Cube.TestBehaviour.NotAFunction()" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0


  #--------------------------------------------------------------------------------
  it 'should be able to handle variables that do not exist', ->
    req.get( "#{ config.url }/q/scene/Game/Cube.TestBehaviour.NotAValue=5" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0


  #--------------------------------------------------------------------------------
  it 'should be able to handle bad function parameters conversions', ->
    req.get( "#{ config.url }/q/scene/Game/Cube[0].TestBehaviour.CallThisFunctionWithParams(a,b)" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0
  

  #--------------------------------------------------------------------------------
  it 'should be able to handle function without enough parameters', ->
    req.get( "#{ config.url }/q/scene/Game/Cube[0].TestBehaviour.CallThisFunctionWithParams(7)" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0


  #--------------------------------------------------------------------------------
  it 'should be able to handle function with too many parameters', ->
    req.get( "#{ config.url }/q/scene/Game/Cube[0].TestBehaviour.CallThisFunctionWithParams(7,1,2)" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0


  #--------------------------------------------------------------------------------
  it 'should be able to handle bad variable conversions', ->
    req.get( "#{ config.url }/q/scene/Game/Cube.TestBehaviour.RandomValue=fish" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0


  #--------------------------------------------------------------------------------
  it 'should not reflect events', ->
    req.get( "#{ config.url }/q/scene/Game.Test.TickEvent" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 0

