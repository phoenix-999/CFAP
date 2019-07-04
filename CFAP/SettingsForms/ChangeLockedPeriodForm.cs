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
using System.Linq;

namespace CFAP
{
    public partial class ChangeLockedPeriodForm : Telerik.WinControls.UI.RadForm
    {
        const int START_PERIOD_YEAR = 2000;

        int endPeriodYear;

        Period period;
        ChangeDataOptions changeDataOption;
        CFAPBusinessLogic businessLogic;
        public ChangeLockedPeriodForm(Period period, ChangeDataOptions changeDataOption)
        {
            InitializeComponent();

            this.period = period;
            this.changeDataOption = changeDataOption;

            endPeriodYear = DateTime.Now.Year;

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
            this.radDropDownList_Month.Text = period.Month.ToString();
            this.radCheckBox_Lock.Checked = period.IsLocked;

            this.radDropDownList_Year.DataSource = new List<int>(Enumerable.Range(START_PERIOD_YEAR, endPeriodYear - START_PERIOD_YEAR + 1));
            this.radDropDownList_Year.SortStyle = Telerik.WinControls.Enumerations.SortStyle.Descending;
            this.radDropDownList_Year.Text = period.Year.ToString();
        }

        bool ValidateFormData()
        {
            bool result = true;

            try
            {
                int month = int.Parse(this.radDropDownList_Month.Text);

                if (this.radDropDownList_Month.Text == null
                    || month <= 0
                    || month > 12
                    || string.IsNullOrEmpty(this.radDropDownList_Month.Text)
                    )
                {
                    this.radDropDownList_Month.BackColor = Color.Red;
                    result = false;
                }
            }
            catch (Exception)
            {
                this.radDropDownList_Month.BackColor = Color.Red;
                result = false;
            }

            try
            {
                int year = int.Parse(this.radDropDownList_Year.Text);

                if (this.radDropDownList_Year.Text == null
                    || year <= START_PERIOD_YEAR
                    || year > endPeriodYear
                    || string.IsNullOrEmpty(this.radDropDownList_Year.Text)
                    )
                {
                    this.radDropDownList_Year.BackColor = Color.Red;
                    result = false;
                }
            }
            catch (Exception)
            {
                this.radDropDownList_Year.BackColor = Color.Red;
                result = false;
            }

            return result;
        }
        private void radButton_Add_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.AddPeriod(period);

            this.Close();
        }

        private void radButton_Update_Click(object sender, EventArgs e)
        {
            if (!ValidateFormData())
                return;

            SetData();

            businessLogic.UpdatePeriod(period);

            this.Close();
        }

        void SetData()
        {
            period.Year = int.Parse(this.radDropDownList_Year.Text);
            period.Month = int.Parse(this.radDropDownList_Month.Text);
            period.IsLocked = radCheckBox_Lock.Checked;
        }

        private void radDropDownList_Click(object sender, EventArgs e)
        {
            ((RadDropDownList)sender).BackColor = Color.White;
        }

        private void radDropDownList_Year_Click(object sender, EventArgs e)
        {
            radDropDownList_Click(sender, e);
            this.radDropDownList_Year.SelectedIndex = 0;
        }
    }
}
