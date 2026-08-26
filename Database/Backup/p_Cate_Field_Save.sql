CREATE PROCEDURE dbo.p_Cate_Field_Save
    @FieldId INT = NULL,
    @FieldCode NVARCHAR(50),
    @FieldName NVARCHAR(255),
    @Description NVARCHAR(1000) = NULL,
    @IsActive BIT = 1,
    @UserName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra trùng mã
    IF EXISTS (
        SELECT 1 FROM dbo.Cate_Fields
        WHERE FieldCode = @FieldCode
          AND IsDeleted = 0
          AND (@FieldId IS NULL OR @FieldId = 0 OR FieldId <> @FieldId)
    )
    BEGIN
        SELECT -1 AS Result; -- -1: Trùng dữ liệu (Existed)
        RETURN;
    END

    IF (@FieldId IS NULL OR @FieldId = 0)
    BEGIN
        INSERT INTO dbo.Cate_Fields (FieldCode, FieldName, Description, IsActive, IsDeleted, CreatedDate, CreatedBy)
        VALUES (@FieldCode, @FieldName, @Description, @IsActive, 0, GETDATE(), @UserName);
        SELECT SCOPE_IDENTITY() AS Result;
    END
    ELSE
    BEGIN
        UPDATE dbo.Cate_Fields
        SET FieldCode = @FieldCode,
            FieldName = @FieldName,
            Description = @Description,
            IsActive = @IsActive,
            UpdatedDate = GETDATE(),
            UpdatedBy = @UserName
        WHERE FieldId = @FieldId;
        SELECT @FieldId AS Result;
    END
END