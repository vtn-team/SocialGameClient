using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.UIElements;

namespace Outgame
{
    /// <summary>
    /// ガチャ管理ビュー
    /// </summary>
    public class UICardListView : UIStackableView
    {
        [SerializeField] ListView _listView;

        protected override void AwakeCall()
        {
            ViewId = ViewID.CardList;
            _hasPopUI = true;
        }

        private void Start()
        {
            _listView.Setup();
            Active();
        }

        public void Back()
        {
            UIManager.Back();
        }
    }
}
