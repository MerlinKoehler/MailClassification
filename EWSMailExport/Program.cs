// <copyright file="Program.cs" company="Maastricht Univerity">
// Copyright (c) 2021 All Rights Reserved
// <author>Merlin Koehler</author>
// </copyright>

namespace EWSMailExport
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Exchange.WebServices.Data;

    /// <summary>
    /// The main export program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The autodiscovery URL.
        /// </summary>
        private static string url = string.Empty;

        /// <summary>
        /// The login username / mail address.
        /// </summary>
        private static string login = string.Empty;

        /// <summary>
        /// The password used to authenticate.
        /// </summary>
        private static string password = string.Empty;

        /// <summary>
        /// The link to the exchange service.
        /// </summary>
        private static ExchangeService service;

        /// <summary>
        /// Main entry point.
        /// </summary>
        /// <param name="args">Arguments passed to the program: #Autodiscovery-URL# #Login# #Password#</param>
        private static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("EWS Inbox Mail Exporter V." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
                Console.WriteLine("Usage: EWSMailExport.exe <Autodiscovery-URL> <Login> <Password>");
            }

            try
            {
                // Parse arguments:
                url = args[0];
                login = args[1];
                password = args[2];

                // Connect to service:
                service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
                service.Credentials = new WebCredentials(login, password);

                try
                {
                    service.AutodiscoverUrl(url, SslRedirectionCallback);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not connect to serice: " + ex.Message);
                    service = null;
                    Environment.Exit(-1);
                }

                // Create EML-Export directory
                if (!Directory.Exists("Export_EML"))
                {
                    Directory.CreateDirectory("Export_EML");
                }

                // Start export
                ExportMails();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occured: " + ex.Message);
            }
        }

        /// <summary>
        /// Method for exporting mails from EWS.
        /// </summary>
        private static void ExportMails()
        {
            // Collection of mail information retrieved.
            List<Mail> mails = new List<Mail>();

            try
            {
                // Load first 100 mails
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

                            // Load mail properties:
                            PropertySet prop = new PropertySet(new PropertyDefinitionBase[] { EmailMessageSchema.Sender, EmailMessageSchema.CcRecipients, ItemSchema.Subject, ItemSchema.Body, ItemSchema.InternetMessageHeaders, ItemSchema.DateTimeReceived, ItemSchema.MimeContent });
                            prop.RequestedBodyType = BodyType.Text;
                            itm.Load(prop);

                            // Save EML-File:
                            var mimeContent = itm.MimeContent;
                            using (var fileStream = new FileStream(Path.Combine("Export_EML", @"Mail_" + i + ".eml"), FileMode.Create))
                            {
                                fileStream.Write(mimeContent.Content, 0, mimeContent.Content.Length);
                            }

                            Console.WriteLine("Message: " + i + " date: " + itm.DateTimeReceived + " subject: " + itm.Subject);

                            // Store mail information:
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
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error retrieving mail: " + ex.Message);
                        }
                    }

                    view.Offset = i - 1;
                    items = folder.FindItems(view);
                }

                // Build CSV file and save:
                StringBuilder sb = new StringBuilder();

                foreach (Mail mail in mails)
                {
                    sb.Append(mail.ToCSVLine("!#!"));
                }

                File.WriteAllText("Mails.csv", sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Checks whether the URL is a secure HTTPS URL
        /// </summary>
        /// <param name="serviceUrl">The URL found during autodiscovery.</param>
        /// <returns>True if HTTPS, else False</returns>
        private static bool SslRedirectionCallback(string serviceUrl)
        {
            // Return true if the URL is an HTTPS URL.
            return serviceUrl.ToLower().StartsWith("https://");
        }
    }
}
