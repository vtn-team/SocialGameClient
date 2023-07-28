using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Outgame
{
    public class UIEventRankingView : UIStackableView
    {
        [SerializeField] TMPro.TextMeshProUGUI _rank;
        [SerializeField] EventRankingListView _listView;

        protected override void AwakeCall()
        {
            ViewId = ViewID.EventRanking;
            _hasPopUI = true;
        }

        public override void Enter()
        {
            base.Enter();

            _rank.text = RankingEventModel.Rank.ToString();
            _listView.Setup();
        }

        public void GoBack()
        {
            UIManager.Back();
        }
    }
}
