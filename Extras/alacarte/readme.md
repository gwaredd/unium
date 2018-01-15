# Unium À La Carte

> Alacarte is an external debug menu implementation onto a game running unium. It allows simple debug menus to be created quickly.

## Install

To install, copy the contents of the dist folder to the StreamingAssets folder in your project.

Then point your webrowser at the unium endpoint, e.g. `http://localhost:8342/` when the game is running.

Configuration files are saved to the persisterent storage on device.

See the [documentation](https://github.com/gwaredd/unium/raw/master/Extras/alacarte/Unium%20A%20La%20Carte.pdf)

## Building

If you want to build the source ...

### Requirements

* [node.js](https://nodejs.org/) - v6.11.4
* [yarn](https://yarnpkg.com/) - v1.3.2


### To Build

* First install all the dependent libraries with `yarn install`
* Then use `yarn start` to start a development server locally, or `yarn release` to build a release version of the application (output goes to the dist folder).

