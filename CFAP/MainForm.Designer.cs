namespace CFAP
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radMenu = new Telerik.WinControls.UI.RadMenu();
            this.radMenuItem_Settings = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Users = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Projects = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_BudgetItems = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Accountables = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Rates = new Telerik.WinControls.UI.RadMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radMenu
            // 
            this.radMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.radMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radMenu.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem_Settings,
            this.radMenuItem_Users,
            this.radMenuItem_Projects,
            this.radMenuItem_BudgetItems,
            this.radMenuItem_Accountables,
            this.radMenuItem_Rates});
            this.radMenu.Location = new System.Drawing.Point(0, 0);
            this.radMenu.Name = "radMenu";
            this.radMenu.Size = new System.Drawing.Size(1139, 20);
            this.radMenu.TabIndex = 0;
            // 
            // radMenuItem_Settings
            // 
            this.radMenuItem_Settings.EnableFocusBorder = false;
            this.radMenuItem_Settings.Name = "radMenuItem_Settings";
            this.radMenuItem_Settings.Text = "Настройки";
            // 
            // radMenuItem_Users
            // 
            this.radMenuItem_Users.Name = "radMenuItem_Users";
            this.radMenuItem_Users.Text = "Пользователи";
            this.radMenuItem_Users.Click += new System.EventHandler(this.radMenuItem_Users_Click);
            // 
            // radMenuItem_Projects
            // 
            this.radMenuItem_Projects.Name = "radMenuItem_Projects";
            this.radMenuItem_Projects.Text = "Проекты";
            this.radMenuItem_Projects.Click += new System.EventHandler(this.radMenuItem_Projects_Click);
            // 
            // radMenuItem_BudgetItems
            // 
            this.radMenuItem_BudgetItems.Name = "radMenuItem_BudgetItems";
            this.radMenuItem_BudgetItems.Text = "Бюджетные статьи";
            this.radMenuItem_BudgetItems.Click += new System.EventHandler(this.radMenuItem_BudgetItems_Click);
            // 
            // radMenuItem_Accountables
            // 
            this.radMenuItem_Accountables.Name = "radMenuItem_Accountables";
            this.radMenuItem_Accountables.Text = "Подотчетники";
            this.radMenuItem_Accountables.Click += new System.EventHandler(this.radMenuItem_Accountables_Click);
            // 
            // radMenuItem_Rates
            // 
            this.radMenuItem_Rates.Name = "radMenuItem_Rates";
            this.radMenuItem_Rates.Text = "Курсы валют";
            this.radMenuItem_Rates.Click += new System.EventHandler(this.radMenuItem1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 677);
            this.Controls.Add(this.radMenu);
            this.Name = "MainForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "CFAP - Движение денежных средств подотчетных лиц";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.radMenu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadMenu radMenu;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem_Settings;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem_Users;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem_Projects;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem_BudgetItems;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem_Accountables;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem_Rates;
    }
}
