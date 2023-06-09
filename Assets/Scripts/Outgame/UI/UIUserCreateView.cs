using Assets.Scripts.Outgame.Game;
using Cysharp.Threading.Tasks;
using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Outgame
{
    public class UIUserCreateView : UIStackableView
    {
        [SerializeField] TMP_InputField _input;

        bool _isCreate = false;
        private void Start()
        {
            _input.Select();
        }


        public void UserCreate()
        {
            string name = _input.text;
            if (name == "") return;
            //if (_isCreate) return;

            _isCreate = true;

            //ログインあったら消す
            SequenceBridge.DeleteSequence("Login");
            //遷移しながらログインシーケンスを進める
            SequenceBridge.RegisterSequence("Login", SequencePackage.Create<LoginPackage>(UniTask.RunOnThreadPool(async () =>
            {
                //UniTask.Post(SequenceBridge.GetSequencePackage<LoginPackage>("Login"));
                var package = SequenceBridge.GetSequencePackage<LoginPackage>("Login");

                //ユーザの作成
                var usercreate = await GameAPI.API.CreateUser(name);

                //ログインAPI通す
                var login = await GameAPI.API.Login(usercreate.udid);
                if (login.udid == null)
                {
                    //ここに来るのはおかしい
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
    }
}
