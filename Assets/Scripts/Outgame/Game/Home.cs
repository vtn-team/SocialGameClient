using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Outgame
{
    public class Home : MonoBehaviour
    {
        /*
        [SerializeField]
        class HomeButton
        {
            public ;
            public Button Button;
        }
        [SerializeField] List<HomeButton> _buttons = new List<HomeButton>();
        */

        bool _isCreate = false;
        private void Start()
        {

        }

        public void GachaDrawSingle(int gachaId)
        {
            GameAPI.API.Gacha(gachaId, 1, (data) =>
            {

            });
        }
        public void GachaDrawMulti(int gachaId)
        {
            //TODO: ひけるだけ引くという処理は必要
            GameAPI.API.Gacha(gachaId, 10, (data) =>
            {

            });
        }
    }
}