using System;
using System.Collections.Generic;
using System.Linq;

namespace MD
{
    //スプレッドシートからダウンロードしてくるデータたち
    [Serializable]
    public class TextData
    {
        public string Key;
        public string Text;
    }

    [Serializable]
    public class TextMaster
    {
        public int Version;
        public TextData[] Data;
    }

    [Serializable]
    public class CardData
    {
        public int Id;
        public string Name;
        public int Rare;
        public int EffectId;
    }

    [Serializable]
    public class CardMaster
    {
        public int Version;
        public CardData[] Data;
    }


    [Serializable]
    public class EffectData
    {
        public int Id;
        public string Text;
    }

    [Serializable]
    public class EffectMaster
    {
        public int Version;
        public EffectData[] Data;
    }
    //


    /// <summary>
    /// マージ後のデータ
    /// </summary>
    public class CardEffect
    {
        public string Text;
    }

    public class Card
    {
        public int Id;
        public string Name;
        public int Rare;
        public CardEffect Effect;
    }
}