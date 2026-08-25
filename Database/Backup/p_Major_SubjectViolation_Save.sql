CREATE PROCEDURE dbo.p_Major_SubjectViolation_Save
    @ViolationId UNIQUEIDENTIFIER = NULL,
    @SubjectId UNIQUEIDENTIFIER,
    @ViolationDate DATETIME,
    @TreatmentMeasures NVARCHAR(MAX) = NULL,
    @RelatedDocuments NVARCHAR(MAX) = NULL,
    @Images NVARCHAR(MAX) = NULL,
    @Notes NVARCHAR(1000) = NULL,
    @UserName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @ActualViolationId UNIQUEIDENTIFIER;

    IF (@ViolationId IS NULL OR @ViolationId = '00000000-0000-0000-0000-000000000000')
    BEGIN
        SET @ActualViolationId = NEWID();
        INSERT INTO dbo.Major_SubjectViolations (
            ViolationId, SubjectId, ViolationDate, TreatmentMeasures, RelatedDocuments,
            Images, Notes, IsDeleted, CreatedDate, CreatedBy
        )
        VALUES (
            @ActualViolationId, @SubjectId, @ViolationDate, @TreatmentMeasures, @RelatedDocuments,
            @Images, @Notes, 0, GETDATE(), @UserName
        );
    END
    ELSE
    BEGIN
        SET @ActualViolationId = @ViolationId;
        UPDATE dbo.Major_SubjectViolations
        SET SubjectId = @SubjectId,
            ViolationDate = @ViolationDate,
            TreatmentMeasures = @TreatmentMeasures,
            RelatedDocuments = @RelatedDocuments,
            Images = ISNULL(@Images, Images),
            Notes = @Notes,
            UpdatedDate = GETDATE(),
            UpdatedBy = @UserName
        WHERE ViolationId = @ActualViolationId;
    END

    SELECT CAST(@ActualViolationId AS VARCHAR(50)) AS Result;
END