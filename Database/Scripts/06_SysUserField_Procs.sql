/* =============================================================================
   06. STORED PROCEDURE PHÂN QUYỀN LĨNH VỰC CHO NGƯỜI DÙNG
   -----------------------------------------------------------------------------
   Phục vụ màn hình "Phân quyền lĩnh vực": lấy danh sách người dùng kèm lĩnh vực
   đang được phân, lấy chi tiết theo 1 người dùng, và lưu lại danh sách lĩnh vực.
   ============================================================================= */

/* -----------------------------------------------------------------------------
   6.1. DANH SÁCH NGƯỜI DÙNG KÈM LĨNH VỰC ĐƯỢC PHÂN
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Sys_UserField_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Sys_UserField_Get;
GO

CREATE PROCEDURE dbo.p_Sys_UserField_Get
    @Key        NVARCHAR(500) = NULL,
    @Search     NVARCHAR(500) = NULL,
    @Order      NVARCHAR(10)  = '0',
    @OrderDir   NVARCHAR(10)  = 'ASC',
    @StartIndex INT           = 0,
    @PageSize   INT           = -1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Key    = NULLIF(LTRIM(RTRIM(ISNULL(@Key, ''))), '');
    SET @Search = NULLIF(LTRIM(RTRIM(ISNULL(@Search, ''))), '');

    ;WITH UserList AS
    (
        SELECT
            u.UserName,
            u.FullName,
            u.Email,
            /* Danh sách id lĩnh vực - dùng để tích sẵn checkbox khi mở form sửa */
            ISNULL(STUFF((SELECT ',' + CAST(uf2.FieldId AS NVARCHAR(20))
                          FROM dbo.Sys_User_Field AS uf2
                          WHERE uf2.UserName = u.UserName
                          ORDER BY uf2.FieldId
                          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 1, ''), '') AS FieldIds,
            /* Danh sách tên lĩnh vực - dùng để hiển thị trên lưới */
            ISNULL(STUFF((SELECT N', ' + f2.FieldName
                          FROM dbo.Sys_User_Field AS uf3
                          INNER JOIN dbo.Cate_Fields AS f2 ON f2.FieldId = uf3.FieldId
                          WHERE uf3.UserName = u.UserName
                            AND ISNULL(f2.IsDeleted, 0) = 0
                          ORDER BY f2.FieldName
                          FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, ''), N'') AS FieldNames,
            (SELECT COUNT(1) FROM dbo.Sys_User_Field AS uf4 WHERE uf4.UserName = u.UserName) AS TotalField
        FROM dbo.Sys_Users AS u
        WHERE ISNULL(u.IsDeleted, 0) = 0
          AND (@Key IS NULL
               OR u.UserName LIKE N'%' + @Key + '%'
               OR u.FullName LIKE N'%' + @Key + '%')
          AND (@Search IS NULL
               OR u.UserName LIKE N'%' + @Search + '%'
               OR u.FullName LIKE N'%' + @Search + '%')
    ),
    Counted AS
    (
        SELECT ul.*, COUNT(*) OVER () AS TotalRow
        FROM UserList AS ul
    )
    SELECT *
    FROM Counted
    ORDER BY
        CASE WHEN @OrderDir = 'DESC' THEN UserName END DESC,
        CASE WHEN @OrderDir <> 'DESC' THEN UserName END ASC
    OFFSET (CASE WHEN @StartIndex < 0 THEN 0 ELSE @StartIndex END) ROWS
    FETCH NEXT (CASE WHEN @PageSize IS NULL OR @PageSize <= 0 THEN 2147483647 ELSE @PageSize END) ROWS ONLY;
END
GO


/* -----------------------------------------------------------------------------
   6.2. LẤY LĨNH VỰC ĐƯỢC PHÂN CỦA MỘT NGƯỜI DÙNG
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Sys_UserField_GetByUser', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Sys_UserField_GetByUser;
GO

CREATE PROCEDURE dbo.p_Sys_UserField_GetByUser
    @UserName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        f.FieldId,
        f.FieldCode,
        f.FieldName,
        f.Description,
        f.IsActive,
        f.IsDeleted
    FROM dbo.Sys_User_Field AS uf
    INNER JOIN dbo.Cate_Fields AS f ON f.FieldId = uf.FieldId
    WHERE uf.UserName = @UserName
      AND ISNULL(f.IsDeleted, 0) = 0
    ORDER BY f.FieldName;
END
GO


/* -----------------------------------------------------------------------------
   6.3. LƯU DANH SÁCH LĨNH VỰC CHO MỘT NGƯỜI DÙNG
   Xoá toàn bộ phân quyền cũ rồi ghi lại theo danh sách mới (đơn giản, an toàn).
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Sys_UserField_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Sys_UserField_Save;
GO

CREATE PROCEDURE dbo.p_Sys_UserField_Save
    @UserName  NVARCHAR(100),
    @FieldIds  NVARCHAR(MAX) = NULL,   -- Danh sách id lĩnh vực, phân tách bởi dấu phẩy
    @CreatedBy NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @UserName IS NULL OR LTRIM(RTRIM(@UserName)) = ''
    BEGIN
        SELECT CAST(0 AS BIT) AS Result;
        RETURN;
    END

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM dbo.Sys_User_Field WHERE UserName = @UserName;

        IF @FieldIds IS NOT NULL AND LTRIM(RTRIM(@FieldIds)) <> ''
        BEGIN
            INSERT INTO dbo.Sys_User_Field (UserName, FieldId, CreatedDate, CreatedBy)
            SELECT DISTINCT @UserName, TRY_CAST(LTRIM(RTRIM(value)) AS INT), GETDATE(), @CreatedBy
            FROM STRING_SPLIT(@FieldIds, ',')
            WHERE TRY_CAST(LTRIM(RTRIM(value)) AS INT) IS NOT NULL
              AND EXISTS (SELECT 1 FROM dbo.Cate_Fields AS f
                          WHERE f.FieldId = TRY_CAST(LTRIM(RTRIM(value)) AS INT)
                            AND ISNULL(f.IsDeleted, 0) = 0);
        END

        COMMIT TRANSACTION;
        SELECT CAST(1 AS BIT) AS Result;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        SELECT CAST(0 AS BIT) AS Result;
    END CATCH
END
GO
