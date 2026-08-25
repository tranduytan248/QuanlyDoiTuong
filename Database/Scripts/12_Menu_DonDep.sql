/* =============================================================================
   12. DỌN DẸP MENU SAU KHI SẮP XẾP LẠI
   -----------------------------------------------------------------------------
   Sau khi chạy script 08 phát hiện 3 vấn đề:
     1. "Hành vi vi phạm" bị trùng: MenuId=36 (bản gốc) và MenuId=1081 (bản mới).
     2. "Lĩnh vực" (1080) và "Hành vi vi phạm" (1081) có LevelMenu=1 trong khi
        đã nằm dưới menu cha "Danh mục" -> phải là 2.
     3. Cột IsShow đang NULL - nhưng TOÀN BỘ menu cũ đang chạy tốt cũng NULL,
        nên KHÔNG đụng tới cột này để tránh thay đổi hành vi hiện có.

   Script này idempotent: chạy lại nhiều lần vẫn cho kết quả như nhau.
   ============================================================================= */

SET NOCOUNT ON;

DECLARE @DanhMucId INT;
SELECT TOP 1 @DanhMucId = MenuId
FROM dbo.Sys_Menus
WHERE Name = N'Danh mục' AND (ParentId IS NULL OR ParentId = 0);

PRINT 'MenuId cua nhom "Danh muc" = ' + ISNULL(CAST(@DanhMucId AS VARCHAR(20)), 'NULL');

/* -----------------------------------------------------------------------------
   12.1. Gỡ bỏ mục trùng lặp
   Giữ lại bản ghi có MenuId NHỎ NHẤT cho mỗi đường dẫn (bản gốc của hệ thống),
   ẩn các bản ghi trùng còn lại.
   ----------------------------------------------------------------------------- */
;WITH Dup AS
(
    SELECT MenuId, Link,
           ROW_NUMBER() OVER (PARTITION BY Link ORDER BY MenuId) AS Rn
    FROM dbo.Sys_Menus
    WHERE Link IN ('/Cate/Field', '/Cate/ViolationBehavior', '/Cate/UserField',
                   '/Major/Subject', '/Major/SubjectViolation')
      AND ISNULL(IsDelete, 0) = 0
)
UPDATE m
SET m.IsDelete = 1
FROM dbo.Sys_Menus AS m
INNER JOIN Dup AS d ON d.MenuId = m.MenuId
WHERE d.Rn > 1;

PRINT 'Da an cac muc menu bi trung.';


/* -----------------------------------------------------------------------------
   12.2. Chuẩn hoá lại vị trí và cấp độ cho các mục còn lại
   ----------------------------------------------------------------------------- */

/* Lĩnh vực -> con cua Danh muc */
UPDATE dbo.Sys_Menus
SET ParentId = @DanhMucId, LevelMenu = 2, Position = 1,
    Icon = ISNULL(NULLIF(Icon, ''), 'fas fa-layer-group')
WHERE Link = '/Cate/Field' AND ISNULL(IsDelete, 0) = 0;

/* Hanh vi vi pham -> con cua Danh muc */
UPDATE dbo.Sys_Menus
SET ParentId = @DanhMucId, LevelMenu = 2, Position = 2,
    Icon = ISNULL(NULLIF(Icon, ''), 'fas fa-gavel')
WHERE Link = '/Cate/ViolationBehavior' AND ISNULL(IsDelete, 0) = 0;

/* Phan quyen linh vuc -> con cua Danh muc */
UPDATE dbo.Sys_Menus
SET ParentId = @DanhMucId, LevelMenu = 2, Position = 3,
    Icon = ISNULL(NULLIF(Icon, ''), 'fas fa-user-shield')
WHERE Link = '/Cate/UserField' AND ISNULL(IsDelete, 0) = 0;

/* Quan ly doi tuong -> menu goc */
UPDATE dbo.Sys_Menus
SET ParentId = NULL, LevelMenu = 1, Position = 1,
    Icon = ISNULL(NULLIF(Icon, ''), 'fas fa-users')
WHERE Link = '/Major/Subject' AND ISNULL(IsDelete, 0) = 0;

/* Lich su vi pham -> menu goc, ngay sau Quan ly doi tuong */
UPDATE dbo.Sys_Menus
SET ParentId = NULL, LevelMenu = 1, Position = 2,
    Icon = ISNULL(NULLIF(Icon, ''), 'fas fa-exclamation-triangle')
WHERE Link = '/Major/SubjectViolation' AND ISNULL(IsDelete, 0) = 0;

/* Bảo đảm nhóm "Danh mục" hiển thị */
UPDATE dbo.Sys_Menus
SET LevelMenu = 1
WHERE MenuId = @DanhMucId;

PRINT 'Da chuan hoa vi tri va cap do menu.';


/* -----------------------------------------------------------------------------
   12.3. Kiểm tra kết quả
   ----------------------------------------------------------------------------- */
SELECT
    m.MenuId,
    CASE WHEN m.ParentId IS NULL THEN m.Name ELSE N'    - ' + m.Name END AS MenuHienThi,
    p.Name AS MenuCha,
    m.Link,
    m.Position,
    m.LevelMenu,
    m.IsShow,
    m.IsDelete
FROM dbo.Sys_Menus AS m
LEFT JOIN dbo.Sys_Menus AS p ON p.MenuId = m.ParentId
WHERE m.Link IN ('/Cate/Field', '/Cate/ViolationBehavior', '/Cate/UserField',
                 '/Major/Subject', '/Major/SubjectViolation')
   OR m.MenuId = @DanhMucId
ORDER BY ISNULL(m.ParentId, m.MenuId), m.Position;
GO
