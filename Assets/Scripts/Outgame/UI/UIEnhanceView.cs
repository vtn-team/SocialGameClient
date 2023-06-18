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
    public class UIEnhanceView : UIStackableView
    {
        [SerializeField] EnhanceListView _listView;

        protected override void AwakeCall()
        {
            ViewId = ViewID.Enhance;
            _hasPopUI = true;
        }

        private void Start()
        {
            _listView.Setup();
            Active();
        }

        public void DoEnhance()
        {
            //TODO:
        }

        public void Back()
        {
            UIManager.Back();
        }
    }
}
