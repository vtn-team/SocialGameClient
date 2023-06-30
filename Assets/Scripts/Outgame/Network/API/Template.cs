using Cysharp.Threading.Tasks;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using static IGameAPIImplement;
using static Network.WebRequest;

/// <summary>
/// APIのテンプレート
/// </summary>
namespace Outgame
{
    [Serializable]
    public class APIRequest : APIRequestBase
    {
    }

    [Serializable]
    public class APIResponce : APIResponceBase
    {
    }


    public partial class NodeJSImplement : IGameAPIImplement
    {
        public async UniTask<APIResponce> XXXXXX(string name)
        {
            string request = string.Format("{0}/", GameSetting.GameAPIURI);

            var user = CreateRequest<APIRequest>();

            //TODO:

            string json = await PostRequest(request, user);
            var res = GetPacketBody<APIResponce>(json);
            return res;
        }
    }

    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponce>XXXXXX()
        {
            return await LocalData.LoadAsync<APIResponce>("DummyPacket/xxxxxx.json", GameSetting.DataPath, false);
        }
    }
}
