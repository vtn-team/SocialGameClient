using MD;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// 強化リストを表示するビュー
    /// NOTE: 選択UIを使いまわしたいときは別途考える
    /// </summary>
    public class EnhanceListView : ListView
    {
        [SerializeField] GameObject _item;
        [SerializeField] CardImage _baseCard;
        [SerializeField] TMPro.TextMeshProUGUI _name;
        [SerializeField] TMPro.TextMeshProUGUI _level;
        [SerializeField] TMPro.TextMeshProUGUI _luck;

        [SerializeField] GameObject _baseDecideButton;
        [SerializeField] GameObject _enhanceButton;

        enum State
        {
            BaseSelect,
            MaterialSelect
        }

        State _viewState = State.BaseSelect;
        int _baseId = -1;
        List<ListItemBase> _selectedItems = new List<ListItemBase>();

        public bool IsSelected { get; set; }
        public List<ListItemBase> SelectedItems => _selectedItems;

        private void Start()
        {
            _baseDecideButton.SetActive(false);
            _enhanceButton.SetActive(false);
        }

        public void BaseSelect()
        {
            _viewState = State.MaterialSelect;
            _baseDecideButton.SetActive(false);
            Setup();
        }

        /// <summary>
        /// ビューを作る
        /// </summary>
        public override void Setup()
        {
            _lineList.ForEach(l => GameObject.Destroy(l));
            _itemList.Clear();
            _scrollPos = 0;

            //フィルタする場合にフィルタ
            //TODO: カードは絞り込みをいつかやる
            var itemList = ItemListFilter.FilteredList(ItemListFilter.FilterType.EnhanceItem);
            var cardList = CardListFilter.FilteredList(CardListFilter.FilterType.All);

            //ビューアイテムの生成
            //レイアウトを使用する
            GameObject _currentLine = null;
            int totalCount = 0;

            //素材選択時はアイテムも混ぜる(最初に置く)
            if (_viewState == State.MaterialSelect)
            {
                for (int i = 0; i < itemList.Count; ++i, ++totalCount)
                {
                    if (totalCount % LineContentCount == 0)
                    {
                        var line = GameObject.Instantiate(_line, _content.RectTransform);
                        _lineList.Add(line);
                        _currentLine = line;
                    }

                    var it = GameObject.Instantiate(_item, _currentLine.transform);
                    var script = it.GetComponent<ItemImage>();
                    if (script)
                    {
                        script.Setup(itemList[i]);
                    }

                    var item = ListItemBase.ListItemSetup<ListItemEnhanceItem>(totalCount, it, (int evtId, int index) => OnCardClick(evtId, index));
                    _itemList.Add(item);
                }
            }

            //カード
            for (int i = 0; i < cardList.Count; ++i)
            {
                if (_viewState == State.MaterialSelect)
                {
                    if(_baseCard.Info.Id == cardList[i].Id) continue;
                }
                
                if (totalCount % LineContentCount == 0)
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

                var item = ListItemBase.ListItemSetup<ListItemCardImage>(totalCount, card, (int evtId, int index) => OnCardClick(evtId, index));
                _itemList.Add(item);

                ++totalCount;
            }

            //サイズ計算して最大スクロール値を決める
            _content.RectTransform.sizeDelta = new Vector2(800, (_itemList.Count / LineContentCount + 1) * CardUIHeight);

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

        protected override void OnCardClick(int evtId, int index)
        {
            switch(_viewState)
            {
                case State.BaseSelect:
                    _baseId = index;
                    _baseDecideButton.SetActive(true);
                    {
                        var cardInfo = _itemList[_baseId].GetComponent<CardImage>().Info;
                        var cardData = MasterData.GetCard(cardInfo.CardId);
                        _baseCard.Setup(cardInfo);
                        _baseCard.Load();
                        _baseCard.Active();
                        _name.text = MasterData.GetLocalizedText(cardData.Name);
                        _level.text = cardInfo.Level.ToString();
                        _luck.text = cardInfo.Luck.ToString();
                    }
                    break;

                case State.MaterialSelect:
                    if (_selectedItems.Any(i => i == _itemList[index]))
                    {
                        _selectedItems.Remove(_itemList[index]);
                    }
                    else
                    {
                        _selectedItems.Add(_itemList[index]);
                    }

                    if (_selectedItems.Count > 0)
                    {
                        _enhanceButton.SetActive(true);
                    }
                    break;
            }
        }
    }
}
