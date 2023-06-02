using MD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            //同期的にやる。Taskでやるとデッドロックする。
            //ユーザ情報があったら拾う
            var user = User.Load();
            string guid = "";
            if (user != null)
            {
                guid = user.GUID;
            }

            //ユーザが無かったら作成する
            if (guid == null)
            {
                SceneManager.LoadScene("NewUser");
                return;
            }

            //遷移しながらログインシーケンスを進める
            NetworkSequence.RegisterSequence("Login", Task.Run(async () => 
            {
                //ログインAPI
                var login = await GameAPI.API.Login(guid);

                //各種データ取得
                var cards = await GameAPI.API.GetCards();
                return;
            }));

            SceneManager.LoadScene("Field");
        }

        void NewUser()
        {
            SceneManager.LoadScene("NewUser");
        }
        void InGame()
        {
            SceneManager.LoadScene("Field");
        }

        /*
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
        */
    }
}