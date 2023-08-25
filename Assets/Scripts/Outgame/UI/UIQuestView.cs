using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Outgame
{
    public class UIQuestInformation : UIInformationBase
    {
        public bool FromEvent;
        public int Chapter;
    }

    public class UIQuestView : UIStackableView
    {
        [SerializeField] QuestListView _listView;
        UIQuestInformation _qInfo = null;

        protected override void AwakeCall()
        {
            ViewId = ViewID.Quest;
            _hasPopUI = false;
        }
        public override void SetupFromInfo()
        {
            _qInfo = _info as UIQuestInformation;
        }

        private async void Start()
        {
            await QuestListModel.LoadAsync();

            _listView.SetQuestInformation(_qInfo);
            _listView.Setup();
            _listView.SetReadyCallback(Ready);
            Active();
        }

        void Ready(int questId)
        {
            SequenceBridge.RegisterSequence("Quest", SequencePackage.Create<QuestPackage>(UniTask.RunOnThreadPool(async () =>
            {
                var start = await GameAPI.API.QuestStart(questId);
                //本来はインゲームに行く
                //成功ってことにする
                var result = await GameAPI.API.QuestResult(1);

                //アイテム付与


                //パッケージ
                var package = SequenceBridge.GetSequencePackage<QuestPackage>("Quest");
                package.QuestResult = result;
                package.IsEventQuest = questId >= 100; // TODO:もっといいやり方がある

                //リザルトへ
                UniTask.Post(GoResult);
            })));
        }

        void GoResult()
        {
            UIManager.NextView(ViewID.QuestResult);
        }

        public void Back()
        {
            UIManager.Back();
        }
    }
}
