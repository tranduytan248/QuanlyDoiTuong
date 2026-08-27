/* =============================================================================
   25. DANG KY CHUC NANG VA PHAN QUYEN CHO MAN HINH GIAM SAT TRUC TUYEN
   -----------------------------------------------------------------------------
   Dang ky man hinh vao he thong phan quyen va them vao menu.

   Mac dinh CHI cap quyen cho vai tro "Quan tri he thong": day la man hinh theo
   doi hoat dong nguoi dung nen khong nen mo rong cho vai tro khac.

   Menu dat canh muc "Nguoi dung" (MenuId 3) trong nhom "He thong" (MenuId 1),
   dung dung cau truc Depth dang "cha,con" ma bang Sys_Menus dang su dung.
   ============================================================================= */

SET NOCOUNT ON;
BEGIN TRANSACTION;
BEGIN TRY

DECLARE @ModuleId INT, @FunctionId INT, @ActionId INT;

/* ------------------------------------------------ 1. Chuc nang trong Sys */
SELECT TOP 1 @ModuleId = ModuleId FROM dbo.Sys_Modules WHERE ModuleName = 'Sys';
IF @ModuleId IS NULL THROW 50001, N'Khong tim thay module Sys.', 1;

SELECT @FunctionId = FunctionId FROM dbo.Sys_Functions
 WHERE Area = 'Sys' AND Name = 'UserActivity';

IF @FunctionId IS NULL
BEGIN
    INSERT INTO dbo.Sys_Functions (ModuleId, Area, Name, Description, IsDeleted)
    VALUES (@ModuleId, 'Sys', 'UserActivity', N'Giám sát trực tuyến', 0);
    SET @FunctionId = SCOPE_IDENTITY();
END
ELSE
    UPDATE dbo.Sys_Functions SET IsDeleted = 0 WHERE FunctionId = @FunctionId;

/* Man hinh chi doc nen chi can quyen View */
IF NOT EXISTS (SELECT 1 FROM dbo.Sys_FunctionActions
                WHERE FunctionId = @FunctionId AND [Action] = 'View')
    INSERT INTO dbo.Sys_FunctionActions (FunctionId, [Action], IsDeleted)
    VALUES (@FunctionId, 'View', 0);

SELECT @ActionId = FunctionActionId FROM dbo.Sys_FunctionActions
 WHERE FunctionId = @FunctionId AND [Action] = 'View';

/* --------------------------------- 2. Cap quyen cho Quan tri he thong */
DECLARE @RoleId INT;
SELECT TOP 1 @RoleId = RoleId FROM dbo.Sys_Roles
 WHERE Name = N'Quản trị hệ thống' AND ISNULL(IsDeleted, 0) = 0;

/* Ten vai tro trong CSDL co dau - tim lai theo ma khong dau khong ra thi
   doi chieu bang cach bo dau khong kha thi trong T-SQL, nen tim truc tiep. */
IF @RoleId IS NULL
    SELECT TOP 1 @RoleId = RoleId FROM dbo.Sys_Roles
     WHERE RoleId = 1 AND ISNULL(IsDeleted, 0) = 0;

IF @RoleId IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.Sys_Permissions
                    WHERE RoleId = @RoleId AND FunctionId = @FunctionId AND [Action] = 'View')
    INSERT INTO dbo.Sys_Permissions (RoleId, FunctionId, [Action])
    VALUES (@RoleId, @FunctionId, 'View');

/* ------------------------------------------------------- 3. Menu */
DECLARE @ParentId INT = 1;          /* Nhom "He thong" */
DECLARE @Position INT;

/* Dat ngay sau muc "Nguoi dung" */
SELECT @Position = ISNULL(Position, 5) FROM dbo.Sys_Menus WHERE MenuId = 3;
SET @Position = ISNULL(@Position, 5);

IF NOT EXISTS (SELECT 1 FROM dbo.Sys_Menus WHERE Link = '/Sys/UserActivity')
BEGIN
    INSERT INTO dbo.Sys_Menus
        (Name, Position, LevelMenu, Depth, ParentId, Link, Icon, FunctionActionId, IsShow, UseModal, IsDelete)
    VALUES
        (N'Giám sát trực tuyến', @Position, 2,
         CAST(@ParentId AS VARCHAR(10)),
         @ParentId, '/Sys/UserActivity', 'fas fa-satellite-dish', @ActionId, 1, 0, 0);

    /* Depth phai chua chinh MenuId vua sinh nen chi dat duoc sau khi INSERT */
    DECLARE @NewId INT = SCOPE_IDENTITY();
    UPDATE dbo.Sys_Menus
       SET Depth = CAST(@ParentId AS VARCHAR(10)) + ',' + CAST(@NewId AS VARCHAR(10))
     WHERE MenuId = @NewId;
END
ELSE
    UPDATE dbo.Sys_Menus
       SET IsDelete = 0, IsShow = 1, FunctionActionId = @ActionId,
           Name = N'Giám sát trực tuyến'
     WHERE Link = '/Sys/UserActivity';

COMMIT TRANSACTION;
PRINT N'Hoan tat: da dang ky chuc nang, cap quyen Quan tri he thong va them menu.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT N'LOI - da huy toan bo thay doi:';
    PRINT ERROR_MESSAGE();
    THROW;
END CATCH
