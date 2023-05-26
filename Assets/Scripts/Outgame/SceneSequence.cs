using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Outgame
{
    public class SceneSequence : MonoBehaviour
    {
        enum SceneIdentifier
        {
            Title,
            UserCreate,
        }

        SceneIdentifier _sequence;

        private void Update()
        {
            switch (_sequence)
            {
            }
        }
    }
}
