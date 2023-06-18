using Cysharp.Threading.Tasks;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static IGameAPIImplement;
using static Network.WebRequest;

namespace Outgame
{
    public class NodeJSImplement : IGameAPIImplement
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

        public async UniTask<APIResponceGetItems> GetItems()
        {
            string request = string.Format("{0}/ud/items?session={1}", GameSetting.GameAPIURI, _session);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetItems>(json);
            return res;
        }

        public async UniTask<APIResponceUserCreate> CreateUser(string name)
        {
            string request = string.Format("{0}/user/create", GameSetting.GameAPIURI);

            APIRequestUserCreate user = new APIRequestUserCreate();
            user.name = name;
            user.session = _session;
            user.token = _token;

            string json = await PostRequest(request, user);
            var res = GetPacketBody<APIResponceUserCreate>(json);
            return res;
        }

        public async UniTask<APIResponceGachaDraw> Gacha(int gachaId, int drawCount)
        {
            string request = string.Format("{0}/gacha/draw", GameSetting.GameAPIURI);

            APIRequestGachaDraw gacha = new APIRequestGachaDraw();
            gacha.gachaId = gachaId;
            gacha.drawCount = drawCount;
            gacha.session = _session;
            gacha.token = _token;

            string json = await PostRequest(request, gacha);
            var res = GetPacketBody<APIResponceGachaDraw>(json);
            return res;
        }

        public async UniTask<APIResponceEnhance> Enhance(int baseId, APIRequestEnhanceMaterials materials)
        {
            string request = string.Format("{0}/enhance", GameSetting.GameAPIURI);

            APIRequestEnhance enhance = new APIRequestEnhance();
            enhance.baseId = baseId;
            enhance.materials = materials;
            enhance.session = _session;
            enhance.token = _token;

            string json = await PostRequest(request, enhance);
            var res = GetPacketBody<APIResponceEnhance>(json);
            return res;
        }
    }
}