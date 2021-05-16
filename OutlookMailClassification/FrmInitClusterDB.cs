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
    public partial class FrmInitClusterDB : Form
    {

        public Outlook.Folder ClusterFolder;
        private List<Outlook.Folder> Folders;
        public FrmInitClusterDB(List<Outlook.Folder> folders)
        {

            InitializeComponent();

            Folders = folders;
            ClusterFolder = null;
            foreach (Outlook.Folder folder in folders)
            {
                cbDirectories.Items.Add(folder.FolderPath);
            }
            cbDirectories.SelectedIndex = 1;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ClusterFolder = Folders[cbDirectories.SelectedIndex];
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmInitClusterDB_Load(object sender, EventArgs e)
        {

        }
    }
}
