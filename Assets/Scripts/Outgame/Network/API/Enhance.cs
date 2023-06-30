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
    public class APIRequestEnhanceItem
    {
        public int itemId;
        public int itemCount;
    }

    [Serializable]
    public class APIRequestEnhanceMaterials
    {
        public int[] cardIds;
        public APIRequestEnhanceItem[] items;
    }

    [Serializable]
    public class APIRequestEnhance : APIRequestBase
    {
        public int baseId;
        public APIRequestEnhanceMaterials materials;
    }

    [Serializable]
    public class APIResponceEnhance : APIResponceBase
    {
        public int _success;
        public int _level;
        public int _luck;
    }

    public partial class NodeJSImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceEnhance> Enhance(int baseId, APIRequestEnhanceMaterials materials)
        {
            string request = string.Format("{0}/enhance", GameSetting.GameAPIURI);

            var enhance = CreateRequest<APIRequestEnhance>();
            enhance.baseId = baseId;
            enhance.materials = materials;

            string json = await PostRequest(request, enhance);
            var res = GetPacketBody<APIResponceEnhance>(json);
            return res;
        }
    }

    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceEnhance> Enhance(int baseId, APIRequestEnhanceMaterials materials)
        {
            //※未実装！！
            return await LocalData.LoadAsync<APIResponceEnhance>("DummyPacket/enhance.json", GameSetting.DataPath, false);
        }
    }
}
