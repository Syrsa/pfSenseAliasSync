using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firewall
{
    public interface WebCompleteHandler
    {
        void Execute();
        MainForm GetMainForm();
    }
}
