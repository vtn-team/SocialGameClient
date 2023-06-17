using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Outgame
{
    /// <summary>
    /// NOTE: 現状だとサーバと同じデータ配列で無駄に思えるかもしれないが、リレーションを含むデータを持つ場合はそれを含めてキャッシュする方が効率が良い
    /// 　　　※現状リレーションは無い
    /// </summary>
    [Serializable]
    public class CardData
    {
        public int Id;
        public int CardId;
        public int Level;
        public int Luck;
    }

    [Serializable]
    public class CardList
    {
        public int Version;
        public List<CardData> List;
    }

    /// <summary>
    /// カードリストのモデル
    /// NOTE: カードデータはログイン時にローカル情報で復元し、バージョン差分があればサーバデータで更新する
    /// NOTE: 非同期と同期でデータのアクセス方法が異なる。キャッシュ済みデータを受け取る前提の場合は同期メソッドで呼ぶ方が良い。
    /// </summary>
    public class CardListModel : LocalCachedModel<CardListModel, CardList>
    {
        /// <summary>
        /// 書き換え可能/不能に対応したアクセサを用意する
        /// </summary>
        static public CardList CardList => _instance._data;

        protected override void Setup()
        {
            _dataName = "CardList";
        }

        /// <summary>
        /// 同期呼び出し
        /// </summary>
        protected override CardList load()
        {
            //ここの処理を期待する人はキャッシュされたデータを受け取りたい人
            if (HasData) return _data;

            //これも一応走らせる
            _data = LocalData.Load<CardList>(_dataName, GameSetting.SavePath, true);
            if (!HasData)
            {
                //データがなかった場合はアサートにする
                Assert.IsNull(_data, "カードリストがありませんでした");
            }
            return _data;
        }

        /// <summary>
        /// 非同期呼び出し
        /// NOTE: モデルデータに通信させる仕様はよくある
        /// </summary>
        static public async UniTask<CardList> LoadAsync() => await _instance.loadAsync();
        protected async UniTask<CardList> loadAsync()
        {
            if (HasData) return _data;

            //test
            //_data = await LocalData.LoadAsync<CardList>(_dataName, Application.persistentDataPath, true);
            //if (!HasData)
            {
                //データがなかった場合は通信して読み込む

                //データ取得
                var cards = await GameAPI.API.GetCards();
                List<CardData> cardDatas = new List<CardData>();
                foreach(var card in cards.cards)
                {
                    cardDatas.Add(new CardData(){ Id = card.id, CardId = card.cardId, Level = card.level, Luck = card.luck });
                }

                //データ格納
                _data = new CardList();
                _data.Version = 0; //TODO:
                _data.List = cardDatas;

                Save();
            }
            return _data;
        }

        void appendCard(CardData card)
        {
            _data.List.Add(card);
        }

        /// <summary>
        /// データを追加する
        /// </summary>
        static public void AppendCard(CardData card) => _instance.appendCard(card);
    }
}
