using Assets.Scripts.Outgame.Game;
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

            //遷移しながらログインシーケンスを進める
            SequenceBridge.RegisterSequence("Login", SequencePackage.Create<LoginPackage>(UniTask.RunOnThreadPool(async () =>
            {
                //UniTask.Post(SequenceBridge.GetSequencePackage<LoginPackage>("Login"));
                var package = SequenceBridge.GetSequencePackage<LoginPackage>("Login");

                //ログインAPI
                var login = await GameAPI.API.Login(guid);
                if (login.udid == null)
                {
                    UniTask.Post(NewUser);
                    return;
                }

                UniTask.Post(GoHome);

                //各種データ取得
                var cards = await GameAPI.API.GetCards();

                //データ格納
                package.Login = login;
                package.Cards = cards;

                //ホームとかで消してもらうか、そのままにしておく
                package.IsReady = true;
            })));
        }

        public void GoHome()
        {
            UIManager.NextView(ViewID.Home);
        }

        public void NewUser()
        {
            UIManager.NextView(ViewID.NewUser);
        }
    }
}
