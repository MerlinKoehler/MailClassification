using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;
using Outlook = Microsoft.Office.Interop.Outlook;

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

        private void btnCluster_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.Folder root = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder.Store.GetRootFolder() as Outlook.Folder;
            List<Outlook.Folder> folders = new List<Outlook.Folder>();
            folders.Add(root);
            this.EnumerateFolders(root, folders);
            FrmClustering frmInitClusterDB = new FrmClustering(folders);
            frmInitClusterDB.ShowDialog();
            if (frmInitClusterDB.ClusterFolder != null)
            {
                try
                {
                    HttpWebRequest httpWebRequest;
                    HttpWebResponse httpResponse;

                    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/deletedb");
                    httpWebRequest.Timeout = 600000;
                    httpWebRequest.Method = "GET";
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }


                    // TODO: Add status bar
                    // TODO: Do in seperate thread
                    foreach (object itm in frmInitClusterDB.ClusterFolder.Items)
                    {
                        if (itm is Outlook.MailItem)
                        {

                            Outlook.MailItem mail = (Outlook.MailItem)itm;

                            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/add");
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                List<string> tag_list = new List<string>();

                                if (mail.Sender != null)
                                {
                                    tag_list.Add(mail.Sender.Address);
                                    tag_list.Add(mail.Sender.Name);
                                }

                                tag_list.AddRange(this.GetSMTPAddressesForRecipients(mail));

                                if (mail.CC != null)
                                {
                                    tag_list.AddRange(mail.CC.Split(';'));
                                }



                                string json = JsonSerializer.Serialize(new
                                {
                                    id = mail.EntryID,
                                    text = mail.Subject + "\r\n" + mail.Body,
                                    tags = tag_list
                                });

                                streamWriter.Write(json);
                            }

                            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var result = streamReader.ReadToEnd();
                            }
                        }
                    }

                    // TODO: Increase timeout
                    // TODO: start new thread on server
                    // TODO: do a status page
                    // TODO: Add multiple document repositories
                    // TODO: Add hierachy clustering

                    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/preprocess");
                    httpWebRequest.Timeout = 600000;
                    httpWebRequest.Method = "GET";
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }

                    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/cluster");
                    httpWebRequest.Timeout = 600000;
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = JsonSerializer.Serialize(new
                        {
                            k = frmInitClusterDB.k
                        });
                        streamWriter.Write(json);
                    }

                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }

                    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/save");
                    httpWebRequest.Timeout = 600000;
                    httpWebRequest.Method = "GET";
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                    }


                    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/topics");
                    httpWebRequest.Timeout = 600000;
                    httpWebRequest.Method = "GET";
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    Dictionary<int, string> categories = new Dictionary<int, string>();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        categories = JsonSerializer.Deserialize<Dictionary<int, string>>(streamReader.ReadToEnd());
                    }

                    Outlook.Categories ol_categories = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder.Store.Categories;

                    foreach (var category in categories)
                    {
                        ol_categories.Add(category.Value);
                    }

                    httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/getclusters");
                    httpWebRequest.Timeout = 600000;
                    httpWebRequest.Method = "GET";
                    httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                    Dictionary<string, int> mail_class = new Dictionary<string, int>();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        mail_class = JsonSerializer.Deserialize<Dictionary<string, int>>(streamReader.ReadToEnd());
                    }

                    foreach (object itm in frmInitClusterDB.ClusterFolder.Items)
                    {
                        if (itm is Outlook.MailItem)
                        {
                            Outlook.MailItem mail = (Outlook.MailItem)itm;
                            if (mail_class.ContainsKey(mail.EntryID))
                            {
                                mail.Categories = categories[mail_class[mail.EntryID]];
                                mail.Save();
                            }
                        }
                    }

                }
                catch (Exception ex)
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

        private void btnDeleteCategories_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MAPIFolder folder = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder;

            this.DeleteCategories((Outlook.Folder)folder);

            Outlook.Categories ol_categories = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder.Store.Categories;
            for (int i = ol_categories.Count; i >= 0; i--)
            {
                ol_categories.Remove(i);
            }
        }

        private void DeleteCategories(Outlook.Folder folder)
        {
            foreach (object itm in folder.Items)
            {
                if (itm is Outlook.MailItem)
                {
                    Outlook.MailItem mail = (Outlook.MailItem)itm;
                    mail.Categories = "";
                    mail.Save();
                }
            }
        }

        private void btnMoveTestData_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.MAPIFolder folder = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder;

            bool existing = false;

            Outlook.MAPIFolder testdata = null;

            foreach (Outlook.MAPIFolder fld in folder.Folders)
            {
                if (fld.Name == "TestData")
                {
                    existing = true;
                    testdata = fld;
                    break;
                }
            }

            if (!existing)
            {
                testdata = folder.Folders.Add("TestData");
            }

            Random random = new Random();

            foreach (object itm in folder.Items)
            {
                if (itm is Outlook.MailItem)
                {
                    if (random.NextDouble() <= 0.25)
                    {
                        Outlook.MailItem mail = (Outlook.MailItem)itm;
                        mail.Move(testdata);
                    }
                }
            }
        }

        private void btnCreateModel_Click(object sender, RibbonControlEventArgs e)
        {
            Outlook.Folder root = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder.Store.GetRootFolder() as Outlook.Folder;
            List<Outlook.Folder> folders = new List<Outlook.Folder>();
            folders.Add(root);
            this.EnumerateFolders(root, folders);
            FrmClassification frmClassify = new FrmClassification(folders);
            frmClassify.ShowDialog();
            int i = 0;

            HttpWebRequest httpWebRequest;
            HttpWebResponse httpResponse;

            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/delclasses");
            httpWebRequest.Timeout = 600000;
            httpWebRequest.Method = "GET";
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            foreach (Outlook.Folder folder in frmClassify.SelectedFolders)
            {
                this.DeleteCategories(folder);
                foreach (object itm in folder.Items)
                {

                    if (itm is Outlook.MailItem)
                    {

                        Outlook.MailItem mail = (Outlook.MailItem)itm;

                        httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/add");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            List<string> tag_list = new List<string>();

                            if (mail.Sender != null)
                            {
                                tag_list.Add(mail.Sender.Address);
                                tag_list.Add(mail.Sender.Name);
                            }

                            tag_list.AddRange(this.GetSMTPAddressesForRecipients(mail));

                            if (mail.CC != null)
                            {
                                tag_list.AddRange(mail.CC.Split(';'));
                            }



                            string json = JsonSerializer.Serialize(new
                            {
                                id = mail.EntryID,
                                text = mail.Subject + "\r\n" + mail.Body,
                                tags = tag_list,
                                @class = i
                            });

                            streamWriter.Write(json);
                        }

                        httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var result = streamReader.ReadToEnd();
                        }
                    }
                }

                i++;
            }

            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/preprocess");
            httpWebRequest.Timeout = 600000;
            httpWebRequest.Method = "GET";
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/createmodel");
            httpWebRequest.Timeout = 600000;
            httpWebRequest.Method = "GET";
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }

            httpWebRequest = (HttpWebRequest)WebRequest.Create("http://" + ThisAddIn.Config["Configuration"]["URL"] + ":" + ThisAddIn.Config["Configuration"]["Port"] + "/api/save");
            httpWebRequest.Timeout = 600000;
            httpWebRequest.Method = "GET";
            httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }


            Outlook.Categories ol_categories = ThisAddIn.OutlookApp.ActiveExplorer().CurrentFolder.Store.Categories;

            if (ol_categories.Count > 0)
            {
                for (int x = ol_categories.Count; x >= 0; x--)
                {
                    ol_categories.Remove(x);
                }
            }

            foreach (Outlook.Folder folder in frmClassify.SelectedFolders)
            {
                ol_categories.Add(folder.Name);
            }

        }

        private void btnClassify_Click(object sender, RibbonControlEventArgs e)
        {

        }
    }
}
