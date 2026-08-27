/* =============================================================================
   27. THEO DÕI ĐỐI TƯỢNG ĐA ĐƠN VỊ & CHI TIẾT CÁC ĐƠN VỊ GIÁM SÁT
   -----------------------------------------------------------------------------
   1. Cập nhật p_Major_Subject_Get:
      - Bổ sung TrackingUnitCount (Số đơn vị cùng phát sinh hồ sơ cho đối tượng)
      - Bổ sung TrackingUnits (Tên các đơn vị theo dõi)
   2. Tạo mới p_Major_Subject_GetMonitoringUnits:
      - Trả về danh sách chi tiết các đơn vị đã nhập / ghi nhận hồ sơ cho đối tượng
        (gồm hồ sơ khai báo gốc + các lần ghi nhận vi phạm) theo số CCCD.
   ============================================================================= */

-- 1. CẬP NHẬT p_Major_Subject_Get
IF OBJECT_ID('dbo.p_Major_Subject_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Get;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Get
    @IdentityCardNumber NVARCHAR(50)  = NULL,
    @FullName           NVARCHAR(200) = NULL,
    @BehaviorIds        NVARCHAR(MAX) = NULL,
    @Gender             NVARCHAR(20)  = NULL,
    @UserName           NVARCHAR(100) = NULL,
    @Search             NVARCHAR(500) = NULL,
    @Order              NVARCHAR(10)  = '0',
    @OrderDir           NVARCHAR(10)  = 'ASC',
    @StartIndex         INT           = 0,
    @PageSize           INT           = 10
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentityCardNumber = NULLIF(LTRIM(RTRIM(@IdentityCardNumber)), '');
    SET @FullName           = NULLIF(LTRIM(RTRIM(@FullName)), '');
    SET @BehaviorIds        = NULLIF(LTRIM(RTRIM(@BehaviorIds)), '');
    SET @Gender             = NULLIF(LTRIM(RTRIM(@Gender)), '');
    SET @Search             = NULLIF(LTRIM(RTRIM(@Search)), '');
    SET @Order              = ISNULL(@Order, '0');
    SET @OrderDir           = ISNULL(@OrderDir, 'ASC');

    DECLARE @NoScope BIT = CASE WHEN @UserName IS NULL THEN 1 ELSE 0 END;
    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);

    DECLARE @TblBehaviors TABLE (BehaviorId INT);
    DECLARE @HasBehaviorFilter BIT = 0;
    IF @BehaviorIds IS NOT NULL
    BEGIN
        INSERT INTO @TblBehaviors (BehaviorId)
        SELECT CAST(value AS INT)
        FROM STRING_SPLIT(@BehaviorIds, ',')
        WHERE LTRIM(RTRIM(value)) <> '';

        IF EXISTS (SELECT 1 FROM @TblBehaviors)
            SET @HasBehaviorFilter = 1;
    END

    ;WITH ScopedSubject AS
    (
        SELECT
            s.SubjectId,
            s.IdentityCardNumber,
            s.FullName,
            s.OtherName,
            s.DateOfBirth,
            s.Gender,
            s.Ethnicity,
            s.Religion,
            s.Nationality,
            s.PlaceOfOrigin,
            s.CurrentResidence,
            s.PhoneNumber,
            s.AvatarUrl,
            s.IdentityCardFrontUrl,
            s.IdentityCardBackUrl,
            s.ReporterName,
            s.ReporterUnit,
            s.ReporterPhone,
            s.ReporterPosition,
            s.ReporterUnionId,
            s.CreatedDate,
            s.CreatedBy,
            s.UpdatedDate,
            s.UpdatedBy
        FROM dbo.Major_Subjects AS s
        WHERE ISNULL(s.IsDeleted, 0) = 0

          /* --- Lọc theo tiêu chí tra cứu --- */
          AND (@IdentityCardNumber IS NULL OR s.IdentityCardNumber LIKE '%' + @IdentityCardNumber + '%')
          AND (@FullName IS NULL OR s.FullName LIKE N'%' + @FullName + '%' OR s.OtherName LIKE N'%' + @FullName + '%')
          AND (@Gender IS NULL OR s.Gender = @Gender)
          AND (@Search IS NULL
               OR s.IdentityCardNumber LIKE '%' + @Search + '%'
               OR s.FullName LIKE N'%' + @Search + '%'
               OR s.PhoneNumber LIKE '%' + @Search + '%'
               OR s.CurrentResidence LIKE N'%' + @Search + '%')

          /* --- Lọc theo hành vi vi phạm --- */
          AND (@HasBehaviorFilter = 0 OR EXISTS (
                  SELECT 1
                  FROM dbo.Major_SubjectViolations AS v
                  INNER JOIN dbo.Major_SubjectViolation_Behaviors AS vb ON vb.ViolationId = v.ViolationId
                  INNER JOIN @TblBehaviors AS tb ON tb.BehaviorId = vb.BehaviorId
                  WHERE v.SubjectId = s.SubjectId AND ISNULL(v.IsDeleted, 0) = 0))

          /* --- PHÂN QUYỀN: đơn vị khai báo phải nằm trong phạm vi của người dùng --- */
          AND (@NoScope = 1 OR @IsSuperAdmin = 1
               OR s.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))
               /* Cho phép thấy đối tượng nếu có lần vi phạm do đơn vị trong phạm vi khai báo */
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))))

          /* --- PHÂN QUYỀN: đối tượng phải có vi phạm thuộc lĩnh vực được phân công --- */
          AND (@NoScope = 1
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   INNER JOIN dbo.Major_SubjectViolation_Behaviors AS vb ON vb.ViolationId = v.ViolationId
                   INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND b.FieldId IN (SELECT FieldId FROM dbo.fn_GetPermittedFields(@UserName))))
    ),
    Counted AS
    (
        SELECT sc.*, COUNT(*) OVER () AS TotalRow
        FROM ScopedSubject AS sc
    )
    SELECT
        c.SubjectId,
        c.IdentityCardNumber,
        c.FullName,
        c.OtherName,
        c.DateOfBirth,
        c.Gender,
        c.Ethnicity,
        c.Religion,
        c.Nationality,
        c.PlaceOfOrigin,
        c.CurrentResidence,
        c.PhoneNumber,
        c.AvatarUrl,
        c.IdentityCardFrontUrl,
        c.IdentityCardBackUrl,
        c.ReporterName,
        c.ReporterUnit,
        c.ReporterPhone,
        c.ReporterPosition,
        c.ReporterUnionId,
        c.CreatedDate,
        c.CreatedBy,
        c.UpdatedDate,
        c.UpdatedBy,

        /* Số lần vi phạm - chỉ đếm những lần người dùng được phép xem */
        (SELECT COUNT(1)
         FROM dbo.Major_SubjectViolations AS v
         WHERE v.SubjectId = c.SubjectId
           AND ISNULL(v.IsDeleted, 0) = 0
           AND (@NoScope = 1 OR @IsSuperAdmin = 1
                OR v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))
        ) AS ViolationCount,

        /* Số lượng đơn vị cùng quản lý / theo dõi (khai báo gốc + các lần vi phạm) */
        (
            SELECT COUNT(DISTINCT COALESCE(CAST(u.UnionId AS NVARCHAR(50)), u.UnitName, u.CreatedBy))
            FROM (
                SELECT s2.ReporterUnionId AS UnionId, s2.ReporterUnit AS UnitName, s2.CreatedBy
                FROM dbo.Major_Subjects s2
                WHERE (s2.SubjectId = c.SubjectId OR (s2.IdentityCardNumber = c.IdentityCardNumber AND c.IdentityCardNumber <> ''))
                  AND ISNULL(s2.IsDeleted, 0) = 0
                UNION ALL
                SELECT v2.ReporterUnionId AS UnionId, v2.ReporterUnit AS UnitName, v2.CreatedBy
                FROM dbo.Major_SubjectViolations v2
                INNER JOIN dbo.Major_Subjects s3 ON s3.SubjectId = v2.SubjectId
                WHERE (v2.SubjectId = c.SubjectId OR (s3.IdentityCardNumber = c.IdentityCardNumber AND c.IdentityCardNumber <> ''))
                  AND ISNULL(v2.IsDeleted, 0) = 0
                  AND ISNULL(s3.IsDeleted, 0) = 0
            ) u
        ) AS TrackingUnitCount,

        /* Danh sách tên các đơn vị cùng theo dõi */
        (
            SELECT STUFF((
                SELECT DISTINCT ', ' + COALESCE(un2.UnionName, u2.UnitName)
                FROM (
                    SELECT s4.ReporterUnionId AS UnionId, s4.ReporterUnit AS UnitName
                    FROM dbo.Major_Subjects s4
                    WHERE (s4.SubjectId = c.SubjectId OR (s4.IdentityCardNumber = c.IdentityCardNumber AND c.IdentityCardNumber <> ''))
                      AND ISNULL(s4.IsDeleted, 0) = 0
                    UNION ALL
                    SELECT v4.ReporterUnionId AS UnionId, v4.ReporterUnit AS UnitName
                    FROM dbo.Major_SubjectViolations v4
                    INNER JOIN dbo.Major_Subjects s5 ON s5.SubjectId = v4.SubjectId
                    WHERE (v4.SubjectId = c.SubjectId OR (s5.IdentityCardNumber = c.IdentityCardNumber AND c.IdentityCardNumber <> ''))
                      AND ISNULL(v4.IsDeleted, 0) = 0
                      AND ISNULL(s5.IsDeleted, 0) = 0
                ) u2
                LEFT JOIN dbo.Cate_Unions un2 ON un2.UnionId = u2.UnionId
                WHERE COALESCE(un2.UnionName, u2.UnitName) IS NOT NULL
                FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')
        ) AS TrackingUnits,

        c.TotalRow
    FROM Counted AS c
    ORDER BY
        CASE WHEN @Order = '1' AND @OrderDir = 'ASC'  THEN c.IdentityCardNumber END ASC,
        CASE WHEN @Order = '1' AND @OrderDir = 'DESC' THEN c.IdentityCardNumber END DESC,
        CASE WHEN @Order = '2' AND @OrderDir = 'ASC'  THEN c.Gender END ASC,
        CASE WHEN @Order = '2' AND @OrderDir = 'DESC' THEN c.Gender END DESC,
        CASE WHEN @Order = '3' AND @OrderDir = 'ASC'  THEN c.DateOfBirth END ASC,
        CASE WHEN @Order = '3' AND @OrderDir = 'DESC' THEN c.DateOfBirth END DESC,
        CASE WHEN @Order = '4' AND @OrderDir = 'ASC'  THEN c.PlaceOfOrigin END ASC,
        CASE WHEN @Order = '4' AND @OrderDir = 'DESC' THEN c.PlaceOfOrigin END DESC,
        c.CreatedDate DESC
    OFFSET (CASE WHEN @StartIndex < 0 THEN 0 ELSE @StartIndex END) ROWS
    FETCH NEXT (CASE WHEN @PageSize IS NULL OR @PageSize <= 0 THEN 2147483647 ELSE @PageSize END) ROWS ONLY;
END
GO


-- 2. TẠO MỚI p_Major_Subject_GetMonitoringUnits
IF OBJECT_ID('dbo.p_Major_Subject_GetMonitoringUnits', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_GetMonitoringUnits;
GO

CREATE PROCEDURE dbo.p_Major_Subject_GetMonitoringUnits
    @SubjectId UNIQUEIDENTIFIER,
    @UserName  NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NoScope BIT = CASE WHEN @UserName IS NULL THEN 1 ELSE 0 END;
    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);

    /* Lấy số CCCD của đối tượng */
    DECLARE @IdentityCardNumber NVARCHAR(50);
    SELECT @IdentityCardNumber = IdentityCardNumber
    FROM dbo.Major_Subjects
    WHERE SubjectId = @SubjectId;

    /* 1. Lấy thông tin bản ghi khai báo đối tượng gốc */
    SELECT
        s.SubjectId AS RecordId,
        'KHAI_BAO' AS RecordType,
        N'Khai báo hồ sơ đối tượng' AS RecordTypeName,
        s.CreatedDate AS RecordDate,
        s.ReporterUnionId AS UnionId,
        COALESCE(un.UnionName, s.ReporterUnit, N'Chưa phân đơn vị') AS UnitName,
        s.ReporterName,
        s.ReporterPosition,
        s.ReporterPhone,
        s.CreatedBy,
        NULL AS FieldNames,
        NULL AS BehaviorNames,
        NULL AS TreatmentMeasures,
        NULL AS Notes,
        NULL AS RelatedDocuments,
        NULL AS Images,
        CASE WHEN s.CreatedBy = @UserName OR @IsSuperAdmin = 1 THEN 1 ELSE 0 END AS IsOwner
    FROM dbo.Major_Subjects AS s
    LEFT JOIN dbo.Cate_Unions AS un ON un.UnionId = s.ReporterUnionId
    WHERE (s.SubjectId = @SubjectId OR (s.IdentityCardNumber = @IdentityCardNumber AND @IdentityCardNumber <> ''))
      AND ISNULL(s.IsDeleted, 0) = 0

    UNION ALL

    /* 2. Lấy thông tin các lần ghi nhận vi phạm */
    SELECT
        v.ViolationId AS RecordId,
        'VI_PHAM' AS RecordType,
        N'Ghi nhận vi phạm' AS RecordTypeName,
        v.ViolationDate AS RecordDate,
        v.ReporterUnionId AS UnionId,
        COALESCE(un.UnionName, v.ReporterUnit, N'Chưa phân đơn vị') AS UnitName,
        v.ReporterName,
        v.ReporterPosition,
        v.ReporterPhone,
        v.CreatedBy,
        (SELECT STUFF((
             SELECT DISTINCT ', ' + f.FieldName
             FROM dbo.Major_SubjectViolation_Behaviors AS vb2
             INNER JOIN dbo.Cate_ViolationBehaviors AS b2 ON b2.BehaviorId = vb2.BehaviorId
             INNER JOIN dbo.Cate_Fields AS f ON f.FieldId = b2.FieldId
             WHERE vb2.ViolationId = v.ViolationId
             FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')) AS FieldNames,
        (SELECT STUFF((
             SELECT DISTINCT ', ' + b3.BehaviorName
             FROM dbo.Major_SubjectViolation_Behaviors AS vb3
             INNER JOIN dbo.Cate_ViolationBehaviors AS b3 ON b3.BehaviorId = vb3.BehaviorId
             WHERE vb3.ViolationId = v.ViolationId
             FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '')) AS BehaviorNames,
        v.TreatmentMeasures,
        v.Notes,
        v.RelatedDocuments,
        v.Images,
        CASE WHEN v.CreatedBy = @UserName OR @IsSuperAdmin = 1 THEN 1 ELSE 0 END AS IsOwner
    FROM dbo.Major_SubjectViolations AS v
    INNER JOIN dbo.Major_Subjects AS s ON s.SubjectId = v.SubjectId
    LEFT JOIN dbo.Cate_Unions AS un ON un.UnionId = v.ReporterUnionId
    WHERE (v.SubjectId = @SubjectId OR (s.IdentityCardNumber = @IdentityCardNumber AND @IdentityCardNumber <> ''))
      AND ISNULL(v.IsDeleted, 0) = 0
      AND ISNULL(s.IsDeleted, 0) = 0
      AND (@NoScope = 1 OR @IsSuperAdmin = 1
           OR v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))
    ORDER BY RecordDate DESC;
END
GO
