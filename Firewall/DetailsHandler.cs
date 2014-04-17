using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firewall
{
    class DetailsHandler : WebCompleteHandler
    {
        public WebCompleteHandler aliasHandler;

        public DetailsHandler(WebCompleteHandler in_aliasHandler)
        {
            aliasHandler = in_aliasHandler;
        }

        public void Execute()
        {
            HandleAlias();
            aliasHandler.GetMainForm().webBrowser.Navigate("https://10.90.97.40/firewall_aliases.php");
            aliasHandler.GetMainForm().webCompleteHandler = aliasHandler;
            aliasHandler.GetMainForm().Refresh();
        }


        public MainForm GetMainForm()
        {
            return aliasHandler.GetMainForm();
        }


        virtual protected void HandleAlias()
        {
            Alias newAlias;

            newAlias = new Alias(GetElementAttribute("name", "value"), GetElementAttribute("descr", "value"));
            int i = 0;
            while (FindElement("address" + i.ToString()))
            {
                newAlias.AddHost(GetElementAttribute("address" + i.ToString(), "value"), GetElementAttribute("detail" + i.ToString(), "value"));
                i++;
            }
            aliasHandler.GetMainForm().AddAliasToCurrentProfile(newAlias);
        }

        public string GetElementAttribute(string elementId, string attributeName)
        {
            return aliasHandler.GetMainForm().webBrowser.Document.GetElementById(elementId).GetAttribute(attributeName);
        }

        public void SetElementAttribute(string elementId, string attributeName, string attributeValue)
        {
            aliasHandler.GetMainForm().webBrowser.Document.GetElementById(elementId).SetAttribute(attributeName, attributeValue);
        }

        public bool FindElement(string elementId)
        {
            return (aliasHandler.GetMainForm().webBrowser.Document.GetElementById(elementId) != null);
        }
    }
}
