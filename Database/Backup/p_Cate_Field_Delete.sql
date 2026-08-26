CREATE PROCEDURE dbo.p_Cate_Field_Delete
    @FieldId INT,
    @UserName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Cate_Fields
    SET IsDeleted = 1,
        DeletedDate = GETDATE(),
        DeletedBy = @UserName
    WHERE FieldId = @FieldId;
    SELECT @FieldId AS Result;
END