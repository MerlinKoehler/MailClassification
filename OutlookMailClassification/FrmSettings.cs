using System;
using System.Windows.Forms;

namespace OutlookMailClassification
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            this.InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            ThisAddIn.Config["Configuration"]["URL"] = this.tbURL.Text;
            ThisAddIn.Config["Configuration"]["Port"] = this.tbPort.Text;
            ThisAddIn.SaveConfig();
            this.Close();
        }

        private void FrmSettings_Load(object sender, EventArgs e)
        {
            tbURL.Text = ThisAddIn.Config["Configuration"]["URL"];
            tbPort.Text = ThisAddIn.Config["Configuration"]["Port"];
        }

        private void tbPort_Leave(object sender, EventArgs e)
        {
            int i;
            if(!int.TryParse(tbPort.Text, out i))
            {
                MessageBox.Show("Please enter a numeric value into port!");
                tbPort.Focus();
            }

        }
    }
}
