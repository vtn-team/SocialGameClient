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

        GameObject _questStack;
        List<ListItemQuestBoard> _questBoardList = new List<ListItemQuestBoard>();

        public bool HasQuestStack => _questStack != null;
        public Chapter Info => _chapterData;
        int _chapterId;
        Chapter _chapterData;


        public void SetupChapterData(Chapter data)
        {
            _chapterId = data.Id;
            _chapterData = MasterData.GetChapter(_chapterId);

            Debug.Log(_chapterTitle);
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

        public void SetQuestStack(GameObject qs)
        {
            _questStack = qs;
            _questStack.SetActive(false);
        }

        protected override void OnClick()
        {
            _evt?.Invoke(0, _index);

            //子オブジェクトの展開をする
            _questStack.SetActive(!_questStack.activeSelf);
        }

        void OnItemClick(int evtId, int index)
        {

        }
    }
}
