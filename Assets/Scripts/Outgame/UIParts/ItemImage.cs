using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEditor.Progress;

namespace Outgame
{
    public class ItemImage : UIView
    {
        //[SerializeField] UnityEngine.UI.RawImage _image;
        //[SerializeField] TMPro.TextMeshProUGUI _id;
        [SerializeField] TMPro.TextMeshProUGUI _name;
        [SerializeField] TMPro.TextMeshProUGUI _count;

        public bool IsReady { get; set; }
        public ItemData Info => _itemInfo;

        int _itemId;
        int _index;
        MD.Item _itemData;
        ItemData _itemInfo;
        //Texture _tex;

        protected override void AwakeCall()
        {
            base.AwakeCall();

            IsReady = false;
            Disactive();
            //_image.texture = null;
        }

        void SetItemData()
        {
            //_id.text = _itemId.ToString();
            _name.text = MasterData.GetLocalizedText(_itemData.Name);
            _count.text = _itemInfo?.Count.ToString();
        }

        public void Setup(ItemData item)
        {
            _itemId = item.ItemId;
            _itemData = MasterData.GetItem(_itemId);
            _itemInfo = item;

            SetItemData();
        }

        public void Setup(int itemId)
        {
            _itemId = itemId;
            _itemData = MasterData.GetItem(_itemId);

            SetItemData();
        }

        public void SetCount(int count)
        {
            _count.text = count.ToString();
        }

        public void Load()
        {
            if (_itemData == null) return;

            Active();
            //Addressables.LoadAssetAsync<Texture>(string.Format("Assets/DataAsset/Card/{0}.png", _cardData.Resource))
            //.Completed += t =>{}

            IsReady = true;
        }

        public void Release()
        {
            //if (!_tex) return;
            //_image.texture = null;
            //Addressables.Release(_tex);
            Disactive();
        }
    }
}
