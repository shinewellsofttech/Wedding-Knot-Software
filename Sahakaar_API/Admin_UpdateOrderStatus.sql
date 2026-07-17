CREATE PROCEDURE [dbo].[Admin_UpdateOrderStatus]
(
    @F_UserMaster NUMERIC(18,0), -- Admin User ID
    @OrderId NUMERIC(18,0),
    @F_StatusMaster NUMERIC(18,0),
    @Remarks NVARCHAR(500) = ''
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verify order exists and is an e-commerce order
    IF NOT EXISTS (SELECT 1 FROM SalesEntryH WHERE Id = @OrderId AND EntryNo LIKE 'SE/ECOM/%')
    BEGIN
        SELECT 0 AS Success, 'Order not found or not an e-commerce order' AS Message;
        RETURN;
    END

    -- Map F_StatusMaster to OrderStatus string
    DECLARE @Status NVARCHAR(50) = 'Pending';
    IF @F_StatusMaster = 2
        SET @Status = 'Approved';
    ELSE IF @F_StatusMaster = 3
        SET @Status = 'Rejected';

    BEGIN TRY
        BEGIN TRAN;

        -- Update header
        UPDATE SalesEntryH
        SET 
            F_StatusMaster = @F_StatusMaster,
            OrderStatus = @Status,
            OrderStatusRemarks = @Remarks,
            OrderStatusUpdatedOn = GETDATE(),
            OrderStatusUpdatedBy = @F_UserMaster
        WHERE Id = @OrderId;

        -- Update lines
        UPDATE SalesEntryL
        SET 
            F_StatusMaster = @F_StatusMaster,
            LastUpdateOn = GETDATE()
        WHERE F_SalesEntryH = @OrderId;

        -- Clear User's Cart if the order is approved (status = 2)
        IF @F_StatusMaster = 2
        BEGIN
            DECLARE @OrderUserId NUMERIC(18,0) = (SELECT UserId FROM SalesEntryH WHERE Id = @OrderId);
            IF @OrderUserId IS NOT NULL AND @OrderUserId > 0
            BEGIN
                DELETE FROM Cart WHERE F_UserMaster = @OrderUserId;
            END
        END

        COMMIT TRAN;

        SELECT 1 AS Success, 'Order status updated successfully' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;
            
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        SELECT 0 AS Success, @ErrMsg AS Message;
    END CATCH
END
