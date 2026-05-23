using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.Common;

namespace Sahakaar_API.Interfaces
{
    public interface IDapperManager : IDisposable
    {
        DbConnection GetConnection();
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        T Insert_Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        DataSet GetDataSet(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure);
    }
}
