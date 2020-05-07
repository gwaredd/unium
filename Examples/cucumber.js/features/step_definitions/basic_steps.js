// example steps for the basic example

const { Given, When, Then } = require( 'cucumber' );

function isItFriday( today ) {
  return today === "Friday" ? "TGIF" : "Nope";
}

Given( 'today is {string}', function(today) {
  this.today = today;
});

When( 'I ask whether it\'s Friday yet', function() {
  this.actualAnswer = isItFriday( this.today );
});

Then( 'I should be told {string}', function(expectedAnswer) {
  this.actualAnswer.should.equal( expectedAnswer );
});

