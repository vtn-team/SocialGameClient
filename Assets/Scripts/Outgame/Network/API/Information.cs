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
    public class APIResponceInfomationListItem : APIRequestBase
    {
        public string id;
        public string title;
    }

    [Serializable]
    public class APIResponceInfomationContents : APIRequestBase
    {
        public string type;
        public string[] param;
    }

    [Serializable]
    public class APIResponceGetInfomationList : APIResponceBase
    {
        public APIResponceInfomationListItem[] list;
    }

    [Serializable]
    public class APIResponceGetInfomation : APIResponceBase
    {
        public string title;
        public APIResponceInfomationContents[] contents;
    }


    public partial class NodeJSImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceGetInfomationList> GetInformationList()
        {
            string request = string.Format("{0}/info/list", GameSetting.GameAPIURI);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetInfomationList>(json);
            return res;
        }

        public async UniTask<APIResponceGetInfomation> GetInformation(string id)
        {
            string request = string.Format("{0}/info/{1}", GameSetting.GameAPIURI, id);

            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceGetInfomation>(json);
            return res;
        }
    }

    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceGetInfomationList> GetInformationList()
        {
            //ローカルでは無視する
            return default(APIResponceGetInfomationList);
        }

        public async UniTask<APIResponceGetInfomation> GetInformation(string id)
        {
            //ローカルでは無視する
            return default(APIResponceGetInfomation);
        }
    }
}