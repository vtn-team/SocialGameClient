using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// チャプター表示用のボード
    /// NOTE: 移譲すべきオブジェクトは無いので、このクラスで表示管理する
    /// </summary>
    internal class ListItemChapterBoard : ListItemBase
    {
        [SerializeField] TMPro.TextMeshProUGUI _chapterTitle;

        public Chapter Info => _chapterData;
        int _chapterId;
        Chapter _chapterData;


        public void SetupChapterData(Chapter data)
        {
            _chapterId = data.Id;
            _chapterData = MasterData.GetChapter(_chapterId);

            _chapterTitle.text = _chapterData.Name;
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
            _evt?.Invoke((int)QuestListView.BoardType.Chapter, _index);
        }

        void OnItemClick(int evtId, int index)
        {

        }
    }
}
