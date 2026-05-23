using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;
using Microsoft.Extensions.Configuration;

namespace Sahakaar_API.Interfaces.Masters
{
    public interface IMaster
    {
        //Task<decimal> Insert_Update(decimal? id, mEventMaster item);
        //Task<int> Delete(decimal Id);
        //Task<decimal> Count(string search);
        //Task<mEventMaster> GetById(decimal Id);
        Task<List<object>> ListAll(string ListFor, decimal Id = 0, string IdFieldName = "Id");

    }
}
