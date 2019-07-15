fs    = require 'fs-extra'
path  = require 'path'
glob  = require 'glob'
chalk = require 'chalk'

# config

dest = '../../Assets/StreamingAssets/tutorial'
static_folder = '/tutorial/static/'
examples = '../../Extras/examples/*.html'

# utils

info = (action,object) ->
  console.log chalk.green( action ), chalk.white object

cpf = (file) ->
  to = "#{dest}/#{path.basename file}"
  info 'copy', "#{file} -> #{to}"
  fs.copySync file, to

rmf = (f) ->
  info 'delete', f
  fs.unlinkSync f if fs.existsSync f

rebase = (f,alt) ->
  info 'rebase', f
  contents = fs.readFileSync f, 'utf8'
  contents = contents.replace /\/static\//g, static_folder
  fs.writeFileSync f, contents, 'utf8'

# release

rmf 'build/service-worker.js'
glob.sync( 'build/*manifest*' ).forEach rmf
glob.sync( 'build/static/css/*.map' ).forEach rmf

rebase 'build/index.html'
glob.sync( 'build/static/css/*.css').forEach rebase

info 'copy to', dest
fs.removeSync dest
fs.copySync 'build', dest

glob.sync( examples ).forEach cpf
