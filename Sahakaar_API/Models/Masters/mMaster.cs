using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;

namespace Sahakaar_API.Models.Masters
{
    public class mMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
    public class mAddEdit_Master
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string TableName { get; set; }
        public Decimal Id { get; set; }
    }
    public class mAdminLogin
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "UserPassword is required.")]
        public string UserPassword { get; set; }
    }
    public class GlobalOptionsUpdate
    {
        public string FirmName { get; set; } = string.Empty;
        public decimal F_FinancialYearMaster { get; set; } = 0;
        public decimal F_StateMaster { get; set; } = 0;
        public decimal F_CityMaster { get; set; } = 0;
        public string FirmAddress { get; set; } = string.Empty;
        public string GSTNo { get; set; } = string.Empty;
        public string PANNo { get; set; } = string.Empty;
        public string MobileNo { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;

    }
    public class mItemMainGroupMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
    public class mItemGroupMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public decimal F_ItemMainGroupMaster { get; set; } = 0;
    }
    public class mImageFile
    {
        public IFormFile File { get; set; }
    }
    public class mItemMaster
    {
        public decimal F_CategoryMaster { get; set; } = 0;
        public string ItemName { get; set; } = null;
        public bool HasSize { get; set; } = false;
        public List<mItemDesign> DesignDetails { get; set; }
    }
    public class mItemDesign
    {
        public mImageData DesignPhoto { get; set; }
        public mImageData DesignPhoto2 { get; set; }
        public mImageData DesignPhoto3 { get; set; }
        public mImageData DesignPhoto4 { get; set; }
        public mImageData DesignPhoto5 { get; set; }
        public string VideoLink { get; set; }
        public string SizeName { get; set; }
        public decimal SalePrice { get; set; }
        public string Barcode { get; set; }
        public decimal OpeningStock { get; set; } = 0;
        public decimal Length { get; set; } = 0;
        public decimal Width { get; set; } = 0;
        public decimal Height { get; set; } = 0;
        public decimal Weight { get; set; } = 0;
    }
    public class mCategoryMaster
    {
        public string Name { get; set; } = null;
    }
    public class mLedgerMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;   // Required field

        public string? Alias { get; set; }
        public decimal F_LedgerGroupMaster { get; set; } = 0;

        public string? Address { get; set; }
        public string? Address1 { get; set; }

        public decimal F_CityMaster { get; set; } = 0;
        public decimal F_StateMaster { get; set; } = 0;
        public decimal F_CountryMaster { get; set; } = 0;

        public string? PinCode { get; set; }
        public string? PhoneNo { get; set; }
        public string? MobileNo { get; set; }
        public string? Email { get; set; }
        public string? GSTIN { get; set; }
        public string? PANNo { get; set; }

        public decimal F_GSTGroupMaster { get; set; } = 0;
        public string? F_GSTType { get; set; }
        public string? F_TaxPayerType { get; set; }

        public decimal CreditDays { get; set; } = 0;
        public decimal CreditLimit { get; set; } = 0;
        public decimal Rate { get; set; } = 0;

        public string? F_Type { get; set; }
        public string? F_CalculationType { get; set; }
        public string? F_AddLess { get; set; }

        public bool YesNoActs { get; set; } = false;

        public decimal F_LedgerMasterSales { get; set; } = 0;
        public decimal F_LedgerMasterPurchase { get; set; } = 0;

        public string? F_YearScheme { get; set; }
        public string? F_IntCalcMethod { get; set; }

        public string? BankName { get; set; }
        public string? BankAccountNo { get; set; }
        public string? BankIFSCCode { get; set; }

        public bool ISDalal { get; set; } = false;
        public decimal F_LedgerMasterDalal { get; set; } = 0;

        public bool IsTransport { get; set; } = false;
        public decimal F_TCSonSales { get; set; } = 0;

        public decimal UserId { get; set; } = 0;
        public decimal F_CompanyMaster { get; set; } = 0;
    }
    public class mChangePasswords
    {
        [Required(ErrorMessage = "F_UserMaster")]
        public string F_UserMaster { get; set; }

        [Required(ErrorMessage = "Old Password is required.")]
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
    public class mSearchByAccountNo
    {
        [Required(ErrorMessage = "MemberAccountNo is required.")]
        public string MemberAccountNo { get; set; }

        public string Name { get; set; }
    }
    public class mSearchLedger
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
    public class mCityMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public decimal F_StateMaster { get; set; }
      
    }
    public class mDeleteTransactions
    {
        [Required(ErrorMessage = "TillDate is required.")]
        public DateTime TillDate { get; set; }
    } 
    public class mRestoreTransactions
    {
        [Required(ErrorMessage = "FromDate is required.")]
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
      
    } 
     public class mStateMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string StateCode { get; set; }
    }
    public class mUser
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        public Decimal F_UserRole { get; set; }

        public string UserPass { get; set; }
    }
    public class mPOApprovedByAdmin
    {
        public decimal F_PurchaseMasterL { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        public decimal ApprovedQty { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
    }
    public class mSOApprovedByAdmin
    {
        public decimal F_SalesMasterL { get; set; } = 0;
        public string Status { get; set; } = string.Empty;
        public decimal ApprovedQty { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
    }
    public class mGetGroupWiseReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal F_ItemGroupMaster { get; set; } = 0;
        public decimal F_ItemMaster { get; set; } = 0;
    }
    public class mGetOrderDetail
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal F_StatusMaster { get; set; } = 0;
        public decimal F_PurchaseMasterH { get; set; } = 0;
        public decimal F_SalesMasterH { get; set; } = 0;
    }
    public class mUserRights
    {
        public decimal F_UserMaster { get; set; } = 0;
        public string StrPageList { get; set; } = string.Empty;
    }
    public class mPageMaster
    {
        public string Name { get; set; } = null;
        public string PageType { get; set; } = null;
    }
    public class mVoucherMasterH
    {
        public string VoucherNo { get; set; } = string.Empty;
        public DateTime? VoucherDate { get; set; } = null;
        public string ChequeNo { get; set; } = string.Empty;
        public DateTime? ChequeDate { get; set; } = null;
        public string ReceiptNo { get; set; } = string.Empty;
        public DateTime? ReceiptDate { get; set; } = null;
        public string Narration { get; set; } = string.Empty;
        public decimal TotalCr { get; set; } = 0;
        public decimal TotalDr { get; set; } = 0;
        public decimal CurrBal { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
        public string StrVoucherMasterL { get; set; } = string.Empty;
    }
    public class mUpdatePassword
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
    }
    public class mUpdateItemAndItemDesignMaster
    {
        public string Id { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string FieldValue { get; set; } = string.Empty;
        public IFormFile DesignPhoto { get; set; }
        public IFormFile DesignPhoto2 { get; set; }
        public IFormFile DesignPhoto3 { get; set; }
        public IFormFile DesignPhoto4 { get; set; }
        public IFormFile DesignPhoto5 { get; set; }
    }
    public class mGSTGroupMaster
    {
        public string GSTGroupName { get; set; } = string.Empty;
        public string GSTType { get; set; } = string.Empty;
        public string HSN_SAC_Code { get; set; } = string.Empty;
        public decimal GSTPercent { get; set; }    = 0;
        public decimal CGSTPercent { get; set; }   = 0;
        public decimal SGSTPercent { get; set; }   = 0;
        public decimal IGSTPercent { get; set; }   = 0;
        public decimal CessPercent { get; set; } = 0;
        public bool IsReverseCharge { get; set; } = false;
        public string Remarks { get; set; } = string.Empty;
    }
    public class mUnitMaster
    {
        public string UnitName { get; set; } = string.Empty;
        public string UnitCode { get; set; } = string.Empty;
        public bool DecimalAllowed { get; set; } = false;
    }
    public class mAlterUnitMaster
    {
        public decimal F_UnitMaster { get; set; } = 0;
        public decimal F_AlterUnit { get; set; } = 0;
        public decimal ConversionValue { get; set; } = 0;
    }
    public class mLedgerGroupMaster
    {
        public string Name { get; set; } = string.Empty;
        public decimal GroupType { get; set; } = 0;
        public decimal RelatedTo { get; set; } = 0;
        public decimal RelatedType { get; set; } = 0;
        public decimal Preference { get; set; } = 0;
        public bool CanChangeYesNo { get; set; } = false;
        public decimal F_TLedgerGroupMaster { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
        public decimal F_CompanyMaster { get; set; } = 0;
    }
    public class mPurchaseEntry
    {
        public DateTime? EntryDate { get; set; } = null;
        public String? EntryNo { get; set; } = null;
        public decimal F_LedgerMaster { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public decimal TotalCGST { get; set; } = 0;
        public decimal TotalSGST { get; set; } = 0;
        public decimal TotalIGST { get; set; } = 0;
        public decimal TotalTax { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
        public decimal F_CompanyMaster { get; set; } = 0;
        public string JsonData { get; set; } = string.Empty;
    }
    public class mGetItemDetailByBarcode
    {
        public string Barcode { get; set; } = string.Empty;
    }
    public class mGetItemDetailByReference
    {
        public decimal F_CategoryMaster { get; set; } = 0;
        public decimal F_GSTGroupMaster { get; set; } = 0;

    }
    public class mFinancialYearMaster
    {
        public DateTime? FinancialYearFrom { get; set; } = null;
        public DateTime? FinancialYearTo { get; set; } = null;
        public decimal F_FirmMaster { get; set; } = 0;
        public bool IsCurrentFinancialYear { get; set; } = false;
        
    }
    public class mSalesEntry
    {
        public DateTime? EntryDate { get; set; } = null;
        public String? EntryNo { get; set; } = null;
        public decimal F_LedgerMaster { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public decimal TotalCGST { get; set; } = 0;
        public decimal TotalSGST { get; set; } = 0;
        public decimal TotalIGST { get; set; } = 0;
        public decimal TotalTax { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
        public decimal F_CompanyMaster { get; set; } = 0;
        public string JsonData { get; set; } = string.Empty;
        public string OtherChargesJson { get; set; } = string.Empty;

    }

}
