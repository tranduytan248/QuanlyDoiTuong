/* =============================================================================
   28. LƯU DANH SÁCH ĐƠN VỊ QUẢN LÝ CHO NGƯỜI DÙNG (SaveList)
   -----------------------------------------------------------------------------
   Tạo Stored Procedure p_Cate_Union_Manager_SaveList để cập nhật toàn bộ
   danh sách đơn vị quản lý (phân quyền dữ liệu) cho 1 tài khoản người dùng
   trong một thao tác.
   ============================================================================= */

IF OBJECT_ID('dbo.p_Cate_Union_Manager_SaveList', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_Union_Manager_SaveList;
GO

CREATE PROCEDURE dbo.p_Cate_Union_Manager_SaveList
    @UserName VARCHAR(100),
    @UnionIds NVARCHAR(MAX),
    @SavedBy  VARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @UserName = LTRIM(RTRIM(ISNULL(@UserName, '')));
    IF @UserName = ''
    BEGIN
        RAISERROR(N'UserName không được để trống.', 16, 1);
        RETURN 0;
    END

    /* Bảng tạm chứa danh sách UnionId được chọn */
    DECLARE @TblUnions TABLE (UnionId UNIQUEIDENTIFIER PRIMARY KEY);

    IF @UnionIds IS NOT NULL AND LTRIM(RTRIM(@UnionIds)) <> ''
    BEGIN
        INSERT INTO @TblUnions (UnionId)
        SELECT DISTINCT TRY_CAST(LTRIM(RTRIM(value)) AS UNIQUEIDENTIFIER)
        FROM STRING_SPLIT(@UnionIds, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(value)) AS UNIQUEIDENTIFIER) IS NOT NULL;
    END

    BEGIN TRANSACTION;
    BEGIN TRY
        /* 1. Xóa các đơn vị bị bỏ chọn */
        DELETE cum
        FROM dbo.Cate_Unions_Mangers AS cum
        WHERE cum.Manager = @UserName
          AND cum.UnionId NOT IN (SELECT UnionId FROM @TblUnions);

        /* 2. Thêm các đơn vị mới được chọn */
        INSERT INTO dbo.Cate_Unions_Mangers (UnionId, Manager, CreatedOn, CreatedBy)
        SELECT 
            t.UnionId,
            @UserName,
            GETDATE(),
            @SavedBy
        FROM @TblUnions AS t
        WHERE NOT EXISTS (
            SELECT 1 
            FROM dbo.Cate_Unions_Mangers AS cum
            WHERE cum.Manager = @UserName 
              AND cum.UnionId = t.UnionId
        );

        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION;

        RETURN 1;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        INSERT INTO dbo.Sys_ProcedureLogs (LogDate, ProcedureName, ErrorLine, ErrorMessage, AdditionalInfo)
        SELECT GETDATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE(), @UserName;

        RETURN 0;
    END CATCH
END
GO
