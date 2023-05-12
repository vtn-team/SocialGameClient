using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Outgame
{
    /// <summary>
    /// 
    /// </summary>
    public class Title : MonoBehaviour
    {
        public void Login()
        {
            User user = User.Load();
            string guid = "";
            if (user != null)
            {
                guid = user.GUID;
            }
            GameAPI.API.Login(guid, LoginCallback);
        }

        void LoginCallback(APIResponceLogin data)
        {
            if (data.udid == null)
            {
                SceneManager.LoadScene("NewUser");
            }
        }
    }
}