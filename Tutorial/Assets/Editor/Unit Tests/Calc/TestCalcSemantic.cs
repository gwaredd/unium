// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using System;

using gw.gql.calc;

////////////////////////////////////////////////////////////////////////////////////////////////////

[TestFixture, Category( "Calc" )]
public class TestCalcSemantic
{
    //----------------------------------------------------------------------------------------------------

    string Parse( string input )
    {
        var output = "";
        new Parser().Parse( input, op => output += op.Text + " " );
        return output;
    }


    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldOutputPostfixSymbols()
    {
        // basics

        Assert.AreEqual( "1 2 + ",                     Parse( "1 + 2" ) );             // basic
        Assert.AreEqual( "1 2 3 * + ",                 Parse( "1 + 2 * 3" ) );         // precedence
        Assert.AreEqual( "1 2 + 3 * ",                 Parse( "( 1 + 2 ) * 3" ) );     // precedence
        Assert.AreEqual( "1 2 + 3 == ",                Parse( "1 + 2 == 3" ) );        // precedence
        Assert.AreEqual( "3 x - + ",                   Parse( "3 + -x" ) );            // unary
        Assert.AreEqual( "a b ! c = & ",               Parse( "a & !b = c" ) );        // unary


        // examples

        Assert.AreEqual( "3 4 2 1 - * + ",             Parse( "3 + 4 * ( 2 - 1 )" ) );
        Assert.AreEqual( "3 4 2 * 1 5 - 2 3 ^ ^ / + ", Parse( "3 + 4 * 2 / ( 1 - 5 ) ^ 2 ^ 3" ) );
        Assert.AreEqual( "2 3 max 3 / 3.1415 * sin ",  Parse( "sin( max( 2, 3 ) / 3 * 3.1415 )" ) );
        Assert.AreEqual( "2 3 | 4 5 * 7 8 ! / max ",   Parse( "max( 2 | 3, 4 * 5, 7 / !8 )" ) );
        Assert.AreEqual( "false false true and ! or ", Parse( "false or ! (false and true)" ) );
        Assert.AreEqual( "3 2 % 10 3 % != ",           Parse( "3 % 2 != 10 % 3" ) );

        // Assert.AreEqual( "3 2 > 1 3 2 - <= && ",        Parse( "3 > 2 && 1 <= (3-2)" ) );
        //"3 2 1 && > 3 2 - <= "
    }


    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldHandleMalformedInput()
    {
        // bad input

        Assert.Throws<NullReferenceException>( () => Parse( null ) );       // no string
        Assert.Throws<FormatException>( () => Parse( "£" ) );               // unknown symbol
        Assert.Throws<FormatException>( () => Parse( "a + b * (" ) );       // mismatched ()
        Assert.Throws<FormatException>( () => Parse( "((a + b) * 2" ) );    // mismatched ()
        Assert.Throws<FormatException>( () => Parse( "fn(a, max(b)" ) );    // mismatched ()
        Assert.Throws<FormatException>( () => Parse( "fn(a,b))" ) );        // mismatched ()
    }
}

#endif
