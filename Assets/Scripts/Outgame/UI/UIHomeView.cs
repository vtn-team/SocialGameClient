using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Outgame
{
    public class UIHomeView : UIStackableView
    {
        protected override void AwakeCall()
        {
            ViewId = ViewID.Home;
            _hasPopUI = true;
        }

        public void GoGacha()
        {
            UIManager.NextView(ViewID.Gacha);
        }

        public void GoCardList()
        {
            UIManager.NextView(ViewID.CardList);
        }

        public void GoEnhance()
        {
            UIManager.NextView(ViewID.Enhance);
        }


        public void DialogTest()
        {
            UICommonDialog.OpenOKDialog("テスト", "テストダイアログですよ", Test);
        }

        void Test(int type)
        {
            Debug.Log("here");
        }
    }
}
