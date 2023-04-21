using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameAPI : MonoBehaviour
{
    [SerializeReference, SubclassSelector] INetworkImplement _inplements;

    static GameAPI _instance = null;

    static public INetworkImplement API => _instance?._inplements;

    private void Awake()
    {
        _instance = this;
    }

}

