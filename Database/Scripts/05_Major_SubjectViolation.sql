/* =============================================================================
   05. LỊCH SỬ VI PHẠM - LƯU NGƯỜI KHAI BÁO & PHÂN QUYỀN DỮ LIỆU
   -----------------------------------------------------------------------------
   Gồm 3 proc:
     p_Major_SubjectViolation_Save         : bổ sung 5 tham số người khai báo.
     p_Major_SubjectViolation_Get          : danh sách vi phạm có phân quyền.
     p_Major_SubjectViolation_GetBySubjectId: lịch sử của 1 đối tượng, có phân quyền.

   Quy tắc: người dùng chỉ thấy lần vi phạm do đơn vị thuộc phạm vi mình khai báo,
   VÀ lần vi phạm đó phải thuộc lĩnh vực mình được phân quản lý.

   Phạm vi đơn vị là ĐỆ QUY THEO CÂY (xem fn_GetPermittedUnions ở script 03):
   nếu Đơn vị 1 là cha của Đơn vị 2, Đơn vị 2 là cha của 3, 4, 5 thì người được
   phân quản lý Đơn vị 1 sẽ thấy dữ liệu của cả 2, 3, 4 và 5.
   ============================================================================= */

/* -----------------------------------------------------------------------------
   5.1. LƯU LỊCH SỬ VI PHẠM
   Tham số phải giữ ĐÚNG THỨ TỰ vì tầng C# truyền theo vị trí (positional).
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_SubjectViolation_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_SubjectViolation_Save;
GO

CREATE PROCEDURE dbo.p_Major_SubjectViolation_Save
    @ViolationId       UNIQUEIDENTIFIER = NULL,
    @SubjectId         UNIQUEIDENTIFIER,
    @ViolationDate     DATETIME,
    @TreatmentMeasures NVARCHAR(MAX)    = NULL,
    @RelatedDocuments  NVARCHAR(MAX)    = NULL,
    @Images            NVARCHAR(MAX)    = NULL,
    @Notes             NVARCHAR(MAX)    = NULL,
    /* --- Thông tin người khai báo: không hiển thị trên giao diện, chỉ lưu xuống --- */
    @ReporterName      NVARCHAR(200)    = NULL,
    @ReporterUnit      NVARCHAR(500)    = NULL,
    @ReporterPosition  NVARCHAR(200)    = NULL,
    @ReporterPhone     NVARCHAR(50)     = NULL,
    @ReporterUnionId   UNIQUEIDENTIFIER = NULL,
    @UserName          NVARCHAR(100)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @ViolationId IS NULL OR @ViolationId = '00000000-0000-0000-0000-000000000000'
    BEGIN
        SET @ViolationId = NEWID();

        INSERT INTO dbo.Major_SubjectViolations
        (
            ViolationId, SubjectId, ViolationDate, TreatmentMeasures, RelatedDocuments,
            Images, Notes, ReporterName, ReporterUnit, ReporterPosition, ReporterPhone,
            ReporterUnionId, IsDeleted, CreatedDate, CreatedBy
        )
        VALUES
        (
            @ViolationId, @SubjectId, @ViolationDate, @TreatmentMeasures, @RelatedDocuments,
            @Images, @Notes, @ReporterName, @ReporterUnit, @ReporterPosition, @ReporterPhone,
            @ReporterUnionId, 0, GETDATE(), @UserName
        );
    END
    ELSE
    BEGIN
        UPDATE dbo.Major_SubjectViolations
        SET SubjectId         = @SubjectId,
            ViolationDate     = @ViolationDate,
            TreatmentMeasures = @TreatmentMeasures,
            RelatedDocuments  = @RelatedDocuments,
            Images            = @Images,
            Notes             = @Notes,
            /* Chỉ ghi đè thông tin khai báo khi có truyền vào, tránh xoá mất dữ liệu cũ */
            ReporterName      = ISNULL(@ReporterName,     ReporterName),
            ReporterUnit      = ISNULL(@ReporterUnit,     ReporterUnit),
            ReporterPosition  = ISNULL(@ReporterPosition, ReporterPosition),
            ReporterPhone     = ISNULL(@ReporterPhone,    ReporterPhone),
            ReporterUnionId   = ISNULL(@ReporterUnionId,  ReporterUnionId),
            UpdatedDate       = GETDATE(),
            UpdatedBy         = @UserName
        WHERE ViolationId = @ViolationId;
    END

    SELECT CAST(@ViolationId AS NVARCHAR(50)) AS ViolationId;
END
GO


/* -----------------------------------------------------------------------------
   5.2. DANH SÁCH LỊCH SỬ VI PHẠM - CÓ PHÂN QUYỀN
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_SubjectViolation_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_SubjectViolation_Get;
GO

CREATE PROCEDURE dbo.p_Major_SubjectViolation_Get
    @Key        NVARCHAR(500)    = NULL,
    @SubjectId  UNIQUEIDENTIFIER = NULL,
    @FieldId    INT              = NULL,
    @UserName   NVARCHAR(100)    = NULL,   -- Dùng để phân quyền dữ liệu
    @Search     NVARCHAR(500)    = NULL,
    @Order      NVARCHAR(10)     = '0',
    @OrderDir   NVARCHAR(10)     = 'DESC',
    @StartIndex INT              = 0,
    @PageSize   INT              = -1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);

    SET @Key    = NULLIF(LTRIM(RTRIM(ISNULL(@Key, ''))), '');
    SET @Search = NULLIF(LTRIM(RTRIM(ISNULL(@Search, ''))), '');

    ;WITH ScopedViolation AS
    (
        SELECT
            v.ViolationId,
            v.SubjectId,
            s.FullName           AS SubjectName,
            s.IdentityCardNumber,
            s.PhoneNumber,
            v.ViolationDate,
            v.TreatmentMeasures,
            v.RelatedDocuments,
            v.Images,
            v.Notes,
            v.ReporterName,
            v.ReporterUnit,
            v.ReporterPosition,
            v.ReporterPhone,
            v.CreatedDate,
            v.CreatedBy,
            v.UpdatedDate,
            v.UpdatedBy,
            /* Gom tên các hành vi của lần vi phạm này thành một chuỗi */
            STUFF((SELECT N', ' + b2.BehaviorName
                   FROM dbo.Major_SubjectViolation_Behaviors AS vb2
                   INNER JOIN dbo.Cate_ViolationBehaviors AS b2 ON b2.BehaviorId = vb2.BehaviorId
                   WHERE vb2.ViolationId = v.ViolationId
                   FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS BehaviorNames
        FROM dbo.Major_SubjectViolations AS v
        INNER JOIN dbo.Major_Subjects AS s ON s.SubjectId = v.SubjectId
        WHERE ISNULL(v.IsDeleted, 0) = 0

          AND (@SubjectId IS NULL OR v.SubjectId = @SubjectId)

          AND (@Key IS NULL
               OR s.FullName LIKE N'%' + @Key + '%'
               OR s.IdentityCardNumber LIKE '%' + @Key + '%')

          AND (@Search IS NULL
               OR s.FullName LIKE N'%' + @Search + '%'
               OR s.IdentityCardNumber LIKE '%' + @Search + '%'
               OR v.TreatmentMeasures LIKE N'%' + @Search + '%')

          /* Lọc theo lĩnh vực người dùng chọn trên giao diện */
          AND (@FieldId IS NULL OR EXISTS (
                  SELECT 1
                  FROM dbo.Major_SubjectViolation_Behaviors AS vb
                  INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                  WHERE vb.ViolationId = v.ViolationId AND b.FieldId = @FieldId))

          /* --- PHÂN QUYỀN theo đơn vị khai báo (đệ quy toàn bộ cây đơn vị con) --- */
          AND (@IsSuperAdmin = 1
               OR v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))

          /* --- PHÂN QUYỀN theo lĩnh vực được phân công --- */
          AND (@IsSuperAdmin = 1
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolation_Behaviors AS vb
                   INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                   WHERE vb.ViolationId = v.ViolationId
                     AND b.FieldId IN (SELECT FieldId FROM dbo.fn_GetPermittedFields(@UserName))))
    ),
    Counted AS
    (
        SELECT sv.*, COUNT(*) OVER () AS TotalRow
        FROM ScopedViolation AS sv
    )
    SELECT *
    FROM Counted
    ORDER BY
        CASE WHEN @OrderDir = 'ASC' THEN ViolationDate END ASC,
        CASE WHEN @OrderDir <> 'ASC' THEN ViolationDate END DESC
    OFFSET (CASE WHEN @StartIndex < 0 THEN 0 ELSE @StartIndex END) ROWS
    FETCH NEXT (CASE WHEN @PageSize IS NULL OR @PageSize <= 0 THEN 2147483647 ELSE @PageSize END) ROWS ONLY;
END
GO


/* -----------------------------------------------------------------------------
   5.3. LỊCH SỬ VI PHẠM CỦA MỘT ĐỐI TƯỢNG - CÓ PHÂN QUYỀN
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_SubjectViolation_GetBySubjectId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_SubjectViolation_GetBySubjectId;
GO

CREATE PROCEDURE dbo.p_Major_SubjectViolation_GetBySubjectId
    @SubjectId UNIQUEIDENTIFIER,
    @UserName  NVARCHAR(100) = NULL   -- NULL = không giới hạn (dùng cho tác vụ nội bộ)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);
    DECLARE @NoScope      BIT = CASE WHEN @UserName IS NULL THEN 1 ELSE 0 END;

    SELECT
        v.ViolationId,
        v.SubjectId,
        s.FullName           AS SubjectName,
        s.IdentityCardNumber,
        s.PhoneNumber,
        v.ViolationDate,
        v.TreatmentMeasures,
        v.RelatedDocuments,
        v.Images,
        v.Notes,
        v.ReporterName,
        v.ReporterUnit,
        v.ReporterPosition,
        v.ReporterPhone,
        v.ReporterUnionId,
        v.CreatedDate,
        v.CreatedBy,
        v.UpdatedDate,
        v.UpdatedBy,
        STUFF((SELECT N', ' + b2.BehaviorName
               FROM dbo.Major_SubjectViolation_Behaviors AS vb2
               INNER JOIN dbo.Cate_ViolationBehaviors AS b2 ON b2.BehaviorId = vb2.BehaviorId
               WHERE vb2.ViolationId = v.ViolationId
               FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS BehaviorNames,
        /* Danh sách lĩnh vực (không trùng lặp) của các hành vi trong lần vi phạm này */
        STUFF((SELECT DISTINCT N', ' + f2.FieldName
               FROM dbo.Major_SubjectViolation_Behaviors AS vb3
               INNER JOIN dbo.Cate_ViolationBehaviors AS b3 ON b3.BehaviorId = vb3.BehaviorId
               INNER JOIN dbo.Cate_Fields AS f2 ON f2.FieldId = b3.FieldId
               WHERE vb3.ViolationId = v.ViolationId
               FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS FieldNames,
        /* Chỉ đúng tài khoản đã khai báo (hoặc super admin) mới được Sửa / Xoá.
           Đây chỉ là cờ hiển thị nút - quyền thật vẫn được kiểm tra lại ở tầng ứng dụng. */
        CAST(CASE WHEN v.CreatedBy = @UserName OR dbo.fn_IsSuperAdmin(@UserName) = 1
                  THEN 1 ELSE 0 END AS BIT) AS IsOwner
    FROM dbo.Major_SubjectViolations AS v
    INNER JOIN dbo.Major_Subjects AS s ON s.SubjectId = v.SubjectId
    WHERE v.SubjectId = @SubjectId
      AND ISNULL(v.IsDeleted, 0) = 0

      AND (@NoScope = 1 OR @IsSuperAdmin = 1
           OR v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))

      AND (@NoScope = 1 OR @IsSuperAdmin = 1
           OR EXISTS (
               SELECT 1
               FROM dbo.Major_SubjectViolation_Behaviors AS vb
               INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
               WHERE vb.ViolationId = v.ViolationId
                 AND b.FieldId IN (SELECT FieldId FROM dbo.fn_GetPermittedFields(@UserName))))
    ORDER BY v.ViolationDate DESC, v.CreatedDate DESC;
END
GO
