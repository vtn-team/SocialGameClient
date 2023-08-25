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
    public class APIResponceEventStat : APIResponceBase
    {
        public int eventStatus;
        public bool isOpen;
        public bool isGameOpen;
        public int point;
    }

    [Serializable]
    public class APIResponceEventRankingInfo : APIResponceBase
    {
        public int userId;
        public int point;
        public string name;
    }

    [Serializable]
    public class APIResponceEventRanking : APIResponceBase
    {
        public int rank;
        public int baseRank;
        public APIResponceEventRankingInfo[] ranking;
    }


    public partial class NodeJSImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceEventStat> EventStat(int eventId)
        {
            string request = string.Format("{0}/event/stat?session={1}&eventId={2}", GameSetting.GameAPIURI, _session, eventId);
            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceEventStat>(json);
            return res;
        }

        public async UniTask<APIResponceEventRanking> EventRanking(int eventId, int rankingType)
        {
            string request = string.Format("{0}/event/ranking?session={1}&eventId={2}&rankingType={3}", GameSetting.GameAPIURI, _session, eventId, rankingType);
            string json = await GetRequest(request);
            var res = GetPacketBody<APIResponceEventRanking>(json);
            return res;
        }
    }

    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceEventStat> EventStat(int eventId)
        {
            return await LocalData.LoadAsync<APIResponceEventStat>("DummyPacket/eventStat.json", GameSetting.DataPath, false);
        }
        public async UniTask<APIResponceEventRanking> EventRanking(int eventId, int rankingType)
        {
            return await LocalData.LoadAsync<APIResponceEventRanking>("DummyPacket/eventRanking.json", GameSetting.DataPath, false);
        }
    }
}
