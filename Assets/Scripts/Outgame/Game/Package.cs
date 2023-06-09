using Cysharp.Threading.Tasks;
using Outgame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Outgame.Game
{
    public class LoginPackage : SequencePackage
    {
        public APIResponceLogin Login { get; set; }
        public APIResponceGetCards Cards { get; set; }
    }

    public class GachaDrawPackage : SequencePackage
    {
        public APIResponceGachaDraw Gacha { get; set; }
    }
    
}
