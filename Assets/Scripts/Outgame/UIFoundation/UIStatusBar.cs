using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using System.Data;

namespace Outgame
{
    public class UIStatusBar : UIView
    {
        static UIStatusBar _instance = null;

        [SerializeField] Image _moveGauge;
        [SerializeField] Image[] _battlePoints;
        [SerializeField] TextMeshProUGUI _money;
        [SerializeField] TextMeshProUGUI _moveHealTime;

        int _movePoint = 0;

        static public bool IsNull => _instance == null;


        protected override void AwakeCall()
        {
            base.AwakeCall();

            StartCoroutine("UpdateStatusBar");
            _instance = this;
            Disactive();
        }

        void OnDestroy()
        {
            StopCoroutine("UpdateStatusBar");
        }

        //時間処理があるのでupdateはどのみちかける
        IEnumerator UpdateStatusBar()
        {
            for(; ; )
            {
                yield return new WaitForSeconds(1);

                UpdateStatus();
            }
        }

        /*
        void Update()
        {
            UpdateStatus();
        }
        */
        public static long GetCurrentUnixTime()
        {
            var unixTimestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0,0,0, DateTimeKind.Utc))).TotalMilliseconds;
            return unixTimestamp;
        }

        void UpdateStatus()
        {
            //プレイヤーから情報拾ってきて値入れる
            if(UserModel.PlayerInfo.MovePoint < UserModel.PlayerInfo.MovePointMax)
            {
                int targetPoint = UserModel.PlayerInfo.MovePointMax - UserModel.PlayerInfo.MovePoint;
                long maxHealTime = UserModel.PlayerInfo.LastPointUpdate + targetPoint * 1000 * 60;
                long now = GetCurrentUnixTime();

                _movePoint = UserModel.PlayerInfo.MovePoint + Mathf.FloorToInt((now - UserModel.PlayerInfo.LastPointUpdate) / 1000 / 60);

                if (_movePoint > UserModel.PlayerInfo.MovePointMax) _movePoint = UserModel.PlayerInfo.MovePointMax;

                _moveGauge.fillAmount = (float)UserModel.PlayerInfo.MovePoint / UserModel.PlayerInfo.MovePointMax;

                //時刻使った方がスマートだけど今回は
                _moveHealTime.text = string.Format("{0}Pt 全回復まであと{1}:{2}", _movePoint, Mathf.FloorToInt((maxHealTime-now)/1000/60), ((maxHealTime - now) / 1000) % 60);
            }
            else
            {
                _moveHealTime.text = string.Format("{0}Pt", UserModel.PlayerInfo.MovePoint);
            }

            //AttackPointは値変わらないので後で

            //
            _money.text = UserModel.PlayerInfo.Money.ToString();

            //
            _instance.Active();
        }

        static public void Show()
        {
            _instance.UpdateStatus();
            _instance.Active();
        }

        static public void Hide()
        {
            _instance.Disactive();
        }
    }
}
