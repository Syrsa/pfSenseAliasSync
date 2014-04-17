using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firewall
{
    class LoginComplete : WebCompleteHandler
    {
        MainForm form;

        public LoginComplete(MainForm in_form)
        {
            this.form = in_form;
        }

        public void Execute()
        {
            form.webBrowser.Navigate("https://10.90.97.40/firewall_aliases.php");
            form.webCompleteHandler = new ListAliasHandler(form);
        }

        public MainForm GetMainForm()
        {
            return form;
        }

    }
}
