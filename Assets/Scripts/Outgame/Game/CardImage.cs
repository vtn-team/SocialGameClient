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
        [SerializeField] string _debugStr = "ampere";

        private void Start()
        {
            Addressables.LoadAssetAsync<Texture>(string.Format("Assets/DataAsset/Card/{0}.png", _debugStr))
            .Completed += t =>
            {
                _image.texture = t.Result;
            };
        }
    }
}
