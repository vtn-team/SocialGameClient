using Cysharp.Threading.Tasks;
using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Outgame
{
    public class UIEventHomeView : UIStackableView
    {
        [SerializeField] int _eventId;

        bool _isEventEnable = false;
        protected override void AwakeCall()
        {
            ViewId = ViewID.EventHome;
            _hasPopUI = true;
        }

        public override void Enter()
        {
            base.Enter();

            UniTask.RunOnThreadPool(async () =>
            {
                var stat = await RankingEventModel.EventStatAsync(_eventId);
                
                if(!stat.isOpen)
                {
                    UniTask.Post(() =>
                    {
                        EventIsNotOpen();
                    });
                    return;
                }

                _isEventEnable = true;
            }).Forget();
        }

        public void GoEventQuest()
        {
            if (!_isEventEnable) return;
            UIManager.NextView(ViewID.Quest);
        }

        public void GoBackHome()
        {
            if (!_isEventEnable) return;
            UIManager.Back();
        }

        public void GoEventRanking()
        {
            if (!_isEventEnable) return;

            SequenceBridge.RegisterSequence("EventRanking", SequencePackage.Create<QuestPackage>(UniTask.RunOnThreadPool(async () =>
            {
                await RankingEventModel.EventRankingAsync(_eventId, 1);

                //ランキングへ
                UniTask.Post(() => {
                    UIManager.NextView(ViewID.EventRanking);
                });
            })));
        }

        public void OpenInformation()
        {
            UIManager.StackView(ViewID.Information);
        }


        public void EventIsNotOpen()
        {
            UICommonDialog.OpenOKDialog("注意", "イベント開催期間ではありません", BackHome);
        }

        void BackHome(int type)
        {
            UniTask.Post(() =>
            {
                UIManager.Back();
            });
        }
    }
}
