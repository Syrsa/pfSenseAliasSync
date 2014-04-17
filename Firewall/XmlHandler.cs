using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Windows.Forms;


namespace Firewall
{
    public static class XmlHandler
    {
        
        public static void XmlWriter(Profile profile, string filepath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("Profile");
            xmlDoc.AppendChild(rootNode);

            foreach (Alias alias in profile.aliases)
            {
                rootNode.AppendChild(CreateXMLElement(xmlDoc, alias));
            }

            xmlDoc.Save(@filepath);

        }

        public static Profile XmlReader(string filepath)
        {
            Profile savedProfile = new Profile();
            Alias alias;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(@filepath);
            foreach (XmlNode node in xmlDoc.DocumentElement)
            {
                alias = new Alias(node.Attributes[0].Value, node["Description"].InnerText);
                foreach (XmlNode host in node.ChildNodes)
                {
                    if(host.Name == "Host")
                    alias.AddHost(host.Attributes[0].Value,host.InnerText);
                }
                savedProfile.aliases.Add(alias);
            }
            return savedProfile;
        }

        private static XmlElement CreateXMLElement(XmlDocument xmlDoc, Alias alias)
        {
            XmlElement aliasNode = xmlDoc.CreateElement("Alias");
            aliasNode.SetAttribute("Name", alias.Name);

            XmlElement description = xmlDoc.CreateElement("Description");
            description.InnerText = alias.Description;
            aliasNode.AppendChild(description);

            XmlElement hostnode;
            foreach (KeyValuePair<string, string> host in alias.Hosts)
            {
                hostnode = xmlDoc.CreateElement("Host");
                hostnode.SetAttribute("Ip", host.Key);
                hostnode.InnerText = host.Value;
                aliasNode.AppendChild(hostnode);
            }
            return aliasNode;
        }


    }
}
