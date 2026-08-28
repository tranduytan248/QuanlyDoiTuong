/* =============================================================================
   30. BỔ SUNG TRƯỜNG LOẠI ĐỐI TƯỢNG (ĐA LỰA CHỌN) CHO MODULE ĐỐI TƯỢNG
   ============================================================================= */

-- 1. Bổ sung cột SubjectTypeIds vào bảng Major_Subjects nếu chưa có
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Major_Subjects' AND COLUMN_NAME = 'SubjectTypeIds'
)
BEGIN
    ALTER TABLE dbo.Major_Subjects ADD SubjectTypeIds NVARCHAR(500) NULL;
END
GO

-- 2. Tạo bảng liên kết quan hệ Major_Subject_SubjectTypes
IF OBJECT_ID('dbo.Major_Subject_SubjectTypes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Major_Subject_SubjectTypes
    (
        SubjectId     UNIQUEIDENTIFIER NOT NULL,
        SubjectTypeId INT NOT NULL,
        CreatedDate   DATETIME NOT NULL DEFAULT(GETDATE()),
        CreatedBy     VARCHAR(50) NULL,
        CONSTRAINT PK_Major_Subject_SubjectTypes PRIMARY KEY CLUSTERED (SubjectId, SubjectTypeId)
    );
END
GO

-- 3. Stored Procedure: p_Major_Subject_Save
IF OBJECT_ID('dbo.p_Major_Subject_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Save;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Save
    @SubjectId               UNIQUEIDENTIFIER,
    @IdentityCardNumber      NVARCHAR(50),
    @FullName                NVARCHAR(250),
    @OtherName               NVARCHAR(250),
    @DateOfBirth             DATETIME,
    @Gender                  NVARCHAR(20),
    @Ethnicity               NVARCHAR(100),
    @Religion                NVARCHAR(100),
    @Nationality             NVARCHAR(100),
    @PlaceOfOrigin           NVARCHAR(500),
    @IdentityCardFrontUrl    NVARCHAR(500),
    @IdentityCardBackUrl     NVARCHAR(500),
    @AvatarUrl               NVARCHAR(500),
    @BirthRegistrationPlace  NVARCHAR(500),
    @CurrentResidence        NVARCHAR(500),
    @PhoneNumber             NVARCHAR(50),
    @ReporterName            NVARCHAR(250),
    @ReporterUnit            NVARCHAR(500),
    @ReporterPhone           NVARCHAR(50),
    @ReporterPosition        NVARCHAR(250),
    @ReporterUnionId         UNIQUEIDENTIFIER,
    @SubjectTypeIds          NVARCHAR(500) = NULL,
    @UserName                NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentityCardNumber = LTRIM(RTRIM(ISNULL(@IdentityCardNumber, '')));
    SET @SubjectTypeIds     = NULLIF(LTRIM(RTRIM(ISNULL(@SubjectTypeIds, ''))), '');

    IF @IdentityCardNumber = ''
    BEGIN
        SELECT 'INVALID' AS Result;
        RETURN;
    END

    /* ---------------------------- THÊM MỚI ---------------------------- */
    IF @SubjectId IS NULL OR @SubjectId = '00000000-0000-0000-0000-000000000000'
    BEGIN
        DECLARE @MineId UNIQUEIDENTIFIER;

        SELECT TOP 1 @MineId = SubjectId
          FROM dbo.Major_Subjects
         WHERE IdentityCardNumber = @IdentityCardNumber
           AND CreatedBy = @UserName
           AND ISNULL(IsDeleted, 0) = 0;

        IF @MineId IS NOT NULL
            SET @SubjectId = @MineId;
        ELSE
        BEGIN
            SET @SubjectId = NEWID();

            INSERT INTO dbo.Major_Subjects
                (SubjectId, IdentityCardNumber, FullName, OtherName, DateOfBirth, Gender,
                 Ethnicity, Religion, Nationality, PlaceOfOrigin,
                 IdentityCardFrontUrl, IdentityCardBackUrl, AvatarUrl,
                 BirthRegistrationPlace, CurrentResidence, PhoneNumber,
                 ReporterName, ReporterUnit, ReporterPhone, ReporterPosition,
                 ReporterUnionId, SubjectTypeIds, IsDeleted, CreatedDate, CreatedBy)
            VALUES
                (@SubjectId, @IdentityCardNumber, @FullName, @OtherName, @DateOfBirth, @Gender,
                 @Ethnicity, @Religion, @Nationality, @PlaceOfOrigin,
                 @IdentityCardFrontUrl, @IdentityCardBackUrl, @AvatarUrl,
                 @BirthRegistrationPlace, @CurrentResidence, @PhoneNumber,
                 @ReporterName, @ReporterUnit, @ReporterPhone, @ReporterPosition,
                 @ReporterUnionId, @SubjectTypeIds, 0, GETDATE(), @UserName);

            -- Đồng bộ bảng quan hệ Major_Subject_SubjectTypes
            DELETE FROM dbo.Major_Subject_SubjectTypes WHERE SubjectId = @SubjectId;
            IF @SubjectTypeIds IS NOT NULL
            BEGIN
                INSERT INTO dbo.Major_Subject_SubjectTypes (SubjectId, SubjectTypeId, CreatedDate, CreatedBy)
                SELECT DISTINCT @SubjectId, TRY_CAST(LTRIM(RTRIM(Name)) AS INT), GETDATE(), @UserName
                FROM dbo.fnSplit(@SubjectTypeIds, ',')
                WHERE TRY_CAST(LTRIM(RTRIM(Name)) AS INT) IS NOT NULL;
            END

            SELECT CAST(@SubjectId AS NVARCHAR(50)) AS Result;
            RETURN;
        END
    END

    /* ---------------------------- CẬP NHẬT ---------------------------- */
    UPDATE dbo.Major_Subjects
       SET IdentityCardNumber     = @IdentityCardNumber,
           FullName               = @FullName,
           OtherName              = @OtherName,
           DateOfBirth            = @DateOfBirth,
           Gender                 = @Gender,
           Ethnicity              = @Ethnicity,
           Religion               = @Religion,
           Nationality            = @Nationality,
           PlaceOfOrigin          = @PlaceOfOrigin,
           BirthRegistrationPlace = @BirthRegistrationPlace,
           CurrentResidence       = @CurrentResidence,
           PhoneNumber            = @PhoneNumber,
           SubjectTypeIds         = @SubjectTypeIds,
           AvatarUrl              = ISNULL(@AvatarUrl, AvatarUrl),
           IdentityCardFrontUrl   = ISNULL(@IdentityCardFrontUrl, IdentityCardFrontUrl),
           IdentityCardBackUrl    = ISNULL(@IdentityCardBackUrl, IdentityCardBackUrl),
           UpdatedDate            = GETDATE(),
           UpdatedBy              = @UserName
     WHERE SubjectId = @SubjectId;

    -- Đồng bộ bảng quan hệ Major_Subject_SubjectTypes
    DELETE FROM dbo.Major_Subject_SubjectTypes WHERE SubjectId = @SubjectId;
    IF @SubjectTypeIds IS NOT NULL
    BEGIN
        INSERT INTO dbo.Major_Subject_SubjectTypes (SubjectId, SubjectTypeId, CreatedDate, CreatedBy)
        SELECT DISTINCT @SubjectId, TRY_CAST(LTRIM(RTRIM(Name)) AS INT), GETDATE(), @UserName
        FROM dbo.fnSplit(@SubjectTypeIds, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(Name)) AS INT) IS NOT NULL;
    END

    SELECT CAST(@SubjectId AS NVARCHAR(50)) AS Result;
END
GO

-- 4. Stored Procedure: p_Major_Subject_GetById
IF OBJECT_ID('dbo.p_Major_Subject_GetById', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_GetById;
GO

CREATE PROCEDURE dbo.p_Major_Subject_GetById
    @SubjectId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

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
        s.BirthRegistrationPlace,
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
        s.IsDeleted,
        s.CreatedDate,
        s.CreatedBy,
        s.UpdatedDate,
        s.UpdatedBy,
        (SELECT COUNT(1) FROM dbo.Major_SubjectViolations WHERE SubjectId = s.SubjectId AND ISNULL(IsDeleted, 0) = 0) AS ViolationCount
    FROM dbo.Major_Subjects s
    WHERE s.SubjectId = @SubjectId
      AND ISNULL(s.IsDeleted, 0) = 0;
END
GO

-- 5. Stored Procedure: p_Major_Subject_LookupByCard
IF OBJECT_ID('dbo.p_Major_Subject_LookupByCard', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_LookupByCard;
GO

CREATE PROCEDURE dbo.p_Major_Subject_LookupByCard
    @IdentityCardNumber NVARCHAR(50),
    @UserName           NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @IdentityCardNumber = LTRIM(RTRIM(ISNULL(@IdentityCardNumber, '')));
    IF @IdentityCardNumber = '' RETURN;

    SELECT TOP 1
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
           s.BirthRegistrationPlace,
           s.CurrentResidence,
           s.PhoneNumber,
           s.AvatarUrl,
           s.IdentityCardFrontUrl,
           s.IdentityCardBackUrl,
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
           s.CreatedBy,
           s.CreatedDate,
           s.ReporterName,
           s.ReporterUnit,

           CAST(CASE WHEN s.CreatedBy = @UserName THEN 1 ELSE 0 END AS BIT) AS IsMine,

           (SELECT COUNT(*) FROM dbo.Major_Subjects AS s2
             WHERE s2.IdentityCardNumber = @IdentityCardNumber
               AND ISNULL(s2.IsDeleted, 0) = 0) AS TotalRecords

      FROM dbo.Major_Subjects AS s
     WHERE s.IdentityCardNumber = @IdentityCardNumber
       AND ISNULL(s.IsDeleted, 0) = 0
     ORDER BY CASE WHEN s.CreatedBy = @UserName THEN 0 ELSE 1 END,
              s.CreatedDate DESC;
END
GO

-- 6. Stored Procedure: p_Major_Subject_Get
IF OBJECT_ID('dbo.p_Major_Subject_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Get;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Get
    @IdentityCardNumber NVARCHAR(50)   = NULL,
    @FullName           NVARCHAR(200)  = NULL,
    @BehaviorIds        NVARCHAR(MAX)  = NULL,
    @Gender             NVARCHAR(20)   = NULL,
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
    DistinctPerson AS
    (
        SELECT
            c.*,
            ROW_NUMBER() OVER (
                PARTITION BY c.IdentityCardNumber, c.FullName, c.DateOfBirth, c.PlaceOfOrigin
                ORDER BY
                    CASE WHEN c.CreatedBy = @UserName THEN 0 ELSE 1 END,
                    c.CreatedDate DESC
            ) AS RowInGroup,
            COUNT(*) OVER (
                PARTITION BY c.IdentityCardNumber, c.FullName, c.DateOfBirth, c.PlaceOfOrigin
            ) AS RecordCount
        FROM ScopedSubject AS c
    ),
    FinalList AS
    (
        SELECT
            d.SubjectId,
            d.IdentityCardNumber,
            d.FullName,
            d.OtherName,
            d.DateOfBirth,
            d.Gender,
            d.Ethnicity,
            d.Religion,
            d.Nationality,
            d.PlaceOfOrigin,
            d.CurrentResidence,
            d.PhoneNumber,
            d.AvatarUrl,
            d.IdentityCardFrontUrl,
            d.IdentityCardBackUrl,
            d.ReporterName,
            d.ReporterUnit,
            d.ReporterPhone,
            d.ReporterPosition,
            d.SubjectTypeIds,
            d.SubjectTypeNames,
            d.CreatedDate,
            d.CreatedBy,
            d.UpdatedDate,
            d.UpdatedBy,
            d.RecordCount,

            (SELECT COUNT(DISTINCT u.UnionId)
             FROM (
                 SELECT s2.ReporterUnionId AS UnionId
                 FROM dbo.Major_Subjects AS s2
                 WHERE s2.IdentityCardNumber = d.IdentityCardNumber
                   AND ISNULL(s2.IsDeleted, 0) = 0
                   AND s2.ReporterUnionId IS NOT NULL
                 UNION
                 SELECT v2.ReporterUnionId AS UnionId
                 FROM dbo.Major_SubjectViolations AS v2
                 INNER JOIN dbo.Major_Subjects AS s3 ON s3.SubjectId = v2.SubjectId
                 WHERE s3.IdentityCardNumber = d.IdentityCardNumber
                   AND ISNULL(v2.IsDeleted, 0) = 0
                   AND ISNULL(s3.IsDeleted, 0) = 0
                   AND v2.ReporterUnionId IS NOT NULL
             ) AS u) AS TrackingUnitCount,

            STUFF((
                SELECT DISTINCT ', ' + cu.UnionName
                FROM (
                    SELECT s2.ReporterUnionId AS UnionId
                    FROM dbo.Major_Subjects AS s2
                    WHERE s2.IdentityCardNumber = d.IdentityCardNumber
                      AND ISNULL(s2.IsDeleted, 0) = 0
                      AND s2.ReporterUnionId IS NOT NULL
                    UNION
                    SELECT v2.ReporterUnionId AS UnionId
                    FROM dbo.Major_SubjectViolations AS v2
                    INNER JOIN dbo.Major_Subjects AS s3 ON s3.SubjectId = v2.SubjectId
                    WHERE s3.IdentityCardNumber = d.IdentityCardNumber
                      AND ISNULL(v2.IsDeleted, 0) = 0
                      AND ISNULL(s3.IsDeleted, 0) = 0
                      AND v2.ReporterUnionId IS NOT NULL
                ) AS u2
                INNER JOIN dbo.Cate_Unions AS cu ON cu.UnionId = u2.UnionId
                FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS TrackingUnits,

            (SELECT COUNT(1)
             FROM dbo.Major_SubjectViolations AS v
             WHERE v.SubjectId = d.SubjectId
               AND ISNULL(v.IsDeleted, 0) = 0) AS ViolationCount,

            COUNT(*) OVER() AS TotalRow
        FROM DistinctPerson AS d
        WHERE d.RowInGroup = 1
    )
    SELECT
        SubjectId,
        IdentityCardNumber,
        FullName,
        OtherName,
        DateOfBirth,
        Gender,
        Ethnicity,
        Religion,
        Nationality,
        PlaceOfOrigin,
        CurrentResidence,
        PhoneNumber,
        AvatarUrl,
        IdentityCardFrontUrl,
        IdentityCardBackUrl,
        ReporterName,
        ReporterUnit,
        ReporterPhone,
        ReporterPosition,
        SubjectTypeIds,
        SubjectTypeNames,
        CreatedDate,
        CreatedBy,
        UpdatedDate,
        UpdatedBy,
        RecordCount,
        TrackingUnitCount,
        TrackingUnits,
        ViolationCount,
        TotalRow
    FROM FinalList
    ORDER BY
        CASE WHEN @Order = '1' AND @OrderDir = 'ASC'  THEN FullName END ASC,
        CASE WHEN @Order = '1' AND @OrderDir = 'DESC' THEN FullName END DESC,
        CreatedDate DESC
    OFFSET CASE WHEN @PageSize < 0 THEN 0 ELSE @StartIndex END ROWS
    FETCH NEXT CASE WHEN @PageSize < 0 THEN 2147483647 ELSE @PageSize END ROWS ONLY;
END
GO
