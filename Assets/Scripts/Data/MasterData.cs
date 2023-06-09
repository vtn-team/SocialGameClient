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

        //マスターデータたち
        //NOTE: そもそもコードで参照するのであればべた書きもあり
        PrettyData<int, Card> _cardMaster = new PrettyData<int, Card>();
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

            List<string> masterDataList = new List<string>() { "JP_Text", "EN_Text", "Card", "Effect" };
            //NOTE: そもそもコードで参照するのであればべた書きもあり
            List<UniTask> masterDataDownloads = new List<UniTask>()
            {
                LoadMasterData<TextMaster>("JP_Text"),
                LoadMasterData<TextMaster>("EN_Text"),
                LoadMasterData<CardMaster>("Card"),
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
            var effect = await LocalData.LoadAsync<EffectMaster>(GetFileName("Effect"));
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
                d.Effect = new CardEffect();
                d.Effect.Text = efectList.GetData(c.EffectId)?.Text;
                cards.Add(d);
            }
            _cardMaster = PrettyData<int, Card>.Create(cards.ToArray(), (Card line) => { return line.Id; });

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

#if UNITY_EDITOR
        static public string[] GetTextKeys()
        {
            return _instance._textMaster.GetKeys();
        }
#endif
    }
}