using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface INetworkImplement
{
    public delegate void APICallback<T>(T dataClass);

    //async
    Task<APIResponceLogin> Login(string uuid);
    Task<APIResponceGetCards> GetCards();
    //

    //callback
    void Login(string uuid, APICallback<APIResponceLogin> callback);
    void GetCards(APICallback<APIResponceGetCards> callback);
    void CreateUser(string name, APICallback<APIResponceUserCreate> callback);
    void Gacha(int gachaId, int drawCount, APICallback<APIResponceGachaDraw> callback);
}
