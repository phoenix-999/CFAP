﻿using System;
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
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        CFAPBusinessLogic businessLogic;

        public MainForm()
        {
            InitializeComponent();

            if (CFAPBusinessLogic.User == null)
            {
                Application.Exit();
            }
            businessLogic = new CFAPBusinessLogic();

            InitializeRadMenu();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        void InitializeRadMenu()
        {
            if (CFAPBusinessLogic.User.CanChangeUsersData == false)
            {
                this.radMenuItem_Users.Visibility = ElementVisibility.Hidden;
            }
            else
            {
                businessLogic.LoadUsers();
            }

            if (CFAPBusinessLogic.User.IsAdmin == false)
            {
                this.radMenuItem_Accountables.Visibility = ElementVisibility.Hidden;
                this.radMenuItem_BudgetItems.Visibility = ElementVisibility.Hidden;
                this.radMenuItem_Projects.Visibility = ElementVisibility.Hidden;
            }
        }

        void InitializeMenuItemUsers()
        {

        }

        private void radMenuItem_Users_Click(object sender, EventArgs e)
        {
            if (CFAPBusinessLogic.UsersData == null)
                return;

            new UsersForm().Show();
        }
    }
}