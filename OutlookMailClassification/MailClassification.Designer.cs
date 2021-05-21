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
            this.grpClustering = this.Factory.CreateRibbonGroup();
            this.btnCluster = this.Factory.CreateRibbonButton();
            this.grpClassification = this.Factory.CreateRibbonGroup();
            this.btnCreateModel = this.Factory.CreateRibbonButton();
            this.btnClassify = this.Factory.CreateRibbonButton();
            this.grpSettings = this.Factory.CreateRibbonGroup();
            this.btnSettings = this.Factory.CreateRibbonButton();
            this.grpDebugHelpers = this.Factory.CreateRibbonGroup();
            this.btnDeleteCategories = this.Factory.CreateRibbonButton();
            this.btnMoveTestData = this.Factory.CreateRibbonButton();
            this.tabMailClassification.SuspendLayout();
            this.grpClustering.SuspendLayout();
            this.grpClassification.SuspendLayout();
            this.grpSettings.SuspendLayout();
            this.grpDebugHelpers.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMailClassification
            // 
            this.tabMailClassification.Groups.Add(this.grpClustering);
            this.tabMailClassification.Groups.Add(this.grpClassification);
            this.tabMailClassification.Groups.Add(this.grpSettings);
            this.tabMailClassification.Groups.Add(this.grpDebugHelpers);
            this.tabMailClassification.Label = "Mail Classification";
            this.tabMailClassification.Name = "tabMailClassification";
            // 
            // grpClustering
            // 
            this.grpClustering.Items.Add(this.btnCluster);
            this.grpClustering.Label = "Clustering";
            this.grpClustering.Name = "grpClustering";
            // 
            // btnCluster
            // 
            this.btnCluster.Label = "Cluster Mails";
            this.btnCluster.Name = "btnCluster";
            this.btnCluster.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCluster_Click);
            // 
            // grpClassification
            // 
            this.grpClassification.Items.Add(this.btnCreateModel);
            this.grpClassification.Items.Add(this.btnClassify);
            this.grpClassification.Label = "Classification";
            this.grpClassification.Name = "grpClassification";
            // 
            // btnCreateModel
            // 
            this.btnCreateModel.Label = "Create Model";
            this.btnCreateModel.Name = "btnCreateModel";
            this.btnCreateModel.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnCreateModel_Click);
            // 
            // btnClassify
            // 
            this.btnClassify.Label = "Classify Mails";
            this.btnClassify.Name = "btnClassify";
            this.btnClassify.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnClassify_Click);
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
            // grpDebugHelpers
            // 
            this.grpDebugHelpers.Items.Add(this.btnDeleteCategories);
            this.grpDebugHelpers.Items.Add(this.btnMoveTestData);
            this.grpDebugHelpers.Label = "Debug Helpers";
            this.grpDebugHelpers.Name = "grpDebugHelpers";
            // 
            // btnDeleteCategories
            // 
            this.btnDeleteCategories.Label = "Delete all categories";
            this.btnDeleteCategories.Name = "btnDeleteCategories";
            this.btnDeleteCategories.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnDeleteCategories_Click);
            // 
            // btnMoveTestData
            // 
            this.btnMoveTestData.Label = "Create Hold Out Data";
            this.btnMoveTestData.Name = "btnMoveTestData";
            this.btnMoveTestData.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnMoveTestData_Click);
            // 
            // MailClassification
            // 
            this.Name = "MailClassification";
            this.RibbonType = "Microsoft.Outlook.Explorer";
            this.Tabs.Add(this.tabMailClassification);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.MailClassification_Load);
            this.tabMailClassification.ResumeLayout(false);
            this.tabMailClassification.PerformLayout();
            this.grpClustering.ResumeLayout(false);
            this.grpClustering.PerformLayout();
            this.grpClassification.ResumeLayout(false);
            this.grpClassification.PerformLayout();
            this.grpSettings.ResumeLayout(false);
            this.grpSettings.PerformLayout();
            this.grpDebugHelpers.ResumeLayout(false);
            this.grpDebugHelpers.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabMailClassification;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpClustering;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCluster;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpDebugHelpers;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnDeleteCategories;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnMoveTestData;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup grpClassification;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnCreateModel;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnClassify;
    }

    partial class ThisRibbonCollection
    {
        internal MailClassification MailClassification
        {
            get { return this.GetRibbon<MailClassification>(); }
        }
    }
}
