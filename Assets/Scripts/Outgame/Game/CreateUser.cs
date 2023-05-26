using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace Outgame
{
    public class CreateUser : MonoBehaviour
    {
        [SerializeField] TMP_InputField _input;

        bool _isCreate = false;
        private void Start()
        {
            _input.Select();
        }


        public void UserCreate()
        {
            string name = _input.text;
            if (name == "") return;
            //if (_isCreate) return;

            _isCreate = true;

            GameAPI.API.CreateUser(name, (data) =>
            {
                User.Create(data.udid);

                //UnityEngine.SceneManagement.SceneManager.LoadScene((int)SCENEID.Field);
            });
        }
    }
}