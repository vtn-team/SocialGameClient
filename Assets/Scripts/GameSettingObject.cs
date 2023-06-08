using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 設定ファイル
/// </summary>
[CreateAssetMenu(fileName = "GameSetting", menuName = "SocialGameSample/GameSetting", order = 1)]
public class GameSettingObject : ScriptableObject
{
    public string StatusCheckAPIURI;
    public string LoginAPIURI;
    public string GameAPIURI;
    public string SocialAPIURI;
    public string MasterDataAPIURI;
    public string UserDataAPIURI;
}