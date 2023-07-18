using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Outgame
{
    /// <summary>
    /// お知らせのモデル
    /// NOTE: お知らせは取得したものを一定時間キャッシュする
    /// </summary>
    public class InformationModel : LocalCachedModel<InformationModel, APIResponceGetInfomationList>
    {
        /// <summary>
        /// 書き換え可能/不能に対応したアクセサを用意する
        /// </summary>
        static public APIResponceGetInfomationList InfoList => _instance._data;

        Dictionary<string, APIResponceGetInfomation> _infoCache = new Dictionary<string, APIResponceGetInfomation>();


        protected override void Setup()
        {
            _dataName = "InfoList";
        }

        /// <summary>
        /// 同期呼び出し
        /// </summary>
        protected override APIResponceGetInfomationList load()
        {
            //ここの処理を期待する人はキャッシュされたデータを受け取りたい人
            if (HasData) return _data;
            return null;
        }

        /// <summary>
        /// 非同期呼び出し
        /// NOTE: モデルデータに通信させる仕様はよくある
        /// </summary>
        static public async UniTask<APIResponceGetInfomationList> LoadListAsync() => await _instance.loadAsync();
        protected override async UniTask<APIResponceGetInfomationList> loadAsync()
        {
            if (HasData) return _data;

            //通信して読み込む
            _data = await GameAPI.API.GetInformationList();

            return _data;
        }


        /// <summary>
        /// 非同期呼び出し
        /// NOTE: モデルデータに通信させる仕様はよくある
        /// </summary>
        static public async UniTask<APIResponceGetInfomation> LoadContentAsync(string id) => await _instance.loadContentAsync(id);
        protected async UniTask<APIResponceGetInfomation> loadContentAsync(string id)
        {
            if (_infoCache.ContainsKey(id)) return _infoCache[id];

            //通信して読み込む
            APIResponceGetInfomation ret = await GameAPI.API.GetInformation(id);
            _infoCache.Add(id, ret);

            return ret;
        }


    }
}
