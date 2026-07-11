
CREATE PROCEDURE [dbo].[CreateEccomOrder]
(
    @F_UserMaster NUMERIC(18,0),
    @Remarks NVARCHAR(500) = '',
    @DispatchedThrough NVARCHAR(250) = '',
    @DispatchDocNo NVARCHAR(100) = '',
    @OtherChargesJson NVARCHAR(MAX) = '',
    @F_CompanyMaster NUMERIC(18,0) = 1,
    @F_ShippingAddressId NUMERIC(18,0) = 0,
    @F_BillingAddressId NUMERIC(18,0) = 0,
    @ItemsJson NVARCHAR(MAX) = ''
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verify user exists and has items in cart
    IF NOT EXISTS (SELECT 1 FROM Cart WHERE F_UserMaster = @F_UserMaster)
    BEGIN
        SELECT 0 AS Success, 'Cart is empty' AS Message, 0 AS SalesEntryId;
        RETURN;
    END

    BEGIN TRY
        BEGIN TRAN;

        -- Determine Shipping Address
        DECLARE @ShipAddrId NUMERIC(18,0) = @F_ShippingAddressId;
        IF @ShipAddrId = 0 OR @ShipAddrId IS NULL
        BEGIN
            -- Fallback to default address
            SET @ShipAddrId = (SELECT TOP 1 Id FROM UserAddress WHERE F_UserMaster = @F_UserMaster AND IsActive = 1 AND IsDefault = 1);
            IF @ShipAddrId IS NULL
            BEGIN
                -- Fallback to latest address
                SET @ShipAddrId = (SELECT TOP 1 Id FROM UserAddress WHERE F_UserMaster = @F_UserMaster AND IsActive = 1 ORDER BY LastUpdateOn DESC);
            END
        END

        -- Determine Billing Address
        DECLARE @BillAddrId NUMERIC(18,0) = @F_BillingAddressId;
        IF @BillAddrId = 0 OR @BillAddrId IS NULL
        BEGIN
            SET @BillAddrId = @ShipAddrId;
        END

        -- If no address exists at all, throw error
        IF @ShipAddrId IS NULL
        BEGIN
            SELECT 0 AS Success, 'Delivery Address is required. Please save an address first.' AS Message, 0 AS SalesEntryId;
            ROLLBACK TRAN;
            RETURN;
        END

        -- Fetch Shipping and Billing Address details
        DECLARE @ShipFullName NVARCHAR(150), @ShipMobile NVARCHAR(20), @ShipLine1 NVARCHAR(250), @ShipLine2 NVARCHAR(250), @ShipCity NVARCHAR(100), @ShipState NVARCHAR(100), @ShipPin NVARCHAR(20);
        SELECT 
            @ShipFullName = A.FullName, @ShipMobile = A.MobileNo, @ShipLine1 = A.AddressLine1, @ShipLine2 = ISNULL(A.AddressLine2,''),
            @ShipCity = ISNULL(C.Name,''), @ShipState = ISNULL(S.Name,''), @ShipPin = A.PinCode
        FROM UserAddress A
        LEFT JOIN CityMaster C ON A.F_CityMaster = C.Id
        LEFT JOIN StateMaster S ON A.F_StateMaster = S.Id
        WHERE A.Id = @ShipAddrId;

        DECLARE @BillFullName NVARCHAR(150), @BillMobile NVARCHAR(20), @BillLine1 NVARCHAR(250), @BillLine2 NVARCHAR(250), @BillCity NVARCHAR(100), @BillState NVARCHAR(100), @BillPin NVARCHAR(20), @BillStateId NUMERIC(18,0);
        SELECT 
            @BillFullName = A.FullName, @BillMobile = A.MobileNo, @BillLine1 = A.AddressLine1, @BillLine2 = ISNULL(A.AddressLine2,''),
            @BillCity = ISNULL(C.Name,''), @BillState = ISNULL(S.Name,''), @BillPin = A.PinCode, @BillStateId = ISNULL(A.F_StateMaster, 0)
        FROM UserAddress A
        LEFT JOIN CityMaster C ON A.F_CityMaster = C.Id
        LEFT JOIN StateMaster S ON A.F_StateMaster = S.Id
        WHERE A.Id = @BillAddrId;

        -- Format addresses for Remarks
        DECLARE @ShippingText NVARCHAR(1000) = @ShipFullName + ' (Ph: ' + @ShipMobile + '), ' + @ShipLine1 + CASE WHEN @ShipLine2 <> '' THEN ', ' + @ShipLine2 ELSE '' END + ', ' + @ShipCity + ', ' + @ShipState + ' - ' + @ShipPin;
        DECLARE @BillingText NVARCHAR(1000) = @BillFullName + ' (Ph: ' + @BillMobile + '), ' + @BillLine1 + CASE WHEN @BillLine2 <> '' THEN ', ' + @BillLine2 ELSE '' END + ', ' + @BillCity + ', ' + @BillState + ' - ' + @BillPin;

        DECLARE @FinalRemarks NVARCHAR(2000) = ISNULL(@Remarks,'') + CHAR(13) + CHAR(10) + '--- Shipping Address ---' + CHAR(13) + CHAR(10) + @ShippingText + CHAR(13) + CHAR(10) + '--- Billing Address ---' + CHAR(13) + CHAR(10) + @BillingText;

        -- Get or create Ledger ID for the user
        DECLARE @F_LedgerMaster NUMERIC(18,0) = (SELECT F_LedgerMaster FROM UserMaster WHERE Id = @F_UserMaster);
        
        IF @F_LedgerMaster IS NULL OR @F_LedgerMaster = 0
        BEGIN
            DECLARE @Name NVARCHAR(200), @ContactEmail NVARCHAR(100), @ContactMobile NVARCHAR(20), @Address NVARCHAR(500), @F_CityMaster NUMERIC(18,0), @F_StateMaster NUMERIC(18,0);
            SELECT @Name = Name, @ContactEmail = ContactEmail, @ContactMobile = ContactMobile, @Address = Address1, @F_CityMaster = ISNULL(F_CityMaster,0), @F_StateMaster = ISNULL(F_StateMaster,0)
            FROM UserMaster WHERE Id = @F_UserMaster;
            
            INSERT INTO LedgerMaster (
                Name, Alias, F_LedgerGroupMaster,
                Address, Address1,
                F_CityMaster, F_StateMaster, F_CountryMaster,
                PinCode, PhoneNo, MobileNo, Email,
                GSTIN, PANNo, F_GSTGroupMaster, F_GSTType, F_TaxPayerType,
                CreditDays, CreditLimit, Rate,
                F_Type, F_CalculationType, F_AddLess,
                YesNoActs, F_LedgerMasterSales, F_LedgerMasterPurchase,
                F_YearScheme, F_IntCalcMethod,
                BankName, BankAccountNo, BankIFSCCode,
                ISDalal, F_LedgerMasterDalal,
                IsTransport, F_TCSonSales,
                UserId, DateOfCreation, F_CompanyMaster
            )
            VALUES (
                @Name, '', 36,
                ISNULL(@Address, ''), '',
                @F_CityMaster, @F_StateMaster, 0,
                '', '', ISNULL(@ContactMobile, ''), ISNULL(@ContactEmail, ''),
                '', '', 0, '', '',
                0, 0, 0,
                '', '', '',
                0, 0, 0,
                '', '',
                '', '', '',
                0, 0,
                0, 0,
                @F_UserMaster, GETDATE(), @F_CompanyMaster
            );
            SET @F_LedgerMaster = SCOPE_IDENTITY();
            
            UPDATE UserMaster SET F_LedgerMaster = @F_LedgerMaster WHERE Id = @F_UserMaster;
        END

        -- Determine state for tax calculations (Local vs Interstate) based on Billing Address
        DECLARE @CustState NUMERIC(18,0) = @BillStateId;
        DECLARE @FirmState NUMERIC(18,0) = (SELECT TOP 1 ISNULL(F_StateMaster, 0) FROM GlobalOptions);

        -- Build the JSON of items from the Cart
        DECLARE @JsonData NVARCHAR(MAX);
        
        SET @JsonData = (
            SELECT 
                C.F_ItemDesignMaster,
                IM.F_CategoryMaster,
                IDM.F_ItemMaster,
                IDM.Barcode,
                IM.ItemName,
                IDM.DesignPhoto,
                C.Qty,
                ISNULL(IDM.EcomPrice, ISNULL(IDM.SalePrice, 0)) AS Rate,
                1 AS F_StatusMaster,
                (C.Qty * ISNULL(IDM.EcomPrice, ISNULL(IDM.SalePrice, 0))) AS Amount,
                -- CGST
                CASE WHEN @CustState = @FirmState 
                     THEN ROUND((C.Qty * ISNULL(IDM.EcomPrice, ISNULL(IDM.SalePrice, 0)) * (ISNULL(G.CGSTPercent, 0) / 100.0)), 2)
                     ELSE 0.0 
                END AS CGST,
                -- SGST
                CASE WHEN @CustState = @FirmState 
                     THEN ROUND((C.Qty * ISNULL(IDM.EcomPrice, ISNULL(IDM.SalePrice, 0)) * (ISNULL(G.SGSTPercent, 0) / 100.0)), 2)
                     ELSE 0.0 
                END AS SGST,
                -- IGST
                CASE WHEN @CustState <> @FirmState 
                     THEN ROUND((C.Qty * ISNULL(IDM.EcomPrice, ISNULL(IDM.SalePrice, 0)) * (ISNULL(G.IGSTPercent, 0) / 100.0)), 2)
                     ELSE 0.0 
                END AS IGST
            FROM Cart C
            INNER JOIN ItemDesignMaster IDM ON C.F_ItemDesignMaster = IDM.Id
            INNER JOIN ItemMaster IM ON IDM.F_ItemMaster = IM.Id
            LEFT JOIN GSTGroupMaster G ON IM.F_GSTGroupMaster = G.Id
            WHERE C.F_UserMaster = @F_UserMaster
            FOR JSON PATH
        );

        -- Calculate total tax values from the json table
        DECLARE @TotalCGST NUMERIC(18,2) = 0;
        DECLARE @TotalSGST NUMERIC(18,2) = 0;
        DECLARE @TotalIGST NUMERIC(18,2) = 0;
        DECLARE @TotalTax NUMERIC(18,2) = 0;

        SELECT 
            @TotalCGST = SUM(CGST),
            @TotalSGST = SUM(SGST),
            @TotalIGST = SUM(IGST)
        FROM OPENJSON(@JsonData)
        WITH (
            CGST NUMERIC(18,2),
            SGST NUMERIC(18,2),
            IGST NUMERIC(18,2)
        );

        SET @TotalTax = ISNULL(@TotalCGST, 0) + ISNULL(@TotalSGST, 0) + ISNULL(@TotalIGST, 0);

        -- Generate EntryNo (SE/ECOM/AutoID)
        DECLARE @EntryNoAuto INT = (SELECT ISNULL(MAX(EntryNoAuto), 0) + 1 FROM SalesEntryH);
        DECLARE @EntryNo NVARCHAR(50) = 'SE/ECOM/' + CAST(@EntryNoAuto AS NVARCHAR(20));
        DECLARE @EntryDate DATE = CAST(GETDATE() AS DATE);

        -- Tax ledger defaults
        DECLARE @F_LedgerMaster_CGST NUMERIC(18,0) = 18;
        DECLARE @F_LedgerMaster_SGST NUMERIC(18,0) = 19;
        DECLARE @F_LedgerMaster_IGST NUMERIC(18,0) = 17;

        -- Call existing AddEdit_SalesEntry
        EXEC [dbo].[AddEdit_SalesEntry]
            @Id = 0,
            @EntryNo = @EntryNo,
            @EntryDate = @EntryDate,
            @F_LedgerMaster = @F_LedgerMaster,
            @Remarks = @FinalRemarks,
            @F_CompanyMaster = @F_CompanyMaster,
            @UserId = @F_UserMaster,
            @TotalCGST = @TotalCGST,
            @TotalSGST = @TotalSGST,
            @TotalIGST = @TotalIGST,
            @TotalTax = @TotalTax,
            @JsonData = @JsonData,
            @OtherChargesJson = @OtherChargesJson,
            @DispatchDocNo = @DispatchDocNo,
            @DispatchedThrough = @DispatchedThrough,
            @F_LedgerMaster_CGST = @F_LedgerMaster_CGST,
            @F_LedgerMaster_SGST = @F_LedgerMaster_SGST,
            @F_LedgerMaster_IGST = @F_LedgerMaster_IGST,
            @SuppressSelect = 1;

        DECLARE @SalesEntryId NUMERIC(18,0) = (SELECT TOP 1 Id FROM SalesEntryH WHERE EntryNo = @EntryNo);

        -- Clear User's Cart
        DELETE FROM Cart WHERE F_UserMaster = @F_UserMaster;

        COMMIT TRAN;

        SELECT 1 AS Success, 'Order placed successfully' AS Message, @SalesEntryId AS SalesEntryId;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        SELECT 0 AS Success, @ErrMsg AS Message, 0 AS SalesEntryId;
    END CATCH
END
