using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;
using System.Collections.Specialized;
using Telerik.WinControls.UI;

namespace CFAP
{
    public partial class ProjectsForm : Telerik.WinControls.UI.RadForm
    {
        BindingList<Project> Projects { get; set; }
        public ProjectsForm()
        {
            InitializeComponent();          

            this.Projects = new BindingList<Project>(CFAPBusinessLogic.Projects);

            InitializeDataGrid();
        }

        void InitializeDataGrid()
        {
            this.radGridView.DataSource = this.Projects;

            InitializeColumns();
        }

        void InitializeColumns()
        {
            this.radGridView.Columns["RowVersion"].IsVisible = false;

            this.radGridView.Columns["Id"].IsVisible = false;

            this.radGridView.Columns["ReadOnly"].HeaderText = "Только чтения";
            this.radGridView.Columns["ReadOnly"].ReadOnly = true;

            this.radGridView.Columns["ProjectName"].HeaderText = "Проект";
        }


        private void radButton_AddItem_Click(object sender, EventArgs e)
        {
            new ChangeProjectForm(new Project(), ChangeDataOptions.AddNew).ShowDialog();
            this.Projects.ResetBindings();
        }

        private void radGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is Project)
            {
                Project projectToChange = (Project)e.Row.DataBoundItem;
                new ChangeProjectForm(projectToChange, ChangeDataOptions.ChangeData).ShowDialog();
                this.Projects.ResetBindings();
            }
        }
    }
}
