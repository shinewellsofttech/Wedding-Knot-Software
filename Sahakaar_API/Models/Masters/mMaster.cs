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
    public class InvoiceCharge
    {
        public string charges { get; set; }

        public string amount { get; set; }
    }


    public class InvoiceData
    {
        public decimal F_InvoiceMasterH { get; set; }

        public decimal F_UnitMaster { get; set; }
        public decimal F_ItemMaster { get; set; }
        public decimal F_GodownMaster { get; set; }
        public decimal ItemName { get; set; }
        public string TaxName { get; set; }
        public string CodeNo { get; set; }
        public decimal Qty { get; set; }
       
        public decimal Disc { get; set; }
        public decimal Amount { get; set; }
       
        public decimal Rate { get; set; }
   
    }  
    public class PurchaseData
    {
        public decimal F_InvoiceMasterH { get; set; }
      
        public decimal F_GodownMaster { get; set; }
        public decimal F_UnitMaster { get; set; }
        public decimal F_ItemMaster { get; set; }
        public decimal ItemName { get; set; }
        public decimal Cartoon { get; set; }
   
        public string CodeNo { get; set; }
        public decimal Qty { get; set; }
       
        public decimal Disc { get; set; }
        public decimal Amount { get; set; }
       
        public decimal Rate { get; set; }
   
    }



    public class InvoiceTax
    {
        public string Tax { get; set; }

        public string Amount { get; set; }
    }


    public class InvoiceTaxableAmount
    {
        public string igst { get; set; }

        public string amount { get; set; }
    }


    public class mAccountToAccountRequest
    {
        [Required(ErrorMessage = "SenderTransactionAcNo is required.")]
        public string SenderTransactionAcNo { get; set; }

        [Required(ErrorMessage = "RecipientTransactionAcNo is required.")]
        public string RecipientTransactionAcNo { get; set; }

        [Required(ErrorMessage = "Transaction Amount is required.")]
        public Decimal TransactionAmount { get; set; }

        public string SenderName { get; set; }

        public string RecipientName { get; set; }

        [Required(ErrorMessage = "SenderAccountType is required.")]
        public Decimal SenderAccountType { get; set; }

        [Required(ErrorMessage = "RecipentAccountType is required.")]
        public Decimal RecipentAccountType { get; set; }
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
        public decimal PoNo { get; set; } = 0;
        public decimal SoNo { get; set; } = 0;
        public bool AllowNegativeStock { get; set; } = false;
        public decimal UserId { get; set; } = 0;
        public decimal F_StateMaster { get; set; } = 0;
        public decimal F_CityMaster { get; set; } = 0;
        public string FirmAddress { get; set; } = string.Empty;
        public string GSTNo { get; set; } = string.Empty;
    }

    public class mAgentMaster
    {
        [Required(ErrorMessage = "MembershipNo is required.")]
        public string MembershipNo { get; set; }

        [Required(ErrorMessage = "MemberName is required.")]
        public string MemberName { get; set; }

        [Required(ErrorMessage = "JOB is required.")]
        public Decimal JOB { get; set; }
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
    }
    public class mCategoryMaster
    {
        public string Name { get; set; } = null;
    }

    public class mLedgerMaster
    {
        public decimal F_LedgerGroupMaster { get; set; } = 0;
        public string Name { get; set; } = null;
        public string Address { get; set; } = null;
        public decimal? MobileNo { get; set; } 
    }
    public class mVendorMaster
    {
        public string Name { get; set; } = null;
        public string MobileNo { get; set; } = null;
        public string EmailId { get; set; } = null;
        public string Website { get; set; } = null;
        public string Address { get; set; } = null;
        public string Phone { get; set; } = null;
        public string CompanyName { get; set; } = null;
        public decimal F_CityMaster { get; set; } = 0;
        public decimal F_StateMaster { get; set; } = 0;
    }

    public class mPurchaseMasterH
    {
        public string PNo { get; set; } = string.Empty;
        public DateTime? PDate { get; set; } = null;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal TotalQty { get; set; } = 0;
        public decimal F_StatusMaster { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public string StrPurchaseMasterL { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
    }
    public class mSalesMasterH
    {
        public string SONo { get; set; } = string.Empty;
        public DateTime? SODate { get; set; } = null;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal TotalQty { get; set; } = 0;
        public decimal F_StatusMaster { get; set; } = 0;
        public string StrSalesMasterL { get; set; } = string.Empty;
        public string Remarks { get; set; }
    }
    public class mStockMasterInOut
    {
        public decimal F_WarehouseMaster { get; set; } = 0;
        public decimal F_ItemMaster { get; set; } = 0;
        public decimal TotalQty { get; set; } = 0;
        public decimal MinQty { get; set; } = 0;
    }

    public class mStockMasterH
    {
        public string StockNo { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public decimal F_WarehouseMaster { get; set; } = 0;
        public decimal F_PurchaseMasterH { get; set; } = 0;
        public decimal F_SalesMasterH { get; set; } = 0;
        public DateTime? StockDate { get; set; } = null;
        public decimal TotalInQty { get; set; } = 0;
        public decimal TotalOutQty { get; set; } = 0;
        public string StrStockMasterL { get; set; } = string.Empty;
        public decimal F_VendorMaster { get; set; } = 0;
    }

    public class mAttDate
    {
        [Required(ErrorMessage = " AttDate is required.")]
        public string AttDate { get; set; }

        public string EmployeeCode { get; set; }

        public Decimal F_CompanyMaster { get; set; }
    }
    public class mChangePasswords
    {
        [Required(ErrorMessage = "F_UserMaster")]
        public string F_UserMaster { get; set; }

        [Required(ErrorMessage = "Old Password is required.")]
        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }


public class mStockH
    {
        public string VoucherNo { get; set; }
     
        public DateTime VoucherDate { get; set; }
        public bool OpeningStock { get; set; }
        public string Info { get; set; }

        public string EntryMode { get; set; }

        public string DiscountType { get; set; }
        public Decimal TotalQty { get; set; }


    }


    public class mInvoiceL
    {
        public Decimal F_InvoiceMasterH { get; set; }
        public bool IsOutside { get; set; }

        public string Data { get; set; }
    }

    public class mInvoiceLbyCode
    {
        public string Code { get; set; }

        public Decimal F_LedgerMaster { get; set; }
    }

    public class mInvoiceReport
    {
        public Decimal F_LedgerMaster { get; set; }

        public Decimal F_ItemTypeMaster { get; set; }

        public bool IsDebitNote { get; set; }

        public bool IsCreditNote { get; set; }

        public bool IsSale { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string PONo { get; set; }

        public Decimal Length { get; set; }

        public Decimal Width { get; set; }

        public Decimal Height { get; set; }

        public string LengthSoot { get; set; }

        public string WidthSoot { get; set; }

        public string HeightSoot { get; set; }

        public Decimal F_PlyMaster { get; set; }

        public Decimal Qty { get; set; }

        public string CodeNo { get; set; }
    }



    public class mLedger
    {
        public string Name { get; set; }

        public string Alias { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Phone { get; set; }

        public Decimal Mobile { get; set; }

        public string Email { get; set; }

        public Decimal F_CountryMaster { get; set; }

        public Decimal F_CityMaster { get; set; }

        public Decimal F_LedgerGroupMaster { get; set; }

        public string Zip { get; set; }

        public string GSTNo { get; set; }

        public string PANNo { get; set; }

        public string Area { get; set; }

        public string CrDays { get; set; }

        public Decimal Rate { get; set; }

        public Decimal Type { get; set; }

        public Decimal GSTType { get; set; }

        public Decimal Calculation { get; set; }

        public Decimal Nature { get; set; }

        public Decimal RoundOffSales { get; set; }

        public Decimal F_GSTGroupTypeMaster { get; set; }

        public Decimal F_StateMaster { get; set; }

        public Decimal TaxAddLess { get; set; }

        public string CreditLimit { get; set; }

        public string ContactPerson1 { get; set; }

        public string ContactPerson1_Email { get; set; }

        public string ContactPerson1_Mobile { get; set; }

        public string ContactPerson2 { get; set; }

        public string ContactPerson2_Email { get; set; }

        public string ContactPerson2_Mobile { get; set; }
    }


    public class mLengthCal
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string IsDefault { get; set; }
    }


    public class mMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }

    public class mMember
    {
        [Required(ErrorMessage = "Member Type is required.")]
        public Decimal MembershipTypeId { get; set; }

        [Required(ErrorMessage = "Date Of Joining is required.")]
        public string DateOfJoining { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Sur Name is required.")]
        public string SurName { get; set; }

        public string MaidenName { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required.")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Marital Status is required.")]
        public Decimal MaritalStatusId { get; set; }

        public Decimal BloodGroupId { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public Decimal GenderId { get; set; }

        [Required(ErrorMessage = "Home Town is required.")]
        public string HomeTown { get; set; }

        [Required(ErrorMessage = "Country Code is required.")]
        public Decimal CountryId { get; set; }

        [Required(ErrorMessage = "Education is required.")]
        public Decimal EducationId { get; set; }

        [Required(ErrorMessage = "Profession is required.")]
        public Decimal ProfessionId { get; set; }

        public string Email { get; set; }

        public string MobileNo_SMS { get; set; }

        public string SpouseName { get; set; }

        public Decimal MemberId_Spouse { get; set; }

        [Required(ErrorMessage = "Address (Residential) is required.")]
        public string Address_Residential { get; set; }

        [Required(ErrorMessage = "Pincode (Residential) is required.")]
        public string Pincode_Residential { get; set; }

        public string MobileNo1_Residential { get; set; }

        public string MobileNo2_Residential { get; set; }

        public string PhoneNo1_Residential { get; set; }

        public string PhoneNo2_Residential { get; set; }

        [Required(ErrorMessage = "Address (Business) is required.")]
        public string Address_Business { get; set; }

        [Required(ErrorMessage = "Pincode (Business) is required.")]
        public string Pincode_Business { get; set; }

        public string MobileNo1_Business { get; set; }

        public string MobileNo2_Business { get; set; }

        public string PhoneNo1_Business { get; set; }

        public string PhoneNo2_Business { get; set; }

        public Decimal MemberId_Introducer { get; set; }

        public Decimal RelationshipId_Introducer { get; set; }

        [Required(ErrorMessage = "Agent is required.")]
        public Decimal MemberId_Agent { get; set; }

        [Required(ErrorMessage = "First Name (Father) is required.")]
        public string FirstName_Father { get; set; }

        public string MiddleName_Father { get; set; }

        [Required(ErrorMessage = "Sur Name (Father) is required.")]
        public string SurName_Father { get; set; }

        [Required(ErrorMessage = "Home Town (Father) is required.")]
        public string HomeTown_Father { get; set; }

        [Required(ErrorMessage = "Country Code (Father) is required.")]
        public Decimal CountryId_Father { get; set; }

        [Required(ErrorMessage = "Live/Deade (Father) status is required.")]
        public int IsLive_Father { get; set; }

        [Required(ErrorMessage = "First Name (Mother) is required.")]
        public string FirstName_Mother { get; set; }

        public string MiddleName_Mother { get; set; }

        [Required(ErrorMessage = "Sur Name (Mother) is required.")]
        public string SurName_Mother { get; set; }

        [Required(ErrorMessage = "Home Town (Mother) is required.")]
        public string HomeTown_Mother { get; set; }

        [Required(ErrorMessage = "Country Code (Mother) is required.")]
        public Decimal CountryId_Mother { get; set; }

        [Required(ErrorMessage = "Live/Dead (Mother) status is required.")]
        public int IsLive_Mother { get; set; }

        [Required(ErrorMessage = "First Name (Nominee) is required.")]
        public string FirstName_Nominee { get; set; }

        public string MiddleName_Nominee { get; set; }

        [Required(ErrorMessage = "Sur Name (Nominee) is required.")]
        public string SurName_Nominee { get; set; }

        [Required(ErrorMessage = "Date Of Birth (Nominee) is required.")]
        public string DateOfBirth_Nominee { get; set; }

        [Required(ErrorMessage = "Age (Nominee) is required.")]
        public int Age_Nominee { get; set; }

        [Required(ErrorMessage = "Gender (Nominee) is required.")]
        public Decimal GenderId_Nominee { get; set; }

        [Required(ErrorMessage = "Relationship (Nominee) is required.")]
        public Decimal RelationshipId_Nominee { get; set; }

        public Decimal IDDocumentTypeId_1 { get; set; }

        public string ID_DocNo_1 { get; set; }

        public mImageData ID_ImageURL_1 { get; set; }

        public Decimal IDDocumentTypeId_2 { get; set; }

        public string ID_DocNo_2 { get; set; }

        public mImageData ID_ImageURL_2 { get; set; }

        public mImageData ImageURL_Member { get; set; }

        public mImageData ImageURL_Signature { get; set; }

        [Required(ErrorMessage = "MIS Scheme is required.")]
        public Decimal F_MISScheme { get; set; }

        [Required(ErrorMessage = "MIS Amount is required.")]
        public Decimal MISAmount { get; set; }

        public Decimal ShareValue { get; set; }

        public Decimal NoOfShares { get; set; }

        public Decimal MembershipFees { get; set; }
    }

    public class mMemberDetailsReport
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string FromAge { get; set; }

        public string ToAge { get; set; }

        public Decimal F_MembershipType { get; set; }
    }



    public class mPaperType
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        public string FacePaper { get; set; }
    }


    public class mPersonToPersonRequest
    {
        [Required(ErrorMessage = "SenderTransactionAcNo is required.")]
        public string SenderTransactionAcNo { get; set; }

        [Required(ErrorMessage = "RecipientTransactionAcNo is required.")]
        public string RecipientTransactionAcNo { get; set; }

        [Required(ErrorMessage = "Transaction Amount is required.")]
        public Decimal TransactionAmount { get; set; }

        public string SenderName { get; set; }

        public string RecipientName { get; set; }

        public Decimal Reference { get; set; }
    }




    public class mPly
    {
        [Required(ErrorMessage = "F_PlyGrouypMaster is required.")]
        public Decimal F_PlyGroupMaster { get; set; }

        public string Name { get; set; }

        public Decimal NoOfPly { get; set; }

        public Decimal LeadPastingPlyMargin { get; set; }

        public Decimal StitchingPlyMargin { get; set; }

        public Decimal NoOfPaper { get; set; }

        public Decimal NoOfFloat { get; set; }
    }
    public class mPrintPartydescriptionH
    {
        public Decimal F_LedgerMaster { get; set; }
    }


    public class mPrintPartyDescriptionL
    {
        public string Data { get; set; }

        public Decimal F_LedgerMaster { get; set; }
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
    public class mUnitConversion
    {
        [Required(ErrorMessage = "UnitFrom is required.")]
        public decimal F_ItemMaster { get; set; }
        public string UnitTo { get; set; }

        public decimal Rate { get; set; }
      
    }  public class mStateMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public string StateCode { get; set; }
    }


    public class mTax
    {
        [Required(ErrorMessage = "F_GroupMaster is required.")]
        public Decimal F_GroupMaster { get; set; }

        public Decimal F_GSTGroupTypeMaster { get; set; }
    }


    public class mTransactionDeposit
    {
        [Required(ErrorMessage = "Id is required.")]
        public Decimal Id { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public Decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction Type  is required.")]
        public Decimal TransactionType { get; set; }

        public string Narration { get; set; }

        [Required(ErrorMessage = "UserId  is required.")]
        public Decimal F_UserMaster { get; set; }

        [Required(ErrorMessage = "UserId  is required.")]
        public Decimal F_MemberAccountMaster { get; set; }
    }




    public class mTransactionWithdraw
    {
        [Required(ErrorMessage = "F_UserMaster")]
        public Decimal F_UserMaster { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        public Decimal Amount { get; set; }

        [Required(ErrorMessage = "F_MemberAccountMaster")]
        public Decimal F_MemberAccountMaster { get; set; }
    }


    public class mUser
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }

        public Decimal F_UserRole { get; set; }

        public string UserPass { get; set; }
    }

    public class PlyPartyL
    {
        public string F_PlyParty { get; set; }

        public string Rate { get; set; }
    }



    public class PrintPartyL
    {
        public string F_PrintDiscription { get; set; }

        public string Rate { get; set; }
    }


    public class mChallan
    {
        public string StockNo { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public decimal F_WarehouseMaster { get; set; } = 0;
        public decimal F_PurchaseMasterH { get; set; } = 0;
        public decimal F_SalesMasterH { get; set; } = 0;
        public DateTime? StockDate { get; set; } = null;
        public decimal TotalInQty { get; set; } = 0;
        public decimal TotalOutQty { get; set; } = 0;
        public string StrStockMasterL { get; set; } = string.Empty;
        public decimal F_VendorMaster { get; set; } = 0;
    }

    public class mQuotationH
    {
        public string QuotationNo { get; set; } = string.Empty;
        public decimal F_PartyMaster { get; set; } = 0;
        public DateTime? QDate { get; set; } = null;
        public decimal TotalQty { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
        public decimal RateType { get; set; } = 0;
        public decimal DiscountType { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
        public string StrQuotationL { get; set; } = string.Empty;
        public decimal GST { get; set; } = 0;
        public decimal Freight { get; set; } = 0;
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
    public class mWarehouseTransferH
    {
        public string TransferNo { get; set; } = string.Empty;
        public DateTime? TransferDate { get; set; } = null;
        public decimal F_WarehouseMasterFrom { get; set; } = 0;
        public decimal F_WarehouseMasterTo { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public decimal TotalQty { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
        public string StrWarehouseTransferL { get; set; } = string.Empty;
    }
    public class mItemInspectionH
    {
        public string InspectionNo { get; set; } = string.Empty;
        public DateTime? InspectionDate { get; set; } = null;
        public string F_UserMaster { get; set; } = string.Empty;
        public string Remarks { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
        public string StrItemInspectionL { get; set; } = string.Empty;
    }

    public class mColorMaster
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public bool IsDefault { get; set; } = false; 
        public decimal UserId { get; set; } = 0;
    }
    public class mGetAvailableQtyInWarehouse
    {
        public decimal F_WarehouseMaster { get; set; } = 0;
        public decimal F_ItemGroupMaster { get; set; } = 0;
        public decimal F_ItemMaster { get; set; } = 0;
        public decimal F_ColorMaster { get; set; } = 0;
    }
    public class mGetPurchaseOrderForApproval
    {
        public decimal F_StatusMaster { get; set; } = 0;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal F_PurchaseMasterH { get; set; } = 0;
    }
    public class mGetSalesOrderForApproval
    {
        public decimal F_StatusMaster { get; set; } = 0;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal F_SalesMasterH { get; set; } = 0;
    }
    public class mGetStockDataReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
      //  public decimal F_ItemGroupMaster { get; set; } = 0;
      //  public decimal F_ItemMaster { get; set; } = 0;
    }
    public class mGetGroupWiseReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal F_ItemGroupMaster { get; set; } = 0;
        public decimal F_ItemMaster { get; set; } = 0;
        public decimal F_WarehouseMaster { get; set; } = 0;
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
    public class mGetReOrderQtyDetail
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
     //   public decimal F_VendorMaster { get; set; } = 0;
      //  public decimal F_StatusMaster { get; set; } = 0;
    }
    public class mReorderPurchase
    {
        public string PNo { get; set; } = string.Empty;
        public DateTime? PDate { get; set; } = null;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal TotalQty { get; set; } = 0;
        public decimal F_StatusMaster { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public string StrPurchaseMasterL { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
    }
    public class mGetMinItemQtyInWarehouses
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
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
    public class mDeliveryChallan
    {
        public decimal F_ChallanMaster { get; set; } = 0;
        public string DeliveryChallanNo { get; set; } = string.Empty;
        public DateTime? DeliveryChallanDate { get; set; } = null;
        public string Remarks { get; set; } = string.Empty;
        public decimal TotalQty { get; set; } = 0;
        public decimal TotalUnit1 { get; set; } = 0;
        public decimal TotalUnit2 { get; set; } = 0;
        public decimal TotalUnit3 { get; set; } = 0;
        public string StrDeliveryChallanL { get; set; } = string.Empty;
    }
    public class mGetTransactionsReport
    {
        public decimal TransactionType { get; set; } = 0;
        public decimal TransactionId { get; set; } = 0;
        public decimal VendorId { get; set; } = 0;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
    }
    public class mSalesInvoiceH
    {
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime? InvoiceDate { get; set; } = null;
        public decimal F_PartyMaster { get; set; } = 0;
        public decimal F_SalesMasterH { get; set; } = 0;
        public decimal TotalQty { get; set; } = 0;
        public decimal TotalAmount { get; set; } = 0;
        public decimal RateType { get; set; } = 0;
        public decimal DiscountType { get; set; } = 0;
        public decimal UserId { get; set; } = 0;
        public string StrSalesInvoiceL { get; set; } = string.Empty;
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
    public class mGetStockPositionReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal F_ItemGroupMaster { get; set; } = 0;
        public decimal F_WarehouseMaster { get; set; } = 0;
    }
    public class mGetStockPositionCompanyWiseReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal F_CompanyMaster { get; set; } = 0;
        public decimal F_WarehouseMaster { get; set; } = 0;
    }
    public class mGetStockLedgerReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal F_ItemMaster { get; set; } = 0;
        public decimal F_ColorMaster { get; set; } = 0;
        public decimal F_WarehouseMaster { get; set; } = 0;
        public decimal ReportType { get; set; } = 0;
        
    }
    public class mPurchaseReturnH
    {
        public decimal F_PurchaseMasterH { get; set; } = 0;
        public string PRNo { get; set; } = string.Empty;
        public DateTime? PRDate { get; set; } = null;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal TotalQty { get; set; } = 0;
        public decimal F_StatusMaster { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
        public decimal F_ReturnStatus { get; set; } = 0;
        public string StrPurchaseReturnL { get; set; } = string.Empty;
    }
    public class mSalesReturnH
    {
        public decimal F_SalesMasterH { get; set; } = 0;
        public string SRNo { get; set; } = string.Empty;
        public DateTime? SRDate { get; set; } = null;
        public decimal F_VendorMaster { get; set; } = 0;
        public decimal TotalQty { get; set; } = 0;
        public decimal F_StatusMaster { get; set; } = 0;
        public string Remarks { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
        public decimal F_ReturnStatus { get; set; } = 0;
        public string StrSalesReturnL { get; set; } = string.Empty;
    }
    public class mGetOrdersReturnReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal OrderType { get; set; } = 0;
        public decimal ReturnType { get; set; } = 0;
        public decimal F_VendorMaster { get; set; } = 0;

    }
    public class mGetOrderReturnLedgerReport
    {
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
        public decimal F_ItemMaster { get; set; } = 0;
        public decimal F_ColorMaster { get; set; } = 0;
        public decimal F_VendorMaster { get; set; } = 0;

    }
    public class mGetReplacementReport
    {
        public decimal OrderType { get; set; } = 0;
        public decimal ReturnType { get; set; } = 0;
        public decimal F_VendorMaster { get; set; } = 0;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
    }
    public class mUpdatePassword
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
    }
    public class mUpdateItemOpeningBalance
    {
        public decimal F_ItemGroupMaster { get; set; } = 0;
        public string StrItemOpeningBalance { get; set; } = string.Empty;
        public decimal UserId { get; set; } = 0;
    }
    public class mItemOpeningBalanceH
    {
        public string ItemOpeningBalanceNo { get; set; } = string.Empty;
        public DateTime? ItemOpeningBalanceDate { get; set; } = null; 
        public string Remarks { get; set; } = string.Empty;
        public decimal TotalBalance { get; set; } = 0;
        public string StrItemOpeningBalanceL { get; set; } = string.Empty;
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
    
}
