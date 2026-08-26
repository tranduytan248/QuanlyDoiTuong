CREATE PROCEDURE dbo.p_Cate_ViolationBehavior_Save
    @BehaviorId INT = NULL,
    @FieldId INT,
    @BehaviorCode NVARCHAR(50),
    @BehaviorName NVARCHAR(500),
    @Description NVARCHAR(1000) = NULL,
    @IsActive BIT = 1,
    @UserName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra trùng mã hành vi trong cùng lĩnh vực
    IF EXISTS (
        SELECT 1 FROM dbo.Cate_ViolationBehaviors
        WHERE FieldId = @FieldId
          AND BehaviorCode = @BehaviorCode
          AND IsDeleted = 0
          AND (@BehaviorId IS NULL OR @BehaviorId = 0 OR BehaviorId <> @BehaviorId)
    )
    BEGIN
        SELECT -1 AS Result;
        RETURN;
    END

    IF (@BehaviorId IS NULL OR @BehaviorId = 0)
    BEGIN
        INSERT INTO dbo.Cate_ViolationBehaviors (FieldId, BehaviorCode, BehaviorName, Description, IsActive, IsDeleted, CreatedDate, CreatedBy)
        VALUES (@FieldId, @BehaviorCode, @BehaviorName, @Description, @IsActive, 0, GETDATE(), @UserName);
        SELECT SCOPE_IDENTITY() AS Result;
    END
    ELSE
    BEGIN
        UPDATE dbo.Cate_ViolationBehaviors
        SET FieldId = @FieldId,
            BehaviorCode = @BehaviorCode,
            BehaviorName = @BehaviorName,
            Description = @Description,
            IsActive = @IsActive,
            UpdatedDate = GETDATE(),
            UpdatedBy = @UserName
        WHERE BehaviorId = @BehaviorId;
        SELECT @BehaviorId AS Result;
    END
END