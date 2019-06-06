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
    public partial class ChangeRateForm : Telerik.WinControls.UI.RadForm
    {
        Rate rate;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;
        public ChangeRateForm(Rate rate, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.rate = rate;
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
            this.radTextBox_RateUSD.Text = rate.RateUSD.ToString();
            this.radDateTimePicker_DateRate.Value = rate.DateRate;

            this.radCheckBox_ReadOnly.Checked = rate.ReadOnly;
        }

        bool ValidateFormData()
        {
            bool result = true;

            if (this.radTextBox_RateUSD.Text == null || this.radTextBox_RateUSD.Text.Length == 0)
            {
                this.radTextBox_RateUSD.BackColor = Color.Red;
                result = false;
            }

            try
            {
                double.Parse(this.radTextBox_RateUSD.Text);
            }
            catch (Exception)
            {
                result = false;
                this.radTextBox_RateUSD.BackColor = Color.Red;
                MessageBox.Show("Ошибка преобразования курса валют", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }
        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.AddRate(rate);

            this.Close();
        }

        private void radButton_Update_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.UpdateRate(rate);

            this.Close();
        }

        void SetData()
        {
            
            rate.RateUSD = double.Parse(this.radTextBox_RateUSD.Text);
            rate.DateRate = this.radDateTimePicker_DateRate.Value;
            rate.ReadOnly = this.radCheckBox_ReadOnly.Checked;
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
