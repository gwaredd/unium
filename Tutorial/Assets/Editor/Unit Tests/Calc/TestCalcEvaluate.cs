// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using System;
using System.Collections.Generic;

using gw.gql.calc;


////////////////////////////////////////////////////////////////////////////////////////////////////

[TestFixture, Category( "Calc" )]
public class TestCalcEvaluate
{
    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldParseValues()
    {
        Assert.AreEqual( 123456,   new Expression( "123456" ).Evaluate() );
        Assert.AreEqual( 123.456d, new Expression( "123.456" ).Evaluate() );
        Assert.AreEqual( true,     new Expression( "true" ).Evaluate() );
        Assert.AreEqual( "true",   new Expression( "'true'" ).Evaluate() );
        Assert.AreEqual( "azerty", new Expression( "'azerty'" ).Evaluate() );
        Assert.IsNull(             new Expression( "null" ).Evaluate() );
    }


    [Test]
    public void ShouldParseScientificNotation()
    {
        Assert.AreEqual( 12.2d,        new Expression( "1.22e1" ).Evaluate() );
        Assert.AreEqual( 100d,         new Expression( "1e2" ).Evaluate() );
        Assert.AreEqual( 100d,         new Expression( "1e+2" ).Evaluate() );
        Assert.AreEqual( 0.01d,        new Expression( "1e-2" ).Evaluate() );
        Assert.AreEqual( 0.001d,       new Expression( ".1e-2" ).Evaluate() );
        Assert.AreEqual( 10000000000d, new Expression( "1e10" ).Evaluate() );
    }


    [Test]
    public void ShouldEvaluateOperators()
    {
        var expressions = new Dictionary<string, object> {
            { "!true", false },
            { "not false", true },
            { "2 * 3", 6 },
            { "6 / 2", 3d },
            { "7 % 2", 1 },
            { "2 + 3", 5 },
            { "2 - 1", 1 },
            { "1 < 2", true },
            { "1 > 2", false },
            { "1 <= 2", true },
            { "1 <= 1", true },
            { "1 >= 2", false },
            { "1 >= 1", true },
            { "1 = 1", true },
            { "1 == 1", true },
            { "1 != 1", false },
            { "true && false", false },
            { "true and false", false },
            { "true || false", true },
            { "true or false", true },
        };

        foreach( KeyValuePair<string, object> pair in expressions )
        {
            Assert.AreEqual( pair.Value, new Expression( pair.Key ).Evaluate(), pair.Key + " failed" );
        }
    }

    [Test]
    public void ExpressionShouldEvaluate()
    {
        Assert.AreEqual( 10,    new Expression( "2 + 3 + 5" ).Evaluate() );
        Assert.AreEqual( 16,    new Expression( "2 * (3 + 5)" ).Evaluate() );
        Assert.AreEqual( 24,    new Expression( "2 * (2*(2*(2+1)))" ).Evaluate() );
        Assert.AreEqual( 1,     new Expression( "10 % 3" ).Evaluate() );
        Assert.AreEqual( true,  new Expression( "true or false" ).Evaluate() );
        Assert.AreEqual( false, new Expression( "!true" ).Evaluate() );
        Assert.AreEqual( true,  new Expression( "false || not (false and true)" ).Evaluate() );
        Assert.AreEqual( true,  new Expression( "3 > 2 and 1 <= (3-2)" ).Evaluate() );
        Assert.AreEqual( false, new Expression( "3 % 2 != 10 % 3" ).Evaluate() );

        Assert.AreEqual( true,  new Expression( "1=2 || 3==3" ).Evaluate() );
        Assert.AreEqual( false, new Expression( "1==2 && 3==3" ).Evaluate() );
        Assert.AreEqual( true,  new Expression( "!(1=2) && 3==3" ).Evaluate() );

        Assert.AreEqual( 25, new Expression( "5 ^ 2" ).Evaluate() );
        //Assert.Throws<Exception>( () => new Expression( "true ^ false" ).Evaluate() );
    }

    [Test]
    public void ShouldEscapeCharacters()
    {
        Assert.AreEqual( "'hello'",      new Expression( @"'\'hello\''" ).Evaluate() );
        Assert.AreEqual( " ' hel lo ' ", new Expression( @"' \' hel lo \' '" ).Evaluate() );
    }


    [Test]
    public void ShouldHandleOperatorsPriority()
    {
        Assert.AreEqual( 8,    new Expression( "2+2+2+2" ).Evaluate() );
        Assert.AreEqual( 16,   new Expression( "2*2*2*2" ).Evaluate() );
        Assert.AreEqual( 6,    new Expression( "2*2+2" ).Evaluate() );
        Assert.AreEqual( 6,    new Expression( "2+2*2" ).Evaluate() );

        Assert.AreEqual( 9d,   new Expression( "1 + 2 + 3 * 4 / 2" ).Evaluate() );
        Assert.AreEqual( 13.5, new Expression( "18/2/2*3" ).Evaluate() );
    }

    [Test]
    public void ShouldNotLoosePrecision()
    {
        Assert.AreEqual( 0.5, new Expression( "3/6" ).Evaluate() );
    }

    [Test]
    public void ShouldHandleStringConcatenation()
    {
        Assert.AreEqual( "toto", new Expression( "'to' + 'to'" ).Evaluate() );
        Assert.AreEqual( "one2", new Expression( "'one' + 2" ).Evaluate() );
    }

    [Test]
    public void ExpressionShouldEvaluateParameters()
    {
        var expr =    new Expression( "x * x" );
        var context = new EvaluationContext();

        context.EvaluateParameter = ( name, ctx ) => { Assert.AreEqual( "x", name ); return 3; };
        Assert.AreEqual( 9, expr.Evaluate( context ) );

        context.EvaluateParameter = ( name, ctx ) => 4;
        Assert.AreEqual( 16, expr.Evaluate( context ) ); // multiple invocations, same expression ...

        // negate
        Assert.AreEqual( -16, new Expression( "-x * x" ).Evaluate( context ) );
        Assert.AreEqual( -16, new Expression( "x * -x" ).Evaluate( context ) );
        Assert.AreEqual( 16,  new Expression( "-x * -x" ).Evaluate( context ) );
    }

    [Test]
    public void ExpressionShouldEvaluateCustomFunctions()
    {
        var context = new EvaluationContext();

        context.InvokeFunction = ( name, args, ctx ) =>
        {
            Assert.AreEqual( "fn", name );

            if( args.Length == 2 )
            {
                return Convert.ToInt32( args[ 0 ].Evaluate( ctx ) ) + Convert.ToInt32( args[ 1 ].Evaluate( ctx ) );
            }

            return 3;
        };

        Assert.AreEqual( 9,  new Expression( "fn() * fn()" ).Evaluate( context ) );
        Assert.AreEqual( 3,  new Expression( "fn(1,2)" ).Evaluate( context ) );
        Assert.AreEqual( 32, new Expression( "fn(1+2*3,5*5)" ).Evaluate( context ) );
    }


    [Test]
    public void ShouldShortCircuitBooleanExpressions()
    {
        var context = new EvaluationContext();
        context.EvaluateParameter = ( name, ctx ) => 0;

        Assert.AreEqual( false, new Expression( "(a != 0) && (b/a>2)" ).Evaluate( context ) );
    }

    [Test]
    public void ShouldHandleMalformedExpressions()
    {
        Assert.Throws<FormatException>( () => new Expression( "(3 + 2" ) );
        Assert.Throws<FormatException>( () => new Expression( "a + b * (" ) );
        Assert.Throws<InvalidOperationException>( () => new Expression( " + b" ) );
        Assert.Throws<InvalidOperationException>( () => new Expression( "b + " ) );
        Assert.Throws<FormatException>( () => new Expression( "  " ) );
        Assert.Throws<FormatException>( () => new Expression( "(a,b)" ) );
        Assert.Throws<InvalidOperationException>( () => new Expression( "+" ) );
    }

    [Test]
    public void ShouldHandleBadMaths()
    {
        Assert.Throws<InvalidOperationException>( () => new Expression( "true + 5" ).Evaluate() );
        Assert.Throws<InvalidOperationException>( () => new Expression( "5 + true" ).Evaluate() );
        Assert.Throws<InvalidOperationException>( () => new Expression( "true - 5" ).Evaluate() );
        Assert.Throws<InvalidOperationException>( () => new Expression( "5 - true" ).Evaluate() );
        Assert.Throws<InvalidOperationException>( () => new Expression( "5 * true" ).Evaluate() );
        Assert.Throws<InvalidOperationException>( () => new Expression( "- true" ).Evaluate() );
    }
}

#endif
