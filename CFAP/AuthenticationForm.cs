using System;
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
        BusinessLogicEvents businessLogicEvents;
        public AuthenticationForm()
        {
            InitializeComponent();
            businessLogicEvents = new BusinessLogicEvents();
        }

        private void AuthenticationForm_Load(object sender, EventArgs e)
        {
            this.radDropDownList_Logins.DataSource = businessLogicEvents.GetLogins();
        }

        private void radButton_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton_Ok_Click(object sender, EventArgs e)
        {
            businessLogicEvents.Authenticate(radDropDownList_Logins.SelectedItem.Text, radTextBox_Password.Text);
            if (BusinessLogicEvents.User == null)
            {
                return;
            }
            MessageBox.Show(string.Format("Аутентификация выполнена для пользователя {0}", BusinessLogicEvents.User.UserName));
        }
    }
}
