using Cysharp.Threading.Tasks;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static INetworkImplement;
using static Network.WebRequest;

namespace Outgame
{
    public class NodeJSImplement : INetworkImplement
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

            _token = res.token;
            return res;
        }

        public async UniTask<APIResponceLogin> Login(string uuid)
        {
            string request = string.Format("{0}/login", GameSetting.LoginAPIURI);

            APIRequestLogin login = new APIRequestLogin();
            login.udid = uuid;

            string json = await PostRequest(request, login);
            var res = GetPacketBody<APIResponceLogin>(json);
            _session = res.session;
            _token = res.token;
            return res;
        }

        public async UniTask<APIResponceGetCards> GetCards()
        {
            string request = string.Format("{0}/ud/cards?session={1}", GameSetting.GameAPIURI, _session);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetCards>(json);
            return res;
        }

        public void Login(string uuid, APICallback<APIResponceLogin> callback)
        {
            string request = string.Format("{0}/login", GameSetting.LoginAPIURI);

            APIRequestLogin login = new APIRequestLogin();
            login.udid = uuid;

            PostRequest(request, login, (string json) =>
            {
                var res = GetPacketBody<APIResponceLogin>(json);
                _session = res.session;
                _token = res.token;
                callback?.Invoke(res);
            });
        }

        public void GetCards(APICallback<APIResponceGetCards> callback)
        {
            string request = string.Format("{0}/ud/cards?session={1}", GameSetting.GameAPIURI, _session);

            GetRequest(request, (string data) =>
            {
                var res = GetPacketBody<APIResponceGetCards>(data);
                callback?.Invoke(res);
            });
        }

        public void CreateUser(string name, APICallback<APIResponceUserCreate> callback)
        {
            string request = string.Format("{0}/user/create", GameSetting.GameAPIURI);

            APIRequestUserCreate user = new APIRequestUserCreate();
            user.name = name;
            user.session = _session;
            user.token = _token;

            PostRequest(request, user, (string data) =>
            {
                var res = GetPacketBody<APIResponceUserCreate>(data);
                callback?.Invoke(res);
            });
        }

        public void Gacha(int gachaId, int drawCount, APICallback<APIResponceGachaDraw> callback)
        {
            string request = string.Format("{0}/gacha/draw", GameSetting.GameAPIURI);

            APIRequestGachaDraw user = new APIRequestGachaDraw();
            user.gachaId = gachaId;
            user.drawCount = drawCount;
            user.session = _session;
            user.token = _token;

            PostRequest(request, user, (string data) =>
            {
                var res = GetPacketBody<APIResponceGachaDraw>(data);
                callback?.Invoke(res);
            });
        }
    }
}