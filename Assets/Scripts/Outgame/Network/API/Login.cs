using Cysharp.Threading.Tasks;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using static IGameAPIImplement;
using static Network.WebRequest;

/// <summary>
/// ログインAPI
/// </summary>
namespace Outgame
{
    [Serializable]
    public class APIRequestLogin : APIRequestBase
    {
        public string udid;
    }

    [Serializable]
    public class APIResponceLogin : APIResponceBase
    {
        public string session;

        public int id;
        public string udid;
        public string name;

        public string game_state;
    }

    public partial class NodeJSImplement : IGameAPIImplement
    {
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
    }

    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceLogin> Login(string uuid)
        {
            var result = await LocalData.LoadAsync<APIResponceLogin>("DummyPacket/login.json", GameSetting.DataPath, false);
            result.udid = uuid;
            return result;
        }
    }
}
