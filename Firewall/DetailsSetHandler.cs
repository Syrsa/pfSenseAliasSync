using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firewall
{
    class DetailsSetHandler : DetailsHandler
    {
        List<KeyValuePair<string, string>> unHanledHosts;
        Alias alias;

        public DetailsSetHandler(WebCompleteHandler in_aliasHandler, Alias in_alias) : base(in_aliasHandler)
        {
            aliasHandler = in_aliasHandler;
            alias = in_alias;
            unHanledHosts = new List<KeyValuePair<string, string>>();
            unHanledHosts.AddRange(alias.Hosts);
        }

        override protected void HandleAlias()
        {
            SetElementAttribute("name", "value", alias.Name);
            SetElementAttribute("descr", "value", alias.Description);



            int i = 0;
            while (FindElement("address" + i.ToString()))
            {
                List<KeyValuePair<string, string>> HandledHosts = new List<KeyValuePair<string, string>>();
           
                foreach (KeyValuePair<string, string> host in unHanledHosts)
                {
                    if (GetElementAttribute("address" + i.ToString(), "value") == host.Key)
                    {
                        HandledHosts.Add(host); //break; ?????
                    }
                }

                foreach (KeyValuePair<string, string> host in HandledHosts)
                {
                    unHanledHosts.Remove(host);
                }
                i++;
            }

            HtmlElementCollection elems = aliasHandler.GetMainForm().webBrowser.Document.GetElementsByTagName("img");
            foreach (HtmlElement elem in elems)
            {
                if (elem.GetAttribute("title") == "add another entry")
                {
                    foreach (KeyValuePair<string, string> host in unHanledHosts)
                    {
                        elem.InvokeMember("click");
                        SetElementAttribute("address" + i.ToString(), "value", host.Key);
                        SetElementAttribute("detail" + i.ToString(), "value", host.Value);
                        i++;
                    }             
                }
            }

            aliasHandler.GetMainForm().webBrowser.Document.GetElementById("submit").InvokeMember("click");
        }


            
    }
}
