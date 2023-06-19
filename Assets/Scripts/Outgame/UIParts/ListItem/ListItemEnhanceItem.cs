using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Outgame.ListView;

namespace Outgame
{
    /// <summary>
    /// カードアイテムをリストで操作する
    /// </summary>
    public class ListItemEnhanceItem : ListItemBase
    {
        ItemImage _target;
        ItemData _itemInfo;
        ListBadgeCount _listBadgeCount;
        int _useCount = 0;

        public override void Bind(GameObject target)
        {
            _target = target.GetComponent<ItemImage>();
            _itemInfo = _target.Info;

            _listBadgeCount = target.GetComponentInChildren<ListBadgeCount>();
        }

        public override void Load()
        {
            _target.Load();
        }

        public override void Release()
        {
            _target.Release();
        }

        protected override void OnClick()
        {
            _evt?.Invoke(0, _index);
        }

        public override void SetBudge(int bindex)
        {
            if (bindex != -1)
            {
                _useCount++;
                if (_useCount > _itemInfo?.Count) _useCount = _itemInfo.Count;
                _listBadgeCount?.SetCount(_useCount);
                _target.SetCount(_itemInfo.Count - _useCount);

                _badge?.Active();
            }
            else
            {
                _badge?.Disactive();
            }
        }
    }
}