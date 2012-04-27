using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YubicoDotNetClient
{
    public interface YubicoResponse
    {
        String getH();
        String getT();
        YubicoResponseStatus getStatus();
        int getTimestamp();
        int getSessionCounter();
        int getUseCounter();
        String getSync();
        String getOtp();
        String getNonce();
        SortedDictionary<String, String> getResponseMap();
    }
}
