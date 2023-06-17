using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    [Serializable]
    public class APIRequestEnhanceItem
    {
        public int itemId;
        public int itemCount;
    }

    [Serializable]
    public class APIRequestEnhanceMaterials
    {
        public int[] cardIds;
        public APIRequestEnhanceItem[] items;
    }

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

    [Serializable]
    public class APIRequestEnhance : APIRequestBase
    {
        public int baseId;
        public APIRequestEnhanceMaterials materials;
    }
}
