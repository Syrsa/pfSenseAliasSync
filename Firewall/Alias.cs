using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Firewall
{
    public class Alias
    {
                
        private string name;
        private string description;
        private IDictionary<string, string> hosts;

        [Description("Name of the alias")]
        public string Name
        {
            get { return name; }
            private set { name = value; }
        }


        [Description("Description of the alias")]
        public string Description
        {
            get { return description; }
            private set { description = value; }
        }

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Description("List of hosts")]
        public IDictionary<string, string> Hosts
        {
            get { return hosts; }
            private set { }
        }

        public Alias()
        {
        }

        public Alias(string name, string description)
        {
            this.Name = name;
            this.description = description;
            hosts = new Dictionary<string,string>();
        }

        public void AddHost(string ip, string description)
        {
            this.hosts.Add(ip, description);
        }

        public string StringifyAlias()
        {
            string _return = "";
            _return += "aliasName=" + name + ";" + "descr=" + description + ";";
            foreach (KeyValuePair<string, string> host in hosts)
            {
                _return += "ip=" + host.Key + ";" + "detail=" + host.Value + ";";
            }
            return _return;
        }
    }
}
