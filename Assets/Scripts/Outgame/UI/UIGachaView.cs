using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// ガチャ管理ビュー
    /// </summary>
    public class UIGachaView : UIStackableView
    {
        //ガチャ一覧
        List<UIGachaDisplayView> _gachaList = new List<UIGachaDisplayView>();

        protected override void AwakeCall()
        {
            ViewId = ViewID.Gacha;
            _hasPopUI = true;

            SetupGacha();
        }

        void SetupGacha()
        {
            //常設だけとりあえず出す
            LoadGachaView("NormalGacha");
            _gachaList[0].Active();

            //TODO: イベガチャあったら読んで設定する

        }

        /// <summary>
        /// シーン読みこむ
        /// </summary>
        /// <param name="gachaName"></param>
        void LoadGachaView(string gachaName)
        {
            GameObject sceneOrigin = Addressables.LoadAssetAsync<GameObject>(string.Format("Assets/Scenes/Data/Gacha/{0}.prefab", gachaName)).WaitForCompletion();
            if (sceneOrigin == default)
            {
                Debug.LogError($"{gachaName}: ガチャビューの読み込みに失敗");
                return;
            }

            var scene = GameObject.Instantiate(sceneOrigin, this.RectTransform);
            var view = scene.GetComponent<UIGachaDisplayView>();
            if (view == default)
            {
                Debug.LogError($"{gachaName}: ガチャ管理スクリプトの読み込みに失敗");
                return;
            }

            _gachaList.Add(view);

            Addressables.Release(sceneOrigin);
        }
    }
}
