using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    public class DataCache
    {
        static DataCache _instance = new DataCache();
        public static DataCache Instance { get { return _instance; } }
        private DataCache() { }


        static public User GetUserData() {
            if(_instance._user == null)
            {
                _instance._user = User.Load();
            }
            return _instance._user;
        }

        User _user = null;
        
        public string Session { get; set; }
        public string Token { get; set; }
    }
}
