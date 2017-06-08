#--------------------------------------------------------------------------------

config  = require './config'
req     = require 'requestify'
assert  = require( 'chai' ).assert

describe 'GQL Recursive Find', ->

  #--------------------------------------------------------------------------------
  it 'should be able to perform recrusive finds on objects', ->

    req
      .get "#{ config.url }/q//Cube"
      .then (res) ->
        res.code.should.equal 200
        body = JSON.parse res.body

        body.should.be.an "array"
        body.length.should.equal 4
        cube.name.should.equal "Cube" for cube in body


  #--------------------------------------------------------------------------------
  it 'should be able to perform recrusive finds returning fields', ->

    req
      .get "#{ config.url }/q//Cube.Transform.position"
      .then (res) ->
        res.code.should.equal 200
        body = JSON.parse res.body

        body.should.be.an "array"
        body.length.should.equal 4
        for position in body
          position.should.be.an 'object'
          position.should.have.property 'x'
          position.should.have.property 'y'
          position.should.have.property 'z'


  #--------------------------------------------------------------------------------
  it 'should be able to perform recrusive finds with where clauses', ->

    req
      .get "#{ config.url }/q/scene//[ TestBehaviour.SomeID > 2 ].TestBehaviour.SomeID"
      .then (res) ->
        res.code.should.equal 200
        body = JSON.parse res.body

        body.should.be.an 'array'
        body.length.should.equal 3
        id.should.be.above 2 for id in body


  #--------------------------------------------------------------------------------
  it 'should be able to find objects with tags', ->

    req
      .get "#{ config.url }/q/scene//[tag='Box'].name"
      .then (res) ->
        res.code.should.equal 200
        body = JSON.parse res.body
        body.should.eql [ 'Cube', 'Cube', 'Cube', 'Cube' ]


  #--------------------------------------------------------------------------------
  it 'should be able to find objects with components', ->

    req
      .get "#{ config.url }/q/scene//[has('BoxCollider')=true].BoxCollider.isTrigger"
      .then (res) ->
        res.code.should.equal 200
        body = JSON.parse res.body
        body.should.eql [ false, false, false, false ]


  #--------------------------------------------------------------------------------
  it 'should be able to find objects with attribute chains', ->

    req
      .get "#{ config.url }/q/scene//[has('BoxCollider.isTrigger')=true].BoxCollider.isTrigger"
      .then (res) ->
        res.code.should.equal 200
        body = JSON.parse res.body
        body.should.eql [ false, false, false, false ]



