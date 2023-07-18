using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    public class InformationText : NotionBlock
    {
        [SerializeField] TMPro.TextMeshProUGUI _text;

        protected override void Setup(APIResponceInfomationContents content)
        {
            string text = content.param[0];
            var matches = Regex.Matches(text, @"{%([\s\S]*)}");
            foreach (Match m in matches)
            {
                string key = m.Groups[1].Value;
                switch (key)
                {
                    case "PlayerName":
                        text = text.Replace(m.Value, "主人公");
                        break;
                }
            }

            _text.text = text;
            int lines = text.Count(s => s == '\n');

            this.RectTransform.sizeDelta.Set(1080, 50 * lines);
        }
    }
}
