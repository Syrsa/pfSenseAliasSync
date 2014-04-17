using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Firewall
{
    public partial class FormEdit : Form
    {
        private Profile currentProfile;
        private Profile savedProfile; 
        public Profile uploadProfile;

        public FormEdit(Profile profile)
        {
            InitializeComponent();
            currentProfile = profile;
            printCurrentProfile();
        }


        private void btn_load_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                checkedListBox1.Items.Clear();
                savedProfile = XmlHandler.XmlReader(openFileDialog1.FileName);
                printSavedProfile();
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            if(checkedListBox1.CheckedItems.Count != 0)
            {
                ssss();
                 this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("No aliases was selected for upload!");
            }
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
        private void printCurrentProfile()
        {
            if (currentProfile.aliases.Count != 0)
            {
                foreach (Alias alias in currentProfile.aliases)
                {
                    listBox1.Items.Add(alias);
                    listBox1.DisplayMember = "Name";

                }
            }
        }
        
        private void printSavedProfile()
        {
            foreach (Alias alias in savedProfile.aliases)
            {
               checkedListBox1.Items.Add(alias, checkBox_selectAll.Checked);
               checkedListBox1.DisplayMember = "Name";
            }
        }

        private void profileDiff()
        {
            foreach (Alias savedalias in savedProfile.aliases)
            {
                foreach (Alias currentalias in savedProfile.aliases)
                {
                    if (currentalias.Name == savedalias.Name)
                    { 
                        
                    }
                }              
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                listBox2.Items.Clear();
                listBox2.Items.Add("Name: " + currentProfile.aliases[listBox1.SelectedIndex].Name);
                listBox2.Items.Add("Description: " + currentProfile.aliases[listBox1.SelectedIndex].Description);
                foreach (KeyValuePair<string, string> host in currentProfile.aliases[listBox1.SelectedIndex].Hosts)
                {
                    listBox2.Items.Add(host.Key + " : " + host.Value);
                }
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                //propertyGrid1.SelectedObject = savedprofile.aliases[listBox1.SelectedIndex];
                listBox3.Items.Clear();
                listBox3.Items.Add("Name: " + savedProfile.aliases[checkedListBox1.SelectedIndex].Name);
                listBox3.Items.Add("Description: " + savedProfile.aliases[checkedListBox1.SelectedIndex].Description);
                foreach (KeyValuePair<string, string> host in savedProfile.aliases[checkedListBox1.SelectedIndex].Hosts)
                {
                    listBox3.Items.Add(host.Key + " : " + host.Value);
                }
            }
        }

        private void checkBox_selectAll_CheckStateChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, checkBox_selectAll.Checked);
            }
        }

        private void ssss()
        {
            uploadProfile = new Profile();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                uploadProfile.aliases.Add((Alias)item);
            }
        }
    }
}
