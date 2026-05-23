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

namespace Sahakaar_API.Services
{
    public class svcCommon : ICommon
    {
        public string sTableName = "Master";
        public string sGetList_ProcedureName = "GetList_Master";
        public string sSearch_ProcedureName = "SearchMemberInfoByAccountNo";
        public string sSearchMem_ProcedureName = "SearchMemberInfoByMembershipNo";
        public string sSearchAg_ProcedureName = "SearchAgentByAgentCode";
        public string sgetLedger_ProcedureName = "GetLedgerDetail";
        public string sDelete_ProcedureName = "Delete_DataById";
        public string sbalance_ProcedureName = "GetAccountBalance";
        public string sGetCount_ProcedureName = "GetCount";
        public string sAddEdit_ProcedureName = "AddEdit_Master";
        private readonly string sUpdateImageData_ProcedureName = "Update_Master_ImageData";
        public string sReference_ProcedureName = "SearchReferenceBeforeDelete";
        public string sVerify_ProcedureName = "Verify_DataById";
        public string sGetImageData_ProcedureName = "GetImageNameById";

        private readonly IDapperManager _dapperManager;
        public svcCommon(IDapperManager dapperManager)
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

        public async Task<List<object>> Search(string ListFor)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("MemberAccountNo", ListFor);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(sSearch_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }


        public async Task<List<object>> SearchLed(string ListFor, string proc)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("Search", ListFor);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(proc, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }

        public async Task<List<object>> Getbb(string ListFor, string proc)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("F_LedgerId", ListFor);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(proc, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }


        public async Task<List<object>> SearchMem(string ListFor)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("MembershipNo", ListFor);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(sSearchMem_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }





        public async Task<List<object>> AcBalance(string ListFor, decimal F_AccountType)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("MemberAccountNo", ListFor);
            dbPara.Add("F_AccountType", F_AccountType);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(sbalance_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }

        public async Task<List<object>> GetLedgers(string ListFor)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("F_LedgerMaster", ListFor);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(sgetLedger_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }





        public async Task<List<object>> SearchAg(string ListFor)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("AgentCode", ListFor);
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(sSearchAg_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
        }
        public async Task<decimal> Insert_Update(DynamicParameters dbPara, string sAddEdit_Procedure = "")
        {
            var rId = Task.FromResult(_dapperManager.Insert_Update<decimal>(sAddEdit_Procedure != "" ? sAddEdit_Procedure : sAddEdit_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await rId;
        }

        public async Task<List<object>> Login(DynamicParameters dbPara, string sAddEdit_Procedure = "")
        {
            var lstAll = Task.FromResult(_dapperManager.GetAll<object>(sAddEdit_Procedure != "" ? sAddEdit_Procedure : sAddEdit_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return await lstAll;
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

        public Task<decimal> TransactionAmount(string search)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("TableName", sTableName);
            dbPara.Add("Id", search);
            var rcount = Task.FromResult(_dapperManager.Get<decimal>(sGetCount_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rcount;
        }
        public Task<string> Update_ImageData(string tablename, decimal Id, mImageData item, string sFieldName = "ImageURL")
        {
            var dbPara = new DynamicParameters();
            if (Id > 0)
                dbPara.Add("Id", Id);
            dbPara.Add("TableName", tablename, DbType.String);
            dbPara.Add("ImageFieldName", sFieldName, DbType.String);
            dbPara.Add("ImageURL", item.ImageFileName, DbType.String);
            var rId = Task.FromResult(_dapperManager.Insert_Update<string>(sUpdateImageData_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rId;
        }
        public Task<string> Update_ImageDataNew(string sTableName, decimal Id, string ImageFileName, string sFieldName = "ImageURL")
        {
            var dbPara = new DynamicParameters();
            if (Id > 0)
                dbPara.Add("Id", Id);
            dbPara.Add("TableName", sTableName, DbType.String);
            dbPara.Add("ImageFieldName", sFieldName, DbType.String);
            dbPara.Add("ImageURL", ImageFileName, DbType.String);
            var rId = Task.FromResult(_dapperManager.Insert_Update<string>(sUpdateImageData_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rId;
        }
        public Task<string> Get_ImageName(string sTableName, decimal Id,  string sFieldName = "ImageURL")
        {
            var dbPara = new DynamicParameters();
            if (Id > 0)
                dbPara.Add("Id", Id);
            dbPara.Add("TableName", sTableName, DbType.String);
            dbPara.Add("ImageFieldName", sFieldName, DbType.String);
            var photodesign = Task.FromResult(_dapperManager.Get<string>(sGetImageData_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return photodesign;
        }
        public Task<int> SearchReferenceBeforeDelete(string TableName = "", decimal Id = 0)
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("TableName", TableName);
            dbPara.Add("Id", Id);
            var msg = Task.FromResult(_dapperManager.Get<int>(sReference_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return msg;
        }
        public async Task<DataSet> Insert_Update_WithDataSet( DynamicParameters dbPara, string sAddEdit_Procedure = "")
        {
            var ds = Task.FromResult
            (
                _dapperManager.GetDataSet
                (
                    sAddEdit_Procedure != ""
                    ? sAddEdit_Procedure
                    : sAddEdit_ProcedureName,

                    dbPara,
                    commandType: CommandType.StoredProcedure
                )
            );

            return await ds;
        }
        public Task<int> Verify(decimal Id, string sTableName_ = "")
        {
            var dbPara = new DynamicParameters();
            dbPara.Add("TableName", (sTableName_ != "" ? sTableName_ : sTableName));
            dbPara.Add("Id", Id);
            var rdelete = Task.FromResult(_dapperManager.Execute(sVerify_ProcedureName, dbPara, commandType: CommandType.StoredProcedure));
            return rdelete;
        }
    }

    public class mCommonList
    {
    }
    public class mCommonAdd_Edit
    {
    }
}
