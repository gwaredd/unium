require( 'chai' ).should()
argv = require( 'optimist' ).argv

ip = if argv.ip? then argv.ip else "127.0.0.1"

module.exports = 
  sockets : "ws://#{ip}:8342/ws"
  url     : "http://#{ip}:8342"

