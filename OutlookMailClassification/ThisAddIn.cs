using IniParser;
using IniParser.Model;
using System;
using System.IO;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookMailClassification
{
    // TODO: Web-API Encryption?


    public partial class ThisAddIn
    {
        public static Outlook.Application OutlookApp;

        private static string IniPath;
        private static FileIniDataParser Parser;
        public static IniData Config;



        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            OutlookApp = this.Application;
            Parser = new FileIniDataParser();
            IniPath = Environment.ExpandEnvironmentVariables(@"%appdata%\Microsoft\Outlook\MailClassification.ini");
            

            if (!File.Exists(IniPath))
            {
                File.WriteAllText(IniPath, string.Empty);
                Config = Parser.ReadFile(IniPath);
                Config["Configuration"]["URL"] = "localhost";
                Config["Configuration"]["Port"] = "39567";
                Parser.WriteFile(IniPath, Config);
            }
            else
            {
                Config = Parser.ReadFile(IniPath);
            }
        }

        internal static void SaveConfig()
        {
            Parser.WriteFile(IniPath, Config);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Hinweis: Outlook löst dieses Ereignis nicht mehr aus. Wenn Code vorhanden ist, der 
            //    muss ausgeführt werden, wenn Outlook heruntergefahren wird. Weitere Informationen finden Sie unter https://go.microsoft.com/fwlink/?LinkId=506785.
        }

        #region Von VSTO generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(this.ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(this.ThisAddIn_Shutdown);
        }

        #endregion
    }
}
