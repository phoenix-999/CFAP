using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAPDataModel
{
    public enum DbConcurencyUpdateOptions
    {
        None,
        ClientPriority,
        DatabasePriority
    }
}
