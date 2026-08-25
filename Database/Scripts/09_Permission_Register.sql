/* =============================================================================
   09. ĐĂNG KÝ CHỨC NĂNG & PHÂN QUYỀN CHO MÀN HÌNH MỚI
   -----------------------------------------------------------------------------
   Nút "Thêm đối tượng" và các nút Thao tác chỉ hiển thị khi người dùng có quyền
   tương ứng. Cơ chế do khung TSFramework xử lý sẵn (Html.Button / _renderButton
   -> Sys_Permission_IsAllow), KHÔNG cần sửa mã nguồn.

   Việc cần làm là bảo đảm chức năng đã được đăng ký trong Sys_Functions /
   Sys_FunctionActions để màn hình Phân quyền có mục cho quản trị viên tích chọn.

   Tên cột thực tế trong CSDL (đã đối chiếu):
       Sys_Functions       : FunctionId, ModuleId, Area, Name, Description, IsDeleted
       Sys_FunctionActions : FunctionActionId, FunctionId, Action, IsDeleted
   ============================================================================= */

SET NOCOUNT ON;

/* -----------------------------------------------------------------------------
   9.1. Đăng ký chức năng "Phân quyền lĩnh vực" (Area = Cate, Name = UserField)
   ----------------------------------------------------------------------------- */
DECLARE @ModuleIdCate INT;

/* Lấy ModuleId của một chức năng đã có trong cùng Area để dùng lại */
SELECT TOP 1 @ModuleIdCate = ModuleId
FROM dbo.Sys_Functions
WHERE Area = 'Cate' AND ISNULL(IsDeleted, 0) = 0 AND ModuleId IS NOT NULL;

IF NOT EXISTS (SELECT 1 FROM dbo.Sys_Functions WHERE Name = 'UserField' AND Area = 'Cate')
BEGIN
    INSERT INTO dbo.Sys_Functions (ModuleId, Area, Name, Description, IsDeleted)
    VALUES (@ModuleIdCate, 'Cate', 'UserField', N'Phan quyen linh vuc cho nguoi dung', 0);

    PRINT 'Da dang ky chuc nang Cate/UserField.';
END
ELSE
BEGIN
    PRINT 'Chuc nang Cate/UserField da ton tai.';
END
GO


/* -----------------------------------------------------------------------------
   9.2. Đăng ký các hành động cho chức năng vừa tạo
   ----------------------------------------------------------------------------- */
DECLARE @FunctionIdUserField INT;

SELECT TOP 1 @FunctionIdUserField = FunctionId
FROM dbo.Sys_Functions
WHERE Name = 'UserField' AND Area = 'Cate';

IF @FunctionIdUserField IS NOT NULL
BEGIN
    IF NOT EXISTS (SELECT 1 FROM dbo.Sys_FunctionActions
                   WHERE FunctionId = @FunctionIdUserField AND Action = 'View')
        INSERT INTO dbo.Sys_FunctionActions (FunctionId, Action, IsDeleted)
        VALUES (@FunctionIdUserField, 'View', 0);

    IF NOT EXISTS (SELECT 1 FROM dbo.Sys_FunctionActions
                   WHERE FunctionId = @FunctionIdUserField AND Action = 'Edit')
        INSERT INTO dbo.Sys_FunctionActions (FunctionId, Action, IsDeleted)
        VALUES (@FunctionIdUserField, 'Edit', 0);

    PRINT 'Da dang ky cac hanh dong cho Cate/UserField.';
END
GO


/* -----------------------------------------------------------------------------
   9.3. Bảo đảm chức năng Major/Subject có đủ 4 hành động.
   Nếu thiếu hành động Add, nút "Thêm đối tượng" sẽ KHÔNG hiển thị cho bất kỳ ai
   vì không có mục nào để tích chọn khi phân quyền.
   ----------------------------------------------------------------------------- */
DECLARE @FunctionIdSubject INT;

SELECT TOP 1 @FunctionIdSubject = FunctionId
FROM dbo.Sys_Functions
WHERE Name = 'Subject' AND Area = 'Major';

IF @FunctionIdSubject IS NULL
BEGIN
    PRINT '>>> CANH BAO: chua co chuc nang Major/Subject trong Sys_Functions.';
END
ELSE
BEGIN
    DECLARE @Actions TABLE (ActionName VARCHAR(20));
    INSERT INTO @Actions (ActionName) VALUES ('View'), ('Add'), ('Edit'), ('Delete');

    INSERT INTO dbo.Sys_FunctionActions (FunctionId, Action, IsDeleted)
    SELECT @FunctionIdSubject, a.ActionName, 0
    FROM @Actions AS a
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Sys_FunctionActions AS fa
                      WHERE fa.FunctionId = @FunctionIdSubject AND fa.Action = a.ActionName);

    PRINT 'Da bo sung day du hanh dong cho Major/Subject.';
END
GO


/* -----------------------------------------------------------------------------
   9.4. Tương tự cho Major/SubjectViolation
   ----------------------------------------------------------------------------- */
DECLARE @FunctionIdViolation INT;

SELECT TOP 1 @FunctionIdViolation = FunctionId
FROM dbo.Sys_Functions
WHERE Name = 'SubjectViolation' AND Area = 'Major';

IF @FunctionIdViolation IS NOT NULL
BEGIN
    DECLARE @Actions2 TABLE (ActionName VARCHAR(20));
    INSERT INTO @Actions2 (ActionName) VALUES ('View'), ('Add'), ('Edit'), ('Delete');

    INSERT INTO dbo.Sys_FunctionActions (FunctionId, Action, IsDeleted)
    SELECT @FunctionIdViolation, a.ActionName, 0
    FROM @Actions2 AS a
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Sys_FunctionActions AS fa
                      WHERE fa.FunctionId = @FunctionIdViolation AND fa.Action = a.ActionName);

    PRINT 'Da bo sung day du hanh dong cho Major/SubjectViolation.';
END
GO


/* -----------------------------------------------------------------------------
   9.5. Kiểm tra lại kết quả
   ----------------------------------------------------------------------------- */
SELECT
    f.Area,
    f.Name        AS FunctionName,
    fa.Action     AS ActionName,
    f.FunctionId
FROM dbo.Sys_Functions AS f
LEFT JOIN dbo.Sys_FunctionActions AS fa
       ON fa.FunctionId = f.FunctionId AND ISNULL(fa.IsDeleted, 0) = 0
WHERE f.Name IN ('Subject', 'SubjectViolation', 'UserField', 'Field', 'ViolationBehavior')
  AND ISNULL(f.IsDeleted, 0) = 0
ORDER BY f.Area, f.Name, fa.Action;
GO
