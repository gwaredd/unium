# unium
> A Unity library for automated testing and debugging

At it's core, Unium is a simple web server.

It provides a framework on which you can build an external API, useful for external tools and automated tests that can interact in a technology agnostic way.

Additionally, it comes with a query languague (GQL) that affords the ability to fetch and change values in game objects and invoke functions remotely.

## WebServer

### Quick Facts

* Singleton
* Multi-Threaded
* Supports both REST and WebSockets
* Flexible "routing"

Designed for flexibility and convenience (not necessarily performance)

"Routing" provides the mechanism for dispatching web requests to associated handlers based on the incoming URL path.

<!--* Default handlers
  * debug log
  * screen shot
  * accessing files in persistent and streaming
  * loading levels
  * basic stats / info
  * GQL
-->

## GQL

### Quick Facts

* [XPath](https://en.wikipedia.org/wiki/XPath)-like query language
* Retrieve values from the scene graph
* Set variables and invoke functions
* Watching of variables and events
* Automatic JSON serialisation

