using MD;
using Outgame;
using System;
using UnityEngine;

namespace Outgame
{
    /// <summary>
    /// 
    /// </summary>
    public class MasterDataUpdate : MonoBehaviour
    {
        public void MasterUpdate()
        {
            MasterData.Instance.Setup(false);
        }
    }
}
