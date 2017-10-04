#--------------------------------------------------------------------------------

chalk = require 'chalk'

Logger = ->

  @ip       = "-"
  @colours  = true

  @print = (ip, type, msg) ->

    ip    = @ip unless ip?
    time  = new Date().toISOString()
    type  = "[#{type}]"

    if @colours
      type  = chalk.red "#{type}"     if type == "[ERROR]"
      type  = chalk.magenta "#{type}" if type == "[WARN ]"
      type  = chalk.cyan "#{type}"    if type == "[INFO ]"
    
    console.log "[#{time}] #{ip} #{type} #{msg}"

  @log   = (ip,msg) -> @print ip, "INFO ", msg
  @warn  = (ip,msg) -> @print ip, "WARN ", msg
  @error = (ip,msg) -> @print ip, "ERROR", msg

  return this


#--------------------------------------------------------------------------------

module.exports = new Logger()

