using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAP.DataProviderClient
{
    partial class BudgetItem
    {
        public override string ToString()
        {
            if (this.ItemName != null)
                return this.ItemName;
            else
                return "";
        }
    }
}
