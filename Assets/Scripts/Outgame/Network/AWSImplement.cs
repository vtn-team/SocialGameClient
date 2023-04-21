using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AWSLambdaAPIImplement : INetworkImplement
{
    const string URI = "https://rtpwg9bexj.execute-api.ap-northeast-1.amazonaws.com/default/{Command}/{Param}";

    class AWSLambdaAPIResult
    {
        public int statusCode;
        public string body;
    }

    byte[] GetPacketBody(byte[] data)
    {
        string json = Encoding.UTF8.GetString(data);
        AWSLambdaAPIResult result = JsonUtility.FromJson<AWSLambdaAPIResult>(json);
        return Encoding.UTF8.GetBytes(result.body);
    }


    public void GetUser(string uuid, INetworkImplement.APICallback callback)
    {
        string request = URI.Replace("{Command}/", "GetUser");
        request = request.Replace("/{Param}", "");
        Network.WebRequest.GetRequest(request, (byte[] data) => {
            callback?.Invoke(GetPacketBody(data));
        });
    }

    public void CreateUser(string name, INetworkImplement.APICallback callback)
    {
        string request = URI.Replace("{Command}", "CreateUser");
        request = request.Replace("{Param}", "");
        Network.WebRequest.PostRequest(request, Encoding.UTF8.GetBytes(name), (byte[] data) => {
            callback?.Invoke(GetPacketBody(data));
        });
    }
}
