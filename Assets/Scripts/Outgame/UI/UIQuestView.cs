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
    public class UIQuestView : UIStackableView
    {
        [SerializeField] QuestListView _listView;

        protected override void AwakeCall()
        {
            ViewId = ViewID.Quest;
            _hasPopUI = false;
        }

        private async void Start()
        {
            await QuestListModel.LoadAsync();

            _listView.Setup();
            _listView.SetReadyCallback(Ready);
            Active();
        }

        void Ready(int questId)
        {
            SequenceBridge.RegisterSequence("QuestStart", SequencePackage.Create<QuestStartPackage>(UniTask.RunOnThreadPool(async () =>
            {
                var result = await GameAPI.API.QuestStart(questId);
            })));
        }

        public void Back()
        {
            UIManager.Back();
        }
    }
}
