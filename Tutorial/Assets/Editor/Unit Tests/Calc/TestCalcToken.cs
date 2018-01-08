// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using NUnit.Framework;
using gw.gql.calc;

////////////////////////////////////////////////////////////////////////////////////////////////////

[TestFixture, Category( "Calc" )]
public class TestCalcToken
{
    [Test]
    public void ShouldIdentifyTokenTypes()
    {
        // type

        Assert.IsTrue( new Token( Token.Variable ).IsOperand() );
        Assert.IsTrue( new Token( Token.Integer ).IsOperand() );
        Assert.IsTrue( new Token( Token.Double ).IsOperand() );
        Assert.IsFalse( new Token( Token.Add ).IsOperand() );
        Assert.IsFalse( new Token( Token.AND ).IsOperand() );
        Assert.IsFalse( new Token( Token.None ).IsOperand() );

        Assert.IsFalse( new Token( Token.Variable ).IsOperator() );
        Assert.IsFalse( new Token( Token.Integer ).IsOperator() );
        Assert.IsFalse( new Token( Token.Double ).IsOperator() );
        Assert.IsTrue( new Token( Token.Add ).IsOperator() );
        Assert.IsTrue( new Token( Token.AND ).IsOperator() );
        Assert.IsFalse( new Token( Token.None ).IsOperator() );

        Assert.IsTrue( new Token( Token.NOT ).IsUnaryOperator() );
        Assert.IsTrue( new Token( Token.Negate ).IsUnaryOperator() );
        Assert.IsFalse( new Token( Token.AND ).IsUnaryOperator() );
        Assert.IsFalse( new Token( Token.EQ ).IsUnaryOperator() );
        Assert.IsFalse( new Token( Token.GT ).IsUnaryOperator() );

        Assert.IsFalse( new Token( Token.NOT ).IsBinaryOperator() );
        Assert.IsFalse( new Token( Token.Negate ).IsBinaryOperator() );
        Assert.IsTrue( new Token( Token.AND ).IsBinaryOperator() );
        Assert.IsTrue( new Token( Token.EQ ).IsBinaryOperator() );
        Assert.IsTrue( new Token( Token.GT ).IsBinaryOperator() );
    }

    [Test]
    public void ShouldHaveCorrectPrecedence()
    {
        // precedence


        Assert.Greater( new Token( Token.AND ).Precedence(), new Token( Token.OR ).Precedence() );
        Assert.Greater( new Token( Token.EQ ).Precedence(), new Token( Token.AND ).Precedence() );
        Assert.Greater( new Token( Token.LT ).Precedence(), new Token( Token.EQ ).Precedence() );
        Assert.Greater( new Token( Token.Add ).Precedence(), new Token( Token.LT ).Precedence() );
        Assert.Greater( new Token( Token.Multiply ).Precedence(), new Token( Token.Add ).Precedence() );
        Assert.Greater( new Token( Token.Exp ).Precedence(), new Token( Token.Multiply ).Precedence() );
        Assert.Greater( new Token( Token.Negate ).Precedence(), new Token( Token.Exp ).Precedence() );

        Assert.AreEqual( new Token( Token.EQ ).Precedence(), new Token( Token.NE ).Precedence() );

        Assert.AreEqual( new Token( Token.LT ).Precedence(), new Token( Token.GT ).Precedence() );
        Assert.AreEqual( new Token( Token.LTE ).Precedence(), new Token( Token.GT ).Precedence() );
        Assert.AreEqual( new Token( Token.GTE ).Precedence(), new Token( Token.GT ).Precedence() );

        Assert.AreEqual( new Token( Token.Add ).Precedence(), new Token( Token.Subtract ).Precedence() );

        Assert.AreEqual( new Token( Token.Multiply ).Precedence(), new Token( Token.Divide ).Precedence() );
        Assert.AreEqual( new Token( Token.Multiply ).Precedence(), new Token( Token.Modulus ).Precedence() );

        Assert.AreEqual( new Token( Token.Negate ).Precedence(), new Token( Token.NOT ).Precedence() );
    }
}

#endif
