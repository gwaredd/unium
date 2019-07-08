fs    = require 'fs'
glob  = require 'glob'
chalk = require 'chalk'

# rmf = (f) ->
#   console.log chalk.green( 'delete' ), chalk.white f
#   fs.unlinkSync f if fs.existsSync f

# glob.sync( 'build/*manifest*') .forEach rmf
# rmf 'build/service-worker.js'

# contents = fs.readFileSync 'build/index.html', 'utf8'
# contents = contents.replace /\/static\//g, ''
# fs.writeFileSync 'build/index.html', contents, 'utf8'

