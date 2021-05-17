using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Text.Json;

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

        private void btnInitDB_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.Folder root = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder.Store.GetRootFolder() as Outlook.Folder;
            List<Outlook.Folder> folders = new List<Outlook.Folder>();
            folders.Add(root);
            this.EnumerateFolders(root, folders);
            FrmInitClusterDB frmInitClusterDB = new FrmInitClusterDB(folders);
            frmInitClusterDB.ShowDialog();
            if (frmInitClusterDB.ClusterFolder != null)
            {
                try
                {
                    foreach (object itm in frmInitClusterDB.ClusterFolder.Items)
                    {
                        if (itm is Outlook.MailItem)
                        {

                            Outlook.MailItem mail = (Outlook.MailItem)itm;

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/add");
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                List<string> tags = new List<string>();

                                if(mail.Sender.Address != null %% mail.Sender.Name != null)
                                {
                                    tags.Add(mail.Sender.Address);
                                    tags.Add(mail.Sender.Name);
                                }

                                tags.AddRange(GetSMTPAddressesForRecipients(mail));

                                if(mail.CC != null)
                                {
                                    tags.AddRange(mail.CC.Split(';'));
                                }

                                

                                string json = JsonSerializer.Serialize(new
                                {
                                    text = mail.Body,
                                    tags = tags
                                });

                                streamWriter.Write(json);
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }



            }
        }

        private void EnumerateFolders(Outlook.Folder folder, List<Outlook.Folder> folders)
        {
            Outlook.Folders childFolders =
                folder.Folders;
            if (childFolders.Count > 0)
            {
                foreach (Outlook.Folder childFolder in childFolders)
                {
                    folders.Add(childFolder);
                    // Call EnumerateFolders using childFolder.
                    this.EnumerateFolders(childFolder, folders);
                }
            }
        }


        private List<string> GetSMTPAddressesForRecipients(Outlook.MailItem mail)
        {
            List<string> addresses = new List<string>();
            const string PR_SMTP_ADDRESS = "http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
            Outlook.Recipients recips = mail.Recipients;
            foreach (Outlook.Recipient recip in recips)
            {
                Outlook.PropertyAccessor pa = recip.PropertyAccessor;
                string smtpAddress = pa.GetProperty(PR_SMTP_ADDRESS).ToString();
                addresses.Add(smtpAddress);
                addresses.Add(recip.Name);
            }
            return addresses;
        }
    }
}
