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
    public class APIRequestQuestStart : APIRequestBase
    {
        public int questId;
    }

    [Serializable]
    public class APIResponceQuestStart : APIResponceBase
    {
        public string transactionId;
        public int afterMovePoint;
        public long lastPointUpdate;
        //APIResponceQuestEnemy[] enemies; //TODO: 実際は出現する敵を返す
    }

    [Serializable]
    public class APIRequestQuestResult : APIRequestBase
    {
        public string transactionId;
        public int questId;
        public int result;
    }

    [Serializable]
    public class APIResponceQuestResult : APIResponceBase
    {
        //APIResponceQuestReward[] rewards; //TODO: 実際は報酬を返す
    }


    public partial class NodeJSImplement : IGameAPIImplement
    {
        string _questTransaction = null;

        public async UniTask<APIResponceQuestStart> QuestStart(int questId)
        {
            string request = string.Format("{0}/quest/start", GameSetting.GameAPIURI);

            var quest = CreateRequest<APIRequestQuestStart>();
            quest.questId = questId;

            string json = await PostRequest(request, quest);
            var res = GetPacketBody<APIResponceQuestStart>(json);
            _questTransaction = res.transactionId;
            return res;
        }

        public async UniTask<APIResponceQuestResult> QuestResult(int questId, int result)
        {
            string request = string.Format("{0}/quest/result", GameSetting.GameAPIURI);

            var quest = CreateRequest<APIRequestQuestResult>();
            quest.questId = questId;
            quest.result = result;
            quest.transactionId = _questTransaction;

            string json = await PostRequest(request, quest);
            var res = GetPacketBody<APIResponceQuestResult>(json);
            return res;
        }
    }


    public partial class LocalImplement : IGameAPIImplement
    {
        public async UniTask<APIResponceQuestStart> QuestStart(int questId)
        {
            //※未実装！！
            return await LocalData.LoadAsync<APIResponceQuestStart>("DummyPacket/questStart.json", GameSetting.DataPath, false);
        }

        public async UniTask<APIResponceQuestResult> QuestResult(int questId, int result)
        {
            //※未実装！！
            return await LocalData.LoadAsync<APIResponceQuestResult>("DummyPacket/questresult.json", GameSetting.DataPath, false);
        }
    }
}
