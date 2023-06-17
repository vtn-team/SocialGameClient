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
    /// カードリストを表示するビュー
    /// </summary>
    public class CardListScrollView : MonoBehaviour
    {
        [SerializeField] ScrollRect _rect;
        [SerializeField] GameObject _line;
        [SerializeField] GameObject _card;
        [SerializeField] UIView _content;

        int _scrollPos = 0;
        List<GameObject> _lineList = new List<GameObject>();
        List<CardImage> _cardList = new List<CardImage>();

        const int LineContentCount = 5;
        const int ContentHeight = 6;
        const int CardUIHeight = 160;

        /// <summary>
        /// ビューを作る
        /// </summary>
        private void Start()
        {
            var cardList = CardListModel.CardList;
            GameObject _currentLine = null;
            for (int i=0; i<cardList.List.Count; ++i)
            {
                if(i % LineContentCount == 0)
                {
                    var line = GameObject.Instantiate(_line, _content.RectTransform);
                    _lineList.Add(line);
                    _currentLine = line;
                }

                var card = GameObject.Instantiate(_card, _currentLine.transform);
                var script = card.GetComponent<CardImage>();
                script.Setup(cardList.List[i]);
                //card.GetComponent<RectTransform>().localScale = new Vector3(180.0f / 200.0f, 180.0f / 200.0f, 1.0f);
                _cardList.Add(script);
            }

            _content.RectTransform.sizeDelta = new Vector2(800, (cardList.List.Count / LineContentCount + 1) * CardUIHeight);

            _rect.onValueChanged.AddListener(ScrollUpdate);

            for (int y = _scrollPos; y < _scrollPos + ContentHeight; ++y)
            {
                for (int x = 0; x < LineContentCount; ++x)
                {
                    _cardList[y * LineContentCount + x].Active();
                    _cardList[y * LineContentCount + x].Load();
                }
            }
        }

        void ActiveCurrentArea(int cur)
        {
            if (_scrollPos == cur) return;

            while (_scrollPos != cur)
            {
                Debug.Log($"SCP:{_scrollPos}");
                int remLine = _scrollPos < cur ? _scrollPos : (_scrollPos + 6);
                int addLine = _scrollPos < cur ? _scrollPos+6 : _scrollPos-1;

                if (remLine >= 0 && remLine < _lineList.Count)
                {
                    for (int i = 0; i < LineContentCount; ++i)
                    {
                        if (remLine * LineContentCount + i < 0) continue;
                        if (remLine * LineContentCount + i >= _cardList.Count) continue;

                        _cardList[remLine * LineContentCount + i].ReleaseTexture();
                        _cardList[remLine * LineContentCount + i].Disactive();
                    }
                    Debug.Log($"REM:{remLine}");
                }

                if (addLine >= 0 && addLine < _lineList.Count)
                {
                    for (int i = 0; i < LineContentCount; ++i)
                    {
                        if (addLine * LineContentCount + i < 0) continue;
                        if (addLine * LineContentCount + i >= _cardList.Count) continue;

                        _cardList[addLine * LineContentCount + i].Active();
                        _cardList[addLine * LineContentCount + i].Load();
                    }
                    Debug.Log($"ADD:{addLine}");
                }

                if (_scrollPos < cur) _scrollPos++;
                else _scrollPos--;
            }
        }

        void ScrollUpdate(Vector2 vec)
        {
            ActiveCurrentArea(Mathf.FloorToInt((1.0f-vec.y) * (_cardList.Count / LineContentCount - ContentHeight - 1)));
        }
    }
}