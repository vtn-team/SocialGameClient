using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static INetworkImplement;

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

        T GetPacketBody<T>(byte[] data) where T : APIResponceBase
        {
            string json = Encoding.UTF8.GetString(data);
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
            Network.WebRequest.GetRequest(VersionCheckURI, (byte[] data) =>
            {
                callback?.Invoke(GetPacketBody<APIResponceBase>(data));
            },
            new HTTPRequest.Options()
            {
                Header = new List<HTTPRequest.Header>()
                {
                    new HTTPRequest.Header(){ Name = "x-api-key", Value = apiKey }
                }
            });
        }

        public void Login(string uuid, APICallback<APIResponceLogin> callback)
        {
            string request = string.Format("{0}/login", BaseURI);

            APIRequestLogin login = new APIRequestLogin();
            login.udid = uuid;

            Network.WebRequest.PostRequest(request, login, (byte[] data) =>
            {
                var res = GetPacketBody<APIResponceLogin>(data);
                callback?.Invoke(res);
                _session = res.session;
                _token = res.token;
            });
        }

        public void CreateUser(string name, APICallback<APIResponceUserCreate> callback)
        {
            string request = string.Format("{0}/user/create", BaseURI);

            APIRequestUserCreate user = new APIRequestUserCreate();
            user.name = name;
            user.session = _session;
            user.token = _token;

            Network.WebRequest.PostRequest(request, user, (byte[] data) =>
            {
                var res = GetPacketBody<APIResponceUserCreate>(data);
                callback?.Invoke(res);
            });
        }
    }
}