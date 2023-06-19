using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    public class UIStackableView : UIView
    {
        public ViewID ViewId { get; protected set; }

        protected UIInformationBase _info = null;

        protected bool _hasPopUI = false;
        public bool HasPopUI => _hasPopUI;

        public void SetInformation(UIInformationBase info)
        {
            _info = info;
        }

        public virtual void Enter()
        {
            Active();
        }

        public virtual void Exit()
        {
            Disactive();
        }
    }
}
