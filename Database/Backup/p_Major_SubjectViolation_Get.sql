CREATE PROCEDURE dbo.p_Major_SubjectViolation_Get
    @Key NVARCHAR(255) = NULL,
    @SubjectId UNIQUEIDENTIFIER = NULL,
    @FieldId INT = NULL,
    @Search NVARCHAR(255) = NULL,
    @Order VARCHAR(50) = '0',
    @OrderDir VARCHAR(10) = 'DESC',
    @StartIndex INT = 0,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TotalRow INT;

    SELECT @TotalRow = COUNT(DISTINCT v.ViolationId)
    FROM dbo.Major_SubjectViolations v
    INNER JOIN dbo.Major_Subjects s ON v.SubjectId = s.SubjectId AND s.IsDeleted = 0
    LEFT JOIN dbo.Major_SubjectViolation_Behaviors vb ON v.ViolationId = vb.ViolationId
    LEFT JOIN dbo.Cate_ViolationBehaviors b ON vb.BehaviorId = b.BehaviorId
    WHERE v.IsDeleted = 0
      AND (@SubjectId IS NULL OR @SubjectId = '00000000-0000-0000-0000-000000000000' OR v.SubjectId = @SubjectId)
      AND (@FieldId IS NULL OR @FieldId = 0 OR b.FieldId = @FieldId)
      AND (@Key IS NULL OR @Key = '' OR s.FullName LIKE '%' + @Key + '%' OR s.IdentityCardNumber LIKE '%' + @Key + '%' OR b.BehaviorName LIKE '%' + @Key + '%')
      AND (@Search IS NULL OR @Search = '' OR s.FullName LIKE '%' + @Search + '%' OR s.IdentityCardNumber LIKE '%' + @Search + '%');

    SELECT v.ViolationId,
           v.SubjectId,
           s.FullName AS SubjectName,
           s.IdentityCardNumber,
           s.PhoneNumber,
           v.ViolationDate,
           v.TreatmentMeasures,
           v.RelatedDocuments,
           v.Images,
           v.Notes,
           v.CreatedDate,
           v.CreatedBy,
           v.UpdatedDate,
           v.UpdatedBy,
           @TotalRow AS TotalRow
    FROM dbo.Major_SubjectViolations v
    INNER JOIN dbo.Major_Subjects s ON v.SubjectId = s.SubjectId AND s.IsDeleted = 0
    WHERE v.IsDeleted = 0
      AND (@SubjectId IS NULL OR @SubjectId = '00000000-0000-0000-0000-000000000000' OR v.SubjectId = @SubjectId)
      AND (@FieldId IS NULL OR @FieldId = 0 OR EXISTS (
          SELECT 1 FROM dbo.Major_SubjectViolation_Behaviors vb3
          INNER JOIN dbo.Cate_ViolationBehaviors b3 ON vb3.BehaviorId = b3.BehaviorId
          WHERE vb3.ViolationId = v.ViolationId AND b3.FieldId = @FieldId
      ))
      AND (@Key IS NULL OR @Key = '' OR s.FullName LIKE '%' + @Key + '%' OR s.IdentityCardNumber LIKE '%' + @Key + '%')
      AND (@Search IS NULL OR @Search = '' OR s.FullName LIKE '%' + @Search + '%' OR s.IdentityCardNumber LIKE '%' + @Search + '%')
    GROUP BY v.ViolationId, v.SubjectId, s.FullName, s.IdentityCardNumber, s.PhoneNumber,
             v.ViolationDate, v.TreatmentMeasures, v.RelatedDocuments, v.Images, v.Notes,
             v.CreatedDate, v.CreatedBy, v.UpdatedDate, v.UpdatedBy
    ORDER BY
        CASE WHEN @Order = '1' AND @OrderDir = 'ASC' THEN s.FullName END ASC,
        CASE WHEN @Order = '1' AND @OrderDir = 'DESC' THEN s.FullName END DESC,
        CASE WHEN @Order = '2' AND @OrderDir = 'ASC' THEN v.ViolationDate END ASC,
        CASE WHEN @Order = '2' AND @OrderDir = 'DESC' THEN v.ViolationDate END DESC,
        v.ViolationDate DESC
    OFFSET @StartIndex ROWS
    FETCH NEXT (CASE WHEN @PageSize <= 0 THEN 1000000 ELSE @PageSize END) ROWS ONLY;
END