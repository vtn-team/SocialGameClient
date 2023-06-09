﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Outgame.ListView;

namespace Outgame
{
    /// <summary>
    /// Compositeパターンの大元
    /// </summary>
    public abstract class ListItemBase : MonoBehaviour, IScrollLoad
    {
        public enum ListType
        {
            CardList = 1,
            EnhanceBaseSelect = 2,
            EnhanceMaterialSelect = 3,
        }

        protected int _index = -1;
        protected int _budgeIndex = -1;
        protected ListBadge _badge;
        protected ListSelectEvent _evt = null;

        public int Index => _index;
        public int BudgeIndex => _budgeIndex;

        static public T ListItemSetup<T>(int index, GameObject target, ListSelectEvent evt) where T : ListItemBase
        {
            if (target == null) { Debug.LogError("Prefabアイテムが存在しない"); return null; }

            var script = target.GetComponent<T>();
            if (script == null)
            {
                script = target.AddComponent<T>();
            }
            script._index = index;
            script._evt = evt;
            script.Bind(target);

            var button = target.GetComponent<Button>();
            if(button)
            {
                button.onClick.AddListener(script.OnClick);
            }

            //バッジはビューによってあったりなかったりするのでnullを許容する
            script._badge = target.GetComponentInChildren<ListBadge>();

            return script;
        }

        protected virtual void OnClick()
        {
            _evt?.Invoke(0, _index);
        }

        public virtual void SetBudge(int bindex)
        {
            _budgeIndex = bindex;
            if (bindex != -1)
            {
                _badge?.Active();
            }
            else
            {
                _badge?.Disactive();
            }
        }

        public abstract void Bind(GameObject target);
        public abstract void Load();
        public abstract void Release();
    }
}
