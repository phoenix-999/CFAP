namespace CFAP
{
    partial class LockedPeriodForm
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
            this.radPanel_Header = new Telerik.WinControls.UI.RadPanel();
            this.radButton_AddItem = new Telerik.WinControls.UI.RadButton();
            this.radGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Header)).BeginInit();
            this.radPanel_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel_Header
            // 
            this.radPanel_Header.Controls.Add(this.radButton_AddItem);
            this.radPanel_Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.radPanel_Header.Location = new System.Drawing.Point(0, 0);
            this.radPanel_Header.Name = "radPanel_Header";
            this.radPanel_Header.Size = new System.Drawing.Size(694, 29);
            this.radPanel_Header.TabIndex = 2;
            // 
            // radButton_AddItem
            // 
            this.radButton_AddItem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.radButton_AddItem.Dock = System.Windows.Forms.DockStyle.Left;
            this.radButton_AddItem.Location = new System.Drawing.Point(0, 0);
            this.radButton_AddItem.Name = "radButton_AddItem";
            this.radButton_AddItem.Size = new System.Drawing.Size(110, 29);
            this.radButton_AddItem.TabIndex = 0;
            this.radButton_AddItem.Text = "Добавить";
            // 
            // radGridView
            // 
            this.radGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView.Location = new System.Drawing.Point(0, 29);
            this.radGridView.Margin = new System.Windows.Forms.Padding(10);
            // 
            // 
            // 
            this.radGridView.MasterTemplate.AddNewRowPosition = Telerik.WinControls.UI.SystemRowPosition.Bottom;
            this.radGridView.MasterTemplate.AllowAddNewRow = false;
            this.radGridView.MasterTemplate.AllowDeleteRow = false;
            this.radGridView.MasterTemplate.AllowEditRow = false;
            this.radGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView.MasterTemplate.EnableFiltering = true;
            this.radGridView.MasterTemplate.MultiSelect = true;
            this.radGridView.MasterTemplate.ShowChildViewCaptions = true;
            this.radGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView.Name = "radGridView";
            this.radGridView.ShowChildViewCaptions = true;
            this.radGridView.Size = new System.Drawing.Size(694, 525);
            this.radGridView.TabIndex = 3;
            // 
            // LockedPeriodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 554);
            this.Controls.Add(this.radGridView);
            this.Controls.Add(this.radPanel_Header);
            this.Name = "LockedPeriodForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Блокировка периодов";
            this.Load += new System.EventHandler(this.LockedPeriodForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Header)).EndInit();
            this.radPanel_Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel_Header;
        private Telerik.WinControls.UI.RadButton radButton_AddItem;
        private Telerik.WinControls.UI.RadGridView radGridView;
    }
}
