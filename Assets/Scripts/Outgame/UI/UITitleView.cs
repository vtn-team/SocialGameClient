using Cysharp.Threading.Tasks;
using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
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
            //遷移しながらログインシーケンスを進める
            SequenceBridge.RegisterSequence("Login", SequencePackage.Create<LoginPackage>(UniTask.RunOnThreadPool(async () =>
            {
                //UniTask.Post(SequenceBridge.GetSequencePackage<LoginPackage>("Login"));
                var package = SequenceBridge.GetSequencePackage<LoginPackage>("Login");

                //ユーザ情報拾う
                UserModel.Load();

                //Firebase
                FirebaseAnalyticsLogger.Setup(UserModel.GUID);

                //ログインAPI
                //NOTE: sessionとtokenを受け取るためにユーザがいなくてもloginを通す
                var login = await GameAPI.API.Login(UserModel.GUID);
                if (string.IsNullOrEmpty(login.udid))
                {
                    UniTask.Post(NewUser);
                    return;
                }

                package.Login = login;
                UserModel.UpdatePlayerInfo(login);

                UniTask.Post(GoHome);

                //各種データ取得
                await CardListModel.LoadAsync();
                await ItemListModel.LoadAsync();

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
