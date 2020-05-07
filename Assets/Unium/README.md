
# Unium
> A library for game automation.

Unium is an automation library for [unity](https://unity.com/) projects.

It provides a remote API intended for tools and automated testing.

There are two parts:

* A web server that is embedded into the game
* A query language that lets you search and manipulate the scene graph

This provides a flexible interface so you can remotely control and inspect your game as it runs.

## Getting Started

### Tutorial

For the tutorial

* Clone the [unium repository](https://github.com/gwaredd/unium/)
* Open the Tutorial scene in the editor
* Hit play

Your default browser should open automatically.

The tutorial is best experienced with the editor and browser side-by-side. 

Documentation can be found in [unium.pdf](https://github.com/gwaredd/unium/blob/master/unium.pdf). 


### Installing Unium

To add unium to your project.

* Add the Unium library to your project (either import a [release](https://github.com/gwaredd/unium/releases) package or copy the `Assets/Unium` directory)
* Add the `UniumComponent` to an empty game object in your scene
* Enjoy

**NB:** Remember to enable the `development build` flag if you are making builds (unium is disabled for release by default).


## Notes

### Writing Tests

The Unium API facilitates automated tests but is not a testing framework. Below are a number of popular frameworks that have been used with Unium.

* [Mocha](https://mochajs.org/) and [Chai](https://www.chaijs.com/) - JavaScript
* [Jest](https://jestjs.io/) - JavaScript
* [PyTest](https://pytest.org) - Python
* [RSpec](https://rspec.info/) - Ruby
* [SpecFlow](https://specflow.org/) - C#
* [JUnit](https://junit.org/) - Java

Examples can be found in the [Extras](https://github.com/gwaredd/unium/tree/master/Extras) folder.

### Compatibility

* Compatible with Unity 2017.4.30 and up
* The aim to is to keep it compatible with the lowest LTS version officially supported by Unity

## Philosophy

#### It should just work

You should not have to modify you game in any way in order to automate it.

#### It should work for game developers

It should fit with game developer ways of working.

The intended audience is game developers first and technical testers second. It should not compromise their experience with other considerations.

#### It should not lock you into a specific language or framework

You should be able to write your automation and test scripts in any language or framework you choose. Unium should come with no dependencies or stipulations.

#### It should be open source

Both in spirit and practice.


### Tutorial Video

[![Unium Tutorial Video](http://img.youtube.com/vi/7mTaPr2oaG4/0.jpg)](http://www.youtube.com/watch?v=7mTaPr2oaG4 "Unium Tutorial Video")

### Demonstration of Unium and Appium working together

[![Unium with Appium](http://img.youtube.com/vi/UbPk2VljW78/0.jpg)](https://youtu.be/UbPk2VljW78 "Unium with Appium")

