using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWSMailExport
{
    class Mail
    {
        public string Subject { get; set; }
        public string SenderAddress { get; set; }
        public string SenderName { get; set; }
        public string CCAddresses { get; set; }
        public string Body { get; set; }
        public string Header { get; set; }

        public string ToCSVLine()
        {
            return Subject + ";#;" + SenderAddress + ";#;" + SenderName + ";#;" + CCAddresses + ";#;" + Body + ";#;" + Header + "\r\n";
        }
    }
}
