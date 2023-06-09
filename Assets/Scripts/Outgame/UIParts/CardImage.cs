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
        [SerializeField] int debugCardId = -1;

        private void Start()
        {
            if (debugCardId != -1) LoadTexture(debugCardId);
        }

        public void LoadTexture(int cardId)
        {
            var data = MasterData.GetCard(cardId);
            Addressables.LoadAssetAsync<Texture>(string.Format("Assets/DataAsset/Card/{0}.png", data.Resource))
            .Completed += t =>
            {
                _image.texture = t.Result;
            };
        }
    }
}
