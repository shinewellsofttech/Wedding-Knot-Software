CREATE PROCEDURE [dbo].[Admin_GetEccomOrders]
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
                    L.DesignPhoto,
                    L.Qty,
                    L.Rate,
                    L.Amount,
                    L.CGST,
                    L.SGST,
                    L.IGST,
                    L.F_StatusMaster,
                    -- Details from ItemDesignMaster
                    IDM.SizeName,
                    IDM.DesignPhoto2,
                    IDM.DesignPhoto3,
                    IDM.DesignPhoto4,
                    IDM.DesignPhoto5,
                    IDM.VideoLink,
                    IDM.Length,
                    IDM.Width,
                    IDM.Height,
                    IDM.Weight,
                    IDM.EcomPrice,
                    -- Details from ItemMaster
                    IM.ShortDescription,
                    IM.FullDescription
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
                    L.DesignPhoto,
                    L.Qty,
                    L.Rate,
                    L.Amount,
                    L.CGST,
                    L.SGST,
                    L.IGST,
                    L.F_StatusMaster,
                    -- Details from ItemDesignMaster
                    IDM.SizeName,
                    IDM.DesignPhoto2,
                    IDM.DesignPhoto3,
                    IDM.DesignPhoto4,
                    IDM.DesignPhoto5,
                    IDM.VideoLink,
                    IDM.Length,
                    IDM.Width,
                    IDM.Height,
                    IDM.Weight,
                    IDM.EcomPrice,
                    -- Details from ItemMaster
                    IM.ShortDescription,
                    IM.FullDescription
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

