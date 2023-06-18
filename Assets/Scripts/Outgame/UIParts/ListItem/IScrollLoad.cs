using MD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine;

namespace Outgame
{
    public interface IScrollLoad
    {
        //スクロール用インタフェース
        void Load();
        void Release();
    }
}
