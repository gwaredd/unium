#--------------------------------------------------------------------------------

log       = require 'npmlog'
util      = require 'util'
moment    = require 'moment'


# override log write to augment with dynamic text

log.__write = log.write
log.write = (msg, style) -> this.__write ( if util.isFunction msg then msg() else msg ), style

log.prefixStyle = fg: 'white'
log.heading = -> "[" + moment().format( "YYYY-MM-DD HH:MM:SS" ) + "]"

# set default colours

log.style.warn  = fg: 'yellow', bg: 'black'
log.disp.info   = ''
log.disp.http   = ' [DEBUG]'
log.disp.warn   = ' [WARNING]'
log.disp.error  = ' [ERROR]'

module.exports = log

