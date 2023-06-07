using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    [Serializable]
    public class APIRequestBase
    {
        public string session;
        public string token;
    }

    
    [Serializable]
    public class APIRequestLogin : APIRequestBase
    {
        public string udid;
    }

    [Serializable]
    public class APIRequestUserCreate : APIRequestBase
    {
        public string name;
    }

    [Serializable]
    public class APIRequestGachaDraw : APIRequestBase
    {
        public int gachaId;
        public int drawCount;
    }
}
