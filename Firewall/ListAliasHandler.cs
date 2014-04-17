using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Firewall
{
    class ListAliasHandler : WebCompleteHandler
    {
        public MainForm form;
        protected int listCounter = 0;

        public ListAliasHandler(MainForm in_form)
        {
            form = in_form;
        }

        public MainForm GetMainForm()
        {
            return form;
        }


        protected void webload(int i)
        {
            form.webBrowser.Navigate("https://10.90.97.40/firewall_aliases_edit.php?id=" + i.ToString());
            form.webBrowser.Refresh();
        }

        virtual protected void HandleItem(HtmlElement elem, int index)
        {
            form.webCompleteHandler = new DetailsHandler(this);
            elem.InvokeMember("click");
            webload(index);

        }

        public virtual void Execute()
        {
            form.webBrowser.ScriptErrorsSuppressed = true;
            int i = 0;
            HtmlElementCollection elems = form.webBrowser.Document.GetElementsByTagName("img");
            foreach (HtmlElement elem in elems)
            {
                if (elem.GetAttribute("alt") == "edit")
                {

                    if (listCounter <= i)
                    {
                        listCounter = i + 1;
                        HandleItem(elem, i);
                        return;
                    }
                    i++;
                }
            }
        }
    }
}
