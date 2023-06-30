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
    public class APIRequestGachaDraw : APIRequestBase
    {
        public int gachaId;
        public int drawCount;
    }

    [Serializable]
    public class APIResponceGachaDraw : APIResponceBase
    {
        public APIResponceCard[] cards;
    }

    public partial class NodeJSImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceGachaDraw> Gacha(int gachaId, int drawCount)
        {
            string request = string.Format("{0}/gacha/draw", GameSetting.GameAPIURI);

            var gacha = CreateRequest<APIRequestGachaDraw>();
            gacha.gachaId = gachaId;
            gacha.drawCount = drawCount;

            string json = await PostRequest(request, gacha);
            var res = GetPacketBody<APIResponceGachaDraw>(json);
            return res;
        }
    }

    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceGachaDraw> Gacha(int gachaId, int drawCount)
        {
            return await LocalData.LoadAsync<APIResponceGachaDraw>("DummyPacket/gacha_draw.json", GameSetting.DataPath, false);
        }
    }
}
