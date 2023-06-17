using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// ユーザモデルと対になるデータ
    /// NOTE: UDIDだけをとりあえず保存する
    /// </summary>
    [Serializable]
    public class UDID
    {
        public string udid = "";
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
    }
}
