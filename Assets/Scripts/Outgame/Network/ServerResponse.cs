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
    public class APIResponceLogin : APIResponceBase
    {
        public string session;

        public int id;
        public string udid;
        public string name;

        public string game_state;
    }

    [Serializable]
    public class APIResponceUserCreate : APIResponceBase
    {
        public string udid;
    }
}
