using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameSetting
{
    static GameSetting _instance = new GameSetting();
    GameSettingObject _setting = null;

    public enum APIEnvironment
    {
        Develop = 1,
        Local = 2,
        Release = 99
    };
    APIEnvironment _environment = APIEnvironment.Local; //fix
    string _dataPath = "";
    string _savePath = "";

    public static APIEnvironment Environment => _instance._environment;
    public static string StatusCheckAPIURI => GetConfig().StatusCheckAPIURI;
    public static string LoginAPIURI => GetConfig().LoginAPIURI;
    public static string GameAPIURI => GetConfig().GameAPIURI;
    public static string MasterDataAPIURI => GetConfig().MasterDataAPIURI;
    public static string UserDataAPIURI => GetConfig().UserDataAPIURI;

    public static string SavePath => _instance._savePath;
    public static string DataPath => _instance._dataPath;


    static private GameSettingObject GetConfig()
    {
        if (_instance._setting != null) return _instance._setting;
        _instance._setting = Resources.Load<GameSettingObject>(string.Format("{0}", _instance._environment.ToString()));
        _instance._dataPath = Application.dataPath;
        _instance._savePath = Application.persistentDataPath;
        return _instance._setting;
    }
}