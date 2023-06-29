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
    public class ItemData
    {
        public int ItemId;
        public ItemType Type;
        public int Count;
    }

    [Serializable]
    public class ItemList
    {
        public int Version;
        public List<ItemData> List;
    }

    /// <summary>
    /// アイテムリストのモデル
    /// NOTE: アイテムデータはログイン時にローカル情報で復元する。配布があるため、ログイン後や都度特定項目別にサーバから情報を貰い差分を埋める。今はやらない。
    /// </summary>
    public class ItemListModel : LocalCachedModel<ItemListModel, ItemList>
    {
        /// <summary>
        /// 書き換え可能/不能に対応したアクセサを用意する
        /// </summary>
        static public ItemList ItemList => _instance._data;

        protected override void Setup()
        {
            _dataName = "ItemList";
        }

        /// <summary>
        /// 同期呼び出し
        /// </summary>
        protected override ItemList load()
        {
            //ここの処理を期待する人はキャッシュされたデータを受け取りたい人
            if (HasData) return _data;

            //これも一応走らせる
            _data = LocalData.Load<ItemList>(_dataName, GameSetting.SavePath, true);
            if (!HasData)
            {
                //データがなかった場合はアサートにする
                Assert.IsNull(_data, "アイテムリストがありませんでした");
            }
            return _data;
        }

        /// <summary>
        /// 非同期呼び出し
        /// NOTE: モデルデータに通信させる仕様はよくある
        /// </summary>
        static public async UniTask<ItemList> LoadAsync() => await _instance.loadAsync();
        protected override async UniTask<ItemList> loadAsync()
        {
            if (HasData) return _data;

            //test
            //_data = await LocalData.LoadAsync<CardList>(_dataName, Application.persistentDataPath, true);
            //if (!HasData)
            {
                //データがなかった場合は通信して読み込む

                //データ取得
                var items = await GameAPI.API.GetItems();

                List<ItemData> itemDatas = new List<ItemData>();
                foreach (var item in items.items)
                {
                    var data = MasterData.GetItem(item.itemId);
                    itemDatas.Add(new ItemData() { ItemId = item.itemId, Type = data.Type, Count = item.count });
                }

                //データ格納
                _data = new ItemList();
                _data.Version = 0; //TODO:
                _data.List = itemDatas;

                Save();
            }
            return _data;
        }
    }
}
