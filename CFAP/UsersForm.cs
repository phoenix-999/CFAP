using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CFAP.DataProviderClient;

namespace CFAP
{
    public partial class UsersForm : Telerik.WinControls.UI.RadForm
    {
        BindingList<User> Users { get; set; }
        public UsersForm()
        {
            InitializeComponent();

            if (CFAPBusinessLogic.UsersData == null)
            {
                MessageBox.Show("Данные о пользователях не были загружены.", "Ошибка получения данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Users = new BindingList<User>(CFAPBusinessLogic.UsersData);

            InitializeDataGrid();
        }

        void InitializeDataGrid()
        {
            this.radGridView.DataSource = this.Users;


            this.radGridView.Columns["Password"].IsVisible = false;
            this.radGridView.Columns["UserGroups"].IsVisible = false;

            this.radGridView.Columns["CanChangeUsersData"].HeaderText = "Может править данные других пользователей";
            this.radGridView.Columns["CanChangeUsersData"].WrapText = true;

            this.radGridView.Columns["Id"].ReadOnly = true;

            this.radGridView.Columns["IsAdmin"].HeaderText = "Администратор";

            this.radGridView.Columns["UserName"].HeaderText = "Имя пользователя";

            
        }

    }
}
