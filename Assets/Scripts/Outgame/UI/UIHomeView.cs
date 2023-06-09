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
            Debug.Log("今度やりまーす");
        }
    }
}
