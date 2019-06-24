
# unium
> An open source remote API for Unity games

Unium is an experimental library that provides an external API to your game during development. This is intended for for tools or automated testing.

There are two parts:

* A web server that is embedded into the game
* A query language that lets you search the scene graph

The web server provides both RESTful HTTP and WebSockets, allowing you to develop tools and tests in your preferred framework.


## Getting Started

### Tutorial

For a live tutorial that will guide you through unium and it's features

* Clone the [unium repository](https://github.com/gwaredd/unium/)
* Open the Tutorial scene in the editor
* Hit play

Your default browser should open automatically. The tutorial is best experienced with the editor and browser side-by-side. 

Documentation can be found in the corresponding [unium.pdf](https://github.com/gwaredd/unium/blob/master/unium.pdf) file. 


### Installing Unium into your Project

To install unium into your project.

* Copy the Unium library `Assets/Unium directory` into your project
* Add the UniumComponent to an empty game object in your scene
* Enjoy

**NB:** Remember to enable the `development build` flag if you are making builds (unium is disabled for release by default).

### Writing Tests

The Unium API facilitates automated tests but is not a testing framework. Below are a number of popular frameworks that have been used with Unium.

* [Mocha](https://mochajs.org/) and [Chai](https://www.chaijs.com/) - JavaScript
* [Jest](https://jestjs.io/) - JavaScript
* [PyTest](https://pytest.org) - Python
* [RSpec](https://rspec.info/) - Ruby
* [SpecFlow](https://specflow.org/) - C#
* [JUnit](https://junit.org/) - Java

### Compatibility

* Compatible with Unity v5.4.2 and up

### Tutorial Video

[![Unium Tutorial Video](http://img.youtube.com/vi/7mTaPr2oaG4/0.jpg)](http://www.youtube.com/watch?v=7mTaPr2oaG4 "Unium Tutorial Video")

### Unium working with Appium
[![Unium with Appium](http://img.youtube.com/vi/UbPk2VljW78/0.jpg)](https://youtu.be/UbPk2VljW78 "Unium with Appium")


### Questionnaire

If you have been using unium or even if you decided it wasn't for you, I would appreciate some feedback to help improve the library. Here is a link to [very short questionnaire](https://docs.google.com/forms/d/e/1FAIpQLSdZUC9cnz0zXnjjAqPAXpUrOePSsZRZk6hJb32ShBU7gL7HKA/viewform). All feedback very welcome.

Many thanks.

