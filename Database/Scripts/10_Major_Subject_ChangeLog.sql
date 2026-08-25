/* =============================================================================
   10. BẢNG LOG CẬP NHẬT ĐỐI TƯỢNG & LỊCH SỬ VI PHẠM
   -----------------------------------------------------------------------------
   Ghi lại mọi thay đổi: thêm mới, cập nhật, xoá - đối với cả Đối tượng và
   Lịch sử vi phạm. Dùng cho màn hình "Log cập nhật" và phục vụ báo cáo về sau.

   Nguyên tắc: bảng chỉ ghi thêm (insert-only), không sửa, không xoá.

   Phạm vi xem log: theo phạm vi nhìn thấy ĐỐI TƯỢNG, không theo đơn vị của
   người thao tác. Nếu không như vậy, đơn vị chủ quản sẽ không thấy được việc
   một đơn vị khác đã sửa dữ liệu của mình.
   ============================================================================= */

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Major_Subject_ChangeLog')
BEGIN
    CREATE TABLE dbo.Major_Subject_ChangeLog
    (
        LogId             UNIQUEIDENTIFIER NOT NULL CONSTRAINT DF_Major_Subject_ChangeLog_LogId DEFAULT (NEWID()),
        SubjectId         UNIQUEIDENTIFIER NOT NULL,
        ViolationId       UNIQUEIDENTIFIER NULL,
        EntityType        VARCHAR(30)      NOT NULL,   -- SUBJECT | VIOLATION
        ActionType        VARCHAR(20)      NOT NULL,   -- ADD | UPDATE | DELETE
        ChangedFields     NVARCHAR(MAX)    NULL,       -- JSON: [{Field,Label,OldValue,NewValue}]
        ChangedFieldNames NVARCHAR(MAX)    NULL,       -- Danh sách nhãn, để hiển thị nhanh
        Description       NVARCHAR(500)    NULL,
        ActorUserName     NVARCHAR(100)    NULL,
        ActorName         NVARCHAR(200)    NULL,
        ActorPosition     NVARCHAR(200)    NULL,       -- Chức vụ
        ActorUnit         NVARCHAR(500)    NULL,       -- Đơn vị công tác
        ActorUnionId      UNIQUEIDENTIFIER NULL,
        CreatedDate       DATETIME         NOT NULL CONSTRAINT DF_Major_Subject_ChangeLog_CreatedDate DEFAULT (GETDATE()),
        CONSTRAINT PK_Major_Subject_ChangeLog PRIMARY KEY CLUSTERED (LogId)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Major_Subject_ChangeLog_SubjectId' AND object_id = OBJECT_ID('dbo.Major_Subject_ChangeLog'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Major_Subject_ChangeLog_SubjectId
        ON dbo.Major_Subject_ChangeLog (SubjectId, CreatedDate DESC);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Major_Subject_ChangeLog_ViolationId' AND object_id = OBJECT_ID('dbo.Major_Subject_ChangeLog'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Major_Subject_ChangeLog_ViolationId
        ON dbo.Major_Subject_ChangeLog (ViolationId) WHERE ViolationId IS NOT NULL;
END
GO


/* -----------------------------------------------------------------------------
   10.1. GHI MỘT DÒNG LOG
   Thứ tự tham số phải khớp đúng với MajorSubjectChangeLogBiz.Save (truyền theo vị trí).
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_Subject_ChangeLog_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_ChangeLog_Save;
GO

CREATE PROCEDURE dbo.p_Major_Subject_ChangeLog_Save
    @SubjectId         UNIQUEIDENTIFIER,
    @ViolationId       UNIQUEIDENTIFIER = NULL,
    @EntityType        VARCHAR(30),
    @ActionType        VARCHAR(20),
    @ChangedFields     NVARCHAR(MAX)    = NULL,
    @ChangedFieldNames NVARCHAR(MAX)    = NULL,
    @Description       NVARCHAR(500)    = NULL,
    @ActorUserName     NVARCHAR(100)    = NULL,
    @ActorName         NVARCHAR(200)    = NULL,
    @ActorPosition     NVARCHAR(200)    = NULL,
    @ActorUnit         NVARCHAR(500)    = NULL,
    @ActorUnionId      UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @LogId UNIQUEIDENTIFIER = NEWID();

    INSERT INTO dbo.Major_Subject_ChangeLog
    (
        LogId, SubjectId, ViolationId, EntityType, ActionType, ChangedFields,
        ChangedFieldNames, Description, ActorUserName, ActorName, ActorPosition,
        ActorUnit, ActorUnionId, CreatedDate
    )
    VALUES
    (
        @LogId, @SubjectId, @ViolationId, @EntityType, @ActionType, @ChangedFields,
        @ChangedFieldNames, @Description, @ActorUserName, @ActorName, @ActorPosition,
        @ActorUnit, @ActorUnionId, GETDATE()
    );

    SELECT CAST(@LogId AS NVARCHAR(50)) AS Result;
END
GO


/* -----------------------------------------------------------------------------
   10.2. DANH SÁCH LOG - CÓ PHÂN QUYỀN, SẮP XẾP MỚI NHẤT TRƯỚC
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_Subject_ChangeLog_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_ChangeLog_Get;
GO

CREATE PROCEDURE dbo.p_Major_Subject_ChangeLog_Get
    @SubjectId  UNIQUEIDENTIFIER = NULL,
    @EntityType VARCHAR(30)      = NULL,
    @UserName   NVARCHAR(100)    = NULL,
    @Search     NVARCHAR(500)    = NULL,
    @Order      NVARCHAR(10)     = '0',
    @OrderDir   NVARCHAR(10)     = 'DESC',
    @StartIndex INT              = 0,
    @PageSize   INT              = -1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);
    SET @Search = NULLIF(LTRIM(RTRIM(ISNULL(@Search, ''))), '');

    ;WITH ScopedLog AS
    (
        SELECT
            l.LogId,
            l.SubjectId,
            l.ViolationId,
            l.EntityType,
            l.ActionType,
            l.ChangedFields,
            l.ChangedFieldNames,
            l.Description,
            l.ActorUserName,
            l.ActorName,
            l.ActorPosition,
            l.ActorUnit,
            l.ActorUnionId,
            l.CreatedDate,
            s.FullName           AS SubjectName,
            s.IdentityCardNumber
        FROM dbo.Major_Subject_ChangeLog AS l
        INNER JOIN dbo.Major_Subjects AS s ON s.SubjectId = l.SubjectId
        WHERE (@SubjectId IS NULL OR l.SubjectId = @SubjectId)
          AND (@EntityType IS NULL OR l.EntityType = @EntityType)
          AND (@Search IS NULL
               OR l.ActorName LIKE N'%' + @Search + '%'
               OR l.Description LIKE N'%' + @Search + '%'
               OR l.ChangedFieldNames LIKE N'%' + @Search + '%')

          /* --- PHÂN QUYỀN: theo phạm vi nhìn thấy đối tượng --- */
          AND (@IsSuperAdmin = 1
               OR s.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))))
    ),
    Counted AS
    (
        SELECT sl.*, COUNT(*) OVER () AS TotalRow
        FROM ScopedLog AS sl
    )
    SELECT *
    FROM Counted
    ORDER BY
        CASE WHEN @OrderDir = 'ASC' THEN CreatedDate END ASC,
        CASE WHEN @OrderDir <> 'ASC' THEN CreatedDate END DESC
    OFFSET (CASE WHEN @StartIndex < 0 THEN 0 ELSE @StartIndex END) ROWS
    FETCH NEXT (CASE WHEN @PageSize IS NULL OR @PageSize <= 0 THEN 2147483647 ELSE @PageSize END) ROWS ONLY;
END
GO


/* -----------------------------------------------------------------------------
   10.3. XOÁ ĐỐI TƯỢNG - CHUYỂN SANG XOÁ MỀM (chỉ gán cờ IsDeleted)
   Dữ liệu vẫn giữ lại để phục vụ báo cáo. Dòng log được ghi từ tầng ứng dụng
   (nơi có đủ thông tin người thực hiện, chức vụ, đơn vị).
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_Subject_Delete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Delete;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Delete
    @SubjectId UNIQUEIDENTIFIER,
    @UserName  NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Major_Subjects WHERE SubjectId = @SubjectId)
    BEGIN
        SELECT CAST(0 AS BIT) AS Result;
        RETURN;
    END

    /* Chỉ gán cờ xoá. KHÔNG xoá lịch sử vi phạm đi kèm: các proc tra cứu vi phạm
       đều INNER JOIN sang Major_Subjects nên dữ liệu sẽ tự ẩn theo. */
    UPDATE dbo.Major_Subjects
    SET IsDeleted   = 1,
        UpdatedDate = GETDATE(),
        UpdatedBy   = @UserName
    WHERE SubjectId = @SubjectId;

    SELECT CAST(1 AS BIT) AS Result;
END
GO
