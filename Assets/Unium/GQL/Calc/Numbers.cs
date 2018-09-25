// Copyright ( c ) 2011 Sebastien Ros, https://opensource.org/licenses/MIT
// https://github.com/sheetsync/NCalc

#if !UNIUM_DISABLE && ( DEVELOPMENT_BUILD || UNITY_EDITOR || UNIUM_ENABLE )

using System;
using System.Collections;

namespace gw.gql.calc
{
    public class Numbers
    {
        //----------------------------------------------------------------------------------------------------

        private static Type[] CommonTypes = new[] { typeof(Int64), typeof(Double), typeof(Boolean), typeof(String), typeof(Decimal) };

        private static Type GetMostPreciseType( Type a, Type b )
        {
            foreach( Type t in CommonTypes )
            {
                if( a == t || b == t )
                {
                    return t;
                }
            }

            return a;
        }

        public static int Compare( object a, object b )
        {
            var mpt = GetMostPreciseType( a.GetType(), b.GetType() );
            return Comparer.Default.Compare( Convert.ChangeType( a, mpt ), Convert.ChangeType( b, mpt ) );
        }


        //----------------------------------------------------------------------------------------------------

        public static object Add( object a, object b )
        {
            var typeA = Type.GetTypeCode( a.GetType() );
            var typeB = Type.GetTypeCode( b.GetType() );

            switch( typeA )
            {
                case TypeCode.Boolean:
                    throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );

                case TypeCode.Byte:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (Byte) a + (Byte) b;
                        case TypeCode.SByte:    return (Byte) a + (SByte) b;
                        case TypeCode.Int16:    return (Byte) a + (Int16) b;
                        case TypeCode.UInt16:   return (Byte) a + (UInt16) b;
                        case TypeCode.Int32:    return (Byte) a + (Int32) b;
                        case TypeCode.UInt32:   return (Byte) a + (UInt32) b;
                        case TypeCode.Int64:    return (Byte) a + (Int64) b;
                        case TypeCode.UInt64:   return (Byte) a + (UInt64) b;
                        case TypeCode.Single:   return (Byte) a + (Single) b;
                        case TypeCode.Double:   return (Byte) a + (Double) b;
                        case TypeCode.Decimal:  return (Byte) a + (Decimal) b;
                    }

                    break;

                case TypeCode.SByte:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (SByte) a + (Byte) b;
                        case TypeCode.SByte:    return (SByte) a + (SByte) b;
                        case TypeCode.Int16:    return (SByte) a + (Int16) b;
                        case TypeCode.UInt16:   return (SByte) a + (UInt16) b;
                        case TypeCode.Int32:    return (SByte) a + (Int32) b;
                        case TypeCode.UInt32:   return (SByte) a + (UInt32) b;
                        case TypeCode.Int64:    return (SByte) a + (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'sbyte' and 'ulong'" );
                        case TypeCode.Single:   return (SByte) a + (Single) b;
                        case TypeCode.Double:   return (SByte) a + (Double) b;
                        case TypeCode.Decimal:  return (SByte) a + (Decimal) b;
                    }

                    break;

                case TypeCode.Int16:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (Int16) a + (Byte) b;
                        case TypeCode.SByte:    return (Int16) a + (SByte) b;
                        case TypeCode.Int16:    return (Int16) a + (Int16) b;
                        case TypeCode.UInt16:   return (Int16) a + (UInt16) b;
                        case TypeCode.Int32:    return (Int16) a + (Int32) b;
                        case TypeCode.UInt32:   return (Int16) a + (UInt32) b;
                        case TypeCode.Int64:    return (Int16) a + (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'short' and 'ulong'" );
                        case TypeCode.Single:   return (Int16) a + (Single) b;
                        case TypeCode.Double:   return (Int16) a + (Double) b;
                        case TypeCode.Decimal:  return (Int16) a + (Decimal) b;
                    }
                    break;

                case TypeCode.UInt16:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (UInt16) a + (Byte) b;
                        case TypeCode.SByte:    return (UInt16) a + (SByte) b;
                        case TypeCode.Int16:    return (UInt16) a + (Int16) b;
                        case TypeCode.UInt16:   return (UInt16) a + (UInt16) b;
                        case TypeCode.Int32:    return (UInt16) a + (Int32) b;
                        case TypeCode.UInt32:   return (UInt16) a + (UInt32) b;
                        case TypeCode.Int64:    return (UInt16) a + (Int64) b;
                        case TypeCode.UInt64:   return (UInt16) a + (UInt64) b;
                        case TypeCode.Single:   return (UInt16) a + (Single) b;
                        case TypeCode.Double:   return (UInt16) a + (Double) b;
                        case TypeCode.Decimal:  return (UInt16) a + (Decimal) b;
                    }
                    break;

                case TypeCode.Int32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (Int32) a + (Byte) b;
                        case TypeCode.SByte:    return (Int32) a + (SByte) b;
                        case TypeCode.Int16:    return (Int32) a + (Int16) b;
                        case TypeCode.UInt16:   return (Int32) a + (UInt16) b;
                        case TypeCode.Int32:    return (Int32) a + (Int32) b;
                        case TypeCode.UInt32:   return (Int32) a + (UInt32) b;
                        case TypeCode.Int64:    return (Int32) a + (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'int' and 'ulong'" );
                        case TypeCode.Single:   return (Int32) a + (Single) b;
                        case TypeCode.Double:   return (Int32) a + (Double) b;
                        case TypeCode.Decimal:  return (Int32) a + (Decimal) b;
                    }
                    break;

                case TypeCode.UInt32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (UInt32) a + (Byte) b;
                        case TypeCode.SByte:    return (UInt32) a + (SByte) b;
                        case TypeCode.Int16:    return (UInt32) a + (Int16) b;
                        case TypeCode.UInt16:   return (UInt32) a + (UInt16) b;
                        case TypeCode.Int32:    return (UInt32) a + (Int32) b;
                        case TypeCode.UInt32:   return (UInt32) a + (UInt32) b;
                        case TypeCode.Int64:    return (UInt32) a + (Int64) b;
                        case TypeCode.UInt64:   return (UInt32) a + (UInt64) b;
                        case TypeCode.Single:   return (UInt32) a + (Single) b;
                        case TypeCode.Double:   return (UInt32) a + (Double) b;
                        case TypeCode.Decimal:  return (UInt32) a + (Decimal) b;
                    }
                    break;

                case TypeCode.Int64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (Int64) a + (Byte) b;
                        case TypeCode.SByte:    return (Int64) a + (SByte) b;
                        case TypeCode.Int16:    return (Int64) a + (Int16) b;
                        case TypeCode.UInt16:   return (Int64) a + (UInt16) b;
                        case TypeCode.Int32:    return (Int64) a + (Int32) b;
                        case TypeCode.UInt32:   return (Int64) a + (UInt32) b;
                        case TypeCode.Int64:    return (Int64) a + (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'long' and 'ulong'" );
                        case TypeCode.Single:   return (Int64) a + (Single) b;
                        case TypeCode.Double:   return (Int64) a + (Double) b;
                        case TypeCode.Decimal:  return (Int64) a + (Decimal) b;
                    }
                    break;

                case TypeCode.UInt64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (UInt64) a + (Byte) b;
                        case TypeCode.SByte:    throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'ulong' and 'sbyte'" );
                        case TypeCode.Int16:    throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'ulong' and 'short'" );
                        case TypeCode.UInt16:   return (UInt64) a + (UInt16) b;
                        case TypeCode.Int32:    throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'ulong' and 'int'" );
                        case TypeCode.UInt32:   return (UInt64) a + (UInt32) b;
                        case TypeCode.Int64:    throw new InvalidOperationException( "Operator '+' can't be applied to operands of types 'ulong' and 'ulong'" );
                        case TypeCode.UInt64:   return (UInt64) a + (UInt64) b;
                        case TypeCode.Single:   return (UInt64) a + (Single) b;
                        case TypeCode.Double:   return (UInt64) a + (Double) b;
                        case TypeCode.Decimal:  return (UInt64) a + (Decimal) b;
                    }
                    break;

                case TypeCode.Single:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (Single) a + (Byte) b;
                        case TypeCode.SByte:    return (Single) a + (SByte) b;
                        case TypeCode.Int16:    return (Single) a + (Int16) b;
                        case TypeCode.UInt16:   return (Single) a + (UInt16) b;
                        case TypeCode.Int32:    return (Single) a + (Int32) b;
                        case TypeCode.UInt32:   return (Single) a + (UInt32) b;
                        case TypeCode.Int64:    return (Single) a + (Int64) b;
                        case TypeCode.UInt64:   return (Single) a + (UInt64) b;
                        case TypeCode.Single:   return (Single) a + (Single) b;
                        case TypeCode.Double:   return (Single) a + (Double) b;
                        case TypeCode.Decimal:  return Convert.ToDecimal( a ) + (Decimal) b;
                    }
                    break;

                case TypeCode.Double:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (Double) a + (Byte) b;
                        case TypeCode.SByte:    return (Double) a + (SByte) b;
                        case TypeCode.Int16:    return (Double) a + (Int16) b;
                        case TypeCode.UInt16:   return (Double) a + (UInt16) b;
                        case TypeCode.Int32:    return (Double) a + (Int32) b;
                        case TypeCode.UInt32:   return (Double) a + (UInt32) b;
                        case TypeCode.Int64:    return (Double) a + (Int64) b;
                        case TypeCode.UInt64:   return (Double) a + (UInt64) b;
                        case TypeCode.Single:   return (Double) a + (Single) b;
                        case TypeCode.Double:   return (Double) a + (Double) b;
                        case TypeCode.Decimal:  return Convert.ToDecimal( a ) + (Decimal) b;
                    }
                    break;

                case TypeCode.Decimal:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );
                        case TypeCode.Byte:     return (Decimal) a + (Byte) b;
                        case TypeCode.SByte:    return (Decimal) a + (SByte) b;
                        case TypeCode.Int16:    return (Decimal) a + (Int16) b;
                        case TypeCode.UInt16:   return (Decimal) a + (UInt16) b;
                        case TypeCode.Int32:    return (Decimal) a + (Int32) b;
                        case TypeCode.UInt32:   return (Decimal) a + (UInt32) b;
                        case TypeCode.Int64:    return (Decimal) a + (Int64) b;
                        case TypeCode.UInt64:   return (Decimal) a + (UInt64) b;
                        case TypeCode.Single:   return (Decimal) a + Convert.ToDecimal( b );
                        case TypeCode.Double:   return (Decimal) a + Convert.ToDecimal( b );
                        case TypeCode.Decimal:  return (Decimal) a + (Decimal) b;
                    }
                    break;
            }

            return null;
        }


        //----------------------------------------------------------------------------------------------------

        public static object Substract( object a, object b )
        {
            var typeA = Type.GetTypeCode( a.GetType() );
            var typeB = Type.GetTypeCode( b.GetType() );

            switch( typeA )
            {
                case TypeCode.Boolean:
                    throw new InvalidOperationException( "Operator '+' can't be applied to operands of type 'bool'" );

                case TypeCode.Byte:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'byte' and 'bool'" );
                        case TypeCode.SByte:    return (Byte) a - (SByte) b;
                        case TypeCode.Int16:    return (Byte) a - (Int16) b;
                        case TypeCode.UInt16:   return (Byte) a - (UInt16) b;
                        case TypeCode.Int32:    return (Byte) a - (Int32) b;
                        case TypeCode.UInt32:   return (Byte) a - (UInt32) b;
                        case TypeCode.Int64:    return (Byte) a - (Int64) b;
                        case TypeCode.UInt64:   return (Byte) a - (UInt64) b;
                        case TypeCode.Single:   return (Byte) a - (Single) b;
                        case TypeCode.Double:   return (Byte) a - (Double) b;
                        case TypeCode.Decimal:  return (Byte) a - (Decimal) b;
                    }
                    break;
                case TypeCode.SByte:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'sbyte' and 'bool'" );
                        case TypeCode.SByte:    return (SByte) a - (SByte) b;
                        case TypeCode.Int16:    return (SByte) a - (Int16) b;
                        case TypeCode.UInt16:   return (SByte) a - (UInt16) b;
                        case TypeCode.Int32:    return (SByte) a - (Int32) b;
                        case TypeCode.UInt32:   return (SByte) a - (UInt32) b;
                        case TypeCode.Int64:    return (SByte) a - (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'sbyte' and 'ulong'" );
                        case TypeCode.Single:   return (SByte) a - (Single) b;
                        case TypeCode.Double:   return (SByte) a - (Double) b;
                        case TypeCode.Decimal:  return (SByte) a - (Decimal) b;
                    }
                    break;

                case TypeCode.Int16:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'short' and 'bool'" );
                        case TypeCode.SByte:    return (Int16) a - (SByte) b;
                        case TypeCode.Int16:    return (Int16) a - (Int16) b;
                        case TypeCode.UInt16:   return (Int16) a - (UInt16) b;
                        case TypeCode.Int32:    return (Int16) a - (Int32) b;
                        case TypeCode.UInt32:   return (Int16) a - (UInt32) b;
                        case TypeCode.Int64:    return (Int16) a - (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'short' and 'ulong'" );
                        case TypeCode.Single:   return (Int16) a - (Single) b;
                        case TypeCode.Double:   return (Int16) a - (Double) b;
                        case TypeCode.Decimal:  return (Int16) a - (Decimal) b;
                    }
                    break;


                case TypeCode.UInt16:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'ushort' and 'bool'" );
                        case TypeCode.SByte:    return (UInt16) a - (SByte) b;
                        case TypeCode.Int16:    return (UInt16) a - (Int16) b;
                        case TypeCode.UInt16:   return (UInt16) a - (UInt16) b;
                        case TypeCode.Int32:    return (UInt16) a - (Int32) b;
                        case TypeCode.UInt32:   return (UInt16) a - (UInt32) b;
                        case TypeCode.Int64:    return (UInt16) a - (Int64) b;
                        case TypeCode.UInt64:   return (UInt16) a - (UInt64) b;
                        case TypeCode.Single:   return (UInt16) a - (Single) b;
                        case TypeCode.Double:   return (UInt16) a - (Double) b;
                        case TypeCode.Decimal:  return (UInt16) a - (Decimal) b;
                    }

                    break;

                case TypeCode.Int32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'int' and 'bool'" );
                        case TypeCode.SByte:    return (Int32) a - (SByte) b;
                        case TypeCode.Int16:    return (Int32) a - (Int16) b;
                        case TypeCode.UInt16:   return (Int32) a - (UInt16) b;
                        case TypeCode.Int32:    return (Int32) a - (Int32) b;
                        case TypeCode.UInt32:   return (Int32) a - (UInt32) b;
                        case TypeCode.Int64:    return (Int32) a - (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'int' and 'ulong'" );
                        case TypeCode.Single:   return (Int32) a - (Single) b;
                        case TypeCode.Double:   return (Int32) a - (Double) b;
                        case TypeCode.Decimal:  return (Int32) a - (Decimal) b;
                    }

                    break;


                case TypeCode.UInt32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'uint' and 'bool'" );
                        case TypeCode.SByte:    return (UInt32) a - (SByte) b;
                        case TypeCode.Int16:    return (UInt32) a - (Int16) b;
                        case TypeCode.UInt16:   return (UInt32) a - (UInt16) b;
                        case TypeCode.Int32:    return (UInt32) a - (Int32) b;
                        case TypeCode.UInt32:   return (UInt32) a - (UInt32) b;
                        case TypeCode.Int64:    return (UInt32) a - (Int64) b;
                        case TypeCode.UInt64:   return (UInt32) a - (UInt64) b;
                        case TypeCode.Single:   return (UInt32) a - (Single) b;
                        case TypeCode.Double:   return (UInt32) a - (Double) b;
                        case TypeCode.Decimal:  return (UInt32) a - (Decimal) b;
                    }

                    break;

                case TypeCode.Int64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'long' and 'bool'" );
                        case TypeCode.SByte:    return (Int64) a - (SByte) b;
                        case TypeCode.Int16:    return (Int64) a - (Int16) b;
                        case TypeCode.UInt16:   return (Int64) a - (UInt16) b;
                        case TypeCode.Int32:    return (Int64) a - (Int32) b;
                        case TypeCode.UInt32:   return (Int64) a - (UInt32) b;
                        case TypeCode.Int64:    return (Int64) a - (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'long' and 'ulong'" );
                        case TypeCode.Single:   return (Int64) a - (Single) b;
                        case TypeCode.Double:   return (Int64) a - (Double) b;
                        case TypeCode.Decimal:  return (Int64) a - (Decimal) b;
                    }
                    break;


                case TypeCode.UInt64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'ulong' and 'bool'" );
                        case TypeCode.SByte:    throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'ulong' and 'double'" );
                        case TypeCode.Int16:    throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'ulong' and 'short'" );
                        case TypeCode.UInt16:   return (UInt64) a - (UInt16) b;
                        case TypeCode.Int32:    throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'ulong' and 'int'" );
                        case TypeCode.UInt32:   return (UInt64) a - (UInt32) b;
                        case TypeCode.Int64:    throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'ulong' and 'long'" );
                        case TypeCode.UInt64:   return (UInt64) a - (UInt64) b;
                        case TypeCode.Single:   return (UInt64) a - (Single) b;
                        case TypeCode.Double:   return (UInt64) a - (Double) b;
                        case TypeCode.Decimal:  return (UInt64) a - (Decimal) b;
                    }

                    break;


                case TypeCode.Single:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'float' and 'bool'" );
                        case TypeCode.SByte:    return (Single) a - (SByte) b;
                        case TypeCode.Int16:    return (Single) a - (Int16) b;
                        case TypeCode.UInt16:   return (Single) a - (UInt16) b;
                        case TypeCode.Int32:    return (Single) a - (Int32) b;
                        case TypeCode.UInt32:   return (Single) a - (UInt32) b;
                        case TypeCode.Int64:    return (Single) a - (Int64) b;
                        case TypeCode.UInt64:   return (Single) a - (UInt64) b;
                        case TypeCode.Single:   return (Single) a - (Single) b;
                        case TypeCode.Double:   return (Single) a - (Double) b;
                        case TypeCode.Decimal:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'float' and 'decimal'" );
                    }

                    break;


                case TypeCode.Double:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'double' and 'bool'" );
                        case TypeCode.SByte:    return (Double) a - (SByte) b;
                        case TypeCode.Int16:    return (Double) a - (Int16) b;
                        case TypeCode.UInt16:   return (Double) a - (UInt16) b;
                        case TypeCode.Int32:    return (Double) a - (Int32) b;
                        case TypeCode.UInt32:   return (Double) a - (UInt32) b;
                        case TypeCode.Int64:    return (Double) a - (Int64) b;
                        case TypeCode.UInt64:   return (Double) a - (UInt64) b;
                        case TypeCode.Single:   return (Double) a - (Single) b;
                        case TypeCode.Double:   return (Double) a - (Double) b;
                        case TypeCode.Decimal:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'double' and 'decimal'" );
                    }

                    break;


                case TypeCode.Decimal:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'decimal' and 'bool'" );
                        case TypeCode.SByte:    return (Decimal) a - (SByte) b;
                        case TypeCode.Int16:    return (Decimal) a - (Int16) b;
                        case TypeCode.UInt16:   return (Decimal) a - (UInt16) b;
                        case TypeCode.Int32:    return (Decimal) a - (Int32) b;
                        case TypeCode.UInt32:   return (Decimal) a - (UInt32) b;
                        case TypeCode.Int64:    return (Decimal) a - (Int64) b;
                        case TypeCode.UInt64:   return (Decimal) a - (UInt64) b;
                        case TypeCode.Single:   throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'decimal' and 'float'" );
                        case TypeCode.Double:   throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'decimal' and 'double'" );
                        case TypeCode.Decimal:  return (Decimal) a - (Decimal) b;
                    }

                    break;
            }

            return null;
        }


        //----------------------------------------------------------------------------------------------------

        public static object Multiply( object a, object b )
        {
            var typeA = Type.GetTypeCode( a.GetType() );
            var typeB = Type.GetTypeCode( b.GetType() );

            switch( typeA )
            {
                case TypeCode.Byte:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'byte' and 'bool'" );
                        case TypeCode.SByte:    return (Byte) a * (SByte) b;
                        case TypeCode.Int16:    return (Byte) a * (Int16) b;
                        case TypeCode.UInt16:   return (Byte) a * (UInt16) b;
                        case TypeCode.Int32:    return (Byte) a * (Int32) b;
                        case TypeCode.UInt32:   return (Byte) a * (UInt32) b;
                        case TypeCode.Int64:    return (Byte) a * (Int64) b;
                        case TypeCode.UInt64:   return (Byte) a * (UInt64) b;
                        case TypeCode.Single:   return (Byte) a * (Single) b;
                        case TypeCode.Double:   return (Byte) a * (Double) b;
                        case TypeCode.Decimal:  return (Byte) a * (Decimal) b;
                    }

                    break;

                case TypeCode.SByte:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'sbyte' and 'bool'" );
                        case TypeCode.SByte:    return (SByte) a * (SByte) b;
                        case TypeCode.Int16:    return (SByte) a * (Int16) b;
                        case TypeCode.UInt16:   return (SByte) a * (UInt16) b;
                        case TypeCode.Int32:    return (SByte) a * (Int32) b;
                        case TypeCode.UInt32:   return (SByte) a * (UInt32) b;
                        case TypeCode.Int64:    return (SByte) a * (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'sbyte' and 'ulong'" );
                        case TypeCode.Single:   return (SByte) a * (Single) b;
                        case TypeCode.Double:   return (SByte) a * (Double) b;
                        case TypeCode.Decimal:  return (SByte) a * (Decimal) b;
                    }

                    break;


                case TypeCode.Int16:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'short' and 'bool'" );
                        case TypeCode.SByte:    return (Int16) a * (SByte) b;
                        case TypeCode.Int16:    return (Int16) a * (Int16) b;
                        case TypeCode.UInt16:   return (Int16) a * (UInt16) b;
                        case TypeCode.Int32:    return (Int16) a * (Int32) b;
                        case TypeCode.UInt32:   return (Int16) a * (UInt32) b;
                        case TypeCode.Int64:    return (Int16) a * (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'short' and 'ulong'" );
                        case TypeCode.Single:   return (Int16) a * (Single) b;
                        case TypeCode.Double:   return (Int16) a * (Double) b;
                        case TypeCode.Decimal:  return (Int16) a * (Decimal) b;
                    }
                    break;

                case TypeCode.UInt16:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'ushort' and 'bool'" );
                        case TypeCode.SByte:    return (UInt16) a * (SByte) b;
                        case TypeCode.Int16:    return (UInt16) a * (Int16) b;
                        case TypeCode.UInt16:   return (UInt16) a * (UInt16) b;
                        case TypeCode.Int32:    return (UInt16) a * (Int32) b;
                        case TypeCode.UInt32:   return (UInt16) a * (UInt32) b;
                        case TypeCode.Int64:    return (UInt16) a * (Int64) b;
                        case TypeCode.UInt64:   return (UInt16) a * (UInt64) b;
                        case TypeCode.Single:   return (UInt16) a * (Single) b;
                        case TypeCode.Double:   return (UInt16) a * (Double) b;
                        case TypeCode.Decimal:  return (UInt16) a * (Decimal) b;
                    }
                    break;


                case TypeCode.Int32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'int' and 'bool'" );
                        case TypeCode.SByte:    return (Int32) a * (SByte) b;
                        case TypeCode.Int16:    return (Int32) a * (Int16) b;
                        case TypeCode.UInt16:   return (Int32) a * (UInt16) b;
                        case TypeCode.Int32:    return (Int32) a * (Int32) b;
                        case TypeCode.UInt32:   return (Int32) a * (UInt32) b;
                        case TypeCode.Int64:    return (Int32) a * (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'int' and 'ulong'" );
                        case TypeCode.Single:   return (Int32) a * (Single) b;
                        case TypeCode.Double:   return (Int32) a * (Double) b;
                        case TypeCode.Decimal:  return (Int32) a * (Decimal) b;
                    }
                    break;


                case TypeCode.UInt32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'uint' and 'bool'" );
                        case TypeCode.SByte:    return (UInt32) a * (SByte) b;
                        case TypeCode.Int16:    return (UInt32) a * (Int16) b;
                        case TypeCode.UInt16:   return (UInt32) a * (UInt16) b;
                        case TypeCode.Int32:    return (UInt32) a * (Int32) b;
                        case TypeCode.UInt32:   return (UInt32) a * (UInt32) b;
                        case TypeCode.Int64:    return (UInt32) a * (Int64) b;
                        case TypeCode.UInt64:   return (UInt32) a * (UInt64) b;
                        case TypeCode.Single:   return (UInt32) a * (Single) b;
                        case TypeCode.Double:   return (UInt32) a * (Double) b;
                        case TypeCode.Decimal:  return (UInt32) a * (Decimal) b;
                    }

                    break;


                case TypeCode.Int64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'long' and 'bool'" );
                        case TypeCode.SByte:    return (Int64) a * (SByte) b;
                        case TypeCode.Int16:    return (Int64) a * (Int16) b;
                        case TypeCode.UInt16:   return (Int64) a * (UInt16) b;
                        case TypeCode.Int32:    return (Int64) a * (Int32) b;
                        case TypeCode.UInt32:   return (Int64) a * (UInt32) b;
                        case TypeCode.Int64:    return (Int64) a * (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'long' and 'ulong'" );
                        case TypeCode.Single:   return (Int64) a * (Single) b;
                        case TypeCode.Double:   return (Int64) a * (Double) b;
                        case TypeCode.Decimal:  return (Int64) a * (Decimal) b;
                    }

                    break;


                case TypeCode.UInt64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'ulong' and 'bool'" );
                        case TypeCode.SByte:    throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'ulong' and 'sbyte'" );
                        case TypeCode.Int16:    throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'ulong' and 'short'" );
                        case TypeCode.UInt16:   return (UInt64) a * (UInt16) b;
                        case TypeCode.Int32:    throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'ulong' and 'int'" );
                        case TypeCode.UInt32:   return (UInt64) a * (UInt32) b;
                        case TypeCode.Int64:    throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'ulong' and 'long'" );
                        case TypeCode.UInt64:   return (UInt64) a * (UInt64) b;
                        case TypeCode.Single:   return (UInt64) a * (Single) b;
                        case TypeCode.Double:   return (UInt64) a * (Double) b;
                        case TypeCode.Decimal:  return (UInt64) a * (Decimal) b;
                    }

                    break;


                case TypeCode.Single:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'float' and 'bool'" );
                        case TypeCode.SByte:    return (Single) a * (SByte) b;
                        case TypeCode.Int16:    return (Single) a * (Int16) b;
                        case TypeCode.UInt16:   return (Single) a * (UInt16) b;
                        case TypeCode.Int32:    return (Single) a * (Int32) b;
                        case TypeCode.UInt32:   return (Single) a * (UInt32) b;
                        case TypeCode.Int64:    return (Single) a * (Int64) b;
                        case TypeCode.UInt64:   return (Single) a * (UInt64) b;
                        case TypeCode.Single:   return (Single) a * (Single) b;
                        case TypeCode.Double:   return (Single) a * (Double) b;
                        case TypeCode.Decimal:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'float' and 'decimal'" );
                    }

                    break;


                case TypeCode.Double:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'double' and 'bool'" );
                        case TypeCode.SByte:    return (Double) a * (SByte) b;
                        case TypeCode.Int16:    return (Double) a * (Int16) b;
                        case TypeCode.UInt16:   return (Double) a * (UInt16) b;
                        case TypeCode.Int32:    return (Double) a * (Int32) b;
                        case TypeCode.UInt32:   return (Double) a * (UInt32) b;
                        case TypeCode.Int64:    return (Double) a * (Int64) b;
                        case TypeCode.UInt64:   return (Double) a * (UInt64) b;
                        case TypeCode.Single:   return (Double) a * (Single) b;
                        case TypeCode.Double:   return (Double) a * (Double) b;
                        case TypeCode.Decimal:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'double' and 'decimal'" );
                    }

                    break;


                case TypeCode.Decimal:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'decimal' and 'bool'" );
                        case TypeCode.SByte:    return (Decimal) a * (SByte) b;
                        case TypeCode.Int16:    return (Decimal) a * (Int16) b;
                        case TypeCode.UInt16:   return (Decimal) a * (UInt16) b;
                        case TypeCode.Int32:    return (Decimal) a * (Int32) b;
                        case TypeCode.UInt32:   return (Decimal) a * (UInt32) b;
                        case TypeCode.Int64:    return (Decimal) a * (Int64) b;
                        case TypeCode.UInt64:   return (Decimal) a * (UInt64) b;
                        case TypeCode.Single:   throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'decimal' and 'float'" );
                        case TypeCode.Double:   throw new InvalidOperationException( "Operator '*' can't be applied to operands of types 'decimal' and 'double'" );
                        case TypeCode.Decimal:  return (Decimal) a * (Decimal) b;
                    }

                    break;
            }

            return null;
        }


        //----------------------------------------------------------------------------------------------------

        public static object Divide( object a, object b )
        {
            var typeA = Type.GetTypeCode( a.GetType() );
            var typeB = Type.GetTypeCode( b.GetType() );

            switch( typeA )
            {
                case TypeCode.Byte:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'byte' and 'bool'" );
                        case TypeCode.SByte:    return (Byte) a / (SByte) b;
                        case TypeCode.Int16:    return (Byte) a / (Int16) b;
                        case TypeCode.UInt16:   return (Byte) a / (UInt16) b;
                        case TypeCode.Int32:    return (Byte) a / (Int32) b;
                        case TypeCode.UInt32:   return (Byte) a / (UInt32) b;
                        case TypeCode.Int64:    return (Byte) a / (Int64) b;
                        case TypeCode.UInt64:   return (Byte) a / (UInt64) b;
                        case TypeCode.Single:   return (Byte) a / (Single) b;
                        case TypeCode.Double:   return (Byte) a / (Double) b;
                        case TypeCode.Decimal:  return (Byte) a / (Decimal) b;
                    }

                    break;

                case TypeCode.SByte:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'sbyte' and 'bool'" );
                        case TypeCode.SByte:    return (SByte) a / (SByte) b;
                        case TypeCode.Int16:    return (SByte) a / (Int16) b;
                        case TypeCode.UInt16:   return (SByte) a / (UInt16) b;
                        case TypeCode.Int32:    return (SByte) a / (Int32) b;
                        case TypeCode.UInt32:   return (SByte) a / (UInt32) b;
                        case TypeCode.Int64:    return (SByte) a / (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'sbyte' and 'ulong'" );
                        case TypeCode.Single:   return (SByte) a / (Single) b;
                        case TypeCode.Double:   return (SByte) a / (Double) b;
                        case TypeCode.Decimal:  return (SByte) a / (Decimal) b;
                    }

                    break;


                case TypeCode.Int16:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'short' and 'bool'" );
                        case TypeCode.SByte:    return (Int16) a / (SByte) b;
                        case TypeCode.Int16:    return (Int16) a / (Int16) b;
                        case TypeCode.UInt16:   return (Int16) a / (UInt16) b;
                        case TypeCode.Int32:    return (Int16) a / (Int32) b;
                        case TypeCode.UInt32:   return (Int16) a / (UInt32) b;
                        case TypeCode.Int64:    return (Int16) a / (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'short' and 'ulong'" );
                        case TypeCode.Single:   return (Int16) a / (Single) b;
                        case TypeCode.Double:   return (Int16) a / (Double) b;
                        case TypeCode.Decimal:  return (Int16) a / (Decimal) b;
                    }

                    break;


                case TypeCode.UInt16:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'ushort' and 'bool'" );
                        case TypeCode.SByte:    return (UInt16) a / (SByte) b;
                        case TypeCode.Int16:    return (UInt16) a / (Int16) b;
                        case TypeCode.UInt16:   return (UInt16) a / (UInt16) b;
                        case TypeCode.Int32:    return (UInt16) a / (Int32) b;
                        case TypeCode.UInt32:   return (UInt16) a / (UInt32) b;
                        case TypeCode.Int64:    return (UInt16) a / (Int64) b;
                        case TypeCode.UInt64:   return (UInt16) a / (UInt64) b;
                        case TypeCode.Single:   return (UInt16) a / (Single) b;
                        case TypeCode.Double:   return (UInt16) a / (Double) b;
                        case TypeCode.Decimal:  return (UInt16) a / (Decimal) b;
                    }

                    break;


                case TypeCode.Int32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'int' and 'bool'" );
                        case TypeCode.SByte:    return (Int32) a / (SByte) b;
                        case TypeCode.Int16:    return (Int32) a / (Int16) b;
                        case TypeCode.UInt16:   return (Int32) a / (UInt16) b;
                        case TypeCode.Int32:    return (Int32) a / (Int32) b;
                        case TypeCode.UInt32:   return (Int32) a / (UInt32) b;
                        case TypeCode.Int64:    return (Int32) a / (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'int' and 'ulong'" );
                        case TypeCode.Single:   return (Int32) a / (Single) b;
                        case TypeCode.Double:   return (Int32) a / (Double) b;
                        case TypeCode.Decimal:  return (Int32) a / (Decimal) b;
                    }

                    break;


                case TypeCode.UInt32:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'uint' and 'bool'" );
                        case TypeCode.SByte:    return (UInt32) a / (SByte) b;
                        case TypeCode.Int16:    return (UInt32) a / (Int16) b;
                        case TypeCode.UInt16:   return (UInt32) a / (UInt16) b;
                        case TypeCode.Int32:    return (UInt32) a / (Int32) b;
                        case TypeCode.UInt32:   return (UInt32) a / (UInt32) b;
                        case TypeCode.Int64:    return (UInt32) a / (Int64) b;
                        case TypeCode.UInt64:   return (UInt32) a / (UInt64) b;
                        case TypeCode.Single:   return (UInt32) a / (Single) b;
                        case TypeCode.Double:   return (UInt32) a / (Double) b;
                        case TypeCode.Decimal:  return (UInt32) a / (Decimal) b;
                    }

                    break;


                case TypeCode.Int64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'long' and 'bool'" );
                        case TypeCode.SByte:    return (Int64) a / (SByte) b;
                        case TypeCode.Int16:    return (Int64) a / (Int16) b;
                        case TypeCode.UInt16:   return (Int64) a / (UInt16) b;
                        case TypeCode.Int32:    return (Int64) a / (Int32) b;
                        case TypeCode.UInt32:   return (Int64) a / (UInt32) b;
                        case TypeCode.Int64:    return (Int64) a / (Int64) b;
                        case TypeCode.UInt64:   throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'long' and 'ulong'" );
                        case TypeCode.Single:   return (Int64) a / (Single) b;
                        case TypeCode.Double:   return (Int64) a / (Double) b;
                        case TypeCode.Decimal:  return (Int64) a / (Decimal) b;
                    }

                    break;


                case TypeCode.UInt64:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '-' can't be applied to operands of types 'ulong' and 'bool'" );
                        case TypeCode.SByte:    throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'ulong' and 'sbyte'" );
                        case TypeCode.Int16:    throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'ulong' and 'short'" );
                        case TypeCode.UInt16:   return (UInt64) a / (UInt16) b;
                        case TypeCode.Int32:    throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'ulong' and 'int'" );
                        case TypeCode.UInt32:   return (UInt64) a / (UInt32) b;
                        case TypeCode.Int64:    throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'ulong' and 'long'" );
                        case TypeCode.UInt64:   return (UInt64) a / (UInt64) b;
                        case TypeCode.Single:   return (UInt64) a / (Single) b;
                        case TypeCode.Double:   return (UInt64) a / (Double) b;
                        case TypeCode.Decimal:  return (UInt64) a / (Decimal) b;
                    }

                    break;


                case TypeCode.Single:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'float' and 'bool'" );
                        case TypeCode.SByte:    return (Single) a / (SByte) b;
                        case TypeCode.Int16:    return (Single) a / (Int16) b;
                        case TypeCode.UInt16:   return (Single) a / (UInt16) b;
                        case TypeCode.Int32:    return (Single) a / (Int32) b;
                        case TypeCode.UInt32:   return (Single) a / (UInt32) b;
                        case TypeCode.Int64:    return (Single) a / (Int64) b;
                        case TypeCode.UInt64:   return (Single) a / (UInt64) b;
                        case TypeCode.Single:   return (Single) a / (Single) b;
                        case TypeCode.Double:   return (Single) a / (Double) b;
                        case TypeCode.Decimal:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'float' and 'decimal'" );
                    }

                    break;


                case TypeCode.Double:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'double' and 'bool'" );
                        case TypeCode.SByte:    return (Double) a / (SByte) b;
                        case TypeCode.Int16:    return (Double) a / (Int16) b;
                        case TypeCode.UInt16:   return (Double) a / (UInt16) b;
                        case TypeCode.Int32:    return (Double) a / (Int32) b;
                        case TypeCode.UInt32:   return (Double) a / (UInt32) b;
                        case TypeCode.Int64:    return (Double) a / (Int64) b;
                        case TypeCode.UInt64:   return (Double) a / (UInt64) b;
                        case TypeCode.Single:   return (Double) a / (Single) b;
                        case TypeCode.Double:   return (Double) a / (Double) b;
                        case TypeCode.Decimal:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'double' and 'decimal'" );
                    }

                    break;


                case TypeCode.Decimal:

                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'decimal' and 'bool'" );
                        case TypeCode.SByte:    return (Decimal) a / (SByte) b;
                        case TypeCode.Int16:    return (Decimal) a / (Int16) b;
                        case TypeCode.UInt16:   return (Decimal) a / (UInt16) b;
                        case TypeCode.Int32:    return (Decimal) a / (Int32) b;
                        case TypeCode.UInt32:   return (Decimal) a / (UInt32) b;
                        case TypeCode.Int64:    return (Decimal) a / (Int64) b;
                        case TypeCode.UInt64:   return (Decimal) a / (UInt64) b;
                        case TypeCode.Single:   throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'decimal' and 'float'" );
                        case TypeCode.Double:   throw new InvalidOperationException( "Operator '/' can't be applied to operands of types 'decimal' and 'double'" );
                        case TypeCode.Decimal:  return (Decimal) a / (Decimal) b;
                    }

                    break;
            }

            return null;
        }

        //----------------------------------------------------------------------------------------------------

        public static object Modulo( object a, object b )
        {
            var typeA = Type.GetTypeCode( a.GetType() );
            var typeB = Type.GetTypeCode( b.GetType() );

            switch( typeA )
            {
                case TypeCode.Byte:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:  throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'byte' and 'bool'" );
                        case TypeCode.SByte:    return (Byte) a % (SByte) b;
                        case TypeCode.Int16:    return (Byte) a % (Int16) b;
                        case TypeCode.UInt16:   return (Byte) a % (UInt16) b;
                        case TypeCode.Int32:    return (Byte) a % (Int32) b;
                        case TypeCode.UInt32:   return (Byte) a % (UInt32) b;
                        case TypeCode.Int64:    return (Byte) a % (Int64) b;
                        case TypeCode.UInt64:   return (Byte) a % (UInt64) b;
                        case TypeCode.Single:   return (Byte) a % (Single) b;
                        case TypeCode.Double:   return (Byte) a % (Double) b;
                        case TypeCode.Decimal:  return (Byte) a % (Decimal) b;
                    }
                    break;
                case TypeCode.SByte:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'sbyte' and 'bool'" );
                        case TypeCode.SByte:    return (SByte) a % (SByte) b;
                        case TypeCode.Int16:    return (SByte) a % (Int16) b;
                        case TypeCode.UInt16:   return (SByte) a % (UInt16) b;
                        case TypeCode.Int32:    return (SByte) a % (Int32) b;
                        case TypeCode.UInt32:   return (SByte) a % (UInt32) b;
                        case TypeCode.Int64:    return (SByte) a % (Int64) b;
                        case TypeCode.UInt64:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'sbyte' and 'ulong'" );
                        case TypeCode.Single:   return (SByte) a % (Single) b;
                        case TypeCode.Double:   return (SByte) a % (Double) b;
                        case TypeCode.Decimal:  return (SByte) a % (Decimal) b;
                    }
                    break;

                case TypeCode.Int16:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'short' and 'bool'" );
                        case TypeCode.SByte:    return (Int16) a % (SByte) b;
                        case TypeCode.Int16:    return (Int16) a % (Int16) b;
                        case TypeCode.UInt16:   return (Int16) a % (UInt16) b;
                        case TypeCode.Int32:    return (Int16) a % (Int32) b;
                        case TypeCode.UInt32:   return (Int16) a % (UInt32) b;
                        case TypeCode.Int64:    return (Int16) a % (Int64) b;
                        case TypeCode.UInt64:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'short' and 'ulong'" );
                        case TypeCode.Single:   return (Int16) a % (Single) b;
                        case TypeCode.Double:   return (Int16) a % (Double) b;
                        case TypeCode.Decimal:  return (Int16) a % (Decimal) b;
                    }
                    break;

                case TypeCode.UInt16:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'ushort' and 'bool'" );
                        case TypeCode.SByte:    return (UInt16) a % (SByte) b;
                        case TypeCode.Int16:    return (UInt16) a % (Int16) b;
                        case TypeCode.UInt16:   return (UInt16) a % (UInt16) b;
                        case TypeCode.Int32:    return (UInt16) a % (Int32) b;
                        case TypeCode.UInt32:   return (UInt16) a % (UInt32) b;
                        case TypeCode.Int64:    return (UInt16) a % (Int64) b;
                        case TypeCode.UInt64:   return (UInt16) a % (UInt64) b;
                        case TypeCode.Single:   return (UInt16) a % (Single) b;
                        case TypeCode.Double:   return (UInt16) a % (Double) b;
                        case TypeCode.Decimal:  return (UInt16) a % (Decimal) b;
                    }
                    break;

                case TypeCode.Int32:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'int' and 'bool'" );
                        case TypeCode.SByte:    return (Int32) a % (SByte) b;
                        case TypeCode.Int16:    return (Int32) a % (Int16) b;
                        case TypeCode.UInt16:   return (Int32) a % (UInt16) b;
                        case TypeCode.Int32:    return (Int32) a % (Int32) b;
                        case TypeCode.UInt32:   return (Int32) a % (UInt32) b;
                        case TypeCode.Int64:    return (Int32) a % (Int64) b;
                        case TypeCode.UInt64:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'int' and 'ulong'" );
                        case TypeCode.Single:   return (Int32) a % (Single) b;
                        case TypeCode.Double:   return (Int32) a % (Double) b;
                        case TypeCode.Decimal:  return (Int32) a % (Decimal) b;
                    }
                    break;

                case TypeCode.UInt32:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'uint' and 'bool'" );
                        case TypeCode.SByte:    return (UInt32) a % (SByte) b;
                        case TypeCode.Int16:    return (UInt32) a % (Int16) b;
                        case TypeCode.UInt16:   return (UInt32) a % (UInt16) b;
                        case TypeCode.Int32:    return (UInt32) a % (Int32) b;
                        case TypeCode.UInt32:   return (UInt32) a % (UInt32) b;
                        case TypeCode.Int64:    return (UInt32) a % (Int64) b;
                        case TypeCode.UInt64:   return (UInt32) a % (UInt64) b;
                        case TypeCode.Single:   return (UInt32) a % (Single) b;
                        case TypeCode.Double:   return (UInt32) a % (Double) b;
                        case TypeCode.Decimal:  return (UInt32) a % (Decimal) b;
                    }
                    break;

                case TypeCode.Int64:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'long' and 'bool'" );
                        case TypeCode.SByte:    return (Int64) a % (SByte) b;
                        case TypeCode.Int16:    return (Int64) a % (Int16) b;
                        case TypeCode.UInt16:   return (Int64) a % (UInt16) b;
                        case TypeCode.Int32:    return (Int64) a % (Int32) b;
                        case TypeCode.UInt32:   return (Int64) a % (UInt32) b;
                        case TypeCode.Int64:    return (Int64) a % (Int64) b;
                        case TypeCode.UInt64:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'long' and 'ulong'" );
                        case TypeCode.Single:   return (Int64) a % (Single) b;
                        case TypeCode.Double:   return (Int64) a % (Double) b;
                        case TypeCode.Decimal:  return (Int64) a % (Decimal) b;
                    }
                    break;

                case TypeCode.UInt64:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'ulong' and 'bool'" );
                        case TypeCode.SByte:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'ulong' and 'sbyte'" );
                        case TypeCode.Int16:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'ulong' and 'short'" );
                        case TypeCode.UInt16:   return (UInt64) a % (UInt16) b;
                        case TypeCode.Int32:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'ulong' and 'int'" );
                        case TypeCode.UInt32:   return (UInt64) a % (UInt32) b;
                        case TypeCode.Int64:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'ulong' and 'long'" );
                        case TypeCode.UInt64:   return (UInt64) a % (UInt64) b;
                        case TypeCode.Single:   return (UInt64) a % (Single) b;
                        case TypeCode.Double:   return (UInt64) a % (Double) b;
                        case TypeCode.Decimal:  return (UInt64) a % (Decimal) b;
                    }
                    break;

                case TypeCode.Single:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'float' and 'bool'" );
                        case TypeCode.SByte:    return (Single) a % (SByte) b;
                        case TypeCode.Int16:    return (Single) a % (Int16) b;
                        case TypeCode.UInt16:   return (Single) a % (UInt16) b;
                        case TypeCode.Int32:    return (Single) a % (Int32) b;
                        case TypeCode.UInt32:   return (Single) a % (UInt32) b;
                        case TypeCode.Int64:    return (Single) a % (Int64) b;
                        case TypeCode.UInt64:   return (Single) a % (UInt64) b;
                        case TypeCode.Single:   return (Single) a % (Single) b;
                        case TypeCode.Double:   return (Single) a % (Double) b;
                        case TypeCode.Decimal:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'float' and 'decimal'" );
                    }
                    break;

                case TypeCode.Double:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'double' and 'bool'" );
                        case TypeCode.SByte:    return (Double) a % (SByte) b;
                        case TypeCode.Int16:    return (Double) a % (Int16) b;
                        case TypeCode.UInt16:   return (Double) a % (UInt16) b;
                        case TypeCode.Int32:    return (Double) a % (Int32) b;
                        case TypeCode.UInt32:   return (Double) a % (UInt32) b;
                        case TypeCode.Int64:    return (Double) a % (Int64) b;
                        case TypeCode.UInt64:   return (Double) a % (UInt64) b;
                        case TypeCode.Single:   return (Double) a % (Single) b;
                        case TypeCode.Double:   return (Double) a % (Double) b;
                        case TypeCode.Decimal:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'double' and 'decimal'" );
                    }
                    break;

                case TypeCode.Decimal:
                    switch( typeB )
                    {
                        case TypeCode.Boolean:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'decimal' and 'bool'" );
                        case TypeCode.SByte:    return (Decimal) a % (SByte) b;
                        case TypeCode.Int16:    return (Decimal) a % (Int16) b;
                        case TypeCode.UInt16:   return (Decimal) a % (UInt16) b;
                        case TypeCode.Int32:    return (Decimal) a % (Int32) b;
                        case TypeCode.UInt32:   return (Decimal) a % (UInt32) b;
                        case TypeCode.Int64:    return (Decimal) a % (Int64) b;
                        case TypeCode.UInt64:   return (Decimal) a % (UInt64) b;
                        case TypeCode.Single:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'decimal' and 'float'" );
                        case TypeCode.Double:                                        throw new InvalidOperationException( "Operator '%' can't be applied to operands of types 'decimal' and 'decimal'" );
                        case TypeCode.Decimal:  return (Decimal) a % (Decimal) b;
                    }
                    break;
            }

            return null;
        }

        //----------------------------------------------------------------------------------------------------

        public static object Power( object a, object b )
        {
            return Math.Pow( Convert.ToDouble( a ), Convert.ToDouble( b ) );
        }


        //----------------------------------------------------------------------------------------------------

        public static object Negate( object a )
        {
            switch( Type.GetTypeCode( a.GetType() ) )
            {
                case TypeCode.Boolean:  throw new InvalidOperationException( "Operator unary '-' can't be applied to operands of types 'bool'" );
                case TypeCode.SByte:    return -(SByte) a;
                case TypeCode.Int16:    return -(Int16) a;
                case TypeCode.UInt16:   return -(UInt16) a;
                case TypeCode.Int32:    return -(Int32) a;
                case TypeCode.UInt32:   return -(UInt32)a;
                case TypeCode.Int64:    return -(Int64) a;
                case TypeCode.UInt64:   throw new InvalidOperationException( "Operator unary '-' can't be applied to operands of types 'ulong'" );
                case TypeCode.Single:   return -(Single) a;
                case TypeCode.Double:   return -(Double) a;
                case TypeCode.Decimal:  return -(Decimal) a;
            }

            return null;
        }
    }
}

#endif
