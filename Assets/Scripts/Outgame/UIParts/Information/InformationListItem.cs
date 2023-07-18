using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    public class InformationListItem : MonoBehaviour
    {
        public delegate void InfomationSelectCallback(string id);
        [SerializeField] TMPro.TextMeshProUGUI _title;

        string _id;
        InfomationSelectCallback _callback;

        public void Setup(APIResponceInfomationListItem item, InfomationSelectCallback cb)
        {
            _callback = cb;
            _id = item.id;
            _title.text = item.title;
        }

        public void Select()
        {
            _callback?.Invoke(_id);
        }
    }
}
