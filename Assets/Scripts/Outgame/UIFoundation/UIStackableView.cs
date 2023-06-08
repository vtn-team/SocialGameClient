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

        protected bool _hasPopUI = false;
        public bool HasPopUI => _hasPopUI;


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
