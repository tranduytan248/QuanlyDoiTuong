/* =============================================================================
   15. SỬA LỖI THÊM / CẬP NHẬT DANH MỤC LĨNH VỰC & HÀNH VI VI PHẠM
   -----------------------------------------------------------------------------
   TRIỆU CHỨNG: bấm Lưu ở màn hình Danh mục Lĩnh vực (hoặc Hành vi vi phạm) luôn
   báo "Cập nhật ... thất bại", dù dữ liệu vẫn được ghi xuống bảng.

   NGUYÊN NHÂN: tầng khung đọc kết quả proc bằng giá trị RETURN, không phải
   bằng câu SELECT:

       Plugable.SQLStoreProcedure/SQLStoreProcedure.cs
           public int? ExecuteProcedure(...)
           {
               sp.Execute();
               var outputSp = (int?)sp.OutputValues[0];   // <-- doc RETURN
               return outputSp ?? -1;
           }

   Hai proc dưới đây lại trả kết quả bằng "SELECT ... AS Result" nên tầng ứng
   dụng luôn nhận về -1 và hiểu là thất bại. Các proc cũ của hệ thống
   (vi du p_Cate_Category_Save) đều dùng RETURN - đây mới là quy ước đúng.

   CÁCH SỬA: đổi sang RETURN, giữ nguyên toàn bộ logic nghiệp vụ.
   Quy ước giá trị trả về:
       > 0  : thành công, là khoá chính của bản ghi
       -1   : trùng mã (Existed)
   ============================================================================= */

/* -----------------------------------------------------------------------------
   15.1. Danh mục Lĩnh vực
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_Field_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_Field_Save;
GO

CREATE PROCEDURE dbo.p_Cate_Field_Save
    @FieldId     INT           = NULL,
    @FieldCode   NVARCHAR(50),
    @FieldName   NVARCHAR(255),
    @Description NVARCHAR(1000) = NULL,
    @IsActive    BIT           = 1,
    @UserName    VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    /* Chặn trùng mã lĩnh vực */
    IF EXISTS (
        SELECT 1 FROM dbo.Cate_Fields
        WHERE FieldCode = @FieldCode
          AND ISNULL(IsDeleted, 0) = 0
          AND (@FieldId IS NULL OR @FieldId = 0 OR FieldId <> @FieldId)
    )
    BEGIN
        RETURN -1;   /* -1: trùng dữ liệu */
    END

    IF (@FieldId IS NULL OR @FieldId = 0)
    BEGIN
        INSERT INTO dbo.Cate_Fields
            (FieldCode, FieldName, Description, IsActive, IsDeleted, CreatedDate, CreatedBy)
        VALUES
            (@FieldCode, @FieldName, @Description, @IsActive, 0, GETDATE(), @UserName);

        DECLARE @NewId INT = CAST(SCOPE_IDENTITY() AS INT);
        RETURN @NewId;
    END

    UPDATE dbo.Cate_Fields
    SET FieldCode   = @FieldCode,
        FieldName   = @FieldName,
        Description = @Description,
        IsActive    = @IsActive,
        UpdatedDate = GETDATE(),
        UpdatedBy   = @UserName
    WHERE FieldId = @FieldId;

    RETURN @FieldId;
END
GO


/* -----------------------------------------------------------------------------
   15.2. Danh mục Hành vi vi phạm - cùng lỗi, sửa theo cùng cách
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_ViolationBehavior_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_ViolationBehavior_Save;
GO

CREATE PROCEDURE dbo.p_Cate_ViolationBehavior_Save
    @BehaviorId   INT            = NULL,
    @FieldId      INT,
    @BehaviorCode NVARCHAR(50),
    @BehaviorName NVARCHAR(500),
    @Description  NVARCHAR(1000) = NULL,
    @IsActive     BIT            = 1,
    @UserName     VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    /* Chặn trùng mã hành vi trong cùng lĩnh vực */
    IF EXISTS (
        SELECT 1 FROM dbo.Cate_ViolationBehaviors
        WHERE BehaviorCode = @BehaviorCode
          AND FieldId = @FieldId
          AND ISNULL(IsDeleted, 0) = 0
          AND (@BehaviorId IS NULL OR @BehaviorId = 0 OR BehaviorId <> @BehaviorId)
    )
    BEGIN
        RETURN -1;
    END

    IF (@BehaviorId IS NULL OR @BehaviorId = 0)
    BEGIN
        INSERT INTO dbo.Cate_ViolationBehaviors
            (FieldId, BehaviorCode, BehaviorName, Description, IsActive, IsDeleted, CreatedDate, CreatedBy)
        VALUES
            (@FieldId, @BehaviorCode, @BehaviorName, @Description, @IsActive, 0, GETDATE(), @UserName);

        DECLARE @NewBehaviorId INT = CAST(SCOPE_IDENTITY() AS INT);
        RETURN @NewBehaviorId;
    END

    UPDATE dbo.Cate_ViolationBehaviors
    SET FieldId      = @FieldId,
        BehaviorCode = @BehaviorCode,
        BehaviorName = @BehaviorName,
        Description  = @Description,
        IsActive     = @IsActive,
        UpdatedDate  = GETDATE(),
        UpdatedBy    = @UserName
    WHERE BehaviorId = @BehaviorId;

    RETURN @BehaviorId;
END
GO


/* -----------------------------------------------------------------------------
   15.3. Proc xoá - kiểm tra cùng lỗi
   Tang Biz so sanh: result == model.FieldId, nen cung phai RETURN khoa chinh.
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Cate_Field_Delete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_Field_Delete;
GO

CREATE PROCEDURE dbo.p_Cate_Field_Delete
    @FieldId  INT,
    @UserName VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Fields WHERE FieldId = @FieldId)
        RETURN 0;

    /* Xoá mềm, giữ lại dữ liệu để không hỏng các bản ghi đang tham chiếu */
    UPDATE dbo.Cate_Fields
    SET IsDeleted   = 1,
        UpdatedDate = GETDATE(),
        UpdatedBy   = @UserName
    WHERE FieldId = @FieldId;

    RETURN @FieldId;
END
GO


IF OBJECT_ID('dbo.p_Cate_ViolationBehavior_Delete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Cate_ViolationBehavior_Delete;
GO

CREATE PROCEDURE dbo.p_Cate_ViolationBehavior_Delete
    @BehaviorId INT,
    @UserName   VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Cate_ViolationBehaviors WHERE BehaviorId = @BehaviorId)
        RETURN 0;

    UPDATE dbo.Cate_ViolationBehaviors
    SET IsDeleted   = 1,
        UpdatedDate = GETDATE(),
        UpdatedBy   = @UserName
    WHERE BehaviorId = @BehaviorId;

    RETURN @BehaviorId;
END
GO
