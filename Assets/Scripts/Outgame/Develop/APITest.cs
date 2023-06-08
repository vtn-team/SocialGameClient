using MD;
using Outgame;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Application;

namespace Outgame
{
    /// <summary>
    /// 
    /// </summary>
    public class APITest : MonoBehaviour
    {
        [SerializeField] bool _isAutoLogin = true;
        [SerializeField, ReadOnly(true)] string _guid;
        [SerializeField, ReadOnly(true)] string _name;
        [SerializeField, ReadOnly(true)] int _userId;

        /*
        private void Start()
        {
            if (!_isAutoLogin) return;

            User user = User.Load();
            string guid = "";
            if (user != null)
            {
                guid = user.GUID;
            }
            _guid = guid;

            GameAPI.API.Login(guid, LoginCallback);
        }

        void LoginCallback(APIResponceLogin data)
        {
            //var cards = UserDataCache.Load<APIResponceGetCards>("Cards");
            //if (cards != null) return;

            _name = data.name;
            _userId = data.id;

            GameAPI.API.GetCards((cds) =>
            {
                //UserDataCache.Save("Cards", cds);
            });
        }

        public void Test(string test)
        {
            switch(test)
            {
                case "Gacha":
                    GameAPI.API.Gacha(1, 10, (data) =>
                    {
                        Debug.Log(data.cardIds);
                    });
                    break;
            }
        }
        */
    }
}
