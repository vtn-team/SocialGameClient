using Cysharp.Threading.Tasks;
using Network;
using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGameAPIImplement
{
    UniTask<APIResponceLogin> Login(string uuid);
    UniTask<APIResponceGetCards> GetCards();
    UniTask<APIResponceGetItems> GetItems();
    UniTask<APIResponceGetQuests> GetQuests();
    UniTask<APIResponceGetInfomationList> GetInformationList();
    UniTask<APIResponceGetInfomation> GetInformation(string id);
    UniTask<APIResponceUserCreate> UserCreate(string name);
    UniTask<APIResponceGachaDraw> Gacha(int gachaId, int drawCount);
    UniTask<APIResponceEnhance> Enhance(int baseId, APIRequestEnhanceMaterials msterials);
    UniTask<APIResponceQuestStart> QuestStart(int questId);
    UniTask<APIResponceQuestResult> QuestResult(int result);
}
