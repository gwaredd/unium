{spawn} = require 'child_process'
fs      = require 'fs-extra'
glob    = require 'glob'
chalk   = require 'chalk'


info = (action,object) ->
  console.log chalk.green( action ), chalk.white object


bumpVersion = ->

  file = '../Assets/Unium/Unium.cs'
  re = /Version\(\s*(\d+)\s*,\s*(\d+)\s*,\s*(\d+)\s*\)/

  # read file
  contents = fs.readFileSync file, 'utf8'

  # bump version
  match = re.exec contents
  version = match[ 1..3 ].map (v) -> parseInt v
  version[2]++

  # replace file
  contents = contents.replace re, "Version( #{ version.join ', ' } )"
  fs.writeFileSync file, contents, 'utf8'

  return version.join '.'


ver = bumpVersion()
info 'version', ver

info 'update', 'unium.pdf'
fs.copySync '../unium.pdf', '../Assets/Unium/unium.pdf'

info 'git', 'add files'
git = spawn 'git', "add --verbose ../Assets/Unium".split ' '
git.stdout.on 'data', (data) -> console.log data.toString().trim()
git.stderr.on 'data', (data) -> console.error data.toString().trim()
