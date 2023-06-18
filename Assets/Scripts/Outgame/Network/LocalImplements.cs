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
    public class LocalImplement : IGameAPIImplement
    {
        private string _token = "";
        private string _session = "";

        public async UniTask<APIResponceLogin> Login(string uuid)
        {
            var result = await LocalData.LoadAsync<APIResponceLogin>("DummyPacket/login.json", GameSetting.DataPath, false);
            result.udid = uuid;
            return result;
        }

        public async UniTask<APIResponceGetCards> GetCards()
        {
            return await LocalData.LoadAsync<APIResponceGetCards>("DummyPacket/cards.json", GameSetting.DataPath, false);
        }

        public async UniTask<APIResponceGetItems> GetItems()
        {
            return await LocalData.LoadAsync<APIResponceGetItems>("DummyPacket/items.json", GameSetting.DataPath, false);
        }

        public async UniTask<APIResponceUserCreate> CreateUser(string name)
        {
            //ローカルテストを想定しないパケット
            return await UniTask.RunOnThreadPool(() => default(APIResponceUserCreate));
        }

        public async UniTask<APIResponceGachaDraw> Gacha(int gachaId, int drawCount)
        {
            return await LocalData.LoadAsync<APIResponceGachaDraw>("DummyPacket/gacha_draw.json", GameSetting.DataPath, false);
        }

        public async UniTask<APIResponceEnhance> Enhance(int baseId, APIRequestEnhanceMaterials materials)
        {
            return await LocalData.LoadAsync<APIResponceEnhance>("DummyPacket/enhance.json", GameSetting.DataPath, false);
        }
    }
}

