namespace OutlookMailClassification
{
    partial class MailClassification : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public MailClassification()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">"true", wenn verwaltete Ressourcen gelöscht werden sollen, andernfalls "false".</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabMailClassification = this.Factory.CreateRibbonTab();
            this.grpInitDB = this.Factory.CreateRibbonGroup();
            this.btnInitDB = this.Factory.CreateRibbonButton();
            this.grpSettings = this.Factory.CreateRibbonGroup();
            this.btnSettings = this.Factory.CreateRibbonButton();
            this.tabMailClassification.SuspendLayout();
            this.grpInitDB.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMailClassification
            // 
            this.tabMailClassification.Groups.Add(this.grpInitDB);
            this.tabMailClassification.Groups.Add(this.grpSettings);
            this.tabMailClassification.Label = "Mail Classification";
            this.tabMailClassification.Name = "tabMailClassification";
            // 
            // grpInitDB
            // 
            this.grpInitDB.Items.Add(this.btnInitDB);
            this.grpInitDB.Label = "1. Initialize";
            this.grpInitDB.Name = "grpInitDB";
            // 
            // btnInitDB
            // 
            this.btnInitDB.Label = "Initialize Database";
            this.btnInitDB.Name = "btnInitDB";
            this.btnInitDB.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnInitDB_Click);
            // 
            // grpSettings
            // 
            this.grpSettings.Items.Add(this.btnSettings);
            this.grpSettings.Label = "Settings";
            this.grpSettings.Name = "grpSettings";
            // 
            // btnSettings
            // 
            this.btnSettings.Label = "Settings";
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSettings_Click);
            // 
            // MailClassification
            // 
            this.Name = "MailClassification";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tabMailClassification);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MailClassification_Load);
            this.tabMailClassification.ResumeLayout(false);
            this.tabMailClassification.PerformLayout();
            this.grpInitDB.ResumeLayout(false);
            this.grpInitDB.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabMailClassification;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpInitDB;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnInitDB;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSettings;
    }

    partial class ThisRibbonCollection
    {
        internal MailClassification MailClassification
        {
            get { return this.GetRibbon<MailClassification>(); }
        }
    }
}
