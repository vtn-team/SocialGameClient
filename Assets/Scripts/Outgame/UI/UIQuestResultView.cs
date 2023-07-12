using Cysharp.Threading.Tasks;
using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Outgame
{
    public class UIQuestResultView : UIStackableView
    {
        int _questId = 0;

        protected override void AwakeCall()
        {
            ViewId = ViewID.QuestResult;
            _hasPopUI = false;

            CreateView();
        }

        void CreateView()
        {
            var package = SequenceBridge.GetSequencePackage<QuestPackage>("Quest");

            //
            //package.QuestResult

            /*
            foreach (var card in )
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
                    _cardImages[i].RectTransform.localPosition = new Vector3(-620 + (i - 5) * 300, 150, 0);
                }
            }
            */
        }

        public void GoHome()
        {
            UIManager.NextView(ViewID.Home);
        }
    }
}
