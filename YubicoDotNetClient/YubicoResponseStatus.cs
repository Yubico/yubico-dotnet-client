using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YubicoDotNetClient
{
    enum YubicoResponseStatus
    {
        OK,
        BAD_OTP,
        REPLAYED_OTP,
        BAD_SIGNATURE,
        MISSING_PARAMETER,
        NO_SUCH_CLIENT,
        OPERATION_NOT_ALLOWED,
        BACKEND_ERROR,
        NOT_ENOUGH_ANSWERS,
        REPLAYED_REQUEST
    }
}
