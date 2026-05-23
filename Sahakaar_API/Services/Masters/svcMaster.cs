using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using Sahakaar_API.Interfaces;
using Sahakaar_API.Interfaces.Masters;
using Sahakaar_API.Models.Masters;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Sahakaar_API.Services.Masters
{
    public class svcMaster : IMaster
    {
        public string sTableName = "Master";
        public string sGetList_ProcedureName = "GetList_Master";
        public string sDelete_ProcedureName = "Delete_DataById";
        public string sGetCount_ProcedureName = "GetCount";
        public string sAddEdit_ProcedureName = "AddEdit_Master";
        //
        private readonly IDapperManager _dapperManager;
        public svcMaster(IDapperManager dapperManager)
        {
            this._dapperManager = dapperManager;
        }
        public async Task<List<object>> ListAll(string ListFor, decimal Id = 0, string IdFieldName = "Id")
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("ListFor", ListFor);
            dbPara.Add("Id", Id);
            dbPara.Add("IdFieldName", IdFieldName);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(sGetList_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }
        public async Task<decimal> Insert_Update(DynamicParameters dbPara, string sAddEdit_Procedure = "")
        {
            var rId = Task.FromResult(_dapperManager.Insert_Update<decimal>(sAddEdit_Procedure != "" ? sAddEdit_Procedure : sAddEdit_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await rId;
        }
        public Task<int> Delete(decimal Id, string sTableName_ = "")
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("TableName", (sTableName_ != "" ? sTableName_ : sTableName));
            dbPara.Add("Id", Id);
            var rdelete = Task.FromResult(_dapperManager.Execute(sDelete_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rdelete;
        }
        public Task<decimal> Count(string search)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("TableName", sTableName);
            dbPara.Add("Id", search);
            var rcount = Task.FromResult(_dapperManager.Get<decimal>(sGetCount_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rcount;
        }
    }
}
