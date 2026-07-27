-- =============================================
-- SQL Setup Script for Website Leads Management
-- =============================================

-- 1. Create Table: WebsiteLeads
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WebsiteLeads]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[WebsiteLeads](
        [Id] [bigint] IDENTITY(1,1) NOT NULL,
        [Name] [nvarchar](150) NULL,
        [MobileNo] [nvarchar](20) NOT NULL,
        [Email] [nvarchar](150) NULL,
        [CreatedDate] [datetime] NULL CONSTRAINT [DF_WebsiteLeads_CreatedDate] DEFAULT (GETDATE()),
        [IsDeleted] [bit] NULL CONSTRAINT [DF_WebsiteLeads_IsDeleted] DEFAULT ((0)),
        CONSTRAINT [PK_WebsiteLeads] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
END
GO

-- 2. Stored Procedure: Save_WebsiteLead
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Save_WebsiteLead]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[Save_WebsiteLead]
GO

CREATE PROCEDURE [dbo].[Save_WebsiteLead]
    @Name NVARCHAR(150) = NULL,
    @MobileNo NVARCHAR(20),
    @Email NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Clean mobile number for comparison (remove spaces/dashes if any)
    DECLARE @CleanMobile NVARCHAR(20) = LTRIM(RTRIM(ISNULL(@MobileNo, '')));

    IF (@CleanMobile = '')
    BEGIN
        SELECT 0 AS ResultCode, 'Mobile number cannot be empty' AS Message;
        RETURN;
    END

    -- Check if mobile number already exists in WebsiteLeads (active)
    IF EXISTS (
        SELECT 1 FROM [dbo].[WebsiteLeads] 
        WHERE [MobileNo] = @CleanMobile AND ISNULL([IsDeleted], 0) = 0
    )
    BEGIN
        SELECT -1 AS ResultCode, 'Mobile number already registered in leads' AS Message;
        RETURN;
    END

    -- Check if mobile number exists in AspNetUsers / UserMaster if table exists
    IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
    BEGIN
        IF EXISTS (
            SELECT 1 FROM [dbo].[AspNetUsers] 
            WHERE [PhoneNumber] = @CleanMobile OR [UserName] = @CleanMobile
        )
        BEGIN
            SELECT -1 AS ResultCode, 'Mobile number already registered user' AS Message;
            RETURN;
        END
    END

    -- Insert new lead
    INSERT INTO [dbo].[WebsiteLeads] ([Name], [MobileNo], [Email], [CreatedDate], [IsDeleted])
    VALUES (@Name, @CleanMobile, @Email, GETDATE(), 0);

    DECLARE @NewId BIGINT = SCOPE_IDENTITY();
    SELECT @NewId AS ResultCode, 'Lead saved successfully' AS Message;
END
GO

-- 3. Stored Procedure: GetList_WebsiteLeads
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetList_WebsiteLeads]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[GetList_WebsiteLeads]
GO

CREATE PROCEDURE [dbo].[GetList_WebsiteLeads]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        [Id],
        ISNULL([Name], '') AS [Name],
        [MobileNo],
        ISNULL([Email], '') AS [Email],
        [CreatedDate]
    FROM [dbo].[WebsiteLeads]
    WHERE ISNULL([IsDeleted], 0) = 0
    ORDER BY [Id] DESC;
END
GO

-- 4. Stored Procedure: Delete_WebsiteLeads_Bulk
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Delete_WebsiteLeads_Bulk]') AND type in (N'P', N'PC'))
    DROP PROCEDURE [dbo].[Delete_WebsiteLeads_Bulk]
GO

CREATE PROCEDURE [dbo].[Delete_WebsiteLeads_Bulk]
    @Ids NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    IF (ISNULL(@Ids, '') = '')
    BEGIN
        SELECT 0 AS RowsAffected, 'No IDs provided' AS Message;
        RETURN;
    END

    -- Split string by comma and update IsDeleted flag
    UPDATE [dbo].[WebsiteLeads]
    SET [IsDeleted] = 1
    WHERE [Id] IN (
        SELECT TRY_CAST(value AS BIGINT) 
        FROM STRING_SPLIT(@Ids, ',') 
        WHERE RTRIM(value) <> '' AND TRY_CAST(value AS BIGINT) IS NOT NULL
    );

    DECLARE @Count INT = @@ROWCOUNT;
    SELECT @Count AS RowsAffected, 'Leads deleted successfully' AS Message;
END
GO
