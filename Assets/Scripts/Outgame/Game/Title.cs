using MD;
using System;
using System.Collections;
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
        public void Start()
        {
            MasterData.Instance.Setup(() => { }, true);
        }

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
            else
            {
                StartCoroutine("LoginSequence");
            }
        }

        IEnumerator LoginSequence()
        {
            if (!MasterData.Instance.IsSetupComplete) yield return null;

            //TODO:


            SceneManager.LoadScene("Field");
        }
    }
}