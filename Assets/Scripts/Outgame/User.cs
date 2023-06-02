using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    public class User
    {
        public string GUID { get; private set; }

        [Serializable]
        class UDID
        {
            public string udid = "";
        }

        public void Save()
        {
            UDID udid = new UDID();
            udid.udid = GUID;
            LocalData.Save<UDID>("user", udid, Application.persistentDataPath, true);
        }

        static public User Load()
        {
            UDID udid = LocalData.Load<UDID>("user", Application.persistentDataPath, true);
            if (udid == null) return null;
            if (udid.udid == "") return null;

            User user = new User();
            user.GUID = udid.udid;
            return user;
        }

        static public async Task<User> LoadAsync()
        {
            UDID udid = await LocalData.LoadAsync<UDID>("user", Application.persistentDataPath, true);
            if (udid == null) return null;
            if (udid.udid == "") return null;

            User user = new User();
            user.GUID = udid.udid;
            return user;
        }

        static public User Create(string id)
        {
            User user = new User();
            user.GUID = id;
            user.Save();
            return user;
        }
    }
}
