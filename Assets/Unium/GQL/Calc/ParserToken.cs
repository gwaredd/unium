// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT
#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

namespace gw.gql.calc
{
    public class Token
    {
        //----------------------------------------------------------------------------------------------------
        // types

        // misc

        public const int None        = 0x0000;
        public const int Unknown     = 0x0001;

        public const int LPAREN      = 0x0002;
        public const int RPAREN      = 0x0003;
        public const int Comma       = 0x0004;

        // operands

        public const int Null        = 0x0010;
        public const int Boolean     = 0x0011;
        public const int Integer     = 0x0012;
        public const int Double      = 0x0013;
        public const int String      = 0x0014;
        public const int Variable    = 0x0015;

        // binary operators - grouped into precendence; with associativity

        public const int OR          = 0x0100; // L

        public const int AND         = 0x0200; // L

        public const int EQ          = 0x0300; // L
        public const int NE          = 0x0301; // L

        public const int LT          = 0x0400; // L
        public const int LTE         = 0x0401; // L
        public const int GT          = 0x0402; // L
        public const int GTE         = 0x0403; // L

        public const int Add         = 0x0500; // L
        public const int Subtract    = 0x0501; // L

        public const int Multiply    = 0x0600; // L
        public const int Divide      = 0x0601; // L
        public const int Modulus     = 0x0602; // L

        public const int Exp         = 0x0700; // R - power / exponentiation; 5 ^ 2

        // unary operators

        public const int Negate      = 0x0800; // R - unary negate
        public const int NOT         = 0x0801; // R

        // function

        public const int Function    = 0x1000;


        //----------------------------------------------------------------------------------------------------

        public bool IsOperand()
        {
            return (Type & 0x0010) != 0x0000;
        }

        public bool IsOperator()
        {
            return (Type & 0x0F00) != 0x0000;
        }

        public bool IsUnaryOperator()
        {
            return (Type & 0x0800) != 0x0000;
        }

        public bool IsBinaryOperator()
        {
            return (Type & 0x0700) != 0x0000;
        }

        static public bool IsBooleanOperator( int Type )
        {
            return ( Type & 0x0700 ) < 0x0500;
        }

        public int Precedence()
        {
            return Type >> 8;
        }


        //----------------------------------------------------------------------------------------------------

        public int      Type        = Token.None;
        public string   Text        = "";
        public int      Args        = 0;


        // context for counting number of function arguments(...)

        public int      NumCommas   = 0;    // 
        public int      OutputSize  = 0;

        public Token( int type, string text = null )
        {
            Type = type;
            Text = text;
        }
    }
}

#endif
