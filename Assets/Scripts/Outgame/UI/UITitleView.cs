using Cysharp.Threading.Tasks;
using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Outgame
{
    public class UITitleView : UIStackableView
    {
        protected override void AwakeCall()
        {
            ViewId = ViewID.Title;
            _hasPopUI = false;
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
            NetworkSequence.RegisterSequence("Login", UniTask.RunOnThreadPool(async () =>
            {
                //ログインAPI
                var login = await GameAPI.API.Login(guid);
                if (login.udid == null)
                {
                    UniTask.Post(NewUser);
                    return;
                }

                //各種データ取得
                var cards = await GameAPI.API.GetCards();
                return;
            }));

            UIManager.NextView(ViewID.Home);
            //SceneManager.LoadScene("Home");
        }

        public void NewUser()
        {
            UIManager.NextView(ViewID.NewUser);
        }
    }
}
