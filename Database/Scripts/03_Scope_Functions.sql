/* =============================================================================
   03. HÀM XÁC ĐỊNH PHẠM VI DỮ LIỆU CỦA NGƯỜI DÙNG
   -----------------------------------------------------------------------------
   Đây là phần lõi của việc phân quyền xem dữ liệu. Hai hàm dưới đây trả về:

     fn_GetPermittedUnions(@UserName) : danh sách đơn vị (tổ / phòng) mà người
                                        dùng được xem dữ liệu, ĐÃ BAO GỒM toàn bộ
                                        đơn vị con ở mọi cấp.
     fn_GetPermittedFields(@UserName)  : danh sách lĩnh vực người dùng được xem.

   Quy tắc phạm vi đơn vị:
     1. Đơn vị mà người dùng là thành viên (Cate_Unions_Members).
     2. Đơn vị mà người dùng được phân làm quản lý (Cate_Unions_Mangers).
     3. Toàn bộ đơn vị con của (1) và (2), đệ quy mọi cấp.

   Ví dụ theo yêu cầu nghiệp vụ:
     - Người A thuộc Tổ 1                    -> thấy dữ liệu Tổ 1.
     - Người C thuộc Phòng A, quản lý Tổ 1+2 -> thấy Phòng A, Tổ 1, Tổ 2 và mọi
                                                đơn vị con bên dưới.
   ============================================================================= */

IF OBJECT_ID('dbo.fn_GetPermittedUnions', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetPermittedUnions;
GO

CREATE FUNCTION dbo.fn_GetPermittedUnions (@UserName NVARCHAR(100))
RETURNS TABLE
AS
RETURN
(
    WITH RootUnions AS
    (
        /* (1) Đơn vị người dùng là thành viên
              Lưu ý: bảng Cate_Unions_Members dùng cột BelongUnion làm khoá đơn vị,
              không phải UnionId. */
        SELECT m.BelongUnion AS UnionId
        FROM dbo.Cate_Unions_Members AS m
        WHERE m.UserName = @UserName
          AND m.BelongUnion IS NOT NULL

        UNION

        /* (2) Đơn vị người dùng được phân quản lý */
        SELECT mg.UnionId
        FROM dbo.Cate_Unions_Mangers AS mg
        WHERE mg.Manager = @UserName
    ),
    UnionTree AS
    (
        /* Neo: các đơn vị gốc ở trên */
        SELECT u.UnionId, u.BelongUnion
        FROM dbo.Cate_Unions AS u
        INNER JOIN RootUnions AS r ON r.UnionId = u.UnionId
        WHERE ISNULL(u.IsDeleted, 0) = 0

        UNION ALL

        /* Đệ quy: mọi đơn vị con ở các cấp bên dưới */
        SELECT c.UnionId, c.BelongUnion
        FROM dbo.Cate_Unions AS c
        INNER JOIN UnionTree AS p ON c.BelongUnion = p.UnionId
        WHERE ISNULL(c.IsDeleted, 0) = 0
    )
    SELECT DISTINCT UnionId
    FROM UnionTree
);
GO

IF OBJECT_ID('dbo.fn_GetPermittedFields', 'IF') IS NOT NULL
    DROP FUNCTION dbo.fn_GetPermittedFields;
GO

CREATE FUNCTION dbo.fn_GetPermittedFields (@UserName NVARCHAR(100))
RETURNS TABLE
AS
RETURN
(
    SELECT uf.FieldId
    FROM dbo.Sys_User_Field AS uf
    INNER JOIN dbo.Cate_Fields AS f ON f.FieldId = uf.FieldId
    WHERE uf.UserName = @UserName
      AND ISNULL(f.IsDeleted, 0) = 0
      AND ISNULL(f.IsActive, 1) = 1
);
GO

/* -----------------------------------------------------------------------------
   Hàm kiểm tra người dùng có phải super admin hay không.
   Super admin bỏ qua toàn bộ giới hạn phạm vi dữ liệu.
   Danh sách lấy từ bảng cấu hình Sys_Configs, khoá CONFIG_SUPER_ADMIN_PERMIT
   (danh sách tên đăng nhập, phân tách bằng dấu phẩy) - trùng với cấu hình mà
   tầng ứng dụng đang dùng trong AppController.
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.fn_IsSuperAdmin', 'FN') IS NOT NULL
    DROP FUNCTION dbo.fn_IsSuperAdmin;
GO

CREATE FUNCTION dbo.fn_IsSuperAdmin (@UserName NVARCHAR(100))
RETURNS BIT
AS
BEGIN
    IF @UserName IS NULL OR LTRIM(RTRIM(@UserName)) = '' RETURN 0;

    DECLARE @ConfigValue NVARCHAR(MAX);

    SELECT TOP 1 @ConfigValue = ConfigValue
    FROM dbo.Sys_Configs
    WHERE ConfigKey = 'CONFIG_SUPER_ADMIN_PERMIT';

    IF @ConfigValue IS NULL OR LTRIM(RTRIM(@ConfigValue)) = '' RETURN 0;

    /* So khớp chính xác một phần tử trong danh sách phân tách bởi dấu phẩy */
    IF ',' + REPLACE(@ConfigValue, ' ', '') + ',' LIKE '%,' + LTRIM(RTRIM(@UserName)) + ',%'
        RETURN 1;

    RETURN 0;
END
GO
