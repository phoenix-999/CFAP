﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CFAP
{
    public partial class AuthenticationForm : Telerik.WinControls.UI.RadForm
    {
        CFAPBusinessLogic businessLogic;
        public AuthenticationForm()
        {
            InitializeComponent();
            ExceptionsHandler exceptionsHandler = new ExceptionsHandlerUI();
            businessLogic = new CFAPBusinessLogic(exceptionsHandler);
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            this.radDropDownList_Logins.DataSource = businessLogic.GetLogins();
        }

        private void radButton_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                businessLogic.Authenticate(radDropDownList_Logins.SelectedItem.Text, radTextBox_Password.Text);
            }
            catch (NullReferenceException) //Исключение набоюдаеться при попытке входа в систему с другого домена
            {
                MessageBox.Show("Аутентификация не выполнена", "Ошибка аутентификации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CFAPBusinessLogic.User == null)
            {
                return;
            }
            new MainForm().Show();
            this.Hide();
        }
    }
}
