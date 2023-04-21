using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface INetworkImplement
{
    public delegate void APICallback(byte[] msg);

    void GetUser(string uuid, APICallback callback);
    void CreateUser(string name, APICallback callback);
}
