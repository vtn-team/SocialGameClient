using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    public class UserDataCache
    {


        static public void Save<T>(string key, T data)
        {
            LocalData.Save<T>(key, data, Application.dataPath, true);
        }

        static public T Load<T>(string key)
        {
            return LocalData.Load<T>(key, Application.dataPath, true);
        }
    }
}
