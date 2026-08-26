/* =============================================================================
   16. CẤP QUYỀN CHO CHỨC NĂNG LỊCH SỬ VI PHẠM
   -----------------------------------------------------------------------------
   TRIỆU CHỨNG: bấm nút "Sửa" hoặc "Xoá" trên màn hình Lịch sử vi phạm không
   hiện gì cả.

   NGUYÊN NHÂN: chức năng Major/SubjectViolation đã được đăng ký trong
   Sys_Functions / Sys_FunctionActions (script 09) nhưng CHƯA cấp quyền cho vai
   trò nào trong Sys_Permissions. Khung TSFramework tra Sys_Permission_IsAllow,
   không thấy quyền nên tra ve 403 va modal khong mo duoc.

   NGUYÊN TẮC: vai trò nào đang thao tác được trên Đối tượng (Major/Subject) thì
   cũng thao tác được trên Lịch sử vi phạm của đối tượng đó - vì đây là hai phần
   của cùng một nghiệp vụ.

   Lưu ý: đây chỉ là quyền vào được màn hình. Việc chỉ người khai báo mới được
   Sửa / Xoá một lần vi phạm vẫn được kiểm tra riêng trong
   SubjectViolationController.IsViolationOwner().
   ============================================================================= */

SET NOCOUNT ON;

DECLARE @FnViolation INT, @FnSubject INT;

SELECT @FnViolation = FunctionId FROM dbo.Sys_Functions
WHERE Name = 'SubjectViolation' AND Area = 'Major' AND ISNULL(IsDeleted, 0) = 0;

SELECT @FnSubject = FunctionId FROM dbo.Sys_Functions
WHERE Name = 'Subject' AND Area = 'Major' AND ISNULL(IsDeleted, 0) = 0;

IF @FnViolation IS NULL
BEGIN
    PRINT '>>> Chua co chuc nang Major/SubjectViolation. Hay chay script 09 truoc.';
    RETURN;
END

PRINT 'FunctionId SubjectViolation = ' + CAST(@FnViolation AS VARCHAR(20));
PRINT 'FunctionId Subject          = ' + ISNULL(CAST(@FnSubject AS VARCHAR(20)), 'NULL');

/* -----------------------------------------------------------------------------
   16.1. Cấp quyền theo đúng các vai trò đang có quyền trên Major/Subject
   ----------------------------------------------------------------------------- */
IF @FnSubject IS NOT NULL
BEGIN
    INSERT INTO dbo.Sys_Permissions (RoleId, FunctionId, Action)
    SELECT DISTINCT p.RoleId, @FnViolation, p.Action
    FROM dbo.Sys_Permissions AS p
    WHERE p.FunctionId = @FnSubject
      AND NOT EXISTS (SELECT 1 FROM dbo.Sys_Permissions AS e
                      WHERE e.RoleId = p.RoleId
                        AND e.FunctionId = @FnViolation
                        AND e.Action = p.Action);

    PRINT 'Da cap quyen SubjectViolation theo cac vai tro co quyen tren Subject.';
END

/* -----------------------------------------------------------------------------
   16.2. Neu van chua co vai tro nao, cap cho vai tro quan tri
   ----------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Sys_Permissions WHERE FunctionId = @FnViolation)
BEGIN
    INSERT INTO dbo.Sys_Permissions (RoleId, FunctionId, Action)
    SELECT r.RoleId, @FnViolation, a.Action
    FROM dbo.Sys_Roles AS r
    CROSS JOIN (SELECT 'View' AS Action UNION ALL SELECT 'Add'
                UNION ALL SELECT 'Edit' UNION ALL SELECT 'Delete') AS a
    WHERE (r.Name LIKE N'%quản trị%' OR r.Name LIKE '%dmin%')
      AND NOT EXISTS (SELECT 1 FROM dbo.Sys_Permissions AS e
                      WHERE e.RoleId = r.RoleId
                        AND e.FunctionId = @FnViolation
                        AND e.Action = a.Action);

    PRINT 'Da cap quyen SubjectViolation cho vai tro quan tri.';
END

/* -----------------------------------------------------------------------------
   16.3. Kiểm tra kết quả
   ----------------------------------------------------------------------------- */
SELECT
    r.RoleId,
    r.Name   AS TenVaiTro,
    f.Area,
    f.Name   AS ChucNang,
    p.Action
FROM dbo.Sys_Permissions AS p
INNER JOIN dbo.Sys_Functions AS f ON f.FunctionId = p.FunctionId
LEFT JOIN dbo.Sys_Roles AS r ON r.RoleId = p.RoleId
WHERE f.Name IN ('Subject', 'SubjectViolation') AND f.Area = 'Major'
ORDER BY f.Name, r.RoleId, p.Action;
GO
