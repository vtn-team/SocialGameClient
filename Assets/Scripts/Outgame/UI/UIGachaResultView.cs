using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

namespace Outgame
{
    /// <summary>
    /// ガチャ管理ビュー
    /// </summary>
    public class UIGachaResultView : UIStackableView
    {
        [SerializeField] GameObject _card;

        List<CardImage> _cardImages = new List<CardImage>();

        protected override void AwakeCall()
        {
            ViewId = ViewID.GachaEffect;
            _hasPopUI = true;

            //一応データ待ち
            UniTask.RunOnThreadPool(async () => {
                var gacha = await SequenceBridge.GetSequencePackageWaitForReady<GachaDrawPackage>("GachaDraw");
                CreateView(gacha);
                await UniTask.WaitUntil(() => _cardImages.All(img => img.IsReady));
                //ここでフェード開ける
                SequenceBridge.DeleteSequence("GachaDraw");
            }).Forget();
        }

        void CreateView(GachaDrawPackage gacha)
        {
            foreach (var card in gacha.Gacha.cards)
            {
                Debug.Log(card);
                var cardObj = GameObject.Instantiate(_card, this.RectTransform);
                var image = cardObj.GetComponent<CardImage>();
                image.Setup(card.cardId);
                image.Load();
                _cardImages.Add(image);
            }

            //ザル
            if (_cardImages.Count() == 10)
            {
                for (int i = 0; i < 5; ++i)
                {
                    _cardImages[i].RectTransform.localPosition = new Vector3(-620 + i * 300, -150, 0);
                }
                for (int i = 5; i < 10; ++i)
                {
                    _cardImages[i].RectTransform.localPosition = new Vector3(-620 + (i-5) * 300, 150, 0);
                }
            }
        }

        public void GoHome()
        {
            UIManager.NextView(ViewID.Home);
        }
    }
}
