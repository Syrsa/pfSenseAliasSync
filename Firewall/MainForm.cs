using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
//using mshtml;

namespace Firewall
{
    public partial class MainForm : Form
    {
        public WebCompleteHandler webCompleteHandler = null;
        
        private Profile currentprofile;
        public Profile savedprofile;

        int listCounter = 0;

        public MainForm()
        {
            InitializeComponent();
            webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(browser_DocumentCompleted);
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            if (webBrowser.Url.Equals(@"https://10.90.97.40/index.php"))
            {
                currentprofile = new Profile();
                webBrowser.Document.GetElementById("usernamefld").SetAttribute("value", "admin");
                webBrowser.Document.GetElementById("passwordfld").SetAttribute("value", "pfsense");
                HtmlElementCollection elc = this.webBrowser.Document.GetElementsByTagName("input");
                foreach (HtmlElement el in elc)
                {
                    if (el.GetAttribute("type").Equals("submit"))
                    {
                        el.InvokeMember("click");
                    }
                }
                webCompleteHandler = new LoginComplete(this);
            }
            else
            {
                currentprofile = new Profile();
                webCompleteHandler = new LoginComplete(this);
                webBrowser.Navigate(@"https://10.90.97.40/firewall_aliases.php");
            }
        }

        private void browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webCompleteHandler != null)
            {
                WebCompleteHandler tmpHandler = webCompleteHandler;
                webCompleteHandler = null;
                tmpHandler.Execute();
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (currentprofile != null)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                if(saveFileDialog1.ShowDialog() == DialogResult.OK)
                currentprofile.SaveProfile(saveFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show("No profile is available");
            }
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            if (currentprofile != null)
            {
                listBox1.Items.Clear();
                printProfile(currentprofile);
            }
            else
            {
                MessageBox.Show("No profile is available");
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            if (currentprofile != null)
            {
                //this.Hide();
                FormEdit form = new FormEdit(currentprofile);
                form.ShowDialog();
                if (form.DialogResult == DialogResult.OK)
                {
                    this.savedprofile = form.uploadProfile;
                    webCompleteHandler = new ListAliasSetValueHandler2(this, savedprofile.aliases);
                    webBrowser.Navigate("https://10.90.97.40/firewall_aliases.php");
                }
            }
            else
            {
                MessageBox.Show("No profile is available");
            }
        }

        public void AddAliasToCurrentProfile(Alias alias)
        {
            currentprofile.aliases.Add(alias);
        }

        private void printProfile(Profile profile)
        {
            foreach (Alias alias in profile.aliases)
            {
                listBox1.Items.Add(alias);
                listBox1.DisplayMember = "Name";

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
         {
             if (listBox1.SelectedIndex != -1)
             {
                 listBox2.Items.Clear();
                 listBox2.Items.Add("Name: " + currentprofile.aliases[listBox1.SelectedIndex].Name);
                 listBox2.Items.Add("Description: " + currentprofile.aliases[listBox1.SelectedIndex].Description);
                 foreach (KeyValuePair<string, string> host in currentprofile.aliases[listBox1.SelectedIndex].Hosts)
                 {
                     listBox2.Items.Add(host.Key + " : " + host.Value);
                 }
             }
         }

    }
}
