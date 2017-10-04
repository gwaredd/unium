// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;

namespace gw.gql.calc
{
    // BuildTree()

    public partial class Parser
    {
        Stack<Token> mInput;

        //----------------------------------------------------------------------------------------------------

        public LogicalExpression BuildTree( string expr )
        {
            mInput = new Stack<Token>( 16 );
            new Parser().Parse( expr, tok => mInput.Push( tok ) );

            if( mInput.Count == 0 )
            {
                throw new FormatException( "Invalid expression,  no symbols found" );
            }

            // convert stack into AST

            var AST = CreateExpression( mInput.Pop() );

            // check all tokens where consumed

            if( mInput.Count != 0 || AST == null )
            {
                throw new FormatException( "Invalid expression" );
            }

            return AST;
        }

        //----------------------------------------------------------------------------------------------------

        LogicalExpression CreateExpression( Token token )
        {
            if( token.IsOperand() )
            {
                return new OperandExpression( token );
            }
            else if( token.IsBinaryOperator() )
            {
                var rhs = CreateExpression( mInput.Pop() );
                var lhs = CreateExpression( mInput.Pop() );

                return new BinaryExpression( token, lhs, rhs );
            }
            else if( token.IsUnaryOperator() )
            {
                var operand = CreateExpression( mInput.Pop() );
                return new UnaryExpression( token, operand );
            }
            else if( token.Type == Token.Function )
            {
                var args = new LogicalExpression[ token.Args ];

                for( int i = token.Args - 1; i >= 0; i-- )
                {
                    args[ i ] = CreateExpression( mInput.Pop() );
                }

                return new FunctionExpression( token, args );
            }
            else
            {
                throw new FormatException( "Unexpected token type " + token.Type.ToString() );
            }
        }        
    }
}

#endif
