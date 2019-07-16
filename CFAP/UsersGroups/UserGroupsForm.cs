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
    public partial class UserGroupsForm : Telerik.WinControls.UI.RadForm
    {
        BindingList<UserGroup> UserGroups { get; set; }
        public UserGroupsForm()
        {
            InitializeComponent();          

            this.UserGroups = new BindingList<UserGroup>(CFAPBusinessLogic.UserGroups);

            InitializeDataGrid();
        }

        void InitializeDataGrid()
        {
            this.radGridView.DataSource = this.UserGroups;

            InitializeColumns();
        }

        void InitializeColumns()
        {
            this.radGridView.Columns["Id"].IsVisible = false;

            this.radGridView.Columns["CanReadAccountablesSummary"].HeaderText = "Доступ к данным подотчетников";

            this.radGridView.Columns["GroupName"].HeaderText = "Группа";
        }


        private void radButton_AddItem_Click(object sender, EventArgs e)
        {
            new ChangeUserGroupForm(new UserGroup(), ChangeDataOptions.AddNew).ShowDialog();
            this.UserGroups.ResetBindings();
        }

        private void radGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Row.DataBoundItem is UserGroup)
            {
                UserGroup userGroupToChange = (UserGroup)e.Row.DataBoundItem;
                new ChangeUserGroupForm(userGroupToChange, ChangeDataOptions.ChangeData).ShowDialog();
                this.UserGroups.ResetBindings();
            }
        }
    }
}
