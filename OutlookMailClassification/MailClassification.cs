using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;

namespace OutlookMailClassification
{
    public partial class MailClassification
    {
        private void MailClassification_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnSettings_Click(object sender, RibbonControlEventArgs e)
        {
            FrmSettings frmSettings = new FrmSettings();
            frmSettings.ShowDialog();
        }
    }
}
