using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YubicoDotNetClient
{
    class YubicoValidationFailure : ApplicationException
    {
        public YubicoValidationFailure()
        {
        }

        public YubicoValidationFailure(String message)
        {
        }

        public YubicoValidationFailure(String message, Exception inner)
        {
        }

        protected YubicoValidationFailure(SerializationInfo info, StreamingContext context)
        {
        }
    }
}
