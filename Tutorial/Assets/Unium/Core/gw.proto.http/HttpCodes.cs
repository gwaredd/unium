// Copyright (c) 2017 Gwaredd Mountain, https://opensource.org/licenses/MIT

using System;

namespace gw.proto.http
{
    ////////////////////////////////////////////////////////////////////////////////

    public enum ResponseCode
    {
        SwitchingProtocols          = 101,
        OK                          = 200,
        MovedPermanently            = 301,
        BadRequest                  = 400,
        NotFound                    = 404,
        MethodNotAllowed            = 405,
        RequestTimeout              = 408,
        LengthRequired              = 411,
        EntityTooLarge              = 413,
        ImATeaPot                   = 418,
        UpgradeRequired             = 426,
        InternalServerError         = 500,
        Unimplemented               = 501,
    }


    ////////////////////////////////////////////////////////////////////////////////

    [Serializable()]
    public class HttpResponseException : Exception
    {
        public ResponseCode Code;

        public HttpResponseException()
        {
            Code = ResponseCode.InternalServerError;
        }

        public HttpResponseException( ResponseCode code )
        {
            Code = code;
        }
    }


    ////////////////////////////////////////////////////////////////////////////////

    public class HttpUtils
    {
        public static string CodeToString( ResponseCode code )
        {
            switch( code )
            {
                case ResponseCode.OK:                       return "OK";
                case ResponseCode.SwitchingProtocols:       return "Switching Protocols";
                case ResponseCode.MovedPermanently:         return "Moved Permanently";
                case ResponseCode.BadRequest:               return "Bad Request";
                case ResponseCode.NotFound:                 return "Not Found";
                case ResponseCode.MethodNotAllowed:         return "Method Not Allowed";
                case ResponseCode.RequestTimeout:           return "Request Timeout";
                case ResponseCode.LengthRequired:           return "Length Required";
                case ResponseCode.EntityTooLarge:           return "Entity Too Large";
                case ResponseCode.ImATeaPot:                return "I'm a teapot";
                case ResponseCode.UpgradeRequired:          return "Upgrade Required";
                case ResponseCode.InternalServerError:      return "Internal Server Error";
                case ResponseCode.Unimplemented:            return "Unimplemented";
            }

            return "Unknown Error";
        }
    }
}

