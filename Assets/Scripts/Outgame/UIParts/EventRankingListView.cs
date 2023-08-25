using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// ランキングリストを表示するビュー
    /// </summary>
    public class EventRankingListView : ListView
    {
        const int RankingEventInfoHeight = 150;
        [SerializeField] GameObject _rankingPrefab;

        /// <summary>
        /// ビューを作る
        /// </summary>
        public override void Setup()
        {
            _lineList.ForEach(l => GameObject.Destroy(l));
            _itemList.Clear();
            _scrollPos = 0;

            var rankList = RankingEventModel.Ranking;

            //チャプターとその子供になるクエストをリストに入れる
            for (int i = 0; i < rankList.Count; ++i)
            {
                var rkObj = GameObject.Instantiate(_rankingPrefab, _content.RectTransform);
                var listItem = ListItemBase.ListItemSetup<ListItemRankingInfo>(i, rkObj, null);
                listItem.SetupRankingInfo(rankList[i]);

                _itemList.Add(listItem);
                _lineList.Add(listItem.gameObject);
            }

            //サイズ計算して最大スクロール値を決める
            //クエストはサイズ可変するので毎回再計算する
            _content.RectTransform.sizeDelta = new Vector2(800, _lineList.Count() * RankingEventInfoHeight);
        }
    }
}
