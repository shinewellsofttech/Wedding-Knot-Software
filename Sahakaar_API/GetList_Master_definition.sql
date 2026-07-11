ALTER PROCEDURE [dbo].[GetList_Master]                            
 @Id numeric(18,0) = 0,                                                                                                        
 @IdFieldName nvarchar(50) = 'TBL.Id',                                                                                                        
 @ListFor NVARCHAR(max) = ''                                                                                                        
AS
BEGIN
	IF (@ListFor = 'F_StatusMaster' OR @ListFor = 'StatusMaster')
	BEGIN
		SELECT 1 AS Id, 'Pending' AS Name
		UNION ALL
		SELECT 2 AS Id, 'Approved' AS Name
		UNION ALL
		SELECT 3 AS Id, 'Rejected' AS Name
		UNION ALL
		SELECT 4 AS Id, 'Packed' AS Name
		UNION ALL
		SELECT 5 AS Id, 'Shipped' AS Name
		UNION ALL
		SELECT 6 AS Id, 'Out for Delivery' AS Name
		UNION ALL
		SELECT 7 AS Id, 'Delivered' AS Name;
		RETURN;
	END                                                                                                        
	IF(@IdFieldName='Id') SET @IdFieldName ='TBL.Id'                                                                                                        
	--                                                                                                        
	DECLARE @Tab TABLE(Id Numeric(18,0),Name varchar(50))                                                                           
	                                                                        
	DECLARE @WHERE NVARCHAR(MAX) = ''                                                                                                      
	DECLARE @JOINS NVARCHAR(MAX)  = ''                                                                                                       
	DECLARE @FIELDS NVARCHAR(MAX) = ''                                                                                                        
	DECLARE @ORDERBY NVARCHAR(MAX)  = ''                                                                                                       
	                         
	SET @FIELDS = 'TBL.Name,TBL.Id'                                                                                                        
	SET @WHERE = ' WHERE 1=(CASE WHEN(' + CAST(@Id AS NVARCHAR) + '=0) THEN 1 ELSE (CASE WHEN(' + @IdFieldName + ' =' + CAST(@Id AS NVARCHAR) + ') THEN 1 ELSE 0 END) END)'                                                                                       
	SET @ORDERBY = 'ORDER BY TBL.Name,TBL.Id'                                                                                                        
	                                                                                                       
	IF (ISNULL(@ListFor,'') <>'')                                                                                                        
	BEGIN                                                                                                        
	 DECLARE @TableName NVARCHAR(max)                                                                                                        
	     
	IF(@ListFor='AddressTypeMaster')
	BEGIN
		SET @TableName = 'AddressTypeMaster'
		SET @FIELDS = 'TBL.Id, TBL.Name, TBL.IsActive, TBL.UserId'
		SET @ORDERBY = 'ORDER BY TBL.Name'
	END
	ELSE IF(@ListFor='AddressTypeMasterEdit')
	BEGIN
		SET @TableName = 'AddressTypeMaster'
		SET @FIELDS = 'TBL.Id, TBL.Name, TBL.IsActive, TBL.UserId'
		SET @ORDERBY = ''
	END
	ELSE IF(@ListFor='CityMaster')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
	 SET @TableName = 'CityMaster'                                                                         
	 SET @FIELDS = 'TBL.Id, TBL.Name, TBL.UserId, TBL.F_StateMaster, 
					 TBL.F_CountryMaster, CM.Name AS CountryName, SM.Name AS StateName, TBL.PinCode ' 
	 SET @JOINS = ' LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster
					 LEFT JOIN StateMaster SM ON SM.Id = TBL.F_StateMaster
					 '                                                                                                                                    
	 SET @OrderBY = ' ORDER BY Name '                                                                                                                                    
	END

	ELSE IF(@ListFor='CityMasterEdit')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
	 SET @TableName = 'CityMaster'                                                                         
	 SET @FIELDS = 'TBL.Id, TBL.Name, TBL.UserId, TBL.F_StateMaster, TBL.F_CountryMaster, TBL.PinCode' 
	 SET @OrderBY = ''                                                                                                                                    
	END 

	ELSE IF(@ListFor='CityMasterByStateId')                                                                 
	BEGIN                                                                
	 SET @TableName = 'CityMaster'                                                                         
	 SET @FIELDS = 'TBL.Id, TBL.Name, TBL.UserId, TBL.F_StateMaster, 
					 TBL.F_CountryMaster, CM.Name AS CountryName, SM.Name AS StateName, TBL.PinCode ' 
	 SET @JOINS = ' LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster
					 LEFT JOIN StateMaster SM ON SM.Id = TBL.F_StateMaster
					 '       
	 SET @WHERE = ' WHERE TBL.F_StateMaster = ' + CONVERT(VARCHAR(100), @Id)
	 SET @OrderBY = ''  
	END  

	ELSE IF(@ListFor='CityByPinCode')
	BEGIN
		SET @TableName = 'CityMaster'
		SET @FIELDS = 'TBL.Id, TBL.Name, TBL.UserId, TBL.F_StateMaster, TBL.F_CountryMaster, CM.Name AS CountryName, SM.Name AS StateName, TBL.PinCode'
		SET @JOINS = ' LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster
					   LEFT JOIN StateMaster SM ON SM.Id = TBL.F_StateMaster '
		SET @WHERE = ' WHERE TBL.PinCode = CAST(CAST(' + CONVERT(VARCHAR(100), @Id) + ' AS INT) AS NVARCHAR(20))'
		SET @ORDERBY = ''
	END

	ELSE IF(@ListFor='ItemMasterData')
	BEGIN
	     SELECT
	        IM.Id
		   ,ISNULL(IM.F_CategoryMaster,0) AS F_CategoryMaster
	       ,ISNULL(IM.ItemName,'') AS ItemName
	       ,ISNULL(IM.HasSize,0) AS HasSize
	       ,ISNULL(IM.HSNCode,'') AS HSNCode
	       ,IM.UserId
	       ,IM.DateOfCreation
	       ,IM.LastUpdateOn
		   ,ISNULL(IM.F_GSTGroupMaster,0) AS F_GSTGroupMaster
		   ,ISNULL(IM.F_UnitMaster,0) AS F_UnitMaster
		   ,ISNULL(IM.F_MaterialMaster,0) AS F_MaterialMaster
		   ,ISNULL(IM.UnitConversion,0) AS UnitConversion
		     ,ISNULL(IM.ShortDescription,'') AS ShortDescription
	   ,ISNULL(IM.FullDescription,'') AS FullDescription
	  
	       ,(
	           SELECT
	                IDM.Id
	               ,CASE WHEN ISNULL(IDM.DesignPhoto,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + DesignPhoto END AS DesignPhoto
	               ,CASE WHEN ISNULL(IDM.DesignPhoto2,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + DesignPhoto2 END AS DesignPhoto2
	               ,CASE WHEN ISNULL(IDM.DesignPhoto3,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + DesignPhoto3 END AS DesignPhoto3
	               ,CASE WHEN ISNULL(IDM.DesignPhoto4,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + DesignPhoto4 END AS DesignPhoto4
	               ,CASE WHEN ISNULL(IDM.DesignPhoto5,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + DesignPhoto5 END AS DesignPhoto5
	               ,ISNULL(IDM.VideoLink,0) AS VideoLink
	               ,ISNULL(IDM.SizeName,'') AS SizeName
	               ,ISNULL(IDM.SalePrice,0) AS SalePrice
	               ,ISNULL(IDM.Barcode,'') AS Barcode
	               ,ISNULL(IDM.OpeningStock,0) AS OpeningStock
				   ,(
					ISNULL(IDM.OpeningStock,0)
					+ ISNULL(
					    (
					        SELECT SUM(Qty)
					        FROM PurchaseEntryL PEL
					        WHERE PEL.F_ItemDesignMaster = IDM.Id
					    ),0)
					+ ISNULL(
					    (
					        SELECT SUM(Qty)
					        FROM SalesReturnL SRL
					        WHERE SRL.F_ItemDesignMaster = IDM.Id
					    ),0)
					- ISNULL(
					    (
					        SELECT SUM(Qty)
					        FROM SalesEntryL SEL
					        WHERE SEL.F_ItemDesignMaster = IDM.Id
					    ),0)
					- ISNULL(
					    (
					        SELECT SUM(Qty)
					        FROM PurchaseReturnL PRL
					        WHERE PRL.F_ItemDesignMaster = IDM.Id
					    ),0)
					) AS AvailableQty
				   ,ISNULL(IDM.Length,'') AS Length
				   ,ISNULL(IDM.Width,'') AS Width
				   ,ISNULL(IDM.Height,'') AS Height
				   ,ISNULL(IDM.Weight,'') AS Weight
				   ,ISNULL(IDM.UnitConversion,0) AS UnitConversion
				   ,CASE WHEN ISNULL(IDM.DesignPhoto,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(DesignPhoto, LEN(DesignPhoto) - CHARINDEX('.', REVERSE(DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
	               ,CASE WHEN ISNULL(IDM.DesignPhoto2,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(DesignPhoto2, LEN(DesignPhoto2) - CHARINDEX('.', REVERSE(DesignPhoto2))) + '.webp' END AS DesignPhoto2_Thumb
	               ,CASE WHEN ISNULL(IDM.DesignPhoto3,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(DesignPhoto3, LEN(DesignPhoto3) - CHARINDEX('.', REVERSE(DesignPhoto3))) + '.webp' END AS DesignPhoto3_Thumb
	               ,CASE WHEN ISNULL(IDM.DesignPhoto4,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(DesignPhoto4, LEN(DesignPhoto4) - CHARINDEX('.', REVERSE(DesignPhoto4))) + '.webp' END AS DesignPhoto4_Thumb
	               ,CASE WHEN ISNULL(IDM.DesignPhoto5,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(DesignPhoto5, LEN(DesignPhoto5) - CHARINDEX('.', REVERSE(DesignPhoto5))) + '.webp' END AS DesignPhoto5_Thumb
				   ,ISNULL(IDM.PurchaseRate,0) AS PurchaseRate
				   ,ISNULL(IDM.EcomPrice,0) AS EcomPrice
				   ,ISNULL(IDM.IsEcom,0) AS IsEcom

	           FROM ItemDesignMaster IDM
	           WHERE IDM.F_ItemMaster = IM.Id
	           FOR JSON PATH
	        ) AS DesignDetails
			,(
	           SELECT
	                ISM.Id
	               ,ISNULL(ISM.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(ISM.FromRange,0) AS FromRange
				   ,ISNULL(ISM.ToRange,0) AS ToRange
				   ,ISNULL(ISM.Rate,0) AS Rate
	           FROM ItemSchemeMaster ISM
			   INNER JOIN ItemDesignMaster IDMM ON IDMM.Id = ISM.F_ItemDesignMaster
			   WHERE IDMM.F_ItemMaster = IM.Id
	           FOR JSON PATH
	        ) AS SchemeDetails

	   FROM ItemMaster IM
	   WHERE
	       (@Id = 0 OR IM.Id = @Id)
	   ORDER BY IM.Id DESC

	END

	ELSE IF(@ListFor='CompanyYearMasterEdit')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
	  SET @TableName = 'CompanyYearMaster'                                                                         
	  SET @FIELDS = 'TBL.Id
	                 ,TBL.FinancialYearFrom
	                 ,TBL.FinancialYearTo
	                 ,TBL.F_FirmMaster
	                 ,TBL.IsCurrentFinancialYear
	                 ,TBL.UserId
					  '
	  SET @JOINS = ' LEFT JOIN FirmMaster FM ON FM.Id = TBL.F_FirmMaster
					 ' 
	  set @ORDERBY = ''
	END 

	ELSE IF(@ListFor='CountryMaster')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'CountryMaster'                                                                         
		SET @FIELDS = 'Id, Name, UserId' 
		SET @ORDERBY = 'ORDER BY TBL.Name'                                                                                                                                    
	END 

	ELSE IF(@ListFor='CountryMasterEdit')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'CountryMaster'                                                                         
		SET @FIELDS = 'Id, Name, UserId' 
		SET @ORDERBY = ''                                                                                                                                    
	END 

	ELSE IF(@ListFor='ItemMaster')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'ItemMaster'                                                                         
		SET @FIELDS = 'TBL.Id
						,TBL.ItemName
						,TBL.HasSize
						,TBL.UserId
						,TBL.F_CategoryMaster
						,TBL.HSNCode
						,TBL.F_GSTGroupMaster
						,TBL.F_UnitMaster
						,TBL.UnitConversion
						,TBL.F_MaterialMaster
						'
		SET @ORDERBY = 'ORDER BY TBL.ItemName'                                                                                                                                    
	END 

	ELSE IF(@ListFor='ItemMasterEdit')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'ItemMaster'                                                                         
		SET @FIELDS = 'TBL.*' 
		SET @ORDERBY = ''                                                                                                                                    
	END 

	ELSE IF(@ListFor='StateMaster')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'StateMaster'                                                                         
		SET @FIELDS = 'TBL.Id
					   ,TBL.UserId
					   ,TBL.Name
					   ,TBL.F_CountryMaster
					   ,CM.Name AS CountryName
					   ,TBL.StateCode
					   ' 
		SET @JOINS  = 'LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster' 
		SET @ORDERBY = 'ORDER BY TBL.Name'                                                                                                                                    
	END 

	ELSE IF(@ListFor='StateMasterEdit')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'StateMaster'                                                                         
		SET @FIELDS = 'TBL.Id
					   ,TBL.UserId
					   ,TBL.Name
					   ,TBL.F_CountryMaster
					   ,TBL.StateCode
					   ' 
		SET @ORDERBY = ''                                                                                                                                    
	END 

	ELSE IF(@ListFor='UserTypeMaster')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'UserTypeMaster'                                                                         
		SET @FIELDS = 'TBL.Id, TBL.Name, TBL.UserId' 
		SET @ORDERBY = 'ORDER BY TBL.Id'                                                                                                                                    
	END 

	ELSE IF(@ListFor='UserTypeMasterEdit')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'UserTypeMaster'                                                                         
		SET @FIELDS = 'TBL.Id, TBL.Name, TBL.UserId' 
		SET @ORDERBY = ''                                                                                                                                    
	END 


	ELSE IF(@ListFor = 'UserMaster')                                                                 
	BEGIN                                                                
	    SET @TableName = 'UserMaster'                                                                 
	    SET @FIELDS = 'TBL.Id, TBL.Name
						,TBL.Username
						,TBL.Password
						,TBL.F_UserType
						,TBL.UserId
						,TBL.FullName
						,TBL.ContactPerson
						,TBL.ContactEmail
						,TBL.ContactMobile
						,TBL.Address1
						,TBL.Address2
						,TBL.F_StateMaster
						,TBL.F_CityMaster
						,TBL.F_CountryMaster
						,CM.Name AS CountryName
						,SM.Name AS StateName
						,CMM.Name AS CityName
						,ISNULL(TBL.IsSuperAdmin,''0'') AS IsSuperAdmin
						,TBL.F_UserRole
						'             
	    SET @JOINS  = 'LEFT JOIN UserTypeMaster UTM ON UTM.Id = TBL.F_UserType
		LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster
		LEFT JOIN StateMaster SM ON SM.Id = TBL.F_StateMaster
		LEFT JOIN CityMaster CMM ON CMM.Id = TBL.F_CityMaster
		'            
	    SET @OrderBY = 'ORDER BY TBL.UserName'  
	END  

	ELSE IF(@ListFor='UserMasterEdit')                                                                 
	BEGIN                                                                
	 SET @TableName = 'UserMaster'                                                                 
	    SET @FIELDS = 'TBL.Id, TBL.Name
						,TBL.Username
						,TBL.Password
						,TBL.F_UserType
						,TBL.UserId
						,TBL.FullName
						,TBL.ContactPerson
						,TBL.ContactEmail
						,TBL.ContactMobile
						,TBL.Address1
						,TBL.Address2
						,TBL.F_StateMaster
						,TBL.F_CityMaster
						,TBL.F_CountryMaster
						,ISNULL(TBL.IsSuperAdmin,''0'') AS IsSuperAdmin
						,TBL.F_UserRole
						'                  
	 SET @JOINS  = ' LEFT JOIN UserTypeMaster UTM ON UTM.Id  =  TBL.F_UserType  
		LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster
		LEFT JOIN StateMaster SM ON SM.Id = TBL.F_StateMaster
		LEFT JOIN CityMaster CMM ON CMM.Id = TBL.F_CityMaster
	 '            
	 SET @OrderBY = ''  
	END  

	ELSE IF(@ListFor = 'CompanyMaster')                                                                 
	BEGIN                                                                
	    SET @TableName = 'CompanyMaster'                                                                 
	    SET @FIELDS = ' TBL.ID
						,TBL.CompanyName
						,TBL.ShortName
						,TBL.Address
						,TBL.EmailID
						,TBL.Website
						,TBL.F_CountryMaster
						,TBL.F_StateMaster
						,TBL.F_CityMaster
						,TBL.Zip
						,TBL.OfficePhoneNo
						,TBL.ResidencePhoneNo
						,TBL.MobileNo
						,TBL.FaxNo
						,TBL.PanNo
						,TBL.GSTIN
						,TBL.LSTNo
						,TBL.CSTNo
						,TBL.RegNo
						,TBL.FinancialYearFrom
						,TBL.BooksFrom
						,TBL.CompanyImg
						,TBL.SignatureImg
						,TBL.IsActive
						,CM.Name AS CountryName
						,SM.Name AS StateName
						,CMM.Name AS CityName
						'   
	    SET @JOINS = ' LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster
						LEFT JOIN StateMaster SM ON SM.Id = TBL.F_StateMaster
						LEFT JOIN CityMaster CMM ON CMM.Id = TBL.F_CityMaster
					 ' 
	    SET @OrderBY = 'ORDER BY TBL.CompanyName'  
	END  

	ELSE IF(@ListFor='CompanyMasterEdit')                                                                 
	BEGIN                                                                
	    SET @TableName = 'CompanyMaster'                                                                 
	    SET @FIELDS = ' TBL.ID
						,TBL.CompanyName
						,TBL.ShortName
						,TBL.Address
						,TBL.EmailID
						,TBL.Website
						,TBL.F_CountryMaster
						,TBL.F_StateMaster
						,TBL.F_CityMaster
						,TBL.Zip
						,TBL.OfficePhoneNo
						,TBL.ResidencePhoneNo
						,TBL.MobileNo
						,TBL.FaxNo
						,TBL.PanNo
						,TBL.GSTIN
						,TBL.LSTNo
						,TBL.CSTNo
						,TBL.RegNo
						,TBL.FinancialYearFrom
						,TBL.BooksFrom
						,TBL.CompanyImg
						,TBL.SignatureImg
						,TBL.IsActive
						'   
	    SET @JOINS = ' LEFT JOIN CountryMaster CM ON CM.Id = TBL.F_CountryMaster
						LEFT JOIN StateMaster SM ON SM.Id = TBL.F_StateMaster
						LEFT JOIN CityMaster CMM ON CMM.Id = TBL.F_CityMaster
					 ' 
	    SET @OrderBY = ''  
	END 

	ELSE IF(@ListFor='PageMaster')                                                                                                                                                                                     
	BEGIN                                                                                                                                       
	  SET @TableName = 'PageMaster'                                                                                                                                     
	  SET @FIELDS = ' Id, Name, PageType ' 
	  SET @ORDERBY = ' ORDER BY PageType, Name '
	END  
	

	ELSE IF(@ListFor='PageMasterEdit')                                                                                                                                                                                     
	BEGIN                                                                                                                                       
	  SET @TableName = 'PageMaster'                                                                                                                                     
	  SET @FIELDS = ' Id, Name, PageType, Name '                                                                                                                                            
	END

	ELSE IF(@ListFor='UserRights')                                                                                                                                                                                     
	BEGIN                                                                                                                                       
	  SET @TableName = 'UserRights'                                                                                                                                     
	  SET @FIELDS = ' TBL.Id, TBL.F_UserMaster, TBL.PageID, PM.Name , PM.PageType, TBL.IsHavingRights ' 
	   SET @JOINS  = ' LEFT JOIN PageMaster PM ON PM.Id = TBL.PageID '          
	   SET @OrderBY = ' ORDER BY PM.PageType,PM.Name '
	END  

	ELSE IF(@ListFor='UserRightsById')                                                                                                                                                                                     
	BEGIN                                                                                                                                       
	  SET @TableName = 'UserRights'                                                                                                                                     
	  SET @FIELDS = ' TBL.Id, TBL.F_UserMaster, TBL.PageID, PM.Name , PM.PageType, TBL.IsHavingRights ' 
	   SET @JOINS  = ' LEFT JOIN PageMaster PM ON PM.Id = TBL.PageID '  
		SET @Where = ' WHERE TBL.F_UserMaster = ' + Convert(Nvarchar,@Id)
	   SET @OrderBY = ' ORDER BY PM.PageType, PM.Name '
	END  

	ELSE IF (@ListFor = 'GlobalOptions')
	BEGIN
	   SET @TableName = 'GlobalOptions'
	   SET @FIELDS = ' TBL.* '
	   SET @ORDERBY = ''
	END

	ELSE IF (@ListFor = 'EditGlobalOptions')
	BEGIN
	   SET @TableName = 'GlobalOptions'
	   SET @FIELDS = ' TBL.* '
	   SET @ORDERBY = ''
	END

	ELSE IF (@ListFor = 'CategoryMaster')
	BEGIN
	   SET @TableName = 'CategoryMaster'
	   SET @FIELDS = ' TBL.* '
	   SET @ORDERBY = ' ORDER BY TBL.Name '
	END

	ELSE IF (@ListFor = 'CategoryMasterEdit')
	BEGIN
	   SET @TableName = 'CategoryMaster'
	   SET @FIELDS = ' TBL.* '
	   SET @ORDERBY = ''
	END

	ELSE IF(@ListFor='ItemDesignMaster')                                                                                                                                                                         
	BEGIN                                                                                                                                                 
		SET @TableName = 'ItemDesignMaster'                                                                         
		SET @FIELDS = '
        TBL.Id
        ,TBL.F_ItemMaster

        ,CASE 
            WHEN ISNULL(TBL.DesignPhoto, '''') = '''' 
            THEN '''' 
            ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto 
         END AS DesignPhoto

        ,CASE 
            WHEN ISNULL(TBL.DesignPhoto2, '''') = '''' 
            THEN '''' 
            ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto2 
         END AS DesignPhoto2

        ,CASE 
            WHEN ISNULL(TBL.DesignPhoto3, '''') = '''' 
            THEN '''' 
            ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto3 
         END AS DesignPhoto3

        ,CASE 
            WHEN ISNULL(TBL.DesignPhoto4, '''') = '''' 
            THEN '''' 
            ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto4 
         END AS DesignPhoto4

        ,CASE 
            WHEN ISNULL(TBL.DesignPhoto5, '''') = '''' 
            THEN '''' 
            ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto5 
         END AS DesignPhoto5

        ,TBL.SizeName
        ,TBL.SalePrice
        ,TBL.Barcode
        ,TBL.VideoLink
        ,TBL.OpeningStock
		,TBL.Length
		,TBL.Width
		,TBL.Height
		,TBL.Weight
		,TBL.UnitConversion AS UnitConversion
		,TBL.PurchaseRate AS PurchaseRate
		'
		SET @ORDERBY = ' ORDER BY TBL.Id '                                                                                                                                    
	END 
  
   ELSE IF(@ListFor='NewItemCreate')                                                                                
   BEGIN             
  
	INSERT INTO ItemMaster (ItemName,HasSize,UserId,DateOfCreation,LastUpdateOn,F_CategoryMaster, HSNCode, F_GSTGroupMaster, F_UnitMaster, F_MaterialMaster, UnitConversion)  
					 VALUES('',0,@Id,GETDATE(),GETDATE(),0, '', 0, 0, 0, 0)  
	 
	SELECT SCOPE_IDENTITY() AS Id  
  END  
  
  ELSE IF(@ListFor='NewItemDesignCreate')                                                                                
  BEGIN             
  
	INSERT INTO ItemDesignMaster (SizeName, SalePrice, Barcode, VideoLink, OpeningStock, F_ItemMaster, UserId, DateOfCreation, LastUpdateOn, Length, Width, Height, Weight, UnitConversion, PurchaseRate)  
						   VALUES('',		0,		   '',		'',			0, Convert(Numeric,@Id), 0, GETDATE(), GETDATE(), '', '', '', '', 0, 0)  
	 
	SELECT SCOPE_IDENTITY() AS Id   
  END 

  ELSE IF (@ListFor = 'DeleteCategoryMaster')  
  BEGIN  
    IF EXISTS (  
        SELECT 1   
        FROM ItemMaster   
        WHERE F_CategoryMaster = @Id  
    )  
    BEGIN  
        -- Category is in use â†’ do not delete  
        SELECT -1 AS Id  
    END  
    ELSE  
    BEGIN  
        -- Category not in use â†’ safe to delete  
        DELETE FROM CategoryMaster WHERE Id = @Id  
        SELECT 1 AS Id  
    END  
  END 

  ELSE IF (@ListFor = 'DeleteItemMaster')  
  BEGIN  
	DELETE FROM ItemDesignMaster WHERE F_ItemMaster = @Id 
	DELETE FROM ItemMaster WHERE Id = @Id 
	SELECT 1 AS Id 
  END  

  ELSE IF (@ListFor = 'DeleteItemDesignMaster')  
  BEGIN  
     DELETE FROM ItemDesignMaster WHERE Id = @Id  
     SELECT 1 AS Id 
  END

  ELSE IF (@ListFor = 'GSTGroupMaster')
  BEGIN
     SET @TableName = 'GSTGroupMaster'
     SET @FIELDS = ' TBL.Id, TBL.GSTGroupName,
					 TBL.GSTType,
					 TBL.HSN_SAC_Code,
					 TBL.GSTPercent,
					 TBL.CGSTPercent,
					 TBL.SGSTPercent,
					 TBL.IGSTPercent '
     SET @ORDERBY = ' ORDER BY TBL.Id, TBL.GSTPercent '
  END

  ELSE IF (@ListFor = 'GSTGroupMasterEdit')
  BEGIN
     SET @TableName = 'GSTGroupMaster'
     SET @FIELDS = ' TBL.Id, TBL.GSTGroupName,
					 TBL.GSTType,
					 TBL.HSN_SAC_Code,
					 TBL.GSTPercent,
					 TBL.CGSTPercent,
					 TBL.SGSTPercent,
					 TBL.IGSTPercent '
     SET @ORDERBY = ''
  END
 
  ELSE IF (@ListFor = 'UnitMaster')
  BEGIN
     SET @TableName = 'UnitMaster'
     SET @FIELDS = ' TBL.Id, TBL.UnitName
					 ,TBL.UnitCode
					 ,TBL.DecimalAllowed
					 ,TBL.IsActive '
     SET @ORDERBY = ' ORDER BY TBL.UnitName '
  END

  ELSE IF (@ListFor = 'UnitMasterEdit')
  BEGIN
     SET @TableName = 'UnitMaster'
     SET @FIELDS = ' TBL.Id, TBL.UnitName
					 ,TBL.UnitCode
					 ,TBL.DecimalAllowed
					 ,TBL.IsActive '
     SET @ORDERBY = ''
  END 
  
  ELSE IF (@ListFor = 'AlterUnitMaster')
  BEGIN
     SET @TableName = 'AlterUnitMaster'
     SET @FIELDS = ' TBL.Id, TBL.F_UnitMaster
					 ,UMI.UnitName AS UnitName
					 ,TBL.F_AlterUnit
					 ,UMAI.UnitName AS AlterUnitName
					 ,TBL.ConversionValue '
	 SET @JOINS  = ' LEFT JOIN UnitMaster UMI ON UMI.Id = TBL.F_UnitMaster 
					 LEFT JOIN UnitMaster UMAI ON UMAI.Id = TBL.F_AlterUnit '
     SET @ORDERBY = ' ORDER BY TBL.Id '
  END

  ELSE IF (@ListFor = 'AlterUnitMasterEdit')
  BEGIN
     SET @TableName = 'AlterUnitMaster'
     SET @FIELDS = ' TBL.Id, TBL.F_UnitMaster
					 ,TBL.F_AlterUnit
					 ,TBL.ConversionValue '
     SET @ORDERBY = ''
  END 

   ELSE IF (@ListFor = 'LedgerMaster')
  BEGIN
     SET @TableName = 'LedgerMaster'
     SET @FIELDS = ' TBL.*, LGM.Name AS LedgerGroupName '
	 SET @JOINS  = ' LEFT JOIN LedgerGroupMaster LGM ON LGM.Id = TBL.F_LedgerGroupMaster'
     SET @ORDERBY = ' ORDER BY TBL.Id '
  END

  ELSE IF (@ListFor = 'LedgerMasterEdit')
  BEGIN
     SET @TableName = 'LedgerMaster'
     SET @FIELDS = ' TBL.*, LGM.Name AS LedgerGroupName '
	 SET @JOINS  = ' LEFT JOIN LedgerGroupMaster LGM ON LGM.Id = TBL.F_LedgerGroupMaster '
     SET @ORDERBY = ''
  END 

   ELSE IF (@ListFor = 'LedgerGroupMaster')
  BEGIN
     SET @TableName = 'LedgerGroupMaster'
     SET @FIELDS = ' TBL.* '
     SET @ORDERBY = ' ORDER BY TBL.Id '
  END

  ELSE IF (@ListFor = 'LedgerGroupMasterEdit')
  BEGIN
     SET @TableName = 'LedgerGroupMaster'
     SET @FIELDS = ' TBL.* '
     SET @ORDERBY = ''
  END 

  ELSE IF (@ListFor = 'PartyLedgerMaster')
  BEGIN
     SET @TableName = 'LedgerMaster'                                                                   
     SET @FIELDS = ' TBL.Id, TBL.Name, TBL.F_LedgerGroupMaster,
					 CASE WHEN ISNULL(TBL.F_StateMaster,''0'') = (SELECT ISNULL(F_StateMaster,''0'') AS F_StateMaster  FROM GlobalOptions) THEN 1 ELSE 1 END AS IsInState '  
	 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster = ''40'' '  
	 SET @OrderBY = ' ORDER BY TBL.Name '
  END 

	ELSE IF(@ListFor='PurchaseEntryData')
  BEGIN
	     SELECT
	        PEH.Id
			,ISNULL(PEH.EntryNoAuto,0) AS EntryNoAuto
			,ISNULL(PEH.EntryNo,'') AS EntryNo
			,PEH.EntryDate
			,ISNULL(PEH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(PEH.TotalQty,0) AS TotalQty
			,ISNULL(PEH.Remarks,'') AS Remarks
			,ISNULL(TotalCGST,0)   AS TotalCGST
			,ISNULL(TotalSGST,0)   AS TotalSGST
			,ISNULL(TotalIGST,0)   AS TotalIGST
			,ISNULL(TotalTax,0)	   AS TotalTax
			,ISNULL(TotalAmount,0) AS TotalAmount
			,PEH.UserId
			,PEH.F_VoucherH
			,PEH.F_CompanyMaster
			,ISNULL(PEH.DispatchDocNo,'') AS DispatchDocNo	 
			,ISNULL(PEH.DispatchedThrough,'') AS DispatchedThrough
			
	       ,(
	           SELECT
	                PEL.Id
	               ,CASE WHEN ISNULL(PEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + PEL.DesignPhoto END AS DesignPhoto
	               ,CASE WHEN ISNULL(PEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(PEL.DesignPhoto, LEN(PEL.DesignPhoto) - CHARINDEX('.', REVERSE(PEL.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
				   ,ISNULL(PEL.F_PurchaseEntryH,0)   AS F_PurchaseEntryH
				   ,ISNULL(PEL.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(PEL.F_CategoryMaster,0)	 AS F_CategoryMaster
				   ,ISNULL(PEL.F_ItemMaster,0)		 AS F_ItemMaster
				   ,ISNULL(PEL.Barcode,'')			 AS Barcode
				   ,ISNULL(PEL.ItemName,'')			 AS ItemName
				   ,ISNULL(PEL.Qty,0)			 AS Qty
				   ,ISNULL(PEL.Rate,0)			 AS Rate
				   ,ISNULL(Amount,0)			 AS Amount
				   ,ISNULL(CGST,0)				 AS CGST
				   ,ISNULL(SGST,0)				 AS SGST
				   ,ISNULL(IGST,0)				 AS IGST
				   ,ISNULL(PEL.F_StatusMaster,0) AS F_StatusMaster
				   ,ISNULL(PEL.IsActive,0) AS IsActive
				   ,PEL.UserId
				   ,ISNULL(IM.F_GSTGroupMaster,0) AS F_GSTGroupMaster
				   ,ISNULL(GGM.GSTGroupName,'') AS GSTGroupName
				   ,ISNULL(GGM.GSTPercent,0) AS GSTPercent
	           FROM PurchaseEntryL PEL
			   LEFT JOIN ItemDesignMaster IDM ON IDM.Id = PEL.F_ItemDesignMaster
			   LEFT JOIN ItemMaster IM ON IM.Id = IDM.F_ItemMaster
			   LEFT JOIN GSTGroupMaster GGM ON GGM.Id = IM.F_GSTGroupMaster
	           WHERE PEL.F_PurchaseEntryH = PEH.Id
	           FOR JSON PATH
	        ) AS PurchaseLDetails

	   FROM PurchaseEntryH PEH
	   WHERE (@Id = 0 OR PEH.Id = @Id)
  END

 ELSE IF (@ListFor = 'GetItemDesignDetails')  
 BEGIN  
     SET @TableName = 'ItemDesignMaster'                                                                   
     SET @FIELDS = ' TBL.Id 
					,TBL.F_ItemMaster
					,TBL.SizeName
					,TBL.SalePrice
					,TBL.Barcode
					,CASE 
					     WHEN ISNULL(TBL.DesignPhoto, '''') = '''' 
					     THEN '''' 
					     ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto 
					  END AS DesignPhoto

					 ,CASE 
					     WHEN ISNULL(TBL.DesignPhoto2, '''') = '''' 
					     THEN '''' 
					     ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto2 
					  END AS DesignPhoto2

					 ,CASE 
					     WHEN ISNULL(TBL.DesignPhoto3, '''') = '''' 
					     THEN '''' 
					     ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto3 
					  END AS DesignPhoto3

					 ,CASE 
					     WHEN ISNULL(TBL.DesignPhoto4, '''') = '''' 
					     THEN '''' 
					     ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto4 
					  END AS DesignPhoto4

					 ,CASE 
					     WHEN ISNULL(TBL.DesignPhoto5, '''') = '''' 
					     THEN '''' 
					     ELSE ''https://accountingwkr.shinewellsofttech.co.in/ItemImages/'' + TBL.DesignPhoto5 
					  END AS DesignPhoto5
					,TBL.VideoLink
					,TBL.OpeningStock 
					,IM.ItemName
					,IM.HasSize
					,IM.F_CategoryMaster
					,IM.HSNCode
					,IM.F_GSTGroupMaster
					,IM.F_UnitMaster
					,IM.UnitConversion
					,IM.F_MaterialMaster
					,TBL.Length
					,TBL.Width
					,TBL.Height
					,TBL.Weight
					,CM.Name AS CategoryName
					,ISNULL(TBL.UnitConversion,''0'') AS UnitConversion
					,ISNULL(TBL.PurchaseRate,''0'') AS PurchaseRate
					'      
	 SET @JOINS  = ' LEFT JOIN ItemMaster IM ON IM.Id = TBL.F_ItemMaster
					 LEFT JOIN CategoryMaster CM ON CM.Id = IM.F_CategoryMaster'
	 SET @OrderBY = ' ORDER BY IM.ItemName '  
 END  

 ELSE IF (@ListFor = 'GetVoucherNoByVoucherTypeId')  
 BEGIN  
    SELECT   
		CASE WHEN ISNULL(VTM.IsAutoVoucherNo,0) = 0 THEN ''   
		ELSE CASE WHEN ISNULL(VTM.PrefixType,0) = 1   
		THEN (CASE WHEN ISNULL(tblVoucherNo.VoucherNo,'') = '' THEN ISNULL(VoucherPrefix,'')+ '1' ELSE  ISNULL(VoucherPrefix,'') + CONVERT(VARCHAR,(CONVERT(NUMERIC(18,0),REPLACE(tblVoucherNo.VoucherNo,ISNULL(VoucherPrefix,''),'')) + 1)) END)  
		ELSE (CASE WHEN ISNULL(tblVoucherNo.VoucherNo,'') = '' THEN ISNULL(DefaultVoucherPrefix,'')+ '1' ELSE  ISNULL(DefaultVoucherPrefix,'') + CONVERT(VARCHAR,(CONVERT(NUMERIC(18,0),REPLACE(tblVoucherNo.VoucherNo,ISNULL(DefaultVoucherPrefix,''),'')) + 1)) END)  
		END END AS VoucherNo  
   FROM VoucherTypeMaster VTM  
   OUTER APPLY (SELECT TOP 1 ISNULL(VoucherNo,'') AS VoucherNo FROM VoucherH WHERE F_VoucherTypeMaster = VTM.ID ORDER BY Id DESC) AS tblVoucherNo  
   WHERE VTM.Id = @Id  
 END  

  ELSE IF (@ListFor = 'FinancialYearMaster')
  BEGIN
     SET @TableName = 'FinancialYearMaster'
     SET @FIELDS = ' TBL.* '
     SET @ORDERBY = ' ORDER BY TBL.Id '
  END

  ELSE IF (@ListFor = 'FinancialYearMasterEdit')
  BEGIN
     SET @TableName = 'FinancialYearMaster'
     SET @FIELDS = ' TBL.* '
     SET @ORDERBY = ''
  END 

  ELSE IF (@ListFor = 'GetLedgerByLedgerGroup')
  BEGIN
     SET @TableName = 'LedgerMaster'                                                                   
     SET @FIELDS = ' TBL.Id, TBL.Name, TBL.F_LedgerGroupMaster'  
	 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster IN (''14'',''15'',''16'') '  
	 SET @OrderBY = ' ORDER BY TBL.Name '
  END 

  ELSE IF(@ListFor='SalesEntryData')
  BEGIN
	     SELECT
	        SEH.Id
			,ISNULL(SEH.EntryNoAuto,0) AS EntryNoAuto
			,ISNULL(SEH.EntryNo,'') AS EntryNo
			,SEH.EntryDate
			,ISNULL(SEH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(SEH.Remarks,'') AS Remarks
			,ISNULL(SEH.TotalQty,0) AS TotalQty
			,ISNULL(SEH.TotalCGST,0)   AS TotalCGST
			,ISNULL(SEH.TotalSGST,0)   AS TotalSGST
			,ISNULL(SEH.TotalIGST,0)   AS TotalIGST
			,ISNULL(SEH.TotalTax,0)	   AS TotalTax
			,ISNULL(SEH.TotalAmount,0) AS TotalAmount
			,ISNULL(SEH.TotalOtherCharges,0) AS TotalOtherCharges
			,ISNULL(SEH.UserId,0) AS UserId
			,ISNULL(SEH.F_VoucherH,0) AS F_VoucherH
			,ISNULL(SEH.F_CompanyMaster,0) AS F_CompanyMaster
			,ISNULL(SEH.DispatchDocNo,'') AS DispatchDocNo	 
			,ISNULL(SEH.DispatchedThrough,'') AS DispatchedThrough
	       ,(
	           SELECT
	                SEL.Id
	               ,CASE WHEN ISNULL(SEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + SEL.DesignPhoto END AS DesignPhoto
				   ,CASE WHEN ISNULL(SEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(SEL.DesignPhoto, LEN(SEL.DesignPhoto) - CHARINDEX('.', REVERSE(SEL.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
				   ,ISNULL(SEL.F_SalesEntryH,0)   AS F_SalesEntryH
				   ,ISNULL(SEL.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(SEL.F_CategoryMaster,0)	 AS F_CategoryMaster
				   ,ISNULL(SEL.F_ItemMaster,0)		 AS F_ItemMaster
				   ,ISNULL(SEL.Barcode,'')			 AS Barcode
				   ,ISNULL(SEL.ItemName,'')			 AS ItemName
				   ,ISNULL(SEL.Qty,0)			 AS Qty
				   ,ISNULL(SEL.Rate,0)			 AS Rate
				   ,ISNULL(SEL.Amount,0)		 AS Amount
				   ,ISNULL(SEL.CGST,0)			 AS CGST
				   ,ISNULL(SEL.SGST,0)			 AS SGST
				   ,ISNULL(SEL.IGST,0)			 AS IGST
				   ,ISNULL(SEL.F_StatusMaster,0) AS F_StatusMaster
				   ,ISNULL(SEL.IsActive,0) AS IsActive
				   ,ISNULL(SEL.UserId,0) AS UserId
				   ,ISNULL(IM.F_GSTGroupMaster,0) AS F_GSTGroupMaster
				   ,ISNULL(GGM.GSTGroupName,'') AS GSTGroupName
				   ,ISNULL(GGM.GSTPercent,0) AS GSTPercent
			   FROM SalesEntryL SEL
			   LEFT JOIN ItemDesignMaster IDM ON IDM.Id = SEL.F_ItemDesignMaster
			   LEFT JOIN ItemMaster IM ON IM.Id = IDM.F_ItemMaster
			   LEFT JOIN GSTGroupMaster GGM ON GGM.Id = IM.F_GSTGroupMaster
	           WHERE SEL.F_SalesEntryH = SEH.Id
	           FOR JSON PATH
	        ) AS SalesLDetails
		   ,(
	           SELECT
	                SELOC.Id
				   ,ISNULL(SELOC.F_LedgerMaster,0) AS F_LedgerMaster
				   ,ISNULL(SELOC.Amount,0) AS Amount

	           FROM SalesEntryLOtherCharges SELOC
	           WHERE SELOC.F_SalesEntryH = SEH.Id
	           FOR JSON PATH
	        ) AS SalesLOtherChargesDetails

	   FROM SalesEntryH SEH
	   WHERE (@Id = 0 OR SEH.Id = @Id)
  END

  ELSE IF (@ListFor = 'ItemSchemeMaster')
  BEGIN
     SET @TableName = 'ItemSchemeMaster'
     SET @FIELDS = ' TBL.* '
     SET @ORDERBY = ' ORDER BY TBL.Id '
  END

  ELSE IF(@ListFor='MaterialMaster')                                                                                                                                                                         
  BEGIN                                                                                                                                                 
  	SET @TableName = 'MaterialMaster'                                                                         
  	SET @FIELDS = 'Id, Name, UserId' 
  	SET @ORDERBY = 'ORDER BY TBL.Name'                                                                                                                                    
  END 

  ELSE IF(@ListFor='MaterialMasterEdit')                                                                                                                                                                         
  BEGIN                                                                                                                                                 
  	SET @TableName = 'MaterialMaster'                                                                         
  	SET @FIELDS = 'Id, Name, UserId' 
  	SET @ORDERBY = ''                                                                                                                                    
  END 

  ELSE IF(@ListFor='VoucherData')
  BEGIN
	     SELECT
	        VH.Id
		   ,ISNULL(VH.VoucherNo,'') AS VoucherNo
		   ,ISNULL(VH.VoucherNoAuto,0) AS VoucherNoAuto
		   ,ISNULL(VH.F_VoucherTypeMaster,0) AS F_VoucherTypeMaster
		   ,VH.VoucherDate
	       ,ISNULL(VH.ReferenceNo,'') AS ReferenceNo
	       ,VH.ReferenceDate
	       ,ISNULL(VH.Narration,'') AS Narration
	       ,ISNULL(VH.UserId,0) AS UserId
	       ,VH.DateOfCreation
	       ,VH.LastUpdateOn
		   ,ISNULL(VH.TotalDr,0) AS TotalDr
		   ,ISNULL(VH.TotalCr,0) AS TotalCr
		   ,ISNULL(VH.CurBal,0) AS CurBal
	       ,(
	           SELECT
	                VL.Id
	               ,ISNULL(VL.DrCrType,'') AS DrCrType
	               ,ISNULL(VL.F_VoucherH,0) AS F_VoucherH
	               ,ISNULL(VL.F_LedgerMasterDr,0) AS F_LedgerMasterDr
	               ,ISNULL(VL.F_LedgerMasterCr,0) AS F_LedgerMasterCr
	               ,ISNULL(VL.Amount,0) AS Amount
				   
	           FROM VoucherL VL
	           WHERE VL.F_VoucherH = VH.Id
	           FOR JSON PATH
	        ) AS VoucherDetails

	   FROM VoucherH VH
	   WHERE (@Id = 0 OR VH.Id = @Id)

  END

   ELSE IF (@ListFor = 'VoucherTypeMasterAll')
  BEGIN
     SET @TableName = 'VoucherTypeMaster'
     SET @FIELDS = ' TBL.* '
     SET @ORDERBY = ' ORDER BY TBL.Id '
  END

 ELSE IF (@ListFor = 'GetLedgerMasterExceptBankAndCash')  
 BEGIN  
    SET @TableName = 'LedgerMaster'                                                                   
    SET @FIELDS = ' TBL.*'  
 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster NOT IN (4,8)'  
 SET @OrderBY = ' ORDER BY TBL.Name '   
 END 

 ELSE IF (@ListFor = 'PaymentTransaction')  
 BEGIN  
    SET @TableName = 'PaymentTransaction'                                                                   
    SET @FIELDS = ' TBL.*'  
 SET @OrderBY = ' ORDER BY TBL.Id '   
 END 

  ELSE IF(@ListFor='PurchaseReturnData')
  BEGIN
	     SELECT
	        PEH.Id
			,ISNULL(PEH.F_PurchaseEntryH,0)   AS F_PurchaseEntryH
			,ISNULL(PEH.EntryNoAuto,0) AS EntryNoAuto
			,ISNULL(PEH.EntryNo,'') AS EntryNo
			,PEH.EntryDate
			,ISNULL(PEH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(PEH.TotalQty,0) AS TotalQty
			,ISNULL(PEH.Remarks,'') AS Remarks
			,ISNULL(TotalCGST,0)   AS TotalCGST
			,ISNULL(TotalSGST,0)   AS TotalSGST
			,ISNULL(TotalIGST,0)   AS TotalIGST
			,ISNULL(TotalTax,0)	   AS TotalTax
			,ISNULL(TotalAmount,0) AS TotalAmount
			,PEH.UserId
			,PEH.F_VoucherH
			,PEH.F_CompanyMaster
			
	       ,(
	           SELECT
	                PEL.Id
	               ,CASE WHEN ISNULL(PEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + PEL.DesignPhoto END AS DesignPhoto
	               ,CASE WHEN ISNULL(PEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(PEL.DesignPhoto, LEN(PEL.DesignPhoto) - CHARINDEX('.', REVERSE(PEL.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
				   ,ISNULL(PEL.F_PurchaseReturnH,0)   AS F_PurchaseReturnH
				   ,ISNULL(PEL.F_PurchaseEntryL,0)   AS F_PurchaseEntryL
				   ,ISNULL(PEL.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(PEL.F_CategoryMaster,0)	 AS F_CategoryMaster
				   ,ISNULL(PEL.F_ItemMaster,0)		 AS F_ItemMaster
				   ,ISNULL(PEL.Barcode,'')			 AS Barcode
				   ,ISNULL(PEL.ItemName,'')			 AS ItemName
				   ,ISNULL(PEL.Qty,0)			 AS Qty
				   ,ISNULL(PEL.Rate,0)			 AS Rate
				   ,ISNULL(Amount,0)			 AS Amount
				   ,ISNULL(CGST,0)				 AS CGST
				   ,ISNULL(SGST,0)				 AS SGST
				   ,ISNULL(IGST,0)				 AS IGST
				   ,ISNULL(PEL.F_StatusMaster,0) AS F_StatusMaster
				   ,ISNULL(PEL.IsActive,0) AS IsActive
				   ,PEL.UserId

	           FROM PurchaseReturnL PEL
	           WHERE PEL.F_PurchaseReturnH = PEH.Id
	           FOR JSON PATH
	        ) AS PurchaseReturnLDetails

	   FROM PurchaseReturnH PEH
	   WHERE (@Id = 0 OR PEH.Id = @Id)
  END

  ELSE IF(@ListFor='SalesReturnData')
  BEGIN
	     SELECT
	        SEH.Id
			,ISNULL(SEH.F_SalesEntryH,0)   AS F_SalesEntryH
			,ISNULL(SEH.EntryNoAuto,0) AS EntryNoAuto
			,ISNULL(SEH.EntryNo,'') AS EntryNo
			,SEH.EntryDate
			,ISNULL(SEH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(SEH.Remarks,'') AS Remarks
			,ISNULL(SEH.TotalQty,0) AS TotalQty
			,ISNULL(SEH.TotalCGST,0)   AS TotalCGST
			,ISNULL(SEH.TotalSGST,0)   AS TotalSGST
			,ISNULL(SEH.TotalIGST,0)   AS TotalIGST
			,ISNULL(SEH.TotalTax,0)	   AS TotalTax
			,ISNULL(SEH.TotalAmount,0) AS TotalAmount
			--,ISNULL(SEH.TotalOtherCharges,0) AS TotalOtherCharges
			,ISNULL(SEH.UserId,0) AS UserId
			,ISNULL(SEH.F_VoucherH,0) AS F_VoucherH
			,ISNULL(SEH.F_CompanyMaster,0) AS F_CompanyMaster
	       ,(
	           SELECT
	                SEL.Id
	               ,CASE WHEN ISNULL(SEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + SEL.DesignPhoto END AS DesignPhoto
				   ,CASE WHEN ISNULL(SEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(SEL.DesignPhoto, LEN(SEL.DesignPhoto) - CHARINDEX('.', REVERSE(SEL.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
				   ,ISNULL(SEL.F_SalesReturnH,0)   AS F_SalesReturnH
				   ,ISNULL(SEL.F_SalesEntryL,0)   AS F_SalesEntryL
				   ,ISNULL(SEL.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(SEL.F_CategoryMaster,0)	 AS F_CategoryMaster
				   ,ISNULL(SEL.F_ItemMaster,0)		 AS F_ItemMaster
				   ,ISNULL(SEL.Barcode,'')			 AS Barcode
				   ,ISNULL(SEL.ItemName,'')			 AS ItemName
				   ,ISNULL(SEL.Qty,0)			 AS Qty
				   ,ISNULL(SEL.Rate,0)			 AS Rate
				   ,ISNULL(SEL.Amount,0)		 AS Amount
				   ,ISNULL(SEL.CGST,0)			 AS CGST
				   ,ISNULL(SEL.SGST,0)			 AS SGST
				   ,ISNULL(SEL.IGST,0)			 AS IGST
				   ,ISNULL(SEL.F_StatusMaster,0) AS F_StatusMaster
				   ,ISNULL(SEL.IsActive,0) AS IsActive
				   ,ISNULL(SEL.UserId,0) AS UserId
	           
			   FROM SalesReturnL SEL
	           WHERE SEL.F_SalesReturnH = SEH.Id
	           FOR JSON PATH
	        ) AS SalesReturnLDetails
		   ,(
	           SELECT
	                SELOC.Id
				   ,ISNULL(SELOC.F_LedgerMaster,0) AS F_LedgerMaster
				   ,ISNULL(SELOC.Amount,0) AS Amount

	           FROM SalesReturnLOtherCharges SELOC
	           WHERE SELOC.F_SalesReturnH = SEH.Id
	           FOR JSON PATH
	        ) AS SalesReurnLOtherChargesDetails

	   FROM SalesReturnH SEH
	   WHERE (@Id = 0 OR SEH.Id = @Id)
  END

  ELSE IF (@ListFor = 'SalesPartyLedgerMaster')
  BEGIN
     SET @TableName = 'LedgerMaster'                                                                   
     SET @FIELDS = ' TBL.Id, TBL.Name, TBL.F_LedgerGroupMaster,
					 CASE WHEN ISNULL(TBL.F_StateMaster,''0'') = (SELECT ISNULL(F_StateMaster,''0'') AS F_StateMaster  FROM GlobalOptions) THEN 1 ELSE 1 END AS IsInState '  
	 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster IN (''36'',''8'',''4'') '  
	 SET @OrderBY = ' ORDER BY TBL.Name '
  END 

  ELSE IF (@ListFor = 'PurchasePartyLedgerMaster')
  BEGIN
     SET @TableName = 'LedgerMaster'                                                                   
     SET @FIELDS = ' TBL.Id, TBL.Name, TBL.F_LedgerGroupMaster,
					 CASE WHEN ISNULL(TBL.F_StateMaster,''0'') = (SELECT ISNULL(F_StateMaster,''0'') AS F_StateMaster  FROM GlobalOptions) THEN 1 ELSE 1 END AS IsInState '  
	 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster IN (''35'',''8'',''4'') '  
	 SET @OrderBY = ' ORDER BY TBL.Name '
  END 

  ELSE IF (@ListFor = 'ModuleMaster')
  BEGIN
      SET @TableName = 'dbo.ModuleMaster'
      SET @FIELDS = '
          TBL.Id,
          TBL.Name,
          TBL.Path,
          TBL.IsActive
      '
      SET @ORDERBY = ' ORDER BY TBL.Name'
  END
  
  ELSE IF (@ListFor = 'ModuleMasterEdit')
  BEGIN
      SET @TableName = 'dbo.ModuleMaster'
      SET @FIELDS = '
          TBL.Id,
          TBL.Name,
          TBL.Path,
          TBL.IsActive
      '
      SET @ORDERBY = ''
  END

  ELSE IF (@ListFor = 'UserRole')
  BEGIN
      SET @TableName = 'dbo.UserRole'
      SET @FIELDS = '
          TBL.Id, 
          TBL.Name, 
          TBL.Code,
          TBL.Description,
          TBL.IsActive
      '
      SET @ORDERBY = ' ORDER BY TBL.Name'
  END
  
  ELSE IF (@ListFor = 'UserRoleEdit')
  BEGIN
      SET @TableName = 'dbo.UserRole'
      SET @FIELDS = '
          TBL.Id, 
          TBL.Name, 
          TBL.Code,
          TBL.Description,
          TBL.IsActive
      '
      SET @ORDERBY = ''
  END 

  ELSE IF (@ListFor = 'RoleWisePermission')
  BEGIN
      SET @TableName = 'dbo.RoleWisePermission'
      SET @FIELDS = '
          TBL.Id,
          TBL.F_RoleMaster,
          TBL.F_ModuleMaster,
          TBL.IsView,
          TBL.IsAdd,
          TBL.IsEdit,
          TBL.IsDelete,
          TBL.IsApprove,
          TBL.IsExport,
          TBL.DateOfCreation
      '
      SET @WHERE = ' WHERE TBL.F_RoleMaster = ' + CAST(@Id AS NVARCHAR)
      SET @ORDERBY = ' ORDER BY TBL.F_ModuleMaster'
  END

  ELSE IF(@ListFor='RentManagementData')
  BEGIN
	     SELECT
	        RMH.Id
			,ISNULL(RMH.EntryNoAuto,0) AS EntryNoAuto
			,ISNULL(RMH.EntryNo,'') AS EntryNo
			,RMH.EntryDate
			,RMH.TillDate
			,ISNULL(RMH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(RMH.F_LedgerMaster_Tax,0) AS F_LedgerMaster_Tax
			,ISNULL(RMH.TotalRentAmount,0) AS TotalRentAmount
			,ISNULL(RMH.TotalSecurityDeposit,0) AS TotalSecurityDeposit
			,ISNULL(RMH.TotalTax,0) AS TotalTax
			,ISNULL(RMH.TotalCGST,0)   AS TotalCGST
			,ISNULL(RMH.TotalSGST,0)   AS TotalSGST
			,ISNULL(RMH.TotalIGST,0)   AS TotalIGST
			,ISNULL(RMH.Remarks,'') AS Remarks
			,ISNULL(RMH.CustomerName,'') AS CustomerName
			,ISNULL(RMH.MobileNo,'') AS MobileNo
			,RMH.UserId
			,RMH.F_VoucherH
			,RMH.F_CompanyMaster
			
	       ,(
	           SELECT
	                RML.Id
	               ,CASE WHEN ISNULL(RML.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + RML.DesignPhoto END AS DesignPhoto
	               ,CASE WHEN ISNULL(RML.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(RML.DesignPhoto, LEN(RML.DesignPhoto) - CHARINDEX('.', REVERSE(RML.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
				   ,ISNULL(RML.F_RentManagementH,0)   AS F_RentManagementH
				   ,ISNULL(RML.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(RML.F_CategoryMaster,0)	 AS F_CategoryMaster
				   ,ISNULL(RML.F_ItemMaster,0)		 AS F_ItemMaster
				   ,ISNULL(RML.Barcode,'')			 AS Barcode
				   ,ISNULL(RML.ItemName,'')			 AS ItemName
				   ,ISNULL(RML.Qty,0)			 AS Qty
				   ,ISNULL(RML.RentPrice,0)		 AS RentPrice
				   ,ISNULL(RML.SecurityDeposit,0) AS SecurityDeposit
				   ,ISNULL(RML.Amount,0)			 AS Amount
				   ,ISNULL(RML.F_StatusMaster,0) AS F_StatusMaster
				   ,ISNULL(RML.IsActive,0) AS IsActive
				   ,ISNULL(RML.UserId,0) AS UserId
				   ,ISNULL(IM.F_GSTGroupMaster,0) AS F_GSTGroupMaster
				   ,ISNULL(GGM.GSTGroupName,'') AS GSTGroupName
				   ,ISNULL(GGM.GSTPercent,0) AS GSTPercent
	           FROM RentManagementL RML
			   LEFT JOIN ItemDesignMaster IDM ON IDM.Id = RML.F_ItemDesignMaster
			   LEFT JOIN ItemMaster IM ON IM.Id = IDM.F_ItemMaster
			   LEFT JOIN GSTGroupMaster GGM ON GGM.Id = IM.F_GSTGroupMaster
	           WHERE RML.F_RentManagementH = RMH.Id
	           FOR JSON PATH
	        ) AS RentDetails

	   FROM RentManagementH RMH
	   WHERE (@Id = 0 OR RMH.Id = @Id)
  END

  ELSE IF(@ListFor='RentReturnData')
  BEGIN
	     SELECT
	        RRH.Id
			,ISNULL(RRH.F_RentManagementH,0) AS F_RentManagementH
			,ISNULL(RRH.EntryNoAuto,0) AS EntryNoAuto
			,ISNULL(RRH.EntryNo,'') AS EntryNo
			,RRH.EntryDate
			,RRH.TillDate
			,ISNULL(RRH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(RRH.F_LedgerMaster_Tax,0) AS F_LedgerMaster_Tax
			,ISNULL(RRH.TotalRentAmount,0) AS TotalRentAmount
			,ISNULL(RRH.TotalSecurityDeposit,0) AS TotalSecurityDeposit
			,ISNULL(RRH.TotalTax,0) AS TotalTax
			,ISNULL(RRH.TotalCGST,0)   AS TotalCGST
			,ISNULL(RRH.TotalSGST,0)   AS TotalSGST
			,ISNULL(RRH.TotalIGST,0)   AS TotalIGST
			,ISNULL(RRH.Remarks,'') AS Remarks
			,ISNULL(RRH.CustomerName,'') AS CustomerName
			,ISNULL(RRH.MobileNo,'') AS MobileNo
			,RRH.UserId
			,RRH.F_VoucherH
			,RRH.F_CompanyMaster
			
	       ,(
	           SELECT
	                RRL.Id
	               ,CASE WHEN ISNULL(RRL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + RRL.DesignPhoto END AS DesignPhoto
	               ,CASE WHEN ISNULL(RRL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(RRL.DesignPhoto, LEN(RRL.DesignPhoto) - CHARINDEX('.', REVERSE(RRL.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
				   ,ISNULL(RRL.F_RentReturnH,0)   AS F_RentReturnH
				   ,ISNULL(RRL.F_RentManagementL,0) AS F_RentManagementL
				   ,ISNULL(RRL.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(RRL.F_CategoryMaster,0)	 AS F_CategoryMaster
				   ,ISNULL(RRL.F_ItemMaster,0)		 AS F_ItemMaster
				   ,ISNULL(RRL.Barcode,'')			 AS Barcode
				   ,ISNULL(RRL.ItemName,'')			 AS ItemName
				   ,ISNULL(RRL.Qty,0)			 AS Qty
				   ,ISNULL(RRL.RentPrice,0)		 AS RentPrice
				   ,ISNULL(RRL.SecurityDeposit,0) AS SecurityDeposit
				   ,ISNULL(RRL.Amount,0)			 AS Amount
				   ,ISNULL(RRL.F_StatusMaster,0) AS F_StatusMaster
				   ,ISNULL(RRL.IsActive,0) AS IsActive
				   ,ISNULL(RRL.UserId,0) AS UserId
				   ,ISNULL(IM.F_GSTGroupMaster,0) AS F_GSTGroupMaster
				   ,ISNULL(GGM.GSTGroupName,'') AS GSTGroupName
				   ,ISNULL(GGM.GSTPercent,0) AS GSTPercent
	           FROM RentReturnL RRL
			   LEFT JOIN ItemDesignMaster IDM ON IDM.Id = RRL.F_ItemDesignMaster
			   LEFT JOIN ItemMaster IM ON IM.Id = IDM.F_ItemMaster
			   LEFT JOIN GSTGroupMaster GGM ON GGM.Id = IM.F_GSTGroupMaster
	           WHERE RRL.F_RentReturnH = RRH.Id
	           FOR JSON PATH
	        ) AS RentReturnDetails

	   FROM RentReturnH RRH
	   WHERE (@Id = 0 OR RRH.Id = @Id)
  END

  ELSE IF(@ListFor='MoneyReceiptData')
  BEGIN
	     SELECT
	        MRH.Id
			,ISNULL(MRH.ReceiptNo,'') AS ReceiptNo
			,ISNULL(MRH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(MRH.F_SalesLedger,0) AS F_SalesLedger
			,MRH.ReceiptDate
			,ISNULL(MRH.Narration,'') AS Narration
			,ISNULL(MRH.ModeType,0) AS ModeType
			,ISNULL(MRH.TotalAmount,0) AS TotalAmount
			,ISNULL(MRH.CurrentBalance,0) AS CurrentBalance
			,ISNULL(MRH.LineTotal,0) AS LineTotal
			,ISNULL(MRH.DifferenceAmount,0)   AS DifferenceAmount
			,MRH.UserId
			,MRH.F_VoucherH
			
	       ,(
	           SELECT
	                MRL.Id
				   ,ISNULL(MRL.F_MoneyReceiptH,0)   AS F_MoneyReceiptH
				   ,ISNULL(MRL.F_SalesInvoiceH,0) AS F_SalesInvoiceH
				   ,ISNULL(MRL.InvoiceNo,'') AS InvoiceNo
				   ,MRL.InvoiceDate	 AS InvoiceDate
				   ,ISNULL(MRL.DueAmount,0)		 AS DueAmount
				   ,ISNULL(MRL.PaidAmount,0)			 AS PaidAmount
				   ,ISNULL(MRL.F_VoucherH,0)			 AS F_VoucherH
	           FROM MoneyReceiptL MRL
	           WHERE MRL.F_MoneyReceiptH = MRH.Id
	           FOR JSON PATH
	        ) AS MoneyReceiptDetails

	   FROM MoneyReceiptH MRH
	   WHERE (@Id = 0 OR MRH.Id = @Id)
  END

  ELSE IF(@ListFor='MoneyPaymentData')
  BEGIN
	     SELECT
	        MPH.Id
			,ISNULL(MPH.PaymentNo,'') AS PaymentNo
			,ISNULL(MPH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(MPH.F_PurchaseLedger,0) AS F_PurchaseLedger
			,MPH.PaymentDate
			,ISNULL(MPH.Narration,'') AS Narration
			,ISNULL(MPH.ModeType,0) AS ModeType
			,ISNULL(MPH.TotalAmount,0) AS TotalAmount
			,ISNULL(MPH.CurrentBalance,0) AS CurrentBalance
			,ISNULL(MPH.LineTotal,0) AS LineTotal
			,ISNULL(MPH.DifferenceAmount,0)   AS DifferenceAmount
			,MPH.UserId
			,MPH.F_VoucherH
			
	       ,(
	           SELECT
	                MPL.Id
				   ,ISNULL(MPL.F_MoneyPaymentH,0)   AS F_MoneyPaymentH
				   ,ISNULL(MPL.F_PurchaseEntryH,0) AS F_PurchaseEntryH
				   ,ISNULL(MPL.InvoiceNo,'') AS InvoiceNo
				   ,MPL.InvoiceDate	 AS InvoiceDate
				   ,ISNULL(MPL.DueAmount,0)		 AS DueAmount
				   ,ISNULL(MPL.PaidAmount,0)			 AS PaidAmount
				   ,ISNULL(MPL.F_VoucherH,0)			 AS F_VoucherH
	           FROM MoneyPaymentL MPL
	           WHERE MPL.F_MoneyPaymentH = MPH.Id
	           FOR JSON PATH
	        ) AS MoneyPaymenttDetails

	   FROM MoneyPaymentH MPH
	   WHERE (@Id = 0 OR MPH.Id = @Id)
  END

  ELSE IF (@ListFor = 'CashBankLedger')
  BEGIN
     SET @TableName = 'LedgerMaster'                                                                   
     SET @FIELDS = ' TBL.Id, TBL.Name, TBL.F_LedgerGroupMaster,
					 CASE WHEN ISNULL(TBL.F_StateMaster,''0'') = (SELECT ISNULL(F_StateMaster,''0'') AS F_StateMaster  FROM GlobalOptions) THEN 1 ELSE 1 END AS IsInState '  
	 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster IN (''8'',''4'') '  
	 SET @OrderBY = ' ORDER BY TBL.Name '
  END 

  ELSE IF (@ListFor = 'GetSalesInvoices')
  BEGIN
     SET @TableName = 'SalesEntryH'                                                                   
     SET @FIELDS = ' TBL.*'  
	 SET @WHERE = ' WHERE TBL.F_LedgerMaster = ' + CAST(@Id AS NVARCHAR)
	 SET @OrderBY = ' ORDER BY TBL.EntryNo '
  END 

  ELSE IF (@ListFor = 'GetPurchaseEntry')
  BEGIN
     SET @TableName = 'PurchaseEntryH'                                                                   
     SET @FIELDS = ' TBL.*'  
	 SET @WHERE = ' WHERE TBL.F_LedgerMaster = ' + CAST(@Id AS NVARCHAR)
	 SET @OrderBY = ' ORDER BY TBL.EntryNo '
  END 

  ELSE IF (@ListFor = 'GetSundaryDebtorsLedger')
  BEGIN
     SET @TableName = 'LedgerMaster'                                                                   
     SET @FIELDS = ' TBL.Id, TBL.Name, TBL.F_LedgerGroupMaster,
					 CASE WHEN ISNULL(TBL.F_StateMaster,''0'') = (SELECT ISNULL(F_StateMaster,''0'') AS F_StateMaster  FROM GlobalOptions) THEN 1 ELSE 1 END AS IsInState '  
	 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster IN (''36'') '  
	 SET @OrderBY = ' ORDER BY TBL.Name '
  END

  ELSE IF (@ListFor = 'GetSundaryCreditorsLedger')
  BEGIN
     SET @TableName = 'LedgerMaster'                                                                   
     SET @FIELDS = ' TBL.Id, TBL.Name, TBL.F_LedgerGroupMaster,
					 CASE WHEN ISNULL(TBL.F_StateMaster,''0'') = (SELECT ISNULL(F_StateMaster,''0'') AS F_StateMaster  FROM GlobalOptions) THEN 1 ELSE 1 END AS IsInState '  
	 SET @WHERE = ' WHERE TBL.F_LedgerGroupMaster IN (''35'') '  
	 SET @OrderBY = ' ORDER BY TBL.Name '
  END

  
  ELSE IF(@ListFor='SalesEntryDataById')
  BEGIN
	     SELECT
	        SEH.Id
			,ISNULL(SEH.EntryNoAuto,0) AS EntryNoAuto
			,ISNULL(SEH.EntryNo,'') AS EntryNo
			,SEH.EntryDate
			,ISNULL(SEH.F_LedgerMaster,0) AS F_LedgerMaster
			,ISNULL(SEH.Remarks,'') AS Remarks
			,ISNULL(SEH.TotalQty,0) AS TotalQty
			,ISNULL(SEH.TotalCGST,0)   AS TotalCGST
			,ISNULL(SEH.TotalSGST,0)   AS TotalSGST
			,ISNULL(SEH.TotalIGST,0)   AS TotalIGST
			,ISNULL(SEH.TotalTax,0)	   AS TotalTax
			,ISNULL(SEH.TotalAmount,0) AS TotalAmount
			,ISNULL(SEH.TotalOtherCharges,0) AS TotalOtherCharges
			,ISNULL(SEH.UserId,0) AS UserId
			,ISNULL(SEH.F_VoucherH,0) AS F_VoucherH
			,ISNULL(SEH.F_CompanyMaster,0) AS F_CompanyMaster
			,ISNULL(SEH.DispatchDocNo,'') AS DispatchDocNo	 
			,ISNULL(SEH.DispatchedThrough,'') AS DispatchedThrough
			,ISNULL(SEH.TotalAmount,0)  - ISNULL(tblPaidAmount.PaidAmount,0) AS DueAmount
	       ,(
	           SELECT
	                SEL.Id
	               ,CASE WHEN ISNULL(SEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + SEL.DesignPhoto END AS DesignPhoto
				   ,CASE WHEN ISNULL(SEL.DesignPhoto,'') = '' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' +  LEFT(SEL.DesignPhoto, LEN(SEL.DesignPhoto) - CHARINDEX('.', REVERSE(SEL.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb
				   ,ISNULL(SEL.F_SalesEntryH,0)   AS F_SalesEntryH
				   ,ISNULL(SEL.F_ItemDesignMaster,0) AS F_ItemDesignMaster
				   ,ISNULL(SEL.F_CategoryMaster,0)	 AS F_CategoryMaster
				   ,ISNULL(SEL.F_ItemMaster,0)		 AS F_ItemMaster
				   ,ISNULL(SEL.Barcode,'')			 AS Barcode
				   ,ISNULL(SEL.ItemName,'')			 AS ItemName
				   ,ISNULL(SEL.Qty,0)			 AS Qty
				   ,ISNULL(SEL.Rate,0)			 AS Rate
				   ,ISNULL(SEL.Amount,0)		 AS Amount
				   ,ISNULL(SEL.CGST,0)			 AS CGST
				   ,ISNULL(SEL.SGST,0)			 AS SGST
				   ,ISNULL(SEL.IGST,0)			 AS IGST
				   ,ISNULL(SEL.F_StatusMaster,0) AS F_StatusMaster
				   ,ISNULL(SEL.IsActive,0) AS IsActive
				   ,ISNULL(SEL.UserId,0) AS UserId
				   ,ISNULL(IM.F_GSTGroupMaster,0) AS F_GSTGroupMaster
				   ,ISNULL(GGM.GSTGroupName,'') AS GSTGroupName
				   ,ISNULL(GGM.GSTPercent,0) AS GSTPercent
			   FROM SalesEntryL SEL
			   LEFT JOIN ItemDesignMaster IDM ON IDM.Id = SEL.F_ItemDesignMaster
			   LEFT JOIN ItemMaster IM ON IM.Id = IDM.F_ItemMaster
			   LEFT JOIN GSTGroupMaster GGM ON GGM.Id = IM.F_GSTGroupMaster
	           WHERE SEL.F_SalesEntryH = SEH.Id
	           FOR JSON PATH
	        ) AS SalesLDetails
		   ,(
	           SELECT
	                SELOC.Id
				   ,ISNULL(SELOC.F_LedgerMaster,0) AS F_LedgerMaster
				   ,ISNULL(SELOC.Amount,0) AS Amount

	           FROM SalesEntryLOtherCharges SELOC
	           WHERE SELOC.F_SalesEntryH = SEH.Id
	           FOR JSON PATH
	        ) AS SalesLOtherChargesDetails

	   FROM SalesEntryH SEH
	   OUTER APPLY(SELECT ISNULL(SUM(MRL.PaidAmount),0) AS PaidAmount FROM MoneyReceiptL MRL WHERE MRL.F_SalesInvoiceH = SEH.Id) AS tblPaidAmount
	   WHERE SEH.Id = @Id
  END


	IF( @ListFor <> 'NewItemCreate' AND @ListFor <> 'NewItemDesignCreate' AND @ListFor <> 'DeleteCategoryMaster' AND @ListFor <> 'DeleteItemMaster' AND @ListFor <> 'DeleteItemDesignMaster' AND @ListFor <> 'GetVoucherNoByVoucherTypeId')
	BEGIN
		IF (ISNULL(@TableName, '') <> '')
		BEGIN
		   DECLARE @SQL NVARCHAR(MAX);
		   SET @SQL = 'SELECT ' + @FIELDS + ' FROM ' + @TableName + ' TBL ' + ISNULL(@JOINS, '') + ISNULL(@WHERE, '') + ISNULL(@ORDERBY, '');
		
		   -- For debugging: print the SQL statement
		   PRINT @SQL;
		
		   EXEC sp_executesql @SQL;
		END
		ELSE
		BEGIN
		  PRINT 'Error: Table name is not defined.';
		END
	END
 END                                        
END 

