<!-- -------------------------------------------------------------------------------- -->

# About

Manage a device farm



<!-- -------------------------------------------------------------------------------- -->

# Dev

## Run docker

> docker run -p 8080:8080 -v /d/dev/unium/Extras/overlord:/src -it mhart/alpine-node:6 /bin/sh


To install nodejs on alpine

> apk add --update nodejs nodejs-npm




<!-- -------------------------------------------------------------------------------- -->

# TODO

manage a device farm

unium minion component

  * add header to register
  * websockets

log / list register devices

  * date and time
  * IP / endpoint
  * app name
  * app version
  * scene name
  * status - registered, connected (websocket), closed

actions

  * take screen shot
  * view log
  * send query
  * open endpoint

  * remove / kick device

power mode

  * create custom queries, menus and visualisations
  * conditional - based on scene, in-game event
  * run custom scripts / tests

overlord api endpoint

  * get device list
  * webhooks - on join



