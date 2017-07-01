# unium
> An open source test automation framework for Unity games and the occasional debug tool

Unium is an experimental library for the purposes of facilitating automated testing and tool development for your Unity games.

The main idea is twofold. Firstly, embed a web server into your project to provide an interface onto the game. Secondly, implement a query language that to some degree takes care of the tedious bits.

The advantages of a web server is that HTTP provides a technology agnostic protocol that places no restrictions on whatever tools and frameworks you wish to use. It also means it will work whether the game is running in editor, on device or on some headless server in the clouds.

Hopefully the query language and automatic reflection reduces the amount of manual serialisation code that often makes these kind of systems a pain to work with.


## Getting Started

### Tutorial

For a live tutorial that will guide you through unium and it's features

* Clone the [unium repository](https://github.com/gwaredd/unium/)
* Open the Tutorial scene in the editor
* Hit play

Your default browser should open automtically. The tutorial is best experienced with the editor and browser side-by-side. 

Documentation can be found in the corresponding [unium.pdf](https://github.com/gwaredd/unium/blob/master/unium.pdf) files.


