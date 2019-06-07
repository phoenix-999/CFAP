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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radMenu = new Telerik.WinControls.UI.RadMenu();
            this.radMenuItem_Settings = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Users = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Projects = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_BudgetItems = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Accountables = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_Rates = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem_UsersGroups = new Telerik.WinControls.UI.RadMenuItem();
            this.radPanel_Header = new Telerik.WinControls.UI.RadPanel();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton_Remove = new Telerik.WinControls.UI.RadButton();
            this.radButton_GetData = new Telerik.WinControls.UI.RadButton();
            this.radPanel_Footer = new Telerik.WinControls.UI.RadPanel();
            this.radGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Header)).BeginInit();
            this.radPanel_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Remove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_GetData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Footer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView.MasterTemplate)).BeginInit();
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
            this.radMenuItem_Rates,
            this.radMenuItem_UsersGroups});
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
            this.radMenuItem_Settings.Click += new System.EventHandler(this.radMenuItem_Settings_Click);
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
            // radMenuItem_UsersGroups
            // 
            this.radMenuItem_UsersGroups.Name = "radMenuItem_UsersGroups";
            this.radMenuItem_UsersGroups.Text = "Группы пользователей";
            this.radMenuItem_UsersGroups.Click += new System.EventHandler(this.radMenuItem_UsersGroups_Click);
            // 
            // radPanel_Header
            // 
            this.radPanel_Header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radPanel_Header.Controls.Add(this.radButton1);
            this.radPanel_Header.Controls.Add(this.radButton_Remove);
            this.radPanel_Header.Controls.Add(this.radButton_GetData);
            this.radPanel_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel_Header.Location = new System.Drawing.Point(0, 20);
            this.radPanel_Header.Name = "radPanel_Header";
            this.radPanel_Header.Size = new System.Drawing.Size(1139, 29);
            this.radPanel_Header.TabIndex = 1;
            // 
            // radButton1
            // 
            this.radButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.radButton1.Location = new System.Drawing.Point(919, 0);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(110, 29);
            this.radButton1.TabIndex = 2;
            this.radButton1.Text = "Добавить";
            // 
            // radButton_Remove
            // 
            this.radButton_Remove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radButton_Remove.Dock = System.Windows.Forms.DockStyle.Right;
            this.radButton_Remove.Location = new System.Drawing.Point(1029, 0);
            this.radButton_Remove.Name = "radButton_Remove";
            this.radButton_Remove.Size = new System.Drawing.Size(110, 29);
            this.radButton_Remove.TabIndex = 1;
            this.radButton_Remove.Text = "Удалить";
            // 
            // radButton_GetData
            // 
            this.radButton_GetData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radButton_GetData.Dock = System.Windows.Forms.DockStyle.Left;
            this.radButton_GetData.Location = new System.Drawing.Point(0, 0);
            this.radButton_GetData.Name = "radButton_GetData";
            this.radButton_GetData.Size = new System.Drawing.Size(110, 29);
            this.radButton_GetData.TabIndex = 0;
            this.radButton_GetData.Text = "Получить данные";
            this.radButton_GetData.Click += new System.EventHandler(this.radButton_GetData_Click);
            // 
            // radPanel_Footer
            // 
            this.radPanel_Footer.BackColor = System.Drawing.Color.LightGreen;
            this.radPanel_Footer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanel_Footer.Location = new System.Drawing.Point(0, 654);
            this.radPanel_Footer.Name = "radPanel_Footer";
            this.radPanel_Footer.Size = new System.Drawing.Size(1139, 23);
            this.radPanel_Footer.TabIndex = 2;
            // 
            // radGridView
            // 
            this.radGridView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.radGridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.radGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView.EnterKeyMode = Telerik.WinControls.UI.RadGridViewEnterKeyMode.EnterMovesToNextRow;
            this.radGridView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.radGridView.ForeColor = System.Drawing.Color.Black;
            this.radGridView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radGridView.Location = new System.Drawing.Point(0, 49);
            // 
            // 
            // 
            this.radGridView.MasterTemplate.AddNewRowPosition = Telerik.WinControls.UI.SystemRowPosition.Bottom;
            this.radGridView.MasterTemplate.AllowAddNewRow = false;
            this.radGridView.MasterTemplate.AllowDeleteRow = false;
            this.radGridView.MasterTemplate.AllowEditRow = false;
            this.radGridView.MasterTemplate.AllowSearchRow = true;
            this.radGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView.MasterTemplate.EnableFiltering = true;
            this.radGridView.MasterTemplate.MultiSelect = true;
            this.radGridView.MasterTemplate.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.CellSelect;
            this.radGridView.MasterTemplate.ShowTotals = true;
            this.radGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView.Name = "radGridView";
            this.radGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radGridView.Size = new System.Drawing.Size(1139, 605);
            this.radGridView.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 677);
            this.Controls.Add(this.radGridView);
            this.Controls.Add(this.radPanel_Footer);
            this.Controls.Add(this.radPanel_Header);
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
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Header)).EndInit();
            this.radPanel_Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_Remove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_GetData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Footer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView)).EndInit();
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
        private Telerik.WinControls.UI.RadMenuItem radMenuItem_UsersGroups;
        private Telerik.WinControls.UI.RadPanel radPanel_Header;
        private Telerik.WinControls.UI.RadButton radButton_Remove;
        private Telerik.WinControls.UI.RadButton radButton_GetData;
        private Telerik.WinControls.UI.RadPanel radPanel_Footer;
        private Telerik.WinControls.UI.RadGridView radGridView;
        private Telerik.WinControls.UI.RadButton radButton1;
    }
}
