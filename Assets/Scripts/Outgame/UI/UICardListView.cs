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
    public class UICardListView : UIStackableView
    {
        protected override void AwakeCall()
        {
            ViewId = ViewID.GachaEffect;
            _hasPopUI = true;
            Active();
        }

    }
}
