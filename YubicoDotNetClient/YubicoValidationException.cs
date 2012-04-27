using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YubicoDotNetClient
{
    class YubicoValidationException : ApplicationException
    {
        public YubicoValidationException()
        {
        }

        public YubicoValidationException(String message)
        {
        }

        public YubicoValidationException(String message, Exception inner)
        {
        }

        protected YubicoValidationException(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
