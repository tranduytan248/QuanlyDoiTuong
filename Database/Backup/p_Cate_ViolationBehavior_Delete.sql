CREATE PROCEDURE dbo.p_Cate_ViolationBehavior_Delete
    @BehaviorId INT,
    @UserName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.Cate_ViolationBehaviors
    SET IsDeleted = 1,
        DeletedDate = GETDATE(),
        DeletedBy = @UserName
    WHERE BehaviorId = @BehaviorId;
    SELECT @BehaviorId AS Result;
END