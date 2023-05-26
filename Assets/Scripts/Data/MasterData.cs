using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;

namespace MD
{
    /// <summary>
    /// マスターデータ管理クラス
    /// </summary>
    public class MasterData
    {
        //設定系
        const string URI = "";
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

        string GetFileName(string sheetName)
        {
            return string.Format("{0}/{1}.json", DataPrefix, sheetName);
        }

        public void Setup(bool useCache = false, bool forceReload = false)
        {
            if (_isInit && !forceReload) return;

            _useCache = useCache;
            _loadingCount = 0;

            //マスタ読み込み
            Debug.Log("MasterData Load Start.");
            //NOTE: そもそもコードで参照するのであればべた書きもあり
            LoadMasterData<TextMaster>("JP_Text");
            LoadMasterData<TextMaster>("EN_Text");
            LoadMasterData<CardMaster>("Card");
            LoadMasterData<EffectMaster>("Effect");
            MasterDataCheck();
        }

        /// <summary>
        /// 最後の整形処理をする
        /// </summary>
        private void MasterDataCheck()
        {
            if (_loadingCount > 0) return;

            //マスタ結合 or 整形

            //テキストマスタを設定する
            //日本語を使う
            //TODO: 言語設定を見る
            var jp_text = LocalData.Load<TextMaster>(GetFileName("JP_Text"));
            _textMaster = PrettyData<string, TextData>.Create(jp_text.Data, (TextData line) => { return line.Key; });

            //カードマスタをマージする
            var card = LocalData.Load<CardMaster>(GetFileName("Card"));
            var effect = LocalData.Load<EffectMaster>(GetFileName("Effect"));
            var efectList = PrettyData<int, EffectData>.Create(effect.Data, (EffectData line) => { return line.Id; });

            List<Card> cards = new List<Card>();
            foreach (var c in card.Data)
            {
                //カードデータを組み合わせていく
                Card d = new Card();
                d.Id = c.Id;
                d.Name = c.Name;
                d.Rare = c.Rare;
                d.Effect = new CardEffect();
                d.Effect.Text = efectList.GetData(c.EffectId)?.Text;
                cards.Add(d);
            }
            _cardMaster = PrettyData<int, Card>.Create(cards.ToArray(), (Card line) => { return line.Id; });

            Debug.Log("MasterData Load Done.");
            _isInit = true;
        }

        /// <summary>
        /// マスタデータ読み込み関数
        /// </summary>
        /// <typeparam name="T">マスタの型</typeparam>
        /// <param name="sheetName">シート名</param>
        private void LoadMasterData<T>(string sheetName)
        {
            var filename = GetFileName(sheetName);
            var data = LocalData.Load<T>(filename);
            if (data == null || !_useCache)
            {
                _loadingCount++;
                Network.WebRequest.GetRequest(string.Format("{0}?sheet={1}", URI, sheetName), (byte[] data) =>
                {
                    string json = Encoding.UTF8.GetString(data);
                    Debug.Log(json);
                    T dt = JsonUtility.FromJson<T>(json);

                    LocalData.Save<T>(filename, dt);
                    Debug.Log("Network download. : " + filename + " / " + json + "/" + filename);
                    --_loadingCount;

                    MasterDataCheck();
                });
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