using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firewall
{
    class ListAliasSetValueHandler2 : ListAliasHandler
    {
        public List<Alias> unHandledAliases = new List<Alias>();

        public ListAliasSetValueHandler2(MainForm in_form, List<Alias> aliases)
            : base(in_form)
        {
            unHandledAliases.AddRange(aliases);
        }


        override protected void HandleItem(HtmlElement elem, int index)
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

            foreach (Alias alias in unHandledAliases)
            {
                if (alias.Name == tdName.InnerHtml.Trim())
                {
                    unHandledAliases.Remove(alias);

                    //MessageBox.Show(tdName.InnerHtml.Trim());
                    elem.InvokeMember("click");
                    //webload(index);
                    form.webCompleteHandler = new DetailsSetHandler(this, alias);
                    break;
                }
            }

        }

        public override void Execute()
        {
            //this.GetMainForm().webBrowser.Refresh();
            //base.Execute();

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

            foreach (Alias alias in unHandledAliases)
            {
                unHandledAliases.Remove(alias);
                HtmlElement addbtn = AddAliasButton();
                if (addbtn != null)
                {
                    addbtn.InvokeMember("click");
                }
                else
                {
                   // MessageBox.Show("Add alias button - NOT found!");
                }
                listCounter++;
                form.webBrowser.Navigate("https://10.90.97.40/firewall_aliases_edit.php?tab=ip");
                form.webCompleteHandler = new DetailsSetHandler(this, alias);
                break;
            }
        }

        private HtmlElement AddAliasButton()
        {
            HtmlElementCollection elems = form.webBrowser.Document.GetElementsByTagName("img");
            foreach (HtmlElement elem in elems)
            {
                if (elem.GetAttribute("alt") == "add")
                {
                    elem.InvokeMember("click");
                }

            }
            return null;
        }
    }
}

