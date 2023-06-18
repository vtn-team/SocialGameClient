using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    public class ItemListFilter
    {
        public enum FilterType
        {
            All,
            //NOTE: アイテムと同一
            //TODO: DRY
            QuestPoint,
            EnhanceItem,
            EvolutionItem,
            StoryItem,

            //TODO:
            Anything = 1000,
        }

        static public List<ItemData> FilteredList(FilterType type = 0)
        {
            List<ItemData> ret = new List<ItemData>();
            switch (type)
            {
                case FilterType.All:
                    ret = ItemListModel.ItemList.List;
                    break;

                case FilterType.QuestPoint:
                case FilterType.EnhanceItem:
                case FilterType.EvolutionItem:
                case FilterType.StoryItem:
                    {
                        ItemType iType = (ItemType)type;
                        ret = ItemListModel.ItemList.List.Where(i => i.Type == iType).ToList();
                    }
                    break;
            }
            return ret;
        }
    }
}
