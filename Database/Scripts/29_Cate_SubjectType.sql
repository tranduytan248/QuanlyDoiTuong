/* =============================================================================
   29. DANH MỤC LOẠI ĐỐI TƯỢNG (Cate_SubjectTypes)
   -----------------------------------------------------------------------------
   - Bảng Cate_SubjectTypes
   - Stored procedures CRUD: Get, GetById, GetAll, Save, Delete, ToggleStatus
   - Đăng ký Sys_Messages
   - Đăng ký Sys_Functions, Sys_FunctionActions
   - Phân quyền cho nhóm Quản trị hệ thống
   - Đăng ký Sys_Menus dưới menu "Danh mục"
   ============================================================================= */

SET NOCOUNT ON;

/* -----------------------------------------------------------------------------
   29.1. Tạo bảng Cate_SubjectTypes nếu chưa tồn tại
   ----------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Cate_SubjectTypes' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    CREATE TABLE dbo.Cate_SubjectTypes
    (
        SubjectTypeId   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Cate_SubjectTypes PRIMARY KEY,
        SubjectTypeCode NVARCHAR(50)      NOT NULL,
        SubjectTypeName NVARCHAR(250)     NOT NULL,
        Description     NVARCHAR(500)     NULL,
        SortOrder       INT               NOT NULL CONSTRAINT DF_Cate_SubjectTypes_SortOrder DEFAULT (0),
        IsActive        BIT               NOT NULL CONSTRAINT DF_Cate_SubjectTypes_IsActive DEFAULT (1),
        IsDeleted       BIT               NOT NULL CONSTRAINT DF_Cate_SubjectTypes_IsDeleted DEFAULT (0),
        CreatedDate     DATETIME          NOT NULL CONSTRAINT DF_Cate_SubjectTypes_CreatedDate DEFAULT (GETDATE()),
        CreatedBy       NVARCHAR(100)     NULL,
        UpdatedDate     DATETIME          NULL,
        UpdatedBy       NVARCHAR(100)     NULL
    );

    CREATE UNIQUE NONCLUSTERED INDEX UX_Cate_SubjectTypes_Code
    ON dbo.Cate_SubjectTypes (SubjectTypeCode)
    WHERE IsDeleted = 0;

    PRINT 'Da tao bang dbo.Cate_SubjectTypes.';
END
ELSE
BEGIN
    PRINT 'Bang dbo.Cate_SubjectTypes da ton tai.';
END
GO

/* -----------------------------------------------------------------------------
   29.2. Chèn dữ liệu mẫu ban đầu
   ----------------------------------------------------------------------------- */
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_SubjectTypes WHERE IsDeleted = 0)
BEGIN
    INSERT INTO dbo.Cate_SubjectTypes (SubjectTypeCode, SubjectTypeName, Description, SortOrder, IsActive, IsDeleted, CreatedBy)
    VALUES
        (N'SDMT',     N'Sử dụng ma túy',                  N'Đối tượng có hành vi sử dụng trái phép chất ma túy', 1, 1, 0, N'system'),
        (N'MBD',      N'Mua bán dâm',                     N'Đối tượng tham gia hoạt động mua bán dâm', 2, 1, 0, N'system'),
        (N'TROM_CAP', N'Trộm cắp tài sản',                N'Đối tượng có tiền án, tiền sự hoặc biểu hiện trộm cắp', 3, 1, 0, N'system'),
        (N'CO_BAC',   N'Cờ bạc, cá độ',                   N'Đối tượng tham gia tổ chức đánh bạc, cá độ', 4, 1, 0, N'system'),
        (N'GAY_ROI',  N'Gây rối trật tự công cộng',       N'Đối tượng càn quấy, gây rối trật tự', 5, 1, 0, N'system'),
        (N'BUON_LAU', N'Buôn lậu, gian lận thương mại',   N'Đối tượng có dấu hiệu buôn lậu, hàng giả', 6, 1, 0, N'system'),
        (N'KHAC',     N'Loại đối tượng khác',             N'Các loại đối tượng quản lý khác', 99, 1, 0, N'system');

    PRINT 'Da chen du lieu mau cho Cate_SubjectTypes.';
END
GO

/* -----------------------------------------------------------------------------
   29.3. Stored Procedure: p_Cate_SubjectType_Get
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_SubjectType_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_SubjectType_Get;
GO

CREATE PROCEDURE dbo.p_Cate_SubjectType_Get
    @Key NVARCHAR(250) = NULL,
    @Search NVARCHAR(250) = NULL,
    @Order NVARCHAR(50) = '0',
    @OrderDir NVARCHAR(10) = 'ASC',
    @StartIndex INT = 0,
    @PageSize INT = -1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Key = NULLIF(LTRIM(RTRIM(@Key)), '');
    SET @Search = NULLIF(LTRIM(RTRIM(@Search)), '');

    DECLARE @Keyword NVARCHAR(250) = COALESCE(@Key, @Search);

    ;WITH DataCTE AS
    (
        SELECT
            st.SubjectTypeId,
            st.SubjectTypeCode,
            st.SubjectTypeName,
            st.Description,
            st.SortOrder,
            st.IsActive,
            st.IsDeleted,
            st.CreatedDate,
            st.CreatedBy,
            st.UpdatedDate,
            st.UpdatedBy
        FROM dbo.Cate_SubjectTypes st
        WHERE st.IsDeleted = 0
          AND (
              @Keyword IS NULL
              OR st.SubjectTypeCode LIKE N'%' + @Keyword + N'%'
              OR st.SubjectTypeName LIKE N'%' + @Keyword + N'%'
              OR st.Description LIKE N'%' + @Keyword + N'%'
          )
    ),
    CountCTE AS
    (
        SELECT COUNT(1) AS TotalRow FROM DataCTE
    )
    SELECT
        d.*,
        c.TotalRow
    FROM DataCTE d
    CROSS JOIN CountCTE c
    ORDER BY
        CASE WHEN @Order = '1' AND UPPER(@OrderDir) = 'ASC'  THEN d.SubjectTypeCode END ASC,
        CASE WHEN @Order = '1' AND UPPER(@OrderDir) = 'DESC' THEN d.SubjectTypeCode END DESC,
        CASE WHEN @Order = '2' AND UPPER(@OrderDir) = 'ASC'  THEN d.SubjectTypeName END ASC,
        CASE WHEN @Order = '2' AND UPPER(@OrderDir) = 'DESC' THEN d.SubjectTypeName END DESC,
        CASE WHEN @Order = '3' AND UPPER(@OrderDir) = 'ASC'  THEN d.Description END ASC,
        CASE WHEN @Order = '3' AND UPPER(@OrderDir) = 'DESC' THEN d.Description END DESC,
        CASE WHEN @Order = '4' AND UPPER(@OrderDir) = 'ASC'  THEN d.SortOrder END ASC,
        CASE WHEN @Order = '4' AND UPPER(@OrderDir) = 'DESC' THEN d.SortOrder END DESC,
        d.SortOrder ASC,
        d.SubjectTypeId DESC
    OFFSET CASE WHEN @PageSize > 0 THEN @StartIndex ELSE 0 END ROWS
    FETCH NEXT CASE WHEN @PageSize > 0 THEN @PageSize ELSE 1000000 END ROWS ONLY;
END
GO

/* -----------------------------------------------------------------------------
   29.4. Stored Procedure: p_Cate_SubjectType_GetById
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_SubjectType_GetById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_SubjectType_GetById;
GO

CREATE PROCEDURE dbo.p_Cate_SubjectType_GetById
    @SubjectTypeId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        st.SubjectTypeId,
        st.SubjectTypeCode,
        st.SubjectTypeName,
        st.Description,
        st.SortOrder,
        st.IsActive,
        st.IsDeleted,
        st.CreatedDate,
        st.CreatedBy,
        st.UpdatedDate,
        st.UpdatedBy
    FROM dbo.Cate_SubjectTypes st
    WHERE st.SubjectTypeId = @SubjectTypeId
      AND st.IsDeleted = 0;
END
GO

/* -----------------------------------------------------------------------------
   29.5. Stored Procedure: p_Cate_SubjectType_GetAll
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_SubjectType_GetAll', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_SubjectType_GetAll;
GO

CREATE PROCEDURE dbo.p_Cate_SubjectType_GetAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        st.SubjectTypeId,
        st.SubjectTypeCode,
        st.SubjectTypeName,
        st.Description,
        st.SortOrder,
        st.IsActive,
        st.IsDeleted,
        st.CreatedDate,
        st.CreatedBy,
        st.UpdatedDate,
        st.UpdatedBy
    FROM dbo.Cate_SubjectTypes st
    WHERE st.IsDeleted = 0
      AND st.IsActive = 1
    ORDER BY st.SortOrder ASC, st.SubjectTypeName ASC;
END
GO

/* -----------------------------------------------------------------------------
   29.6. Stored Procedure: p_Cate_SubjectType_Save
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_SubjectType_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_SubjectType_Save;
GO

CREATE PROCEDURE dbo.p_Cate_SubjectType_Save
    @SubjectTypeId INT = 0,
    @SubjectTypeCode NVARCHAR(50),
    @SubjectTypeName NVARCHAR(250),
    @Description NVARCHAR(500) = NULL,
    @SortOrder INT = 0,
    @IsActive BIT = 1,
    @UserName NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @SubjectTypeCode = LTRIM(RTRIM(@SubjectTypeCode));
    SET @SubjectTypeName = LTRIM(RTRIM(@SubjectTypeName));
    SET @Description = LTRIM(RTRIM(@Description));

    -- Kiểm tra trùng mã (trừ bản ghi hiện tại)
    IF EXISTS (
        SELECT 1 FROM dbo.Cate_SubjectTypes
        WHERE SubjectTypeCode = @SubjectTypeCode
          AND SubjectTypeId != @SubjectTypeId
          AND IsDeleted = 0
    )
    BEGIN
        SELECT -2; -- EnumStatus.Existed
        RETURN;
    END

    IF @SubjectTypeId > 0
    BEGIN
        UPDATE dbo.Cate_SubjectTypes
        SET
            SubjectTypeCode = @SubjectTypeCode,
            SubjectTypeName = @SubjectTypeName,
            Description = @Description,
            SortOrder = @SortOrder,
            IsActive = @IsActive,
            UpdatedDate = GETDATE(),
            UpdatedBy = @UserName
        WHERE SubjectTypeId = @SubjectTypeId
          AND IsDeleted = 0;

        SELECT @SubjectTypeId;
    END
    ELSE
    BEGIN
        INSERT INTO dbo.Cate_SubjectTypes
        (
            SubjectTypeCode,
            SubjectTypeName,
            Description,
            SortOrder,
            IsActive,
            IsDeleted,
            CreatedDate,
            CreatedBy
        )
        VALUES
        (
            @SubjectTypeCode,
            @SubjectTypeName,
            @Description,
            @SortOrder,
            @IsActive,
            0,
            GETDATE(),
            @UserName
        );

        SELECT SCOPE_IDENTITY();
    END
END
GO

/* -----------------------------------------------------------------------------
   29.7. Stored Procedure: p_Cate_SubjectType_Delete
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_SubjectType_Delete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_SubjectType_Delete;
GO

CREATE PROCEDURE dbo.p_Cate_SubjectType_Delete
    @SubjectTypeId INT,
    @UserName NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Cate_SubjectTypes
    SET
        IsDeleted = 1,
        UpdatedDate = GETDATE(),
        UpdatedBy = @UserName
    WHERE SubjectTypeId = @SubjectTypeId;

    SELECT 1;
END
GO

/* -----------------------------------------------------------------------------
   29.8. Stored Procedure: p_Cate_SubjectType_ToggleStatus
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_SubjectType_ToggleStatus', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_SubjectType_ToggleStatus;
GO

CREATE PROCEDURE dbo.p_Cate_SubjectType_ToggleStatus
    @SubjectTypeId INT,
    @UserName NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Cate_SubjectTypes
    SET
        IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END,
        UpdatedDate = GETDATE(),
        UpdatedBy = @UserName
    WHERE SubjectTypeId = @SubjectTypeId
      AND IsDeleted = 0;

    SELECT 1;
END
GO

/* -----------------------------------------------------------------------------
   29.9. Đăng ký nhãn vào Sys_Messages
   ----------------------------------------------------------------------------- */
DECLARE @Labels TABLE (LabelKey VARCHAR(200), Message NVARCHAR(500));

INSERT INTO @Labels (LabelKey, Message) VALUES
    ('SubjectType_Title',                  N'Loại đối tượng'),
    ('SubjectType_Label_Code',             N'Mã loại đối tượng'),
    ('SubjectType_Label_Name',             N'Tên loại đối tượng'),
    ('SubjectType_Label_Description',      N'Mô tả / Ghi chú'),
    ('SubjectType_Label_SortOrder',        N'Thứ tự sắp xếp'),
    ('SubjectType_Label_IsActive',         N'Trạng thái hoạt động');

INSERT INTO dbo.Sys_Messages (LangCode, LabelKey, Message)
SELECT 'vi', l.LabelKey, l.Message
FROM @Labels AS l
WHERE NOT EXISTS (
    SELECT 1 FROM dbo.Sys_Messages AS m
    WHERE m.LabelKey = l.LabelKey AND m.LangCode = 'vi'
);

PRINT 'Da dang ky nhan cho SubjectType vao Sys_Messages.';
GO

/* -----------------------------------------------------------------------------
   29.10. Đăng ký Sys_Functions, Sys_FunctionActions & Phân quyền
   ----------------------------------------------------------------------------- */
DECLARE @ModuleIdCate INT;
SELECT TOP 1 @ModuleIdCate = ModuleId
FROM dbo.Sys_Functions
WHERE Area = 'Cate' AND ISNULL(IsDeleted, 0) = 0 AND ModuleId IS NOT NULL;

IF NOT EXISTS (SELECT 1 FROM dbo.Sys_Functions WHERE Name = 'SubjectType' AND Area = 'Cate')
BEGIN
    INSERT INTO dbo.Sys_Functions (ModuleId, Area, Name, Description, IsDeleted)
    VALUES (@ModuleIdCate, 'Cate', 'SubjectType', N'Quản lý danh mục Loại đối tượng', 0);
    PRINT 'Da dang ky chuc nang Cate/SubjectType.';
END

DECLARE @FunctionIdSubjectType INT;
SELECT TOP 1 @FunctionIdSubjectType = FunctionId
FROM dbo.Sys_Functions
WHERE Name = 'SubjectType' AND Area = 'Cate';

IF @FunctionIdSubjectType IS NOT NULL
BEGIN
    DECLARE @Actions TABLE (Action VARCHAR(50));
    INSERT INTO @Actions VALUES ('View'), ('Add'), ('Edit'), ('Delete'), ('ToggleStatus');

    INSERT INTO dbo.Sys_FunctionActions (FunctionId, Action, IsDeleted)
    SELECT @FunctionIdSubjectType, a.Action, 0
    FROM @Actions a
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.Sys_FunctionActions fa
        WHERE fa.FunctionId = @FunctionIdSubjectType AND fa.Action = a.Action
    );

    -- Cấp quyền toàn bộ cho RoleId = 1 (Quản trị hệ thống)
    INSERT INTO dbo.Sys_Permissions (RoleId, FunctionId, Action)
    SELECT 1, fa.FunctionId, fa.Action
    FROM dbo.Sys_FunctionActions fa
    WHERE fa.FunctionId = @FunctionIdSubjectType AND ISNULL(fa.IsDeleted, 0) = 0
      AND NOT EXISTS (
          SELECT 1 FROM dbo.Sys_Permissions p
          WHERE p.RoleId = 1 AND p.FunctionId = fa.FunctionId AND p.Action = fa.Action
      );

    -- Cấp quyền View, Add, Edit cho RoleId = 19 (Cán bộ nghiệp vụ)
    INSERT INTO dbo.Sys_Permissions (RoleId, FunctionId, Action)
    SELECT 19, fa.FunctionId, fa.Action
    FROM dbo.Sys_FunctionActions fa
    WHERE fa.FunctionId = @FunctionIdSubjectType AND ISNULL(fa.IsDeleted, 0) = 0
      AND fa.Action IN ('View', 'Add', 'Edit')
      AND NOT EXISTS (
          SELECT 1 FROM dbo.Sys_Permissions p
          WHERE p.RoleId = 19 AND p.FunctionId = fa.FunctionId AND p.Action = fa.Action
      );

    PRINT 'Da cap quyen Cate/SubjectType cho Quan tri he thong va Can bo nghiep vu.';
END
GO

/* -----------------------------------------------------------------------------
   29.11. Đăng ký Menu "Loại đối tượng" vào Menu "Danh mục" (ParentId = 16)
   ----------------------------------------------------------------------------- */
DECLARE @ParentMenuId INT = 16;
DECLARE @ViewFaId INT;
SELECT TOP 1 @ViewFaId = fa.FunctionActionId
FROM dbo.Sys_FunctionActions fa
INNER JOIN dbo.Sys_Functions f ON fa.FunctionId = f.FunctionId
WHERE f.Name = 'SubjectType' AND fa.Action = 'View';

IF NOT EXISTS (SELECT 1 FROM dbo.Sys_Menus WHERE Link = '/Cate/SubjectType' AND ISNULL(IsDelete, 0) = 0)
BEGIN
    INSERT INTO dbo.Sys_Menus (Name, Link, ParentId, LevelMenu, Position, Icon, FunctionActionId, Depth, IsShow, IsDelete)
    VALUES (N'Loại đối tượng', '/Cate/SubjectType', @ParentMenuId, 2, 3, 'fas fa-tags', ISNULL(@ViewFaId, 0), '16', 1, 0);

    UPDATE Sys_Menus
    SET Depth = '16,' + CAST(MenuId AS VARCHAR(50))
    WHERE Link = '/Cate/SubjectType';

    PRINT 'Da them menu Loai doi tuong.';
END
ELSE
BEGIN
    UPDATE dbo.Sys_Menus
    SET Name = N'Loại đối tượng',
        ParentId = @ParentMenuId,
        LevelMenu = 2,
        Position = 3,
        Icon = 'fas fa-tags',
        FunctionActionId = ISNULL(@ViewFaId, 0),
        Depth = '16,' + CAST(MenuId AS VARCHAR(50)),
        IsShow = 1,
        IsDelete = 0
    WHERE Link = '/Cate/SubjectType';
    PRINT 'Da cap nhat menu Loai doi tuong.';
END
GO
