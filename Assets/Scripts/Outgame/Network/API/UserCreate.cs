using Cysharp.Threading.Tasks;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using static IGameAPIImplement;
using static Network.WebRequest;

/// <summary>
/// ユーザ作成API
/// </summary>
namespace Outgame
{
    [Serializable]
    public class APIRequestUserCreate : APIRequestBase
    {
        public string name;
    }


    [Serializable]
    public class APIResponceUserCreate : APIResponceBase
    {
        public string udid;
    }

    public partial class NodeJSImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceUserCreate> UserCreate(string name)
        {
            string request = string.Format("{0}/user/create", GameSetting.GameAPIURI);

            var user = CreateRequest<APIRequestUserCreate>();
            user.name = name;

            string json = await PostRequest(request, user);
            var res = GetPacketBody<APIResponceUserCreate>(json);
            return res;
        }
    }

    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceUserCreate> UserCreate(string name)
        {
            //ローカルテストを想定しないパケット
            return await UniTask.RunOnThreadPool(() => default(APIResponceUserCreate));
        }
    }
}
