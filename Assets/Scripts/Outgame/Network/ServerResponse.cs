using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    [Serializable]
    public class APIResponceBase
    {
        public int status;
        public string token;
    }

    [Serializable]
    public class APIResponceCard
    {
        public int id;
        public int cardId;
        public int level;
        public int luck;
    }

    [Serializable]
    public class APIResponceLogin : APIResponceBase
    {
        public string session;

        public int id;
        public string udid;
        public string name;

        public string game_state;
    }

    [Serializable]
    public class APIResponceGetCards : APIResponceBase
    {
        public APIResponceCard[] cards;
    }

    [Serializable]
    public class APIResponceUserCreate : APIResponceBase
    {
        public string udid;
    }

    [Serializable]
    public class APIResponceGachaDraw : APIResponceBase
    {
        public APIResponceCard[] cards;
    }

    [Serializable]
    public class APIResponceEnhance : APIResponceBase
    {
        public int _success;
        public int _level;
        public int _luck;
    }
    
}
