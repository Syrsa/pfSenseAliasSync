using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firewall
{
    class ListAliasSetValueHandler : WebCompleteHandler
    {
        public MainForm form;
        int listCounter = 0;
        List<Alias> unHandledAliases = new List<Alias>();

        public ListAliasSetValueHandler(MainForm in_form, List<Alias> aliases)
        {
            unHandledAliases.AddRange(aliases);
            form = in_form;
        }

        private void webload(int i)
        {
            form.webBrowser.Navigate("https://10.90.97.40/firewall_aliases_edit.php?id=" + i.ToString());
            form.webBrowser.Refresh();
        }

        public MainForm GetMainForm()
        {
            return form;
        }


        public void Execute()
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

                        HtmlElement iconTable = elem;
                        while (iconTable.GetAttribute("summary").ToLower() != "icons")
                        {
                            iconTable = iconTable.Parent;
                        }

                        HtmlElement trRow = iconTable;
                        while (trRow.TagName.ToLower() != "tr")
                        {
                            trRow = trRow.Parent;
                        }

                        HtmlElement tdName = trRow.FirstChild;
                        while (tdName.TagName.ToLower() != "td")
                        {
                            tdName = tdName.NextSibling;
                        }

                        listCounter = i + 1;
                        
                        foreach (Alias alias in unHandledAliases)
                        {
                            if (alias.Name == tdName.InnerHtml.Trim())
                            {
                                unHandledAliases.Remove(alias);

                                MessageBox.Show(tdName.InnerHtml.Trim());

                                form.webCompleteHandler = new DetailsSetHandler(this, alias);
                                elem.InvokeMember("click");
                                webload(i);
                                break;
                            }
                        }

                    }
                    i++;
                }
            }
        }

    }
}
