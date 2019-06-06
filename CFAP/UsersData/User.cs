using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAP.DataProviderClient
{
    partial class User
    {
        public override string ToString()
        {
            if (this.UserName != null)
                return this.UserName;
            else
                return "";
        }
    }
}
