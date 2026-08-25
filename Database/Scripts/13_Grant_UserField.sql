/* =============================================================================
   13. CẤP QUYỀN CHO MÀN HÌNH "PHÂN QUYỀN LĨNH VỰC"
   -----------------------------------------------------------------------------
   Script 09 mới chỉ ĐĂNG KÝ chức năng vào Sys_Functions / Sys_FunctionActions.
   Muốn dùng được còn phải CẤP quyền cho vai trò trong bảng Sys_Permissions
   (khoá theo RoleId + FunctionId + Action).

   Nguyên tắc: vai trò nào đang được quản lý danh mục "Lĩnh vực" (Cate/Field)
   thì cũng được quản lý "Phân quyền lĩnh vực" - vì đây là hai mặt của cùng
   một nghiệp vụ.

   Cấu trúc bảng thực tế:
       Sys_Permissions : PermissionId, RoleId, FunctionId, Action
   ============================================================================= */

SET NOCOUNT ON;

DECLARE @FnUserField INT, @FnField INT;

SELECT @FnUserField = FunctionId FROM dbo.Sys_Functions WHERE Name = 'UserField' AND Area = 'Cate';
SELECT @FnField     = FunctionId FROM dbo.Sys_Functions WHERE Name = 'Field'     AND Area = 'Cate';

IF @FnUserField IS NULL
BEGIN
    PRINT '>>> Chua co chuc nang Cate/UserField. Hay chay script 09 truoc.';
    RETURN;
END

PRINT 'FunctionId UserField = ' + CAST(@FnUserField AS VARCHAR(20));
PRINT 'FunctionId Field     = ' + ISNULL(CAST(@FnField AS VARCHAR(20)), 'NULL');

/* -----------------------------------------------------------------------------
   13.1. Cấp quyền View + Edit cho mọi vai trò đang có quyền trên Cate/Field
   ----------------------------------------------------------------------------- */
IF @FnField IS NOT NULL
BEGIN
    INSERT INTO dbo.Sys_Permissions (RoleId, FunctionId, Action)
    SELECT DISTINCT p.RoleId, @FnUserField, a.Action
    FROM dbo.Sys_Permissions AS p
    CROSS JOIN (SELECT 'View' AS Action UNION ALL SELECT 'Edit') AS a
    WHERE p.FunctionId = @FnField
      AND NOT EXISTS (SELECT 1 FROM dbo.Sys_Permissions AS e
                      WHERE e.RoleId = p.RoleId
                        AND e.FunctionId = @FnUserField
                        AND e.Action = a.Action);

    PRINT 'Da cap quyen UserField theo cac vai tro dang co quyen tren Field.';
END

/* -----------------------------------------------------------------------------
   13.2. Nếu chưa vai trò nào có quyền Field, cấp cho vai trò quản trị
   ----------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Sys_Permissions WHERE FunctionId = @FnUserField)
BEGIN
    INSERT INTO dbo.Sys_Permissions (RoleId, FunctionId, Action)
    SELECT r.RoleId, @FnUserField, a.Action
    FROM dbo.Sys_Roles AS r
    CROSS JOIN (SELECT 'View' AS Action UNION ALL SELECT 'Edit') AS a
    WHERE (r.Name LIKE N'%quản trị%' OR r.Name LIKE '%dmin%')
      AND NOT EXISTS (SELECT 1 FROM dbo.Sys_Permissions AS e
                      WHERE e.RoleId = r.RoleId
                        AND e.FunctionId = @FnUserField
                        AND e.Action = a.Action);

    PRINT 'Da cap quyen UserField cho vai tro quan tri.';
END

/* -----------------------------------------------------------------------------
   13.3. Kiểm tra kết quả
   ----------------------------------------------------------------------------- */
SELECT
    r.RoleId,
    r.Name  AS TenVaiTro,
    f.Area,
    f.Name  AS ChucNang,
    p.Action
FROM dbo.Sys_Permissions AS p
INNER JOIN dbo.Sys_Functions AS f ON f.FunctionId = p.FunctionId
LEFT JOIN dbo.Sys_Roles AS r ON r.RoleId = p.RoleId
WHERE f.Name IN ('UserField', 'Field')
ORDER BY f.Name, r.RoleId, p.Action;
GO
