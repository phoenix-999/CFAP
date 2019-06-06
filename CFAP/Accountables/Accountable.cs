using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAP.DataProviderClient
{
    partial class Accountable
    {
        public override string ToString()
        {
            if (this.AccountableName != null)
                return this.AccountableName;
            else
                return "";
        }
    }
}
