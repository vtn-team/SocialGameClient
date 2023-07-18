using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Outgame
{
    public class NotionBlock : UIView
    {
        protected virtual void Setup(APIResponceInfomationContents content)
        {
            this.RectTransform.sizeDelta.Set(1080,50);
        }

        static public GameObject CreateBlock(APIResponceInfomationContents content, Transform parent)
        {
            var origin = Addressables.LoadAssetAsync<GameObject>(string.Format("Assets/Prefabs/Information/{0}.prefab", content.type)).WaitForCompletion();
            var listItem = GameObject.Instantiate(origin, parent);
            var script = listItem.GetComponent<NotionBlock>();
            script.Setup(content);
            Addressables.Release(origin);
            return listItem;
        }
    }
}
