using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using static Network.WebRequest;
using Cysharp.Threading.Tasks;

namespace MD
{
    /// <summary>
    /// マスターデータ管理クラス
    /// </summary>
    public class MasterData
    {
        //設定系
        string URI = "";
        const string DataPrefix = "MasterData";

        //シングルトン運用
        static MasterData _instance = new MasterData();
        static public MasterData Instance => _instance;


        //ゲーム中のマスターデータ
        /// <summary>
        /// 整形済みデータ
        /// </summary>
        class PrettyData<K, T>
        {
            Dictionary<K, T> Data = new Dictionary<K, T>();
            static public PrettyData<K, T> Create(T[] data, Func<T, K> mapper)
            {
                PrettyData<K, T> ret = new PrettyData<K, T>();
                foreach (var d in data)
                {
                    K key = mapper.Invoke(d);
                    if (key == null) continue;

                    if(ret.Data.ContainsKey(key))
                    {
                        Debug.Log($"duplicate key:{key}");
                        continue;
                    }

                    ret.Data.Add(key, d);
                }
                return ret;
            }

            public T GetData(K key)
            {
                if (Data.ContainsKey(key)) return Data[key];
                else return default(T);
            }
#if UNITY_EDITOR
            public K[] GetKeys()
            {
                return Data.Keys.ToArray();
            }
#endif
        }

        //公開マスターデータ
        static public List<Chapter> Chapters => _instance._chapters;
        static public List<Quest> Quests => _instance._quests;
        static public List<GameEvent> Events => _instance._events;

        //マスターデータたち
        List<Chapter> _chapters = new List<Chapter>();
        List<Quest> _quests = new List<Quest>();
        List<GameEvent> _events = new List<GameEvent>();

        //NOTE: そもそもコードで参照するのであればべた書きもあり
        PrettyData<int, Card> _cardMaster = new PrettyData<int, Card>();
        PrettyData<int, Chapter> _chapterMaster = new PrettyData<int, Chapter>();
        PrettyData<int, Quest> _questMaster = new PrettyData<int, Quest>();
        PrettyData<int, Item> _itemMaster = new PrettyData<int, Item>();
        PrettyData<int, GameEvent> _eventMaster = new PrettyData<int, GameEvent>();
        PrettyData<string, TextData> _textMaster = new PrettyData<string, TextData>();

        //読み込み管理
        public bool IsSetupComplete => (_isInit && _loadingCount == 0);
        delegate void LoadMasterDataCallback<T>(T data);
        int _loadingCount = 0;
        bool _isInit = false;
        bool _useCache = false;
        Action _onLoadCallback = null;
        Dictionary<string, int> _versionInfos = new Dictionary<string, int>();

        string GetFileName(string sheetName)
        {
            return string.Format("{0}/{1}.json", DataPrefix, sheetName);
        }

        public async UniTask<int> Setup(bool useCache = true, MasterVersion[] versionInfos= null, Action onLoadCallback = null)
        {
            URI = GameSetting.MasterDataAPIURI;

            _useCache = useCache;
            _onLoadCallback = onLoadCallback;
            _loadingCount = 0;

            //Map
            if (versionInfos != null)
            {
                foreach (var v in versionInfos)
                {
                    _versionInfos.Add(v.masterName, v.version);
                }
            }

            //マスタ読み込み
            Debug.Log("MasterData Load Start.");

            //NOTE: そもそもコードで参照するのであればべた書きもあり
            List<UniTask> masterDataDownloads = new List<UniTask>()
            {
                LoadMasterData<TextMaster>("JP_Text"),
                LoadMasterData<TextMaster>("EN_Text"),
                LoadMasterData<CardMaster>("Card"),
                LoadMasterData<ChapterMaster>("Chapter"),
                LoadMasterData<QuestMaster>("Quest"),
                LoadMasterData<EventMaster>("Event"),
                LoadMasterData<ItemMaster>("Item"),
                LoadMasterData<EffectMaster>("Effect"),
            };
            await UniTask.WhenAll(masterDataDownloads.ToArray());
            await ConstructMasterData();

            return 0;
        }

        /// <summary>
        /// 最後の整形処理をする
        /// </summary>
        private async UniTask ConstructMasterData()
        {
            //マスタ結合 or 整形

            //テキストマスタを設定する
            //日本語を使う
            //TODO: 言語設定を見る
            var jp_text = await LocalData.LoadAsync<TextMaster>(GetFileName("JP_Text"));
            _textMaster = PrettyData<string, TextData>.Create(jp_text.Data, (TextData line) => { return line.Key == "" ? null : line.Key; });

            //カードマスタをマージする
            var card = await LocalData.LoadAsync<CardMaster>(GetFileName("Card"));
            var item = await LocalData.LoadAsync<ItemMaster>(GetFileName("Item"));
            var chapter = await LocalData.LoadAsync<ChapterMaster>(GetFileName("Chapter"));
            var quest = await LocalData.LoadAsync<QuestMaster>(GetFileName("Quest"));
            var effect = await LocalData.LoadAsync<EffectMaster>(GetFileName("Effect"));
            var evt = await LocalData.LoadAsync<EventMaster>(GetFileName("Event"));
            var efectList = PrettyData<int, EffectData>.Create(effect.Data, (EffectData line) => { return line.Id; });

            List<Card> cards = new List<Card>();
            foreach (var c in card.Data)
            {
                //カードデータを組み合わせていく
                Card d = new Card();
                d.Id = c.Id;
                d.Name = c.Name;
                d.Rare = c.Rare;
                d.Resource = c.Resource;
                d.Effect = efectList.GetData(c.EffectId);
                cards.Add(d);
            }
            _cardMaster = PrettyData<int, Card>.Create(cards.ToArray(), (Card line) => { return line.Id; });

            //アイテムマスタをマージする
            List<Item> items = new List<Item>();
            foreach (var i in item.Data)
            {
                //カードデータを組み合わせていく
                Item d = new Item();
                d.Id = i.Id;
                d.Name = i.Name;
                d.Type = (ItemType)i.Type;
                d.Resource = i.Resource;
                d.Effect = efectList.GetData(i.EffectId);
                items.Add(d);
            }
            _itemMaster = PrettyData<int, Item>.Create(items.ToArray(), (Item line) => { return line.Id; });

            //イベントマスタを整形する
            _events.Clear();
            foreach (var ev in evt.Data)
            {
                //カードデータを組み合わせていく
                GameEvent d = new GameEvent();
                d.Id = ev.Id;
                d.Name = ev.Name;
                d.Resource = ev.Resource;
                d.StartAt = DateTime.Parse(ev.StartAt);
                d.GameEndAt = DateTime.Parse(ev.GameEndAt);
                d.EndAt = DateTime.Parse(ev.EndAt);
                _events.Add(d);
            }
            _eventMaster = PrettyData<int, GameEvent>.Create(_events.ToArray(), (GameEvent line) => { return line.Id; });

            //クエストマスタをマージしていく
            _quests.Clear();
            foreach (var q in quest.Data)
            {
                Quest d = new Quest();
                d.Id = q.Id;
                d.Name = q.Name;
                d.Resource = q.Resource;
                d.ChapterId = q.ChapterId;
                d.MovePoint = q.MovePoint;
                _quests.Add(d);
            }

            _chapters.Clear();
            foreach (var c in chapter.Data)
            {
                Chapter d = new Chapter();
                d.Id = c.Id;
                d.Name = c.Name;
                d.Resource = c.Resource;
                d.QuestType = c.QuestType;
                d.Condition = c.Condition;
                d.QuestList = _quests.Where(q => q.ChapterId == c.Id).ToList();
                _chapters.Add(d);
            }
            _questMaster = PrettyData<int, Quest>.Create(_quests.ToArray(), (Quest line) => { return line.Id; });
            _chapterMaster = PrettyData<int, Chapter>.Create(_chapters.ToArray(), (Chapter line) => { return line.Id; });

            Debug.Log("MasterData Load Done.");
            _isInit = true;
            _onLoadCallback?.Invoke();
        }

        /// <summary>
        /// マスタデータ読み込み関数
        /// </summary>
        /// <typeparam name="T">マスタの型</typeparam>
        /// <param name="sheetName">シート名</param>
        private async UniTask LoadMasterData<T>(string sheetName) where T : MasterDataBase
        {
            var filename = GetFileName(sheetName);
            var data = await LocalData.LoadAsync<T>(filename);
            bool isUpdate = data == null || !_useCache;
            if(!isUpdate && _versionInfos.ContainsKey(sheetName))
            {
                Debug.Log($"Server:{_versionInfos[sheetName]} > Local:{data.Version}");
                isUpdate = _versionInfos[sheetName] > data.Version;
            }
            if (isUpdate)
            {
                _loadingCount++;
                string json = await GetRequest(string.Format("{0}?sheet={1}", URI, sheetName));
                Debug.Log(json);
                T dt = JsonUtility.FromJson<T>(json);
                await LocalData.SaveAsync<T>(filename, dt);
                Debug.Log("Network download. : " + filename + " / " + json + "/" + filename);
            }
            else
            {
                Debug.Log("Localfile used. : " + filename);
            }
        }




        //データ取得用ラッパー
        //TODO:

        /// <summary>
        /// テキスト取得
        /// </summary>
        /// <param name="key">テキストのキー</param>
        /// <returns>テキスト</returns>
        static public string GetLocalizedText(string key)
        {
            return _instance._textMaster.GetData(key)?.Text;
        }

        /// <summary>
        /// カード取得
        /// </summary>
        /// <param name="Id">カードのId</param>
        /// <returns>カード情報</returns>
        static public Card GetCard(int Id)
        {
            return _instance._cardMaster.GetData(Id);
        }

        /// <summary>
        /// アイテム取得
        /// </summary>
        /// <param name="Id">アイテムのId</param>
        /// <returns>アイテム情報</returns>
        static public Item GetItem(int Id)
        {
            return _instance._itemMaster.GetData(Id);
        }

        /// <summary>
        /// チャプターデータ取得
        /// </summary>
        /// <param name="Id">チャプターId</param>
        /// <returns>チャプター情報</returns>
        static public Chapter GetChapter(int Id)
        {
            return _instance._chapterMaster.GetData(Id);
        }

        /// <summary>
        /// クエストデータ取得
        /// </summary>
        /// <param name="Id">クエストId</param>
        /// <returns>クエスト情報</returns>
        static public Quest GetQuest(int Id)
        {
            return _instance._questMaster.GetData(Id);
        }

        /// <summary>
        /// イベントデータ取得
        /// </summary>
        /// <param name="Id">イベントId</param>
        /// <returns>イベント情報</returns>
        static public GameEvent GetEvent(int Id)
        {
            return _instance._eventMaster.GetData(Id);
        }


#if UNITY_EDITOR
        static public string[] GetTextKeys()
        {
            return _instance._textMaster.GetKeys();
        }
#endif
    }
}