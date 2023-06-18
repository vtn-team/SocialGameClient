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
    public class ListItemCardImage : ListItemBase
    {
        CardImage _target;

        public override void Bind(GameObject target)
        {
            _target = target.GetComponent<CardImage>();
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
    }
}
