using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSMailExport
{
    class Program
    {
        private static string URL = string.Empty;
        private static string Login = string.Empty;
        private static string Password = string.Empty;
        private static string Mailbox = string.Empty;
        private static ExchangeService service;

        static void Main(string[] args)
        {
            try
            {
                URL = args[0];
                Login = args[1];
                Password = args[2];
                Mailbox = args[3];

                service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);

                service.Credentials = new WebCredentials(Login, Password);

                try
                {
                    // Use Autodiscover to set the URL endpoint.
                    // and using a AutodiscoverRedirectionUrlValidationCallback in case of https enabled clod account
                    service.AutodiscoverUrl(URL, SslRedirectionCallback);
                }
                catch
                {
                    service = null;
                    Environment.Exit(-1);
                }

                if (!Directory.Exists("Export_EML"))
                {
                    Directory.CreateDirectory("Export_EML");
                }

                ExportMails();
            }
            catch(Exception ex)
            {
                Console.WriteLine("An error has occured: " + ex.Message);
            }
        }

        private static void ExportMails()
        {
            List<Mail> mails = new List<Mail>();
            try
            {
                FolderId fid = new FolderId(WellKnownFolderName.Inbox);
                Folder folder = Folder.Bind(service, fid);
                ItemView view = new ItemView(100);
                FindItemsResults<Item> items = folder.FindItems(view);

                int i = 0;
                while (items.Count() > 0)
                {
                    foreach (Item itm in items)
                    {
                        try
                        {
                            i++;

                            PropertySet prop = new PropertySet(new PropertyDefinitionBase[] { EmailMessageSchema.Sender, EmailMessageSchema.CcRecipients, ItemSchema.Subject, ItemSchema.Body, ItemSchema.InternetMessageHeaders, ItemSchema.DateTimeReceived, ItemSchema.MimeContent });
                            prop.RequestedBodyType = BodyType.Text;
                            itm.Load(prop);
                            var mimeContent = itm.MimeContent;
                            using (var fileStream = new FileStream(Path.Combine("Export_EML", @"Mail_" + i + ".eml"), FileMode.Create))
                            {
                                fileStream.Write(mimeContent.Content, 0, mimeContent.Content.Length);
                            }
                            Console.WriteLine("Message: " + i + " date: " + itm.DateTimeReceived + " subject: " + itm.Subject);

                            Mail mail = new Mail();
                            mail.Body = itm.Body;

                            foreach (EmailAddress address in ((EmailMessage)itm).CcRecipients)
                            {
                                mail.CCAddresses += address + ";";
                            }
                            mail.SenderAddress = ((EmailMessage)itm).Sender.Address;
                            mail.SenderName = ((EmailMessage)itm).Sender.Name;
                            mail.Subject = itm.Subject;

                            foreach (InternetMessageHeader header in ((EmailMessage)itm).InternetMessageHeaders)
                            {
                                mail.Header += header + ";";
                            }
                            mails.Add(mail);
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine("Error retrieving mail: " + ex.Message);
                        }
                    }
                    view.Offset = i - 1;
                    items = folder.FindItems(view);
                }

                StringBuilder sb = new StringBuilder();

                foreach(Mail mail in mails)
                {
                    sb.Append(mail.ToCSVLine());
                }

                File.WriteAllText("Mails.csv", sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static bool SslRedirectionCallback(string serviceUrl)
        {
            // Return true if the URL is an HTTPS URL.
            return serviceUrl.ToLower().StartsWith("https://");
        }


    }
}
