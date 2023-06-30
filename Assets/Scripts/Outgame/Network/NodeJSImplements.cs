using Cysharp.Threading.Tasks;
using Network;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static IGameAPIImplement;
using static Network.WebRequest;

namespace Outgame
{
    public partial class NodeJSImplement : IGameAPIImplement
    {
        private string _token = "";
        private string _session = "";

        T GetPacketBody<T>(string json) where T : APIResponceBase
        {
            Debug.Log(json);
            T res = JsonUtility.FromJson<T>(json);

            //TODO: 特定のコードを受け取った場合、セッションを更新するためのリクエストを送信する
            if(res.status != 200)
            {
                return null;
            }

            if (res.token != null && res.token != "")
            {
                _token = res.token;
            }
            return res;
        }

        T CreateRequest<T>() where T : APIRequestBase, new()
        {
            T request = new T();
            request.session = _session;
            request.token = _token;
            return request;
        }

        public async UniTask<APIResponceGetCards> GetCards()
        {
            string request = string.Format("{0}/ud/cards?session={1}", GameSetting.GameAPIURI, _session);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetCards>(json);
            return res;
        }

        public async UniTask<APIResponceGetItems> GetItems()
        {
            string request = string.Format("{0}/ud/items?session={1}", GameSetting.GameAPIURI, _session);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetItems>(json);
            return res;
        }

        public async UniTask<APIResponceGetQuests> GetQuests()
        {
            string request = string.Format("{0}/ud/quests?session={1}", GameSetting.GameAPIURI, _session);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetQuests>(json);
            return res;
        }
    }
}