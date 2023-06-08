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
        Release = 99
    };
    APIEnvironment _environment = APIEnvironment.Develop; //fix

    public static APIEnvironment Environment => _instance._environment;
    public static string StatusCheckAPIURI => GetConfig().StatusCheckAPIURI;
    public static string LoginAPIURI => GetConfig().LoginAPIURI;
    public static string GameAPIURI => GetConfig().GameAPIURI;
    public static string MasterDataAPIURI => GetConfig().MasterDataAPIURI;
    public static string UserDataAPIURI => GetConfig().UserDataAPIURI;

    static private GameSettingObject GetConfig()
    {
        if (_instance._setting != null) return _instance._setting;
        _instance._setting = Resources.Load<GameSettingObject>(string.Format("{0}", _instance._environment.ToString()));
        return _instance._setting;
    }
}