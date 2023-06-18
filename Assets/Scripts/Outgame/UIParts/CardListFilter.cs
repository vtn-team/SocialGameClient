using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    public class CardListFilter
    {
        public enum FilterType
        {
            All,
            //TODO:
        }

        static public List<CardData> FilteredList(FilterType type)
        {
            List<CardData> ret = new List<CardData>();
            switch(type)
            {
            case FilterType.All: ret = CardListModel.CardList.List; break;
            }
            return ret;
        }
    }
}
