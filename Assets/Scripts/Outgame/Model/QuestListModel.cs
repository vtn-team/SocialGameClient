using Cysharp.Threading.Tasks;
using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Outgame
{
    [Serializable]
    public class QuestData
    {
        public int QuestId;
        public int ClearFlag;
        public int Score;
    }

    [Serializable]
    public class QuestList
    {
        public int Version;
        public List<QuestData> List;
    }

    /// <summary>
    /// アイテムリストのモデル
    /// NOTE: アイテムデータはログイン時にローカル情報で復元する。配布があるため、ログイン後や都度特定項目別にサーバから情報を貰い差分を埋める。今はやらない。
    /// </summary>
    public class QuestListModel : LocalCachedModel<QuestListModel, QuestList>
    {
        /// <summary>
        /// 書き換え可能/不能に対応したアクセサを用意する
        /// </summary>
        static public QuestList QuestList => _instance._data;

        protected override void Setup()
        {
            _dataName = "QuestList";
        }

        /// <summary>
        /// 非同期呼び出し
        /// NOTE: モデルデータに通信させる仕様はよくある
        /// </summary>
        static public async UniTask<QuestList> LoadAsync() => await _instance.loadAsync();
        protected override async UniTask<QuestList> loadAsync()
        {
            if (HasData) return _data;

            //test
            //_data = await LocalData.LoadAsync<CardList>(_dataName, Application.persistentDataPath, true);
            //if (!HasData)
            {
                //データがなかった場合は通信して読み込む

                //データ取得
                var quests = await GameAPI.API.GetQuests();

                List<QuestData> questDatas = new List<QuestData>();
                foreach (var item in quests.quests)
                {
                    questDatas.Add(new QuestData() { QuestId = item.questId, ClearFlag = item.clearFlag, Score = item.score });
                }

                //データ格納
                _data = new QuestList();
                _data.Version = 0; //TODO:
                _data.List = questDatas;

                Save();
            }
            return _data;
        }
    }
}
