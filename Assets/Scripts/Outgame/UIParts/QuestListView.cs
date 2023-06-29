using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// クエストリストを表示するビュー
    /// </summary>
    public class QuestListView : ListView
    {
        public delegate void Ready(int questId);
        [SerializeField] GameObject _chapterPrefab;
        [SerializeField] GameObject _questStackPrefab;
        [SerializeField] GameObject _questPrefab;

        List<ListItemChapterBoard> _chapterBoardList = new List<ListItemChapterBoard>();
        Ready _callback;

        /// <summary>
        /// ビューを作る
        /// </summary>
        public override void Setup()
        {
            _lineList.ForEach(l => GameObject.Destroy(l));
            _itemList.Clear();
            _scrollPos = 0;


            /*
            var chapters = MasterData.Chapters;
            //クエスト
            for (int i = 0; i < chapters.Count; ++i)
            {
                var chapter = GameObject.Instantiate(_chapterPrefab, _content.RectTransform);
                var listItem = ListItemBase.ListItemSetup<ListItemChapterBoard>(i, chapter, (int evtId, int index) => OnItemClick(evtId, index));
                listItem.SetupChapterData(chapters[i]);

                _chapterBoardList.Add(listItem);
                _itemList.Add(listItem);
                _lineList.Add(listItem.gameObject);
            }
            */

            var questList = QuestListModel.QuestList.List;
            var quests = MasterData.Quests;
            for (int i = 0; i < quests.Count; ++i)
            {
                var quest = GameObject.Instantiate(_questPrefab, _content.RectTransform);
                var listItem = ListItemBase.ListItemSetup<ListItemQuestBoard>(i, quest, (int evtId, int index) => OnItemClick(evtId, index));
                listItem.SetupQuestData(quests[i].Id, questList.Where(q => q.QuestId == quests[i].Id).FirstOrDefault());

                _itemList.Add(listItem);
                _lineList.Add(listItem.gameObject);
            }

            //サイズ計算して最大スクロール値を決める
            _content.RectTransform.sizeDelta = new Vector2(800, (_itemList.Count + 1) * CardUIHeight);

            //イベント登録
            _rect.onValueChanged.AddListener(ScrollUpdate);
        }

        public void SetReadyCallback(Ready cb)
        {
            _callback = cb;
        }

        public void CreateQuestStack(int index)
        {
            if (_chapterBoardList[index].HasQuestStack) return;

            GameObject questStack = GameObject.Instantiate(_questStackPrefab, _lineList[index].transform);

            var chapters = MasterData.Chapters;
            var questList = QuestListModel.QuestList.List;

            //クエスト
            Debug.Log(MasterData.Quests.Where(q => q.ChapterId == chapters[index].Id).Count());
            for (int i = 0; i < chapters[index].QuestList.Count; ++i)
            {
                var chapter = GameObject.Instantiate(_questPrefab, questStack.transform);
                var listItem = ListItemBase.ListItemSetup<ListItemQuestBoard>(i, chapter, (int evtId, int index) => OnItemClick(evtId, index));
                listItem.SetupQuestData(chapters[index].QuestList[i].Id, questList.Where(q => q.QuestId == chapters[index].QuestList[i].Id).FirstOrDefault());
            }

            _chapterBoardList[index].SetQuestStack(questStack);
        }

        int _selectedIndex = -1;
        protected override void OnItemClick(int evtId, int index)
        {
            //出撃確認
            _selectedIndex = index;
            UICommonDialog.OpenYesNoDialog("出撃します", "よかったらOK", DialogDecide, "UIGoQuest", "UINoQuest");
        }

        void DialogDecide(int type)
        {
            var quests = MasterData.Quests;
            if (type == 1)
            {
                _callback?.Invoke(quests[_selectedIndex].Id);
            }
        }
    }
}
