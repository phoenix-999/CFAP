using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFAPDataModel.Models
{
    public interface ICrudOperations
    {
        IEnumerable<ICrudOperations> Select(Filter filter, User user);
        ICrudOperations Add(DbConcurencyUpdateOptions concurencyUpdateOptions, User user);
        ICrudOperations Update(DbConcurencyUpdateOptions concurencyUpdateOptions, User user);
        ICrudOperations Delete(DbConcurencyUpdateOptions concurencyUpdateOptions, User user);
    }
}
