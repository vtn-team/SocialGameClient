using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Outgame
{
    public class CardImage : UIView
    {
        [SerializeField] UnityEngine.UI.RawImage _image;
        [SerializeField] TMPro.TextMeshProUGUI _id;
        [SerializeField] TMPro.TextMeshProUGUI _load;
        [SerializeField] bool _isLazyLoad = false;

        public bool IsReady { get; set; }
        int _cardId;
        Card _cardData;
        CardData _cardInfo;
        Texture _tex;

        protected override void AwakeCall()
        {
            base.AwakeCall();

            IsReady = false;
            Disactive();
            _load.text = "F";
            _image.texture = null;
        }

        public void Setup(CardData card)
        {
            _cardId = card.CardId;
            _cardData = MasterData.GetCard(_cardId);
            _cardInfo = card;

            _id.text = card.Id.ToString();
        }

        public void Setup(int cardId)
        {
            _cardId = cardId;
            _cardData = MasterData.GetCard(cardId);

            _id.text = "-";
        }

        public void Load()
        {
            if (_cardData == null) return;

            _canvasGroup.alpha = 0;
            _load.text = "L";
            Addressables.LoadAssetAsync<Texture>(string.Format("Assets/DataAsset/Card/{0}.png", _cardData.Resource))
            .Completed += t =>
            {
                _load.text = "T";
                IsReady = true;
                _canvasGroup.alpha = 1;
                _tex = t.Result;
                if (IsActive)
                {
                    _image.texture = t.Result;
                }
                else
                {
                    ReleaseTexture();
                }
            };
        }

        public void ReleaseTexture()
        {
            if (!_tex) return;
            _image.texture = null;
            Addressables.Release(_tex);
            _load.text = "F";
            Disactive();
        }

        public void Detail()
        {

        }
    }
}
