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
    public partial class LocalImplement : IGameAPIImplement
    {
        private string _token = "";
        private string _session = "";

        public async UniTask<APIResponceGetCards> GetCards()
        {
            return await LocalData.LoadAsync<APIResponceGetCards>("DummyPacket/cards.json", GameSetting.DataPath, false);
        }

        public async UniTask<APIResponceGetItems> GetItems()
        {
            return await LocalData.LoadAsync<APIResponceGetItems>("DummyPacket/items.json", GameSetting.DataPath, false);
        }

        public async UniTask<APIResponceGetQuests> GetQuests()
        {
            return await LocalData.LoadAsync<APIResponceGetQuests>("DummyPacket/quests.json", GameSetting.DataPath, false);
        }
    }
}

