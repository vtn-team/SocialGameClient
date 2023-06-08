using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outgame
{
    public class GameAPI
    {
        //TODO: データから参照する形式だとプログラムの修正が少なくて済む
        IGameAPIImplement _inplements = new NodeJSImplement();

        static GameAPI _instance = new GameAPI();

        static public IGameAPIImplement API => _instance?._inplements;
    }
}