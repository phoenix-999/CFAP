namespace CFAP
{
    partial class RatesForm
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radPanel_Header = new Telerik.WinControls.UI.RadPanel();
            this.radButton_AddItem = new Telerik.WinControls.UI.RadButton();
            this.radPanel_Grid = new Telerik.WinControls.UI.RadPanel();
            this.radGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Header)).BeginInit();
            this.radPanel_Header.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddItem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Grid)).BeginInit();
            this.radPanel_Grid.SuspendLayout();
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
            this.radPanel_Header.Size = new System.Drawing.Size(753, 29);
            this.radPanel_Header.TabIndex = 1;
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
            this.radButton_AddItem.Click += new System.EventHandler(this.radButton_AddItem_Click);
            // 
            // radPanel_Grid
            // 
            this.radPanel_Grid.Controls.Add(this.radGridView);
            this.radPanel_Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel_Grid.Location = new System.Drawing.Point(0, 29);
            this.radPanel_Grid.Name = "radPanel_Grid";
            this.radPanel_Grid.Size = new System.Drawing.Size(753, 730);
            this.radPanel_Grid.TabIndex = 2;
            this.radPanel_Grid.Text = "radPanel2";
            // 
            // radGridView
            // 
            this.radGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView.Location = new System.Drawing.Point(0, 0);
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
            this.radGridView.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView.Name = "radGridView";
            this.radGridView.ShowChildViewCaptions = true;
            this.radGridView.Size = new System.Drawing.Size(753, 730);
            this.radGridView.TabIndex = 1;
            this.radGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView_CellDoubleClick);
            // 
            // RatesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 759);
            this.Controls.Add(this.radPanel_Grid);
            this.Controls.Add(this.radPanel_Header);
            this.Name = "RatesForm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Курсы валют";
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Header)).EndInit();
            this.radPanel_Header.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radButton_AddItem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel_Grid)).EndInit();
            this.radPanel_Grid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel_Header;
        private Telerik.WinControls.UI.RadButton radButton_AddItem;
        private Telerik.WinControls.UI.RadPanel radPanel_Grid;
        private Telerik.WinControls.UI.RadGridView radGridView;
    }
}
