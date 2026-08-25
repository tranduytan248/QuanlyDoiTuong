/* =============================================================================
   08. SẮP XẾP LẠI MENU
   -----------------------------------------------------------------------------
   Yêu cầu:
     - "Lĩnh vực" và "Hành vi vi phạm" chuyển vào nhóm menu "Danh mục".
     - "Quản lý đối tượng" đưa ra ngoài, nằm ở cấp menu gốc.
     - Bổ sung menu "Phân quyền lĩnh vực" (màn hình mới) vào nhóm "Danh mục".

   Script viết theo hướng idempotent: chạy lại nhiều lần vẫn cho kết quả như nhau.
   Nếu cấu trúc bảng Sys_Menus ở môi trường của bạn khác, hãy đối chiếu lại tên cột
   trước khi chạy.
   ============================================================================= */

SET NOCOUNT ON;

/* -----------------------------------------------------------------------------
   Bước 1: Xác định menu cha "Danh mục".
   Nếu chưa có thì tạo mới ở cấp gốc.
   ----------------------------------------------------------------------------- */
DECLARE @DanhMucId INT;

SELECT TOP 1 @DanhMucId = MenuId
FROM dbo.Sys_Menus
WHERE (Name = N'Danh mục' OR Name = N'Danh mục hệ thống')
  AND (ParentId IS NULL OR ParentId = 0);

IF @DanhMucId IS NULL
BEGIN
    DECLARE @MaxRootPos INT;
    SELECT @MaxRootPos = ISNULL(MAX(Position), 0) FROM dbo.Sys_Menus WHERE ParentId IS NULL OR ParentId = 0;

    INSERT INTO dbo.Sys_Menus (ParentId, Name, Position, LevelMenu, Link, Icon, IsShow, UseModal)
    VALUES (NULL, N'Danh mục', @MaxRootPos + 1, 1, '#', 'fas fa-list', 1, 0);

    SET @DanhMucId = SCOPE_IDENTITY();
    PRINT N'Đã tạo menu cha "Danh mục", MenuId = ' + CAST(@DanhMucId AS NVARCHAR(20));
END
ELSE
BEGIN
    PRINT N'Menu cha "Danh mục" đã có, MenuId = ' + CAST(@DanhMucId AS NVARCHAR(20));
END


/* -----------------------------------------------------------------------------
   Bước 2: Chuyển "Lĩnh vực" và "Hành vi vi phạm" vào nhóm "Danh mục".
   Nhận diện menu theo Link để không phụ thuộc vào cách đặt tên.
   ----------------------------------------------------------------------------- */
UPDATE dbo.Sys_Menus
SET ParentId  = @DanhMucId,
    LevelMenu = 2,
    Position  = 1,
    Icon      = ISNULL(NULLIF(Icon, ''), 'fas fa-layer-group')
WHERE Link LIKE '%/Cate/Field%'
  AND (ParentId IS NULL OR ParentId <> @DanhMucId);

UPDATE dbo.Sys_Menus
SET ParentId  = @DanhMucId,
    LevelMenu = 2,
    Position  = 2,
    Icon      = ISNULL(NULLIF(Icon, ''), 'fas fa-gavel')
WHERE Link LIKE '%/Cate/ViolationBehavior%'
  AND (ParentId IS NULL OR ParentId <> @DanhMucId);

PRINT N'Đã chuyển "Lĩnh vực" và "Hành vi vi phạm" vào nhóm "Danh mục".';


/* -----------------------------------------------------------------------------
   Bước 3: Thêm menu "Phân quyền lĩnh vực" vào nhóm "Danh mục".
   ----------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Sys_Menus WHERE Link LIKE '%/Cate/UserField%')
BEGIN
    INSERT INTO dbo.Sys_Menus (ParentId, Name, Position, LevelMenu, Link, Icon, IsShow, UseModal)
    VALUES (@DanhMucId, N'Phân quyền lĩnh vực', 3, 2, '/Cate/UserField', 'fas fa-user-shield', 1, 0);

    PRINT N'Đã thêm menu "Phân quyền lĩnh vực".';
END
ELSE
BEGIN
    UPDATE dbo.Sys_Menus
    SET ParentId = @DanhMucId, LevelMenu = 2, Position = 3
    WHERE Link LIKE '%/Cate/UserField%';

    PRINT N'Menu "Phân quyền lĩnh vực" đã có, chỉ cập nhật lại vị trí.';
END


/* -----------------------------------------------------------------------------
   Bước 4: Đưa "Quản lý đối tượng" ra menu cấp gốc.
   ----------------------------------------------------------------------------- */
IF EXISTS (SELECT 1 FROM dbo.Sys_Menus WHERE Link LIKE '%/Major/Subject%' AND Link NOT LIKE '%SubjectViolation%')
BEGIN
    UPDATE dbo.Sys_Menus
    SET ParentId  = NULL,
        LevelMenu = 1,
        Position  = 1,
        Icon      = ISNULL(NULLIF(Icon, ''), 'fas fa-users'),
        IsShow    = 1
    WHERE Link LIKE '%/Major/Subject%'
      AND Link NOT LIKE '%SubjectViolation%';

    PRINT N'Đã đưa "Quản lý đối tượng" ra menu cấp gốc.';
END
ELSE
BEGIN
    INSERT INTO dbo.Sys_Menus (ParentId, Name, Position, LevelMenu, Link, Icon, IsShow, UseModal)
    VALUES (NULL, N'Quản lý đối tượng', 1, 1, '/Major/Subject', 'fas fa-users', 1, 0);

    PRINT N'Đã tạo mới menu "Quản lý đối tượng" ở cấp gốc.';
END


/* -----------------------------------------------------------------------------
   Bước 5: Kiểm tra lại kết quả.
   ----------------------------------------------------------------------------- */
PRINT '';
PRINT N'--- Cấu trúc menu sau khi sắp xếp ---';

SELECT
    m.MenuId,
    CASE WHEN m.ParentId IS NULL THEN m.Name
         ELSE N'    └─ ' + m.Name END AS MenuHienThi,
    p.Name AS MenuCha,
    m.Link,
    m.Position,
    m.LevelMenu,
    m.IsShow
FROM dbo.Sys_Menus AS m
LEFT JOIN dbo.Sys_Menus AS p ON p.MenuId = m.ParentId
WHERE m.Link LIKE '%/Cate/Field%'
   OR m.Link LIKE '%/Cate/ViolationBehavior%'
   OR m.Link LIKE '%/Cate/UserField%'
   OR m.Link LIKE '%/Major/Subject%'
   OR m.MenuId = @DanhMucId
ORDER BY ISNULL(m.ParentId, m.MenuId), m.Position;
GO
