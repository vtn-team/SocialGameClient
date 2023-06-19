using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outgame
{
    public class ListBadgeCount : ListBadge
    {
        [SerializeField] TMPro.TextMeshProUGUI _count;

        protected override void AwakeCall()
        {
            base.AwakeCall();
            _count.text = "";
        }

        public void SetCount(int count)
        {
            _count.text = count.ToString();
        }
    }
}