using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YubicoDotNetClient
{
    public interface YubicoResponse
    {
        public String getH();
        public String getT();
        public YubicoResponseStatus getStatus();
        public int getTimestamp();
        public int getSessionCounter();
        public int getUseCounter();
        public String getSync();
        public String getOtp();
        public String getNonce();
    }
}
