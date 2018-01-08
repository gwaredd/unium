// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using System;

using gw.gql.calc;

////////////////////////////////////////////////////////////////////////////////////////////////////

public static class CalcExtensions
{
    public static Parser With( this Parser p, string str )
    {
        p.Expression = str;
        return p;
    }
}


////////////////////////////////////////////////////////////////////////////////////////////////////

[TestFixture, Category( "Calc" )]
public class TestCalcLexer
{
    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldIdentifyBasicSymbols()
    {
        var parser = new Parser();


        // empty strings

        Assert.Throws<NullReferenceException>( () => parser.With( null ).GetNextToken() );

        Assert.IsNull( parser.With( "" ).GetNextToken() );
        Assert.IsNull( parser.With( "  " ).GetNextToken() );


        // all the tokens

        Token tok;


        // primitives

        tok = parser.With( "null" ).GetNextToken();
        Assert.AreEqual( Token.Null, tok.Type );
        Assert.AreEqual( "null", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "true" ).GetNextToken();
        Assert.AreEqual( Token.Boolean, tok.Type );
        Assert.AreEqual( "true", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "false" ).GetNextToken();
        Assert.AreEqual( Token.Boolean, tok.Type );
        Assert.AreEqual( "false", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "12" ).GetNextToken();
        Assert.AreEqual( Token.Integer, tok.Type );
        Assert.AreEqual( "12", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "3.141" ).GetNextToken();
        Assert.AreEqual( Token.Double, tok.Type );
        Assert.AreEqual( "3.141", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "x" ).GetNextToken();
        Assert.AreEqual( Token.Variable, tok.Type );
        Assert.AreEqual( @"x", tok.Text );
        Assert.IsNull( parser.GetNextToken() );


        // strings

        tok = parser.With( "'hello'" ).GetNextToken();
        Assert.AreEqual( Token.String, tok.Type );
        Assert.AreEqual( "hello", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "'hel\\' lo'" ).GetNextToken();
        Assert.AreEqual( Token.String, tok.Type );
        Assert.AreEqual( "hel' lo", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( @"""hel\"" lo""" ).GetNextToken();
        Assert.AreEqual( Token.String, tok.Type );
        Assert.AreEqual( @"hel"" lo", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "'hel\\\\' lo" ).GetNextToken();
        Assert.AreEqual( Token.String, tok.Type );
        Assert.AreEqual( "hel\\", tok.Text );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );

        // function

        tok = parser.With( "fn()" ).GetNextToken();
        Assert.AreEqual( Token.Function, tok.Type );
        Assert.AreEqual( @"fn", tok.Text );
        Assert.AreEqual( Token.LPAREN, parser.GetNextToken().Type );
        Assert.AreEqual( Token.RPAREN, parser.GetNextToken().Type );
        Assert.IsNull( parser.GetNextToken() );

        Assert.AreEqual( Token.LPAREN, parser.With( "(" ).GetNextToken().Type );
        Assert.AreEqual( Token.RPAREN, parser.With( ")" ).GetNextToken().Type );
        Assert.AreEqual( Token.Comma, parser.With( "," ).GetNextToken().Type );


        // operators

        Assert.AreEqual( Token.Add, parser.With( "+" ).GetNextToken().Type );
        Assert.AreEqual( Token.Multiply, parser.With( "*" ).GetNextToken().Type );
        Assert.AreEqual( Token.Divide, parser.With( "/" ).GetNextToken().Type );
        Assert.AreEqual( Token.Modulus, parser.With( "%" ).GetNextToken().Type );
        Assert.AreEqual( Token.Exp, parser.With( "^" ).GetNextToken().Type );

        parser.With( "a-b" );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Subtract, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );

        parser.With( "-b" );
        Assert.AreEqual( Token.Negate, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.IsNull( parser.GetNextToken() );


        Assert.AreEqual( Token.NE, parser.With( "!=" ).GetNextToken().Type );
        Assert.AreEqual( Token.LT, parser.With( "<" ).GetNextToken().Type );
        Assert.AreEqual( Token.LTE, parser.With( "<=" ).GetNextToken().Type );
        Assert.AreEqual( Token.GT, parser.With( ">" ).GetNextToken().Type );
        Assert.AreEqual( Token.GTE, parser.With( ">=" ).GetNextToken().Type );

        Assert.AreEqual( Token.EQ, parser.With( "=" ).GetNextToken().Type );
        Assert.AreEqual( Token.AND, parser.With( "&" ).GetNextToken().Type );
        Assert.AreEqual( Token.OR, parser.With( "|" ).GetNextToken().Type );
        Assert.AreEqual( Token.NOT, parser.With( "!" ).GetNextToken().Type );

        tok = parser.With( "==" ).GetNextToken();
        Assert.AreEqual( Token.EQ, tok.Type );
        Assert.AreEqual( "==", tok.Text );
        Assert.IsNull( parser.GetNextToken() );

        tok = parser.With( "&&" ).GetNextToken();
        Assert.AreEqual( Token.AND, tok.Type );
        Assert.AreEqual( "&&", tok.Text );
        Assert.IsNull( parser.GetNextToken() );


        // with white space

        Assert.AreEqual( Token.Variable, parser.With( "   x   " ).GetNextToken().Type );
        Assert.IsNull( parser.GetNextToken() );
    }


    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldIdentifySymbolsInExpressions()
    {
        var parser = new Parser();

        parser.With( "   fn(  a  , b  , c )   " );
        Assert.AreEqual( Token.Function, parser.GetNextToken().Type );
        Assert.AreEqual( Token.LPAREN, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Comma, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Comma, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.RPAREN, parser.GetNextToken().Type );
        Assert.IsNull( parser.GetNextToken() );

        parser.With( "   a + 12 * fn( x(), 12.2 / c )   " );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Add, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Integer, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Multiply, parser.GetNextToken().Type );

        Assert.AreEqual( Token.Function, parser.GetNextToken().Type );
        Assert.AreEqual( Token.LPAREN, parser.GetNextToken().Type );

        Assert.AreEqual( Token.Function, parser.GetNextToken().Type );
        Assert.AreEqual( Token.LPAREN, parser.GetNextToken().Type );
        Assert.AreEqual( Token.RPAREN, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Comma, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Double, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Divide, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );

        Assert.AreEqual( Token.RPAREN, parser.GetNextToken().Type );

        Assert.IsNull( parser.GetNextToken() );
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldHandleUnknownSymbols()
    {
        var parser = new Parser();
        Assert.AreEqual( Token.Unknown, parser.With( "£" ).GetNextToken().Type );
        Assert.IsNull( parser.GetNextToken() );
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldHandleUnaryOperators()
    {
        Token tok;
        var parser = new Parser();

        tok = parser.With( "-10" ).GetNextToken();
        Assert.AreEqual( Token.Integer, tok.Type );
        Assert.AreEqual( "-10", tok.Text );

        parser.With( "x-10" );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Subtract, parser.GetNextToken().Type );
        tok = parser.GetNextToken();
        Assert.AreEqual( Token.Integer, tok.Type );
        Assert.AreEqual( "10", tok.Text );

        parser.With( "a * -10" );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Multiply, parser.GetNextToken().Type );
        tok = parser.GetNextToken();
        Assert.AreEqual( Token.Integer, tok.Type );
        Assert.AreEqual( "-10", tok.Text );

        parser.With( "a * -b" );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Multiply, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Negate, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );

        parser.With( "fn(-10,-x,10-5)" );
        Assert.AreEqual( Token.Function, parser.GetNextToken().Type );
        Assert.AreEqual( Token.LPAREN, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Integer, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Comma, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Negate, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Variable, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Comma, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Integer, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Subtract, parser.GetNextToken().Type );
        Assert.AreEqual( Token.Integer, parser.GetNextToken().Type );
    }

    //----------------------------------------------------------------------------------------------------

    [Test]
    public void ShouldIdentifyAttrChainsAsVariables()
    {
        Token tok;
        var parser = new Parser();

        tok = parser.With( "a.b.c" ).GetNextToken();
        Assert.AreEqual( Token.Variable, tok.Type );
        Assert.AreEqual( "a.b.c", tok.Text );

        tok = parser.With( "a1.b2.c3" ).GetNextToken();
        Assert.AreEqual( Token.Variable, tok.Type );
        Assert.AreEqual( "a1.b2.c3", tok.Text );

        tok = parser.With( "a.b.c()" ).GetNextToken();
        Assert.AreEqual( Token.Function, tok.Type );
        Assert.AreEqual( "a.b.c", tok.Text );

        tok = parser.With( "7.b.c" ).GetNextToken();
        Assert.AreEqual( Token.Double, tok.Type );
        tok = parser.GetNextToken();
        Assert.AreEqual( Token.Variable, tok.Type );
        Assert.AreEqual( "b.c", tok.Text );
    }
}

#endif
