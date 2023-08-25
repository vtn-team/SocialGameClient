using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Outgame
{
    public class RankingInfo
    {
        public int Rank;
        public int Point;
        public int UserId;
        public string UserName;
    }

    /// <summary>
    /// イベントのモデル
    /// NOTE: 非同期のみ対応
    /// NOTE: 更新型
    /// </summary>
    public class RankingEventModel
    {
        static protected RankingEventModel _instance = new RankingEventModel();
        public static RankingEventModel Instance { get { return _instance; } }
        protected RankingEventModel() { }

        APIResponceEventStat _stat;
        int _point;
        int _rank;
        List<RankingInfo> _ranking = new List<RankingInfo>();

        /// <summary>
        /// 書き換え可能/不能に対応したアクセサを用意する
        /// </summary>
        static public int CurrentPoint => _instance._point;
        static public int Rank => _instance._rank;

        static public List<RankingInfo> Ranking => _instance._ranking;


        /// <summary>
        /// 非同期呼び出し
        /// NOTE: モデルデータに通信させる仕様はよくある
        /// </summary>
        static public async UniTask<APIResponceEventStat> EventStatAsync(int eventId) => await _instance.eventStatAsync(eventId);
        protected async UniTask<APIResponceEventStat> eventStatAsync(int eventId)
        {
            //データ取得
            _stat = await GameAPI.API.EventStat(eventId);
            _point = _stat.point;
            return _stat;
        }

        /// <summary>
        /// 非同期呼び出し
        /// NOTE: モデルデータに通信させる仕様はよくある
        /// </summary>
        static public async UniTask<List<RankingInfo>> EventRankingAsync(int eventId, int rankingType) => await _instance.eventRankingAsync(eventId, rankingType);
        protected async UniTask<List<RankingInfo>> eventRankingAsync(int eventId, int rankingType)
        {
            //データ取得
            var ranking = await GameAPI.API.EventRanking(eventId, rankingType);
            int rank = ranking.baseRank;
            _rank = ranking.rank;
            _ranking.Clear();
            foreach (var rk in ranking.ranking)
            {
                RankingInfo info = new RankingInfo();
                info.Rank = rank++;
                info.UserId = rk.userId;
                info.UserName = rk.name;
                info.Point = rk.point;
                _ranking.Add(info);
            }
            return _ranking;
        }

        /// <summary>
        /// ポイントを追加する
        /// </summary>
        static public void AppendPoint(int point) => _instance.appendPoint(point);
        void appendPoint(int point)
        {
            _point += point;
        }
    }
}
