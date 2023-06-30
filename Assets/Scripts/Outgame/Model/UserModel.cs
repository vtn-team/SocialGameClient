using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// ローカルに保存するデータ
    /// NOTE: UDIDだけをとりあえず保存する
    /// </summary>
    [Serializable]
    public class UDID
    {
        public string udid = "";
    }

    public class PlayerInfo
    {
        public int Id;
        public string UDID;
        public string Name;

        public int Rank;
        public int Money;
        public int MovePoint;
        public int AttackPoint;

        public int MovePointMax = 30; //TODO: 今後正式なデータとして用意する
        public int AttackPointMax = 5; //TODO: 今後正式なデータとして用意する

        public long LastPointUpdate;
        public string QuestTransaction;

        public string GameState;
    }


    /// <summary>
    /// ユーザモデルはゲーム中通してキャッシュされてよい
    /// NOTE: UDIDだけをとりあえず保存する
    /// </summary>
    public class UserModel : LocalCachedModel<UserModel, UDID>
    {
        /// <summary>
        /// 書き換え可能/不能に対応したアクセサを用意する
        /// </summary>
        public static string GUID {
            get
            {
                return _instance?._data?.udid;
            }
        }

        PlayerInfo _playerInfo = new PlayerInfo();
        bool _isLoad = false;

        static public PlayerInfo PlayerInfo => _instance._isLoad ? _instance._playerInfo : null;
        public override bool hasData => (_data != null && !string.IsNullOrEmpty(_data.udid));


        protected override void Setup()
        {
            _dataName = "UserUDID";
        }

        static public void Create(string id)
        {
            _instance._data = new UDID();
            _instance._data.udid = id;
            Save();
        }

        static public void UpdatePlayerInfo(APIResponceLogin login)
        {
            _instance._playerInfo.Id = login.id;
            _instance._playerInfo.UDID = login.udid;
            _instance._playerInfo.Name = login.name;
            _instance._playerInfo.Rank = login.rank;
            _instance._playerInfo.Money = login.money;
            _instance._playerInfo.MovePoint = login.movePoint;
            _instance._playerInfo.AttackPoint = login.attackPoint;
            _instance._playerInfo.LastPointUpdate = login.lastPointUpdate;
            _instance._playerInfo.QuestTransaction = login.questTransaction;

            _instance._isLoad = true;
        }
    }
}
