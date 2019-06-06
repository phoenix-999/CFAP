using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAP.DataProviderClient
{
    partial class Project
    {
        public override string ToString()
        {
            if (this.ProjectName != null)
                return this.ProjectName;
            else
                return "";
        }
    }
}
