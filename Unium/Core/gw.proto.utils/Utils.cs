// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;
using System.Linq;

namespace gw.proto.utils
{
    public static class Util
    {
        public static Random Rnd = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

        public static string RandomString( int length = 8 )
        {
            return new string( Enumerable.Repeat( chars, length ).Select( s => s[ Rnd.Next( s.Length ) ] ).ToArray() );
        }
    }
}
