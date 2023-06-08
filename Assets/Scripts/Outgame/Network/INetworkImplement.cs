using Cysharp.Threading.Tasks;
using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IGameAPIImplement
{
    //async
    UniTask<APIResponceLogin> Login(string uuid);
    UniTask<APIResponceGetCards> GetCards();
    UniTask<APIResponceUserCreate> CreateUser(string name);
    UniTask<APIResponceGachaDraw> Gacha(int gachaId, int drawCount);
    //
}
