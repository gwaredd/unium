// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections.Generic;

namespace gw.gql.calc
{
    //
    // Parse()
    //
    // implements the shunting yard algorithms and returns symbols in postfix order
    //

    public partial class Parser
    {
        Stack<Token> mStack;

        public delegate void TokenConsumer( Token op );

        public void Parse( string expr, TokenConsumer output )
        {
            if( expr == null || output == null )
            {
                throw new NullReferenceException();
            }

            int numOutput = 0;

            Expression  = expr;
            mStack      = new Stack<Token>( 16 );


            // read tokens

            var token = GetNextToken();

            while( token != null )
            {
                switch( token.Type )
                {
                    // operand

                    case Token.Null:
                    case Token.Boolean:
                    case Token.Integer:
                    case Token.Double:
                    case Token.String:
                    case Token.Variable:
                    {
                        numOutput++;
                        output( token );
                    }
                    break;


                    // operators

                    case Token.Add:
                    case Token.Subtract:
                    case Token.Multiply:
                    case Token.Divide:
                    case Token.Modulus:
                    case Token.EQ:
                    case Token.NE:
                    case Token.LT:
                    case Token.LTE:
                    case Token.GT:
                    case Token.GTE:
                    case Token.AND:
                    case Token.OR:
                    case Token.Exp:
                    case Token.NOT:
                    case Token.Negate:
                    {
                        var p1 = token.Precedence();
                        var left_associative = !token.IsUnaryOperator() && token.Type != Token.Exp;

                        // while there is an operator (o2) token at the top of the operator stack ...

                        while( mStack.Count > 0 )
                        {
                            // o1 is right associative and has precedence less than that of o2, or
                            // o1 is  left associative and has precedence less than or equal to that of o2

                            var p2 = mStack.Peek().Precedence();

                            if( p1 < p2 || ( left_associative && p1 == p2 ) )
                            {
                                // pop o2 off the operator stack, onto the output queue
                                numOutput++;
                                output( mStack.Pop() );
                            }
                            else
                            {
                                break;
                            }
                        }

                        // push on to stack
                        mStack.Push( token );
                    }
                    break;


                    // functions

                    case Token.LPAREN:
                    {
                        // remember number of output tokens (if this changes before a right parenthesis we must have at least one argument)
                        token.OutputSize = numOutput;

                        // push on to stack
                        mStack.Push( token );
                    }
                    break;

                    case Token.Function:
                    {
                        // push on to stack
                        mStack.Push( token );
                    }
                    break;

                    case Token.Comma:
                    {
                        // until the token at the top of the stack is a left parenthesis

                        while( mStack.Count > 0 && mStack.Peek().Type != Token.LPAREN )
                        {
                            // pop operators off the stack onto the output queue
                            numOutput++;
                            output( mStack.Pop() );
                        }

                        // if no left parentheses are encountered, either the separator was misplaced or parentheses were mismatched

                        if( mStack.Peek().Type == Token.LPAREN )
                        {
                            // increment number of arguments this function call has
                            mStack.Peek().NumCommas++;
                        }
                        else
                        {
                            throw new FormatException( "Mismatched parentheses or misplaced comma" );
                        }
                    }
                    break;


                    case Token.RPAREN:
                    {
                        // until the token at the top of the stack is a left parenthesis

                        while( mStack.Count > 0 && mStack.Peek().Type != Token.LPAREN )
                        {
                            // pop operators off the stack onto the output queue
                            numOutput++;
                            output( mStack.Pop() );
                        }

                        if( mStack.Count > 0 && mStack.Peek().Type == Token.LPAREN )
                        {
                            // pop the left parenthesis from the stack, but not onto the output queue

                            var lparen = mStack.Pop();


                            // if the token at the top of the stack is a function token, pop it onto the output queue

                            if( mStack.Count > 0 && mStack.Peek().Type == Token.Function )
                            {
                                var func = mStack.Pop();

                                // update number of arguments to function
                                func.Args = numOutput > lparen.OutputSize ? 1 : 0;
                                func.Args += lparen.NumCommas;

                                numOutput++;
                                output( func );
                            }
                        }
                        else
                        {
                            // if the stack runs out without finding a left parenthesis, then there are mismatched parentheses.
                            throw new FormatException( "Mismatched parentheses" );
                        }
                    }
                    break;

                    // unknown

                    default:
                    case Token.Unknown:
                    {
                        throw new FormatException( "Unknown symbol in input stream" );
                    }
                }

                token = GetNextToken();
            }

            // when there are no more tokens to read

            // while there are still operator tokens in the stack

            while( mStack.Count > 0 )
            {
                // if the operator token on the top of the stack is a parenthesis, then there are mismatched parentheses

                if( mStack.Peek().Type == Token.LPAREN )
                {
                    throw new FormatException( "Mismatched parentheses" );
                }

                output( mStack.Pop() );
            }
        }
    }
}

#endif
