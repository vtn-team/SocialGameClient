using MD;
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
    /// ランキング
    /// </summary>
    public class ListItemRankingInfo : ListItemBase
    {
        [SerializeField] TMPro.TextMeshProUGUI _userName;
        [SerializeField] TMPro.TextMeshProUGUI _rank;
        RankingInfo _info;

        public int UserId => _info.UserId;

        public void SetupRankingInfo(RankingInfo data)
        {
            _info = data;
            _userName.text = _info.UserName;
            _rank.text = _info.Rank.ToString();
        }

        public override void Bind(GameObject target)
        {
        }

        public override void Load()
        {
        }

        public override void Release()
        {
        }

        protected override void OnClick()
        {
            //TODO: フレンド申請とかできるといい
            //_evt?.Invoke((int)QuestListView.BoardType.Quest, _index);
        }

        public override void SetBudge(int bindex)
        {
        }
    }
}