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
        //TODO: ログインAPIはいずれ分ける
        //const string LoginBaseURI = "https://rtpwg9bexj.execute-api.ap-northeast-1.amazonaws.com/default";
        const string VersionCheckURI = "https://f8eo8lpdw0.execute-api.ap-northeast-1.amazonaws.com/default/GameVersionCheck";
        const string apiKey = "ab4qX6DzCH8SlOMASgF8AapOdsTSg3i2t5oh89b9";
        const string BaseURI = "http://node.vtn-game.com";

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

        public void VersionCheck(APICallback<APIResponceBase> callback)
        {
            GetRequest(VersionCheckURI, (string json) =>
            {
                callback?.Invoke(GetPacketBody<APIResponceBase>(json));
            },
            new WebRequest.Options()
            {
                Header = new List<WebRequest.Header>()
                {
                    new WebRequest.Header(){ Name = "x-api-key", Value = apiKey }
                }
            });
        }

        public async Task<APIResponceLogin> Login(string uuid)
        {
            string request = string.Format("{0}/login", BaseURI);

            APIRequestLogin login = new APIRequestLogin();
            login.udid = uuid;

            string json = await PostRequest(request, login);
            var res = GetPacketBody<APIResponceLogin>(json);
            _session = res.session;
            _token = res.token;
            return res;
        }

        public async Task<APIResponceGetCards> GetCards()
        {
            string request = string.Format("{0}/ud/cards?session={1}", BaseURI, _session);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetCards>(json);
            return res;
        }

        public void Login(string uuid, APICallback<APIResponceLogin> callback)
        {
            string request = string.Format("{0}/login", BaseURI);

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
            string request = string.Format("{0}/ud/cards?session={1}", BaseURI, _session);

            GetRequest(request, (string data) =>
            {
                var res = GetPacketBody<APIResponceGetCards>(data);
                callback?.Invoke(res);
            });
        }

        public void CreateUser(string name, APICallback<APIResponceUserCreate> callback)
        {
            string request = string.Format("{0}/user/create", BaseURI);

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
            string request = string.Format("{0}/gacha/draw", BaseURI);

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