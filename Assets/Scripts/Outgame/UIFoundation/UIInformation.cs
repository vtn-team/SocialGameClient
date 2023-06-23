using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outgame
{
    public class UIInformationBase
    {
        public T Information<T>() where T : UIInformationBase => this as T;
    }
}
