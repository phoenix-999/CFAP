using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;
using Telerik.WinControls.UI;
using System.Reflection;

namespace CFAP
{
    public partial class ChangeProjectForm : Telerik.WinControls.UI.RadForm
    {
        Project project;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;
        public ChangeProjectForm(Project project, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.project = project;
            this.changeDataOption = changeDataOption;

            InitializeFileds();
            InitializeButtons();

            businessLogic = new CFAPBusinessLogic(new ExceptionsHandlerUI());
        }

        void InitializeButtons()
        {
            switch(this.changeDataOption)
            {
                case ChangeDataOptions.AddNew:
                    this.radButton_AddAccountable.Enabled = true;
                    break;
                case ChangeDataOptions.ChangeData:
                    this.radButton_UpdateAccountable.Enabled = true;
                    break;
            }
        }

        void InitializeFileds()
        {
            if (project.ProjectName != null)
                this.radTextBox_ItemName.Text = project.ProjectName;

            this.radCheckBox_ReadOnly.Checked = project.ReadOnly;
        }

        bool ValidateFormData()
        {
            bool result = true;

            if (this.radTextBox_ItemName.Text == null || this.radTextBox_ItemName.Text.Length == 0)
            {
                this.radTextBox_ItemName.BackColor = Color.Red;
                result = false;
            }

            return result;
        }
        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.AddProject(project);

            this.Close();
        }

        private void radButton_Update_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.UpdateProject(project);

            this.Close();
        }

        void SetData()
        {
            project.ProjectName = this.radTextBox_ItemName.Text;
            project.ReadOnly = this.radCheckBox_ReadOnly.Checked;
        }

        private void radTextBox_Click(object sender, EventArgs e)
        {
            ((RadTextBox)sender).BackColor = Color.White;
        }

        private void radCheckBox_ReadOnly_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для установки значения \"Только чтения\" обратитесь к администратору.");
        }


    }
}
