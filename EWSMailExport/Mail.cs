// <copyright file="Mail.cs" company="Maastricht Univerity">
// Copyright (c) 2021 All Rights Reserved
// <author>Merlin Koehler</author>
// </copyright>

namespace EWSMailExport
{
    /// <summary>
    /// A class used to store the mail information.
    /// </summary>
    internal class Mail
    {
        /// <summary>
        /// Gets or sets the mail subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the sender mail address
        /// </summary>
        public string SenderAddress { get; set; }

        /// <summary>
        /// Gets or sets the sender name
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the all mail addresses in CC
        /// </summary>
        public string CCAddresses { get; set; }

        /// <summary>
        /// Gets or sets the mail body in text format
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the all mail headers
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Writes information to a CSV-line using custom separators.
        /// </summary>
        /// <param name="separator">A custom separator string.</param>
        /// <returns>A single CSV-Line</returns>
        public string ToCSVLine(string separator)
        {
            return this.Subject + separator + this.SenderAddress + separator + this.SenderName + separator + this.CCAddresses + separator + this.Body + separator + this.Header + "!#ENDOFLINE#!\r\n";
        }
    }
}
