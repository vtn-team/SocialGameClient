using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using MD;

namespace Outgame
{
    /// <summary>
    /// 
    /// </summary>
    public class LocalizedButton : MonoBehaviour
    {
        [SerializeField] TMPro.TextMeshProUGUI _text;
        [SerializeField, LocalizeText] string _textKey;

        private void Awake()
        {
            _text.text = "";
        }

        void Start()
        {
            StartCoroutine("SetText");
        }

        IEnumerator SetText()
        {
            if(!MasterData.Instance.IsSetupComplete) yield return null;
            var text = MasterData.GetLocalizedText(_textKey);
            if(text == null) yield return null;

            _text.text = text;
            yield break;
        }
    }
}