CREATE PROCEDURE dbo.p_Major_Subject_Delete
    @SubjectId UNIQUEIDENTIFIER,
    @UserName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Major_Subjects
    SET IsDeleted = 1,
        DeletedDate = GETDATE(),
        DeletedBy = @UserName
    WHERE SubjectId = @SubjectId;

    -- Xóa mềm luôn lịch sử vi phạm liên quan
    UPDATE dbo.Major_SubjectViolations
    SET IsDeleted = 1,
        DeletedDate = GETDATE(),
        DeletedBy = @UserName
    WHERE SubjectId = @SubjectId;

    SELECT CAST(@SubjectId AS VARCHAR(50)) AS Result;
END