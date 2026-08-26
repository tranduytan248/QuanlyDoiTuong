CREATE PROCEDURE dbo.p_Major_SubjectViolation_GetById
    @ViolationId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
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
           v.UpdatedBy
    FROM dbo.Major_SubjectViolations v
    INNER JOIN dbo.Major_Subjects s ON v.SubjectId = s.SubjectId
    WHERE v.ViolationId = @ViolationId AND v.IsDeleted = 0;
END