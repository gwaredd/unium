fs    = require 'fs-extra'
glob  = require 'glob'
chalk = require 'chalk'

dest = '../../Assets/StreamingAssets/alacarte'

info = (action,object) ->
  console.log chalk.green( action ), chalk.white object

rmf = (f) ->
  info 'delete', f
  fs.unlinkSync f if fs.existsSync f

rebase = (f,alt) ->
  info 'rebase', f
  contents = fs.readFileSync f, 'utf8'
  contents = contents.replace /\/static\//g, '/alacarte/static/'
  fs.writeFileSync f, contents, 'utf8'

rmf 'build/service-worker.js'
glob.sync( 'build/*manifest*' ).forEach rmf
glob.sync( 'build/static/css/*.map' ).forEach rmf
glob.sync( 'build/static/js/*.map' ).forEach rmf

rebase 'build/index.html'
glob.sync( 'build/static/css/*.css').forEach rebase

info 'copy to', dest
fs.removeSync dest
fs.copySync 'build', dest
