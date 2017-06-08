#--------------------------------------------------------------------------------

config  = require './config'
req     = require 'requestify'


describe 'Basic Connectivity', ->

  #--------------------------------------------------------------------------------
  it 'should be able to get information about the service', ->

    req.get( "#{ config.url }/about" ).then (res) ->

      res.code.should.equal 200
      res.headers.should.have.property( 'content-type' ).equal 'application/json'   # is json
      res.headers.should.have.property( 'access-control-allow-origin' ).equal '*'   # allow cors

      body = JSON.parse res.body
      body.should.have.property 'Unium'


  #--------------------------------------------------------------------------------
  it 'should be able to check the status of the service', ->

    req.get( "#{ config.url }/about" ).then (res) ->

      res.code.should.equal 200
      res.headers.should.have.property( 'content-type' ).equal 'application/json'   # is json
      res.headers.should.have.property( 'access-control-allow-origin' ).equal '*'   # allow cors

      body = JSON.parse res.body
      body.should.have.property 'RunningTime'
      body.should.have.property 'FPS'
      body.should.have.property 'Scene'


  #--------------------------------------------------------------------------------
  it 'should be able to get the custom route', ->

    req.get( "#{ config.url }/test" ).then (res) ->
      res.code.should.equal 200
      body = JSON.parse res.body
      body.should.have.property 'test'
      body.test.should.equal "ok"


  #--------------------------------------------------------------------------------
  it 'should handle a not found request', ->

    req.get( "#{ config.url }/notfound" ).fail (res) ->
      res.code.should.equal 404


  #--------------------------------------------------------------------------------
  it 'should have a GQL stats end point', ->

    req.get( "#{ config.url }/q/stats" ).then (res) ->
      res.code.should.equal 200
      res.headers.should.have.property( 'content-type' ).equal 'application/json'   # is json
      res.headers.should.have.property( 'access-control-allow-origin' ).equal '*'   # allow cors


      body = JSON.parse res.body
      body.should.be.an "array"
      body.should.have.length 1

      body[0].should.have.property 'RunningTime'
      body[0].should.have.property 'FPS'
      body[0].should.have.property 'Scene'
      

  #--------------------------------------------------------------------------------
  it 'should have a GQL scene end point', ->

    req.get( "#{ config.url }/q/scene" ).then (res) ->
      res.code.should.equal 200
      res.headers.should.have.property( 'content-type' ).equal 'application/json'   # is json
      res.headers.should.have.property( 'access-control-allow-origin' ).equal '*'   # allow cors

      body = JSON.parse res.body
      body.should.be.an "array"

