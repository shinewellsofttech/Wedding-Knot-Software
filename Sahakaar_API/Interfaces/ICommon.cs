using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sahakaar_API.Models;
using Sahakaar_API.Models.Masters;
using Microsoft.Extensions.Configuration;

namespace Sahakaar_API.Interfaces
{
    public interface ICommon
    {
        //Task<mEventMaster> GetById(decimal Id);
        //Task<List<mMaster>> ListAll(string UserId);
        //Task<decimal> Insert_Update(decimal? id, mEventMaster item);
        Task<int> Delete(decimal Id, string sTableName_="");
        Task<decimal> Count(string search);
    }
}
