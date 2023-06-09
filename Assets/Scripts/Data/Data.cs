using System;
using System.Collections.Generic;
using System.Linq;

namespace MD
{
    [Serializable]
    public class MasterVersion
    {
        public string masterName;
        public int version;
    }

    [Serializable]
    public class MasterDataBase
    {
        public int Version;
        //xxx[] Data;
    }

    //スプレッドシートからダウンロードしてくるデータたち
    [Serializable]
    public class TextData
    {
        public string Key;
        public string Text;
    }

    [Serializable]
    public class TextMaster : MasterDataBase
    {
        public TextData[] Data;
    }

    [Serializable]
    public class CardData
    {
        public int Id;
        public string Name;
        public string Resource;
        public int Rare;
        public int EffectId;
    }

    [Serializable]
    public class CardMaster : MasterDataBase
    {
        public CardData[] Data;
    }


    [Serializable]
    public class EffectData
    {
        public int Id;
        public string Text;
    }

    [Serializable]
    public class EffectMaster : MasterDataBase
    {
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
        public string Resource;
        public CardEffect Effect;
    }
}