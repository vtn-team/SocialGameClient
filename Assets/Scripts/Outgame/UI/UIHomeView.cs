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
        [SerializeField] UIView _eventView;
        protected override void AwakeCall()
        {
            ViewId = ViewID.Home;
            _hasPopUI = true;
        }

        public override void Enter()
        {
            base.Enter();

            UIStatusBar.Show();

            //Debug.Log(EventHelper.GetAllOpenedEvent());
            if(EventHelper.IsEventOpen(1))
            {
                _eventView.Active();
            }

            Debug.Log(EventHelper.IsEventGamePlayable(1));
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

        public void GoQuest()
        {
            UIManager.NextView(ViewID.Quest);
        }

        public void OpenInformation()
        {
            UIManager.StackView(ViewID.Information);
        }

        public void GoEvent()
        {
            UIManager.NextView(ViewID.EventHome);
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
