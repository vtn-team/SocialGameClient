using Cysharp.Threading.Tasks;
using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Network.WebRequest;

public class StatusCheck
{
    //シングルトン運用
    static StatusCheck _instance = new StatusCheck();
    static public StatusCheck Instance => _instance;

    [Serializable]
    public class GameStatus
    {
        public int status;
        public bool isMaintenance;
        public string contentCatalog;
        public MasterVersion[] masterVersion;
    }

    GameStatus _status = null;

    static public async UniTask<GameStatus> Check()
    {
        //チェックAPI読み込み
        Debug.Log("StatusCheck Start.");

        string json = await GetRequest(GameSetting.StatusCheckAPIURI);
        _instance._status = JsonUtility.FromJson<GameStatus>(json);
        Debug.Log(_instance._status.status);
        return _instance._status;
    }
}
