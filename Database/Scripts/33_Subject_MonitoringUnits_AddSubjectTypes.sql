-- 33_Subject_MonitoringUnits_AddSubjectTypes.sql
-- Bổ sung SubjectTypeIds và SubjectTypeNames vào p_Major_Subject_GetMonitoringUnits

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

    /* Chỉ lấy thông tin các đơn vị khai báo hồ sơ đối tượng */
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
        NULL AS FieldNames,
        NULL AS BehaviorNames,
        NULL AS TreatmentMeasures,
        NULL AS Notes,
        NULL AS RelatedDocuments,
        NULL AS Images,
        CAST(CASE WHEN s.CreatedBy = @UserName OR @IsSuperAdmin = 1 THEN 1 ELSE 0 END AS BIT) AS IsOwner
    FROM dbo.Major_Subjects AS s
    LEFT JOIN dbo.Cate_Unions AS un ON un.UnionId = s.ReporterUnionId
    WHERE (s.SubjectId = @SubjectId OR (s.IdentityCardNumber = @IdentityCardNumber AND @IdentityCardNumber <> ''))
      AND ISNULL(s.IsDeleted, 0) = 0
      AND (@NoScope = 1 OR @IsSuperAdmin = 1
           OR s.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))
    ORDER BY s.CreatedDate DESC;
END
GO
