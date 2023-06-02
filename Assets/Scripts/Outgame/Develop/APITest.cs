using MD;
using Outgame;
using System;
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
        private void Start()
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
            //var cards = UserDataCache.Load<APIResponceGetCards>("Cards");
            //if (cards != null) return;

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
    }
}
