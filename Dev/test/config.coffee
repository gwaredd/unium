require( 'chai' ).should()
argv = require( 'optimist' ).argv

ip = if argv.ip? then argv.ip else "localhost"

module.exports = 
  sockets : "ws://#{ip}:8342/ws"
  url     : "http://#{ip}:8342"

