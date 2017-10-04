// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;

namespace gw.proto.http
{
    ////////////////////////////////////////////////////////////////////////////////
    // RFC 6455, 11.7

    public enum WebSocketStatus
    {
        None                        = 0,
        NormalClosure               = 1000,
        GoingAway                   = 1001,
        ProtocolError               = 1002,
        PolicyViolation             = 1008, // generic error
        PayloadTooLarge             = 1009,
        InternalError               = 1011,
    }


    ////////////////////////////////////////////////////////////////////////////////

    [Serializable()]
    public class WebSocketException : Exception
    {
        public WebSocketStatus Code;

        public WebSocketException()
        {
            Code = WebSocketStatus.GoingAway;
        }

        public WebSocketException( WebSocketStatus code )
        {
            Code = code;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////

    public static class WebSocketUtils
    {
        public static string CodeToString( WebSocketStatus code )
        {
            switch( code )
            {

                case WebSocketStatus.NormalClosure:         return "Closing connection";
                case WebSocketStatus.GoingAway:             return "Going away";
                case WebSocketStatus.ProtocolError:         return "No extended protocol negotiated";
                case WebSocketStatus.PolicyViolation:       return "Policy violation";
                case WebSocketStatus.PayloadTooLarge:       return "Max payload size exceeded";
                case WebSocketStatus.InternalError:         return "Fatal error";
            }

            return "Unknown Error";
        }
    }
}

