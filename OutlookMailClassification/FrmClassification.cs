using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookMailClassification
{
    public partial class FrmClassification : Form
    {
        List<Outlook.Folder> Folders;

        public List<Outlook.Folder> SelectedFolders { get; }

        public FrmClassification(List<Outlook.Folder> folders)
        {
            InitializeComponent();

            Folders = folders;
            SelectedFolders = new List<Outlook.Folder>();
            foreach (Outlook.Folder folder in folders)
            {
                lvFolders.Items.Add(folder.FolderPath);
            }
            lvFolders.Columns.Add("Folder", -2);

            
        }

        private void FrmClassify_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for(int i=0;i < lvFolders.Items.Count;i++)
            {
                if (lvFolders.Items[i].Checked)
                {
                    SelectedFolders.Add(Folders[i]);
                }
            }

            this.Close();
        }
    }
}
