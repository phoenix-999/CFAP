﻿using System;
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
    public partial class ChangeUserDataForm : Telerik.WinControls.UI.RadForm
    {
        User user;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;
        public ChangeUserDataForm(User user, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.user = user;
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
                    this.radButton_AddUser.Enabled = true;
                    break;
                case ChangeDataOptions.ChangeData:
                    this.radButton_CahngeUser.Enabled = true;
                    break;
            }
        }

        void InitializeFileds()
        {
            if (user.UserName != null)
                this.radTextBox_UserName.Text = user.UserName;

            if (user.Password != null && user.Password.Length > 0)
            {
                this.radTextBox_Password.PasswordChar = '*';
                //this.radTextBox_Password.ReadOnly = true;
                this.radTextBox_Password.Text = user.Password;
            }

            this.radCheckBox_CanChangeUsersData.Checked = user.CanChangeUsersData;
            this.radCheckBox_IsAdmin.Checked = user.IsAdmin;

            AttachUserGroups();
        }

        void AttachUserGroups()
        {
            if (CFAPBusinessLogic.UserGroups == null && CFAPBusinessLogic.UserGroups.Count == 0)
            {
                MessageBox.Show("Группы пользователя не были получены.", "Ошибка загрузки данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.radCheckedDropDownList_UserGroups.DataSource = CFAPBusinessLogic.UserGroups;

            foreach (var item in this.radCheckedDropDownList_UserGroups.Items)
            {
                UserGroup group = item.DataBoundItem as UserGroup;
                if (group == null)
                    continue;

                if (changeDataOption == ChangeDataOptions.AddNew)
                    continue;

                List<UserGroup> userGroups = new List<UserGroup>(user.UserGroups);
                bool userHasGroup = userGroups.Find(g => g.GroupName == group.GroupName) != null;
                if (!userHasGroup)
                    continue;

                //Обходной маневр. Невозможно присовить значения этому свойсту напрямую, но оно там есть
                Type itemType = item.GetType();
                PropertyInfo checkedProperty = itemType.GetRuntimeProperty("Checked");
                checkedProperty.SetValue(item, true);
            }
        }

        bool ValidateFormData()
        {
            bool result = true;

            if (this.radTextBox_UserName.Text == null || this.radTextBox_UserName.Text.Length == 0)
            {
                this.radTextBox_UserName.BackColor = Color.Red;
                result = false;
            }

            if (this.radTextBox_Password.Text == null || this.radTextBox_Password.Text.Length == 0)
            {
                this.radTextBox_Password.BackColor = Color.Red;
                result = false;
            }

            if (this.radCheckedDropDownList_UserGroups.CheckedItems.Count == 0)
            {
                this.radCheckedDropDownList_UserGroups.BackColor = Color.Red;
                result = false;
            }

            return result;
        }
        private void radButton_AddUser_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetUserData();

            businessLogic.AddUser(user);

            this.Close();
        }

        private void radButton_CahngeUser_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetUserData();

            businessLogic.UpdateUser(user);

            this.Close();
        }

        void SetUserData()
        {
            user.UserName = this.radTextBox_UserName.Text;
            user.Password = this.radTextBox_Password.Text;
            user.IsAdmin = this.radCheckBox_IsAdmin.Checked;
            user.CanChangeUsersData = this.radCheckBox_CanChangeUsersData.Checked;

            List<UserGroup> userGroups = new List<UserGroup>();
            foreach (var group in this.radCheckedDropDownList_UserGroups.CheckedItems)
            {
                userGroups.Add((UserGroup)group.Value);
            }

            user.UserGroups = userGroups.ToArray();
        }

        private void radTextBox_Click(object sender, EventArgs e)
        {
            ((RadTextBox)sender).BackColor = Color.White;
        }

        private void radCheckedDropDownList_UserGroups_Click(object sender, EventArgs e)
        {
            ((RadCheckedDropDownList)sender).BackColor = Color.White;
        }

        
    }
}
