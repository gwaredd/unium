
# unium
> An open source test automation framework for Unity games and the occasional debug tool

Unium is an experimental library for the purposes of facilitating automated testing and tool development for your Unity games.

The main idea is twofold. Firstly, embed a web server into your project to provide an interface onto the game. Secondly, implement a query language that to some degree takes care of the tedious bits.

The advantages of a web server is that HTTP provides a technology agnostic protocol that places no restrictions on whatever tools and frameworks you wish to use. It also means it will work whether the game is running in editor, on device or on some headless server in the clouds.

Hopefully the query language and automatic reflection reduces the amount of manual serialization code that often makes these kind of systems a pain to work with.


## Getting Started

### Tutorial

For a live tutorial that will guide you through unium and it's features

* Clone the [unium repository](https://github.com/gwaredd/unium/)
* Open the Tutorial scene in the editor
* Hit play

Your default browser should open automatically. The tutorial is best experienced with the editor and browser side-by-side. 

Documentation can be found in the corresponding [unium.pdf](https://github.com/gwaredd/unium/blob/master/unium.pdf) file. 


### Install For Your Project

To install unium into your project.

* Copy the Unium library ([Assets/Unium directory](https://github.com/gwaredd/unium_lib)) into your project
* Add the UniumComponent to an empty game object in your scene
* Enjoy

NB: Remember to enable the `development build` flag if you are making builds (unium is disabled for release by default).


### Compatibility

* Compatible with Unity v5.4.2 and up

### Tutorial Video

[![Unium Tutorial Video](http://img.youtube.com/vi/7mTaPr2oaG4/0.jpg)](http://www.youtube.com/watch?v=7mTaPr2oaG4 "Unium Tutorial Video")

### Questionnaire

If you have been using unium or even if you decided it wasn't for you, I would appreciate some feedback to help improve the library. Here is a link to [very short questionnaire](https://docs.google.com/forms/d/e/1FAIpQLSdZUC9cnz0zXnjjAqPAXpUrOePSsZRZk6hJb32ShBU7gL7HKA/viewform). All feedback very welcome.

Many thanks.

