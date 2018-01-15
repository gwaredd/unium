// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

namespace gw.gql.calc
{
    // GetNextToken()

    public partial class Parser
    {
        string  mText           = null;
        Token   mPrev           = null;
        int     mOffset         = 0;
        char[]  mValue          = new char[ 256 ];
        int     mValueLength    = 0;


        //----------------------------------------------------------------------------------------------------

        public string Expression
        {
            get
            {
                return mText;
            }

            set
            {
                mText   = value;
                mPrev   = null;
                mOffset = 0;
            }
        }


        //----------------------------------------------------------------------------------------------------

        // read a number

        Token ReadNumber()
        {
            int type = Token.Integer;
            var start = mOffset - 1; // NB: first character already read

            // read integer part

            while( mOffset < mText.Length && char.IsDigit( mText[ mOffset ] ) )
            {
                mOffset++;
            }

            // has fraction?

            if( mOffset < mText.Length && mText[ mOffset ] == '.' )
            {
                type = Token.Double;
                mOffset++;

                while( mOffset < mText.Length && char.IsDigit( mText[ mOffset ] ) )
                {
                    mOffset++;
                }
            }

            // scientific

            if( Peek() == 'e' )
            {
                type = Token.Double;
                mOffset++;

                var sign = Peek();

                if( sign == '-' || sign == '+' )
                {
                    mOffset++;
                }

                while( mOffset < mText.Length && char.IsDigit( mText[ mOffset ] ) )
                {
                    mOffset++;
                }
            }

            return mPrev = new Token( type, mText.Substring( start, mOffset - start ) );
        }


        // if next character in input stream is 'ch' then consume it

        bool Consume( char ch )
        {
            if( mOffset < mText.Length && mText[ mOffset ] == ch )
            {
                mValue[ mValueLength++ ] = mText[ mOffset++ ];
                return true;
            }

            return false;
        }

        // peek at the next character in input stream

        char Peek()
        {
            return mOffset < mText.Length ? mText[ mOffset ] : '\0';
        }


        //----------------------------------------------------------------------------------------------------
        // return next token from the input stream

        public Token GetNextToken()
        {
            // skip ws

            while( mOffset < mText.Length && char.IsWhiteSpace( mText[ mOffset ] ) )
            {
                mOffset++;
            }

            if( mOffset >= mText.Length )
            {
                return mPrev = null;
            }

            char ch = mText[ mOffset++ ];

            mValue[ 0 ] = ch;
            mValueLength = 1;
            
            switch( ch )
            {
                case '+': return mPrev = new Token( Token.Add, new string( mValue, 0, mValueLength ) );
                case '*': return mPrev = new Token( Token.Multiply, new string( mValue, 0, mValueLength ) );
                case '/': return mPrev = new Token( Token.Divide, new string( mValue, 0, mValueLength ) );
                case '%': return mPrev = new Token( Token.Modulus, new string( mValue, 0, mValueLength ) );
                case '(': return mPrev = new Token( Token.LPAREN, new string( mValue, 0, mValueLength ) );
                case ')': return mPrev = new Token( Token.RPAREN, new string( mValue, 0, mValueLength ) );
                case ',': return mPrev = new Token( Token.Comma, new string( mValue, 0, mValueLength ) );
                case '^': return mPrev = new Token( Token.Exp, new string( mValue, 0, mValueLength ) );
                case '<': return mPrev = new Token( Consume( '=' ) ? Token.LTE : Token.LT, new string( mValue, 0, mValueLength ) );
                case '>': return mPrev = new Token( Consume( '=' ) ? Token.GTE : Token.GT, new string( mValue, 0, mValueLength ) );
                case '!': return mPrev = new Token( Consume( '=' ) ? Token.NE : Token.NOT, new string( mValue, 0, mValueLength ) );
                case '=': Consume( '=' ); return mPrev = new Token( Token.EQ, new string( mValue, 0, mValueLength ) );
                case '&': Consume( '&' ); return mPrev = new Token( Token.AND, new string( mValue, 0, mValueLength ) );
                case '|': Consume( '|' ); return mPrev = new Token( Token.OR, new string( mValue, 0, mValueLength ) );
                    
                case '.': return mPrev = char.IsDigit( Peek() ) ? ReadNumber() : new Token( Token.Unknown, null );

                case '-':
                {
                    if( mPrev == null || mPrev.IsOperator() || mPrev.Type == Token.Comma || mPrev.Type == Token.LPAREN )
                    {
                        if( char.IsDigit( Peek() ) )
                        {
                            return mPrev = ReadNumber();
                        }
                        else
                        {
                            return mPrev = new Token( Token.Negate, new string( mValue, 0, mValueLength ) );
                        }
                    }

                    return mPrev = new Token( Token.Subtract, new string( mValue, 0, mValueLength ) );
                }


                // 'string' or "string"

                case '"':
                case '\'':
                {
                    mValueLength = 0;

                    while( mOffset < mText.Length )
                    {
                        // end of string

                        if( mText[ mOffset ] == ch )
                        {
                            mOffset++;
                            break;
                        }

                        // escaped characters

                        if( mText[ mOffset ] == '\\' )
                        {
                            mOffset++;
                        }

                        mValue[ mValueLength++] = mText[ mOffset++ ];
                    }

                    return mPrev = new Token( Token.String, new string( mValue, 0, mValueLength ) );
                }
            }

            // number

            if( char.IsDigit( ch ) )
            {
                return mPrev = ReadNumber();
            }

            // word

            else if( char.IsLetter( ch ) || ch == '$' )
            {
                var start = mOffset - 1;

                while( mOffset < mText.Length && ( char.IsLetterOrDigit( mText[ mOffset ] ) || mText[ mOffset ] == '.' ) )
                {
                    mOffset++;
                }

                var word = mText.Substring( start, mOffset - start );

                // reserved words

                switch( word )
                {
                    case "true" :   
                    case "false": return mPrev = new Token( Token.Boolean, word );
                    case "null" : return mPrev = new Token( Token.Null, word );
                    case "and"  : return mPrev = new Token( Token.AND, word );
                    case "or"   : return mPrev = new Token( Token.OR, word );
                    case "not"  : return mPrev = new Token( Token.NOT, word );
                }

                // skip ws

                while( mOffset < mText.Length && char.IsWhiteSpace( mText[ mOffset ] ) )
                {
                    mOffset++;
                }

                // function or variable?

                return mPrev = new Token( Peek() == '(' ? Token.Function : Token.Variable, word );
            }

            // unknown

            return mPrev = new Token( Token.Unknown, null );
        }
    }
}

#endif
