
CREATE PROCEDURE dbo.p_Major_Subject_Get
    @Key NVARCHAR(255) = NULL,
    @Gender NVARCHAR(20) = NULL,
    @Search NVARCHAR(255) = NULL,
    @Order VARCHAR(50) = '0',
    @OrderDir VARCHAR(10) = 'ASC',
    @StartIndex INT = 0,
    @PageSize INT = 10
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @TotalRow INT;

    SELECT @TotalRow = COUNT(1)
    FROM dbo.Major_Subjects s
    WHERE s.IsDeleted = 0
      AND (@Gender IS NULL OR @Gender = '' OR s.Gender = @Gender)
      AND (@Key IS NULL OR @Key = '' OR s.IdentityCardNumber LIKE '%' + @Key + '%' OR s.FullName LIKE '%' + @Key + '%' OR s.PhoneNumber LIKE '%' + @Key + '%' OR s.CurrentResidence LIKE '%' + @Key + '%' OR s.ReporterName LIKE '%' + @Key + '%' OR s.ReporterUnit LIKE '%' + @Key + '%')
      AND (@Search IS NULL OR @Search = '' OR s.IdentityCardNumber LIKE '%' + @Search + '%' OR s.FullName LIKE '%' + @Search + '%' OR s.PhoneNumber LIKE '%' + @Search + '%');

    SELECT s.SubjectId,
           s.IdentityCardNumber,
           s.FullName,
           s.OtherName,
           s.DateOfBirth,
           s.Gender,
           s.Ethnicity,
           s.Religion,
           s.Nationality,
           s.PlaceOfOrigin,
           s.IdentityCardFrontUrl,
           s.IdentityCardBackUrl,
           s.AvatarUrl,
           s.BirthRegistrationPlace,
           s.CurrentResidence,
           s.PhoneNumber,
           s.ReporterName,
           s.ReporterUnit,
           s.ReporterPhone,
           s.ReporterPosition,
           s.CreatedDate,
           s.CreatedBy,
           s.UpdatedDate,
           s.UpdatedBy,
           (SELECT COUNT(1) FROM dbo.Major_SubjectViolations v WHERE v.SubjectId = s.SubjectId AND v.IsDeleted = 0) AS ViolationCount,
           @TotalRow AS TotalRow
    FROM dbo.Major_Subjects s
    WHERE s.IsDeleted = 0
      AND (@Gender IS NULL OR @Gender = '' OR s.Gender = @Gender)
      AND (@Key IS NULL OR @Key = '' OR s.IdentityCardNumber LIKE '%' + @Key + '%' OR s.FullName LIKE '%' + @Key + '%' OR s.PhoneNumber LIKE '%' + @Key + '%' OR s.CurrentResidence LIKE '%' + @Key + '%' OR s.ReporterName LIKE '%' + @Key + '%' OR s.ReporterUnit LIKE '%' + @Key + '%')
      AND (@Search IS NULL OR @Search = '' OR s.IdentityCardNumber LIKE '%' + @Search + '%' OR s.FullName LIKE '%' + @Search + '%' OR s.PhoneNumber LIKE '%' + @Search + '%')
    ORDER BY
        CASE WHEN @Order = '1' AND @OrderDir = 'ASC' THEN s.IdentityCardNumber END ASC,
        CASE WHEN @Order = '1' AND @OrderDir = 'DESC' THEN s.IdentityCardNumber END DESC,
        CASE WHEN @Order = '2' AND @OrderDir = 'ASC' THEN s.FullName END ASC,
        CASE WHEN @Order = '2' AND @OrderDir = 'DESC' THEN s.FullName END DESC,
        CASE WHEN @Order = '3' AND @OrderDir = 'ASC' THEN s.DateOfBirth END ASC,
        CASE WHEN @Order = '3' AND @OrderDir = 'DESC' THEN s.DateOfBirth END DESC,
        s.CreatedDate DESC
    OFFSET @StartIndex ROWS
    FETCH NEXT (CASE WHEN @PageSize <= 0 THEN 1000000 ELSE @PageSize END) ROWS ONLY;
END
