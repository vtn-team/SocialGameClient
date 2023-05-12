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


        public void Save()
        {
            string id = GUID;
            LocalData.Save<string>("user", id, Application.persistentDataPath, true);
        }

        static public User Load()
        {
            string id = LocalData.Load<string>("user", Application.persistentDataPath, true);
            if (id == null) return null;
            if (id == "") return null;

            User user = new User();
            user.GUID = id;
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
