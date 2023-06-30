using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    [Serializable]
    public class APIResponceBase
    {
        public int status;
        public string token;
    }


    [Serializable]
    public class APIResponceCard
    {
        public int id;
        public int cardId;
        public int level;
        public int luck;
    }

    [Serializable]
    public class APIResponceItem
    {
        public int itemId;
        public int count;
    }

    [Serializable]
    public class APIResponceQuest
    {
        public int questId;
        public int score;
        public int clearFlag;
    }


    [Serializable]
    public class APIResponceGetCards : APIResponceBase
    {
        public APIResponceCard[] cards;
    }

    [Serializable]
    public class APIResponceGetItems : APIResponceBase
    {
        public APIResponceItem[] items;
    }

    [Serializable]
    public class APIResponceGetQuests : APIResponceBase
    {
        public APIResponceQuest[] quests;
    }
}
