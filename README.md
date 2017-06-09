# unium
> An open source test automation framework for Unity games and the occasional debug tool

Fundamentally, Unium is a web server.

If you embed it into your game, you get an remote interface for querying and changing the game state whether it is running in editor, on device or somewhere in the cloud. Turns out this can be quite useful for writing automated tests and other things.

The principles it tries to adhere to:

* The code you write is the code you test
* It should be technology agnostic

Hopefuly, this means you are free to use your preferred test practices, frameworks and tools and not have to do anything to the runtime code.

This has been inspired at least in part by the [Selenium](http://www.seleniumhq.org/) and [Appium](http://appium.io/) projects, hence 'Unium' in the interests of propagating some kind of awful naming convention.


## Getting Started

### Tutorial

For a live tutorial that will guide you through unium and it's features

* Clone the [unium repository](https://github.com/gwaredd/unium/)
* Open the Tutorial scene in the editor
* Hit play

Your default browser should open automtically. The tutorial is best experienced with the editor and browser side-by-side. 

Documentation can be found in the corresponding [unium.pdf](https://raw.githubusercontent.com/gwaredd/unium/master/unium.pdf) files.

### Your Game

To add unium to your game

* Import the unium asset package
* Add the UniumComponent to a game object

That's it, by default the server will start automatically on port 8342 when the component is enabled.

Note that Unium is compiled out for release builds (non-development, non-editor). See the documentation to change this.

## Technical Details

The webserver is ...

* A singleton
* Multi-Threaded
* Supports REST and WebSockets
* Flexible routing
* Automatically added to the "Don't Destroy" group

Implements a version of GQL - a query language for interacting with the game

* An [XPath](https://en.wikipedia.org/wiki/XPath)-like query language
* Query values from the scene graph
* Change variables and invoke functions
* Watching of variables and events through the WebSockets interface
* Automatic JSON serialisation

