CREATE PROCEDURE [dbo].[GetEcommerceItems]
(
    @CategoryId NVARCHAR(MAX) = '',
    @SearchKeyword NVARCHAR(100) = '',
    @SortOrder NVARCHAR(50) = 'Newest', -- Options: 'Newest', 'PriceLowToHigh', 'PriceHighToLow', 'NameAsc', 'NameDesc'
    @F_UserMaster NUMERIC(18,0) = 0
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Handle NULL parameters by converting them to default values
    SET @CategoryId = ISNULL(@CategoryId, '');
    SET @SearchKeyword = ISNULL(@SearchKeyword, '');
    SET @SortOrder = ISNULL(@SortOrder, 'Newest');

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
       ,CASE WHEN ISNULL(IM.CoverImage,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/' + IM.CoverImage END AS CoverImage
       ,CASE WHEN ISNULL(IM.CoverImage,'')='' THEN '' ELSE 'https://accountingwkr.shinewellsofttech.co.in/ItemImages/Thumbnails_300/' + IM.CoverImage END AS CoverImage_Thumb
       ,@F_UserMaster AS PassedUserMaster
       ,ISNULL((
            SELECT SUM(V.AvailableQty)
            FROM (
                SELECT 
                    ISNULL(IDM.OpeningStock,0)
                    + ISNULL((SELECT SUM(Qty) FROM PurchaseEntryL WHERE F_ItemDesignMaster = IDM.Id), 0)
                    + ISNULL((SELECT SUM(Qty) FROM SalesReturnL WHERE F_ItemDesignMaster = IDM.Id), 0)
                    - ISNULL((SELECT SUM(Qty) FROM SalesEntryL WHERE F_ItemDesignMaster = IDM.Id), 0)
                    - ISNULL((SELECT SUM(Qty) FROM PurchaseReturnL WHERE F_ItemDesignMaster = IDM.Id), 0) AS AvailableQty
                FROM ItemDesignMaster IDM
                WHERE IDM.F_ItemMaster = IM.Id
                  AND ISNULL(IDM.IsEcom, 0) = 1
            ) V
       ),0) AS AvailableQty
       ,CASE WHEN EXISTS (
            SELECT 1 FROM Wishlist W
            INNER JOIN ItemDesignMaster IDM_W ON W.F_ItemDesignMaster = IDM_W.Id
            WHERE IDM_W.F_ItemMaster = IM.Id
              AND W.F_UserMaster = @F_UserMaster
        ) THEN 1 ELSE 0 END AS IsInWishlist
       ,(
           SELECT
                IDM.Id
               ,IDM.Id AS F_ItemDesignMaster
               ,IDM.Id AS ItemDesignId
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
                ,ISNULL((
                    SELECT TOP 1 W.Id 
                    FROM Wishlist W 
                    WHERE W.F_ItemDesignMaster = IDM.Id 
                      AND W.F_UserMaster = @F_UserMaster
                ), 0) AS WishlistId
                ,CASE WHEN EXISTS (
                    SELECT 1 FROM Wishlist W 
                    WHERE W.F_ItemDesignMaster = IDM.Id 
                      AND W.F_UserMaster = @F_UserMaster
                ) THEN 1 ELSE 0 END AS IsInWishlist

           FROM ItemDesignMaster IDM
           WHERE IDM.F_ItemMaster = IM.Id
             AND ISNULL(IDM.IsEcom, 0) = 1 -- Only Ecommerce Items
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
       (@CategoryId = '' OR IM.F_CategoryMaster IN (SELECT value FROM STRING_SPLIT(@CategoryId, ',')))
       AND (
           @SearchKeyword = '' 
           OR IM.ItemName LIKE '%' + @SearchKeyword + '%' 
           OR ISNULL(IM.ShortDescription,'') LIKE '%' + @SearchKeyword + '%'
           OR ISNULL(IM.FullDescription,'') LIKE '%' + @SearchKeyword + '%'
           OR ISNULL(IM.HSNCode,'') LIKE '%' + @SearchKeyword + '%'
           OR EXISTS (
               SELECT 1 
               FROM ItemDesignMaster IDMS 
               WHERE IDMS.F_ItemMaster = IM.Id 
                 AND ISNULL(IDMS.IsEcom, 0) = 1
                 AND (
                     ISNULL(IDMS.Barcode,'') LIKE '%' + @SearchKeyword + '%'
                     OR ISNULL(IDMS.SizeName,'') LIKE '%' + @SearchKeyword + '%'
                 )
           )
       )
       AND EXISTS (
           SELECT 1 
           FROM ItemDesignMaster IDM2 
           WHERE IDM2.F_ItemMaster = IM.Id 
             AND ISNULL(IDM2.IsEcom, 0) = 1
       )
   ORDER BY 
        CASE WHEN @SortOrder = 'NameAsc' THEN IM.ItemName END ASC,
        CASE WHEN @SortOrder = 'NameDesc' THEN IM.ItemName END DESC,
        CASE WHEN @SortOrder = 'PriceLowToHigh' THEN 
            (SELECT MIN(ISNULL(NULLIF(EcomPrice, 0), ISNULL(SalePrice, 0))) FROM ItemDesignMaster WHERE F_ItemMaster = IM.Id AND ISNULL(IsEcom, 0) = 1) 
        END ASC,
        CASE WHEN @SortOrder = 'PriceHighToLow' THEN 
            (SELECT MAX(ISNULL(NULLIF(EcomPrice, 0), ISNULL(SalePrice, 0))) FROM ItemDesignMaster WHERE F_ItemMaster = IM.Id AND ISNULL(IsEcom, 0) = 1) 
        END DESC,
        CASE WHEN @SortOrder = 'Newest' THEN IM.Id END DESC

END
