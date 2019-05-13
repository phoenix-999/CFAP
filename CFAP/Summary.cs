using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAP.DataProviderService
{
    public partial class Summary
    {
        public Summary()
        {
            this.SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            this.ActionDate = DateTime.Now;
        }
    }
}
