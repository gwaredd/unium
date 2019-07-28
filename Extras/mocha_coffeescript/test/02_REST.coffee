# https://github.com/axios/axios

{ test } = require 'mocha'
axios    = require 'axios'
config   = require './config'

context 'Asynchronous HTTP calls ', () ->

  test 'Check we are in the tutorial scene', () ->

    # get data from unium
    res = await axios.get "#{config.url}/about"

    # check web request was OK
    res.status.should.equal 200

    # check data returned is as expected
    { Product, Company, Scene } = res.data
    Product.should.equal 'unium'
    Company.should.equal 'gwaredd'
    Scene.should.equal 'Tutorial'
