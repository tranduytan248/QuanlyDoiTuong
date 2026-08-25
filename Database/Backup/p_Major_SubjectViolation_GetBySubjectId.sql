CREATE PROCEDURE dbo.p_Major_SubjectViolation_GetBySubjectId
    @SubjectId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    SELECT v.ViolationId,
           v.SubjectId,
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
    WHERE v.SubjectId = @SubjectId AND v.IsDeleted = 0
    ORDER BY v.ViolationDate DESC;
END