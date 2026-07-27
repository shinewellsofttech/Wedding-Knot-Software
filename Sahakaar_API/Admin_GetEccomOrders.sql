ALTER PROCEDURE [dbo].[Admin_GetEccomOrders]
(
    @F_UserMaster NUMERIC(18,0),  -- Admin User ID
    @TargetUserId NUMERIC(18,0) = 0
)
AS
BEGIN
    SET NOCOUNT ON;

    -- If the calling user is a customer (UserType = 4), restrict their search to only their own orders
    DECLARE @UserType NUMERIC(18,0) = 0;
    SELECT TOP 1 @UserType = F_UserType FROM UserMaster WHERE Id = @F_UserMaster;

    IF @UserType = 4
    BEGIN
        SET @TargetUserId = @F_UserMaster;
    END

    IF @TargetUserId > 0
    BEGIN
        SELECT 
            H.Id, 
            H.EntryNo, 
            H.EntryDate, 
            H.UserId AS F_UserMaster, 
            H.TotalTax,
            H.Remarks, 
            H.DispatchDocNo, 
            H.DispatchedThrough,
            H.F_StatusMaster,
            H.OrderStatus,
            H.OrderStatusRemarks,
            H.OrderStatusUpdatedOn,
            H.OrderStatusUpdatedBy,
            U.Name AS CustomerName,
            U.ContactMobile,
            U.ContactEmail,
            (
                SELECT 
                    L.Id,
                    L.F_ItemDesignMaster,
                    L.F_CategoryMaster,
                    L.F_ItemMaster,
                    L.Barcode,
                    L.ItemName,
                    -- DesignPhoto with full URL
                    CASE WHEN ISNULL(IDM.DesignPhoto,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto END AS DesignPhoto,
                    CASE WHEN ISNULL(IDM.DesignPhoto2,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto2 END AS DesignPhoto2,
                    CASE WHEN ISNULL(IDM.DesignPhoto3,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto3 END AS DesignPhoto3,
                    CASE WHEN ISNULL(IDM.DesignPhoto4,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto4 END AS DesignPhoto4,
                    CASE WHEN ISNULL(IDM.DesignPhoto5,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto5 END AS DesignPhoto5,
                    -- Thumbnail URLs
                    CASE WHEN ISNULL(IDM.DesignPhoto,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto, LEN(IDM.DesignPhoto) - CHARINDEX('.', REVERSE(IDM.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto2,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto2, LEN(IDM.DesignPhoto2) - CHARINDEX('.', REVERSE(IDM.DesignPhoto2))) + '.webp' END AS DesignPhoto2_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto3,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto3, LEN(IDM.DesignPhoto3) - CHARINDEX('.', REVERSE(IDM.DesignPhoto3))) + '.webp' END AS DesignPhoto3_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto4,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto4, LEN(IDM.DesignPhoto4) - CHARINDEX('.', REVERSE(IDM.DesignPhoto4))) + '.webp' END AS DesignPhoto4_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto5,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto5, LEN(IDM.DesignPhoto5) - CHARINDEX('.', REVERSE(IDM.DesignPhoto5))) + '.webp' END AS DesignPhoto5_Thumb,
                    L.Qty,
                    L.Rate,
                    L.Amount,
                    L.CGST,
                    L.SGST,
                    L.IGST,
                    L.F_StatusMaster,
                    -- Details from ItemDesignMaster
                    IDM.SizeName,
                    IDM.VideoLink,
                    IDM.Length,
                    IDM.Width,
                    IDM.Height,
                    IDM.Weight,
                    IDM.EcomPrice,
                    -- Details from ItemMaster
                    IM.ShortDescription,
                    IM.FullDescription,
                    CASE WHEN ISNULL(IM.CoverImage,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IM.CoverImage END AS CoverImage
                FROM SalesEntryL L
                LEFT JOIN ItemDesignMaster IDM ON L.F_ItemDesignMaster = IDM.Id
                LEFT JOIN ItemMaster IM ON L.F_ItemMaster = IM.Id
                WHERE L.F_SalesEntryH = H.Id
                FOR JSON PATH
            ) AS ItemsJson
        FROM SalesEntryH H
        LEFT JOIN UserMaster U ON H.UserId = U.Id
        WHERE H.UserId = @TargetUserId
          AND H.EntryNo LIKE 'SE/ECOM/%'
        ORDER BY H.Id DESC;
    END
    ELSE
    BEGIN
        SELECT 
            H.Id, 
            H.EntryNo, 
            H.EntryDate, 
            H.UserId AS F_UserMaster, 
            H.TotalTax,
            H.Remarks, 
            H.DispatchDocNo, 
            H.DispatchedThrough,
            H.F_StatusMaster,
            H.OrderStatus,
            H.OrderStatusRemarks,
            H.OrderStatusUpdatedOn,
            H.OrderStatusUpdatedBy,
            U.Name AS CustomerName,
            U.ContactMobile,
            U.ContactEmail,
            (
                SELECT 
                    L.Id,
                    L.F_ItemDesignMaster,
                    L.F_CategoryMaster,
                    L.F_ItemMaster,
                    L.Barcode,
                    L.ItemName,
                    -- DesignPhoto with full URL
                    CASE WHEN ISNULL(IDM.DesignPhoto,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto END AS DesignPhoto,
                    CASE WHEN ISNULL(IDM.DesignPhoto2,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto2 END AS DesignPhoto2,
                    CASE WHEN ISNULL(IDM.DesignPhoto3,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto3 END AS DesignPhoto3,
                    CASE WHEN ISNULL(IDM.DesignPhoto4,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto4 END AS DesignPhoto4,
                    CASE WHEN ISNULL(IDM.DesignPhoto5,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IDM.DesignPhoto5 END AS DesignPhoto5,
                    -- Thumbnail URLs
                    CASE WHEN ISNULL(IDM.DesignPhoto,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto, LEN(IDM.DesignPhoto) - CHARINDEX('.', REVERSE(IDM.DesignPhoto))) + '.webp' END AS DesignPhoto_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto2,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto2, LEN(IDM.DesignPhoto2) - CHARINDEX('.', REVERSE(IDM.DesignPhoto2))) + '.webp' END AS DesignPhoto2_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto3,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto3, LEN(IDM.DesignPhoto3) - CHARINDEX('.', REVERSE(IDM.DesignPhoto3))) + '.webp' END AS DesignPhoto3_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto4,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto4, LEN(IDM.DesignPhoto4) - CHARINDEX('.', REVERSE(IDM.DesignPhoto4))) + '.webp' END AS DesignPhoto4_Thumb,
                    CASE WHEN ISNULL(IDM.DesignPhoto5,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnail/' + LEFT(IDM.DesignPhoto5, LEN(IDM.DesignPhoto5) - CHARINDEX('.', REVERSE(IDM.DesignPhoto5))) + '.webp' END AS DesignPhoto5_Thumb,
                    L.Qty,
                    L.Rate,
                    L.Amount,
                    L.CGST,
                    L.SGST,
                    L.IGST,
                    L.F_StatusMaster,
                    -- Details from ItemDesignMaster
                    IDM.SizeName,
                    IDM.VideoLink,
                    IDM.Length,
                    IDM.Width,
                    IDM.Height,
                    IDM.Weight,
                    IDM.EcomPrice,
                    -- Details from ItemMaster
                    IM.ShortDescription,
                    IM.FullDescription,
                    CASE WHEN ISNULL(IM.CoverImage,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IM.CoverImage END AS CoverImage
                FROM SalesEntryL L
                LEFT JOIN ItemDesignMaster IDM ON L.F_ItemDesignMaster = IDM.Id
                LEFT JOIN ItemMaster IM ON L.F_ItemMaster = IM.Id
                WHERE L.F_SalesEntryH = H.Id
                FOR JSON PATH
            ) AS ItemsJson
        FROM SalesEntryH H
        LEFT JOIN UserMaster U ON H.UserId = U.Id
        WHERE H.EntryNo LIKE 'SE/ECOM/%'
        ORDER BY H.Id DESC;
    END
END
