using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.Common;
using Sahakaar_API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

namespace Sahakaar_API.Services
{
    public class DapperManager : IDapperManager
    {
        private readonly IConfiguration _config;
        private readonly string connectionName = "DBConnectionString";
        public DapperManager(IConfiguration config)
        {
            _config = config;
        }
        public DbConnection GetConnection()
        {
            return new SqlConnection(_config.GetConnectionString(connectionName));
        }
        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }
        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            return db.Query<T>(sp, parms, commandType: commandType, commandTimeout: 600).ToList();
        }
        public int Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            return db.Execute(sp, parms, commandType: commandType);
        }
        public T Insert_Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName));
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public DataSet GetDataSet( string sp, DynamicParameters parameters, CommandType commandType )
        {
            DataSet ds = new DataSet();

            using (IDbConnection db = new SqlConnection(_config.GetConnectionString(connectionName)))
            {
                using (SqlCommand cmd = new SqlCommand(sp, (SqlConnection)db))
                {
                    cmd.CommandType = commandType;

                    if (parameters != null)
                    {
                        foreach (var paramName in parameters.ParameterNames)
                        {
                            cmd.Parameters.AddWithValue
                            (
                                paramName,
                                parameters.Get<dynamic>(paramName)
                                ?? DBNull.Value
                            );
                        }
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(ds);
                    }
                }
            }

            return ds;
        }
        public void Dispose()
        {

        }
    }
}
