using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Outgame
{
    /// <summary>
    /// リストを表示するビュー
    /// NOTE: 用途別に継承して使う
    /// </summary>
    public class ListView : MonoBehaviour
    {
        [SerializeField] protected ScrollRect _rect; //スクロール管理クラス
        [SerializeField] protected GameObject _line; //行のレイアウト設定がなされているprefab
        [SerializeField] protected GameObject _card; //個々のカードのprefab
        [SerializeField] protected UIView _content;  //コンテンツエリア

        public delegate void ListSelectEvent(int evtId, int index);

        protected int _scrollPos = 0; //スクロール座標
        protected List<GameObject> _lineList = new List<GameObject>(); //行リスト
        protected List<ListItemBase> _itemList = new List<ListItemBase>(); //表示クラスのリスト

        protected const int LineContentCount = 5; //1行に何個カード置くか
        protected const int ContentHeight = 6; //ビューに何行カードが入るか
        protected const int CardUIHeight = 160; //カードUIの高さ

        /// <summary>
        /// ビューを作る
        /// </summary>
        public virtual void Setup()
        {
            _lineList.ForEach(l => GameObject.Destroy(l));

            //フィルタする場合にフィルタ
            var cardList = CardListFilter.FilteredList(CardListFilter.FilterType.All);

            //ビューアイテムの生成
            //レイアウトを使用する
            GameObject _currentLine = null;
            for (int i=0; i<cardList.Count; ++i)
            {
                if(i % LineContentCount == 0)
                {
                    var line = GameObject.Instantiate(_line, _content.RectTransform);
                    _lineList.Add(line);
                    _currentLine = line;
                }

                var card = GameObject.Instantiate(_card, _currentLine.transform);
                var script = card.GetComponent<CardImage>();
                if (script)
                {
                    script.Setup(cardList[i]);
                }
                var item = ListItemBase.ListItemSetup<ListItemCardImage>(i,card,(int evtId, int index) => OnItemClick(evtId, index));
                _itemList.Add(item);
            }

            //サイズ計算して最大スクロール値を決める
            _content.RectTransform.sizeDelta = new Vector2(800, (cardList.Count / LineContentCount + 1) * CardUIHeight);

            //イベント登録
            _rect.onValueChanged.AddListener(ScrollUpdate);

            //ファーストビューを表示する
            //NOTE: スクロール位置を覚えられるように考慮しておく
            for (int y = _scrollPos; y < _scrollPos + ContentHeight; ++y)
            {
                for (int x = 0; x < LineContentCount; ++x)
                {
                    _itemList[y * LineContentCount + x].Load();
                }
            }
        }

        protected virtual void OnItemClick(int evtId, int index)
        {
            UIManager.StackView(ViewID.CardInfo);
        }

        /// <summary>
        /// 現在の表示領域にあるものを表示する
        /// </summary>
        /// <param name="cur">次のスクロール位置</param>
        protected void ActiveCurrentArea(int cur)
        {
            if (_scrollPos == cur) return;

            //スクロールバーの位置に来るまで繰り返す
            while (_scrollPos != cur)
            {
                //Debug.Log($"SCP:{_scrollPos}");
                int remLine = _scrollPos < cur ? _scrollPos : (_scrollPos + 6);
                int addLine = _scrollPos < cur ? _scrollPos+6 : _scrollPos-1;

                if (remLine >= 0 && remLine < _lineList.Count)
                {
                    for (int i = 0; i < LineContentCount; ++i)
                    {
                        if (remLine * LineContentCount + i < 0) continue;
                        if (remLine * LineContentCount + i >= _itemList.Count) continue;

                        _itemList[remLine * LineContentCount + i].Release();
                    }
                    //Debug.Log($"REM:{remLine}");
                }

                if (addLine >= 0 && addLine < _lineList.Count)
                {
                    for (int i = 0; i < LineContentCount; ++i)
                    {
                        if (addLine * LineContentCount + i < 0) continue;
                        if (addLine * LineContentCount + i >= _itemList.Count) continue;

                        _itemList[addLine * LineContentCount + i].Load();
                    }
                    //Debug.Log($"ADD:{addLine}");
                }

                if (_scrollPos < cur) _scrollPos++;
                else _scrollPos--;
            }
        }

        /// <summary>
        /// スクロール更新
        /// </summary>
        /// <param name="vec">正規化されたスクロール位置</param>
        protected void ScrollUpdate(Vector2 vec)
        {
            //位置補正して表示処理へ
            ActiveCurrentArea(Mathf.FloorToInt((1.0f-vec.y) * (_itemList.Count / LineContentCount - ContentHeight - 1)));
        }
    }
}