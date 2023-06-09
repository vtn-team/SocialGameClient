using Assets.Scripts.Outgame.Game;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Outgame
{
    public class UIGachaDisplayView : UIView
    {
        [SerializeField] int _gachaId;
        [SerializeField] string _gachaName;

        GameObject _sceneOrigin = null;
        bool _isRun = false;

        private void Draw(int drawNum)
        {
            //遷移しながらログインシーケンスを進める
            SequenceBridge.RegisterSequence("GachaDraw", SequencePackage.Create<GachaDrawPackage>(UniTask.RunOnThreadPool(async () =>
            {
                int dnum = drawNum;
                var package = SequenceBridge.GetSequencePackage<GachaDrawPackage>("GachaDraw");

                //ガチャ
                var gacha = await GameAPI.API.Gacha(_gachaId, dnum);
                if (gacha.cardIds == null)
                {
                    return;
                }

                //ローカルのカードリストに追加

                //
                package.Gacha = gacha;
                package.IsReady = true;
            })));
        }

        public void DrawSingle()
        {
            if (_isRun) return;
            Draw(1);

            //シーンの読み込み待ち
            UniTask.RunOnThreadPool(async () =>
            {
                await UniTask.WaitUntil(() => _sceneOrigin != null);
                UniTask.Post(GoEffectView);
            }).Forget();
            _isRun = true;
        }

        public void DrawMulti()
        {
            if (_isRun) return;
            Draw(10);

            //シーンの読み込み待ち
            UniTask.RunOnThreadPool(async () =>
            {
                await UniTask.WaitUntil(() => _sceneOrigin != null);
                UniTask.Post(GoEffectView);
            }).Forget();
            _isRun = true;
        }


        void GoEffectView()
        {
            UIManager.NextView(_sceneOrigin);
            Addressables.Release(_sceneOrigin);
        }

        protected override void AwakeCall()
        {
            LoadGachaEffectView();
        }

        /// <summary>
        /// シーン読みこむ
        /// </summary>
        /// <param name="gachaName"></param>
        void LoadGachaEffectView()
        {
            _sceneOrigin = Addressables.LoadAssetAsync<GameObject>(string.Format("Assets/Scenes/Data/Gacha/{0}Effect.prefab", _gachaName)).WaitForCompletion();
            if (_sceneOrigin == default)
            {
                Debug.LogError($"{_gachaName}: ガチャ演出の読み込みに失敗");
                return;
            }
        }
    }
}
