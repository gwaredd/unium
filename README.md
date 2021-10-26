# Unium
> A library for game automation.

Unium is an library for [unity](https://unity.com/) that facilities automation. It exposes an HTTP API that can be used for tools or testing.

There are two parts:

* A web server that is embedded into the game
* A query language sits on top that lets you search and manipulate the scene graph

This provides a flexible interface so you can remotely control and inspect your game as it runs. If necessary, you can add your own endpoints to the underlying web server to do custom operations.

## Getting Started

### The Tutorial

For the tutorial

* Clone the [unium repository](https://github.com/gwaredd/unium/)
* Open the project in Unity
* Open the Tutorial scene in the editor
* Hit play

Your default browser should open automatically.

The tutorial is best experienced with the editor and browser side-by-side. 

Documentation can be found in [unium.pdf](https://github.com/gwaredd/unium/blob/master/Assets/Unium/unium.pdf). 

See also the [FAQ](https://github.com/gwaredd/unium/wiki/FAQ)

### For Your Project

There are several ways of installing the unium library in your project.

The preferred approach

* Use the unity package manager (see below)

Alternatively you can

* Copy the `Assets/Unium` folder directly into your project
* Download it from the [asset store](https://assetstore.unity.com/packages/tools/integration/unium-automated-test-tools-95998)
* Or, install one of the [release packages](https://github.com/gwaredd/unium/releases)

**NB:** Remember to enable the `development build` flag if you are making builds (unium is disabled for release by default).

### Package Manager

As of `v1.0.4` you can use the unity packaging to add unium directly.

You can now [import unium as a git package using UPM](https://docs.unity3d.com/Manual/upm-git.html). With Unity 2020.1 or later, you can point to the github repository in your `Packages/manifest.json` dependencies. You can optionally pin to a specific version of unium by adding a revision or tag to this url as per the above documentation.

```
{
  "dependencies": {
    "com.gwaredd.unium": "https://github.com/gwaredd/unium.git?path=/Assets/Unium",
    ...
  }
}
```

For earlier versions of Unity that do not support sub folders in git dependencies, the `upm` branch of the repository contains just the library setup for use with the [package manager](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@1.8/manual/index.html). Note that this version might not contain all latest fixes and improvements.

## Notes

### Writing Tests

The Unium API facilitates automated tests but is not a testing framework. Below are a number of popular frameworks that have been used with Unium.

* [Mocha](https://mochajs.org/) and [Chai](https://www.chaijs.com/) - JavaScript
* [Jest](https://jestjs.io/) - JavaScript
* [PyTest](https://pytest.org) - Python
* [RSpec](https://rspec.info/) - Ruby
* [SpecFlow](https://specflow.org/) - C#
* [JUnit](https://junit.org/) - Java

Examples can be found in the [Examples](https://github.com/gwaredd/unium/tree/master/Examples) folder.

### Compatibility

* Compatible with Unity 2017.4.30 and up
* The aim to is to keep it compatible with the lowest LTS version officially supported by Unity



### Tutorial Video

[![Unium Tutorial Video](http://img.youtube.com/vi/7mTaPr2oaG4/0.jpg)](http://www.youtube.com/watch?v=7mTaPr2oaG4 "Unium Tutorial Video")

### Demonstration of Unium and Appium working together

[![Unium with Appium](http://img.youtube.com/vi/UbPk2VljW78/0.jpg)](https://youtu.be/UbPk2VljW78 "Unium with Appium")

