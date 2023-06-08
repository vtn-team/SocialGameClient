using Cysharp.Threading.Tasks;
using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// ゲーム起動してから最初に呼ばれるスクリプト
    /// </summary>
    class GameExecuter : MonoBehaviour
    {
        void Start()
        {
            Execute();
        }

        async void Execute()
        {
            //バージョンチェックする
            var status = await StatusCheck.Check();

            //アプリバージョンチェックやメンテナンスに引っかかったらここで終了
            //TODO:
            ////

            //マスタデータの読み込みをする
            //マスタデータ側に情報をリレーして更新処理を実行する
            await MasterData.Instance.Setup(true, status.masterVersion);

            //リソース関連(Addressables)の更新をする

            //準備が終わったのでログイン準備
            UIManager.Setup(ViewID.Title);
        }
    }
}
