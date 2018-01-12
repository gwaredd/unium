// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
using System.Collections.Generic;


#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;

namespace gw.gql.calc
{
    //----------------------------------------------------------------------------------------------------

    public abstract class LogicalExpression
    {
        public abstract object  Evaluate( EvaluationContext context );
        public abstract bool    IsBoolean();
    }


    //----------------------------------------------------------------------------------------------------

    public class OperandExpression : LogicalExpression
    {
        int     mType;
        object  mValue;
        
        public OperandExpression( Token token )
        {
            mType = token.Type;

            switch( mType )
            {
                case Token.Null:        break;
                case Token.Boolean:     mValue = Boolean.Parse( token.Text ); break;
                case Token.Integer:     mValue = Int32.Parse( token.Text ); break;
                case Token.Double:      mValue = Double.Parse( token.Text ); break;
                case Token.String:      mValue = token.Text; break;
                case Token.Variable:    mValue = token.Text; break;
                    
                default:
                    throw new FormatException( "Bad token type" );
            }
        }

        public override object Evaluate( EvaluationContext context )
        {
            if( mType == Token.Variable )
            {
                return context != null ? context.EvaluateParameter( mValue as string, context ) : null;
            }
            
            return mValue;
        }

        public override bool IsBoolean()
        {
            return mType == Token.Null || mType == Token.Boolean || mType == Token.Variable;
        }
    }


    //----------------------------------------------------------------------------------------------------

    public class UnaryExpression : LogicalExpression
    {
        int mType;
        LogicalExpression mOperand;

        public UnaryExpression( Token token, LogicalExpression operand )
        {
            if( !token.IsUnaryOperator() )
            {
                throw new FormatException( "Bad token type" );
            }

            mType    = token.Type;
            mOperand = operand;
        }

        public override object Evaluate( EvaluationContext context )
        {
            return mType == Token.Negate ?  Numbers.Negate( mOperand.Evaluate( context ) ) : !Convert.ToBoolean( mOperand.Evaluate( context ) );
        }

        public override bool IsBoolean()
        {
            return mType == Token.NOT;
        }
    }


    //----------------------------------------------------------------------------------------------------

    public class BinaryExpression : LogicalExpression
    {
        int mType;
        LogicalExpression mLHS;
        LogicalExpression mRHS;
        
        public BinaryExpression( Token token, LogicalExpression lhs, LogicalExpression rhs )
        {
            if( !token.IsBinaryOperator() )
            {
                throw new FormatException( "Bad token type" );
            }

            mType = token.Type;
            mLHS    = lhs;
            mRHS    = rhs;
        }
            
        public override object Evaluate( EvaluationContext context )
        {
            switch( mType )
            {
                case Token.EQ:          return Numbers.Compare( mLHS.Evaluate( context ), mRHS.Evaluate( context ) ) == 0;
                case Token.NE:          return Numbers.Compare( mLHS.Evaluate( context ), mRHS.Evaluate( context ) ) != 0;
                case Token.LT:          return Numbers.Compare( mLHS.Evaluate( context ), mRHS.Evaluate( context ) ) < 0;
                case Token.LTE:         return Numbers.Compare( mLHS.Evaluate( context ), mRHS.Evaluate( context ) ) <= 0;
                case Token.GT:          return Numbers.Compare( mLHS.Evaluate( context ), mRHS.Evaluate( context ) ) > 0;
                case Token.GTE:         return Numbers.Compare( mLHS.Evaluate( context ), mRHS.Evaluate( context ) ) >= 0;

                case Token.Subtract:    return Numbers.Substract( mLHS.Evaluate( context ), mRHS.Evaluate( context ) );
                case Token.Multiply:    return Numbers.Multiply( mLHS.Evaluate( context ), mRHS.Evaluate( context ) );
                case Token.Divide:      return Numbers.Divide( Convert.ToDouble( mLHS.Evaluate( context ) ), mRHS.Evaluate( context ) );
                case Token.Modulus:     return Numbers.Modulo( mLHS.Evaluate( context ), mRHS.Evaluate( context ) );
                case Token.Exp:         return Numbers.Power( mLHS.Evaluate( context ), mRHS.Evaluate( context ) );
                    
                case Token.OR:          return Convert.ToBoolean( mLHS.Evaluate( context ) ) || Convert.ToBoolean( mRHS.Evaluate( context ) );
                case Token.AND:         return Convert.ToBoolean( mLHS.Evaluate( context ) ) && Convert.ToBoolean( mRHS.Evaluate( context ) );
                    
                case Token.Add:
                {
                    var lhs = mLHS.Evaluate( context );
                    var rhs = mRHS.Evaluate( context );
                    return lhs is string ? String.Concat( lhs, rhs ) : Numbers.Add( lhs, rhs );
                }
            }

            return null;
        }

        public override bool IsBoolean()
        {
            return Token.IsBooleanOperator( mType );
        }
    }


    //----------------------------------------------------------------------------------------------------

    public class FunctionExpression : LogicalExpression
    {
        string mName;
        LogicalExpression[] mArgs;
        
        public FunctionExpression( Token token, LogicalExpression[] arguments )
        {
            if( token.Type != Token.Function )
            {
                throw new FormatException( "Bad token type" );
            }

            mName = token.Text;
            mArgs = arguments;
        }

        public override object Evaluate( EvaluationContext context )
        {
            return context != null ? context.InvokeFunction( mName, mArgs, context ) : null;
        }

        public override bool IsBoolean()
        {
            return false; // TODO: FunctionExpression.IsNumber()
        }
    }

    //----------------------------------------------------------------------------------------------------

    public class EvaluationContext
    {
        public delegate object ParameterDelegate( string name, EvaluationContext context );
        public delegate object FunctionDelegate( string name, LogicalExpression[] arguments, EvaluationContext context );
        
        public ParameterDelegate    EvaluateParameter   = ( name, context ) => null;
        public FunctionDelegate     InvokeFunction      = ( name, args, context ) => null;
    }


    //----------------------------------------------------------------------------------------------------

    public class Expression
    {
        LogicalExpression mAST;

        public Expression( string expr )
        {
            mAST = new Parser().BuildTree( expr );
        }

        public object Evaluate( EvaluationContext context = null )
        {
            return mAST.Evaluate( context );
        }

        public bool IsBoolean { get { return mAST.IsBoolean(); } }
    }
}

#endif
