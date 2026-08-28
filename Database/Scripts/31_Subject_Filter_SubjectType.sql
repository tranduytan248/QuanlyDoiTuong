-- 31_Subject_Filter_SubjectType.sql
-- Cập nhật Stored Procedure p_Major_Subject_Get bổ sung tham số @SubjectTypeIds để lọc theo Loại đối tượng

IF OBJECT_ID('dbo.p_Major_Subject_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Get;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Get
    @IdentityCardNumber NVARCHAR(50)   = NULL,
    @FullName           NVARCHAR(200)  = NULL,
    @BehaviorIds        NVARCHAR(MAX)  = NULL,
    @Gender             NVARCHAR(20)   = NULL,
    @SubjectTypeIds     NVARCHAR(MAX)  = NULL,   -- Lọc theo loại đối tượng (danh sách ID phân tách bởi dấu phẩy)
    @UserName           NVARCHAR(100)  = NULL,
    @Search             NVARCHAR(500)  = NULL,
    @Order              NVARCHAR(10)   = '0',
    @OrderDir           NVARCHAR(10)   = 'ASC',
    @StartIndex         INT            = 0,
    @PageSize           INT            = -1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);
    DECLARE @NoScope BIT = CASE WHEN @UserName IS NULL THEN 1 ELSE 0 END;

    SET @IdentityCardNumber = NULLIF(LTRIM(RTRIM(ISNULL(@IdentityCardNumber, ''))), '');
    SET @FullName           = NULLIF(LTRIM(RTRIM(ISNULL(@FullName, ''))), '');
    SET @BehaviorIds        = NULLIF(LTRIM(RTRIM(ISNULL(@BehaviorIds, ''))), '');
    SET @Gender             = NULLIF(LTRIM(RTRIM(ISNULL(@Gender, ''))), '');
    SET @SubjectTypeIds     = NULLIF(LTRIM(RTRIM(ISNULL(@SubjectTypeIds, ''))), '');
    SET @Search             = NULLIF(LTRIM(RTRIM(ISNULL(@Search, ''))), '');

    DECLARE @TblBehaviors TABLE (BehaviorId INT PRIMARY KEY);
    IF @BehaviorIds IS NOT NULL
    BEGIN
        INSERT INTO @TblBehaviors (BehaviorId)
        SELECT DISTINCT TRY_CAST(LTRIM(RTRIM(Name)) AS INT)
        FROM dbo.fnSplit(@BehaviorIds, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(Name)) AS INT) IS NOT NULL;
    END

    DECLARE @HasBehaviorFilter BIT = CASE WHEN EXISTS (SELECT 1 FROM @TblBehaviors) THEN 1 ELSE 0 END;

    DECLARE @TblSubjectTypes TABLE (SubjectTypeId INT PRIMARY KEY);
    IF @SubjectTypeIds IS NOT NULL
    BEGIN
        INSERT INTO @TblSubjectTypes (SubjectTypeId)
        SELECT DISTINCT TRY_CAST(LTRIM(RTRIM(Name)) AS INT)
        FROM dbo.fnSplit(@SubjectTypeIds, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(Name)) AS INT) IS NOT NULL;
    END

    DECLARE @HasSubjectTypeFilter BIT = CASE WHEN EXISTS (SELECT 1 FROM @TblSubjectTypes) THEN 1 ELSE 0 END;

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
            s.SubjectTypeIds,
            STUFF((
                SELECT ', ' + st.SubjectTypeName
                FROM dbo.Cate_SubjectTypes st
                WHERE st.IsDeleted = 0
                  AND st.SubjectTypeId IN (
                      SELECT TRY_CAST(LTRIM(RTRIM(Name)) AS INT) 
                      FROM dbo.fnSplit(s.SubjectTypeIds, ',')
                  )
                FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS SubjectTypeNames,
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

          /* --- Lọc theo loại đối tượng --- */
          AND (@HasSubjectTypeFilter = 0 OR EXISTS (
                  SELECT 1
                  FROM dbo.Major_Subject_SubjectTypes AS mst
                  INNER JOIN @TblSubjectTypes AS tst ON tst.SubjectTypeId = mst.SubjectTypeId
                  WHERE mst.SubjectId = s.SubjectId
              ) OR EXISTS (
                  SELECT 1
                  FROM dbo.fnSplit(s.SubjectTypeIds, ',') AS fs
                  INNER JOIN @TblSubjectTypes AS tst2 ON tst2.SubjectTypeId = TRY_CAST(LTRIM(RTRIM(fs.Name)) AS INT)
              ))

          /* --- PHÂN QUYỀN: đơn vị khai báo phải nằm trong phạm vi của người dùng --- */
          AND (@NoScope = 1 OR @IsSuperAdmin = 1
               OR s.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))))

          /* --- PHÂN QUYỀN: đối tượng phải có vi phạm thuộc lĩnh vực được phân công --- */
          AND (@NoScope = 1 OR @IsSuperAdmin = 1
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   INNER JOIN dbo.Major_SubjectViolation_Behaviors AS vb ON vb.ViolationId = v.ViolationId
                   INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND b.FieldId IN (SELECT FieldId FROM dbo.fn_GetPermittedFields(@UserName))))
    ),
    WithTrackingUnitCount AS
    (
        SELECT
            ss.*,
            (SELECT COUNT(DISTINCT ISNULL(unit_source.ReporterUnionId, '00000000-0000-0000-0000-000000000000'))
             FROM
             (
                 SELECT s_orig.ReporterUnionId
                 FROM dbo.Major_Subjects AS s_orig
                 WHERE s_orig.IdentityCardNumber = ss.IdentityCardNumber
                   AND ISNULL(s_orig.IsDeleted, 0) = 0

                 UNION

                 SELECT v.ReporterUnionId
                 FROM dbo.Major_SubjectViolations AS v
                 INNER JOIN dbo.Major_Subjects AS s2 ON s2.SubjectId = v.SubjectId
                 WHERE s2.IdentityCardNumber = ss.IdentityCardNumber
                   AND ISNULL(v.IsDeleted, 0) = 0
                   AND ISNULL(s2.IsDeleted, 0) = 0
             ) AS unit_source
            ) AS TrackingUnitCount
        FROM ScopedSubject AS ss
    ),
    Counted AS
    (
        SELECT w.*, COUNT(*) OVER () AS TotalRow
        FROM WithTrackingUnitCount AS w
    )
    SELECT *
    FROM Counted
    ORDER BY
        CASE WHEN @Order = '0' AND @OrderDir = 'ASC'  THEN FullName END ASC,
        CASE WHEN @Order = '0' AND @OrderDir = 'DESC' THEN FullName END DESC,
        CASE WHEN @Order = '1' AND @OrderDir = 'ASC'  THEN IdentityCardNumber END ASC,
        CASE WHEN @Order = '1' AND @OrderDir = 'DESC' THEN IdentityCardNumber END DESC,
        CASE WHEN @Order = '2' AND @OrderDir = 'ASC'  THEN DateOfBirth END ASC,
        CASE WHEN @Order = '2' AND @OrderDir = 'DESC' THEN DateOfBirth END DESC,
        CASE WHEN @Order = '3' AND @OrderDir = 'ASC'  THEN TrackingUnitCount END ASC,
        CASE WHEN @Order = '3' AND @OrderDir = 'DESC' THEN TrackingUnitCount END DESC,
        CASE WHEN @Order NOT IN ('0','1','2','3') THEN CreatedDate END DESC
    OFFSET (CASE WHEN @StartIndex < 0 THEN 0 ELSE @StartIndex END) ROWS
    FETCH NEXT (CASE WHEN @PageSize IS NULL OR @PageSize <= 0 THEN 2147483647 ELSE @PageSize END) ROWS ONLY;
END
GO
