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
    /// クエストボードをリストで操作する
    /// </summary>
    public class ListItemQuestBoard : ListItemBase
    {
        [SerializeField] TMPro.TextMeshProUGUI _questTitle;
        [SerializeField] TMPro.TextMeshProUGUI _movePoint;
        QuestData _questInfo;

        public int QuestId => _questId;
        public QuestData Info => _questInfo;
        int _questId;
        MD.Quest _questData;

        public void SetupQuestData(int questId, QuestData data)
        {
            _questId = questId;
            _questData = MasterData.GetQuest(_questId);
            _questInfo = data;

            _questTitle.text = _questData.Name;
            _movePoint.text = _questData.MovePoint.ToString();
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
            _evt?.Invoke((int)QuestListView.BoardType.Quest, _questId);
        }

        public override void SetBudge(int bindex)
        {
        }
    }
}