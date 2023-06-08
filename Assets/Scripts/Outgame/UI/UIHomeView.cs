using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    public class UIHomeView : UIStackableView
    {
        public void GoGacha()
        {
        }

        protected override void AwakeCall()
        {
            ViewId = ViewID.Home;
            _hasPopUI = true;
        }
    }
}
