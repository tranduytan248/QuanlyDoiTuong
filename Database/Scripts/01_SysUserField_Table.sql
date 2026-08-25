/* =============================================================================
   01. BẢNG PHÂN QUYỀN LĨNH VỰC CHO NGƯỜI DÙNG
   -----------------------------------------------------------------------------
   Mục đích : Lưu danh sách lĩnh vực (Cate_Fields) mà một người dùng được phân công
              quản lý. Dùng để giới hạn dữ liệu Đối tượng / Lịch sử vi phạm mà
              người dùng đó được phép nhìn thấy.

   Ghi chú  : Phần phân quyền theo Tổ / Phòng ban tái sử dụng bảng có sẵn
              Cate_Unions_Mangers (proc Cate_Union_Manager_*), không tạo bảng mới.
   ============================================================================= */

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Sys_User_Field')
BEGIN
    CREATE TABLE dbo.Sys_User_Field
    (
        Id          INT IDENTITY(1,1) NOT NULL,
        UserName    NVARCHAR(100)     NOT NULL,
        FieldId     INT               NOT NULL,
        CreatedDate DATETIME          NOT NULL CONSTRAINT DF_Sys_User_Field_CreatedDate DEFAULT (GETDATE()),
        CreatedBy   NVARCHAR(100)     NULL,
        CONSTRAINT PK_Sys_User_Field PRIMARY KEY CLUSTERED (Id),
        CONSTRAINT UQ_Sys_User_Field_UserField UNIQUE (UserName, FieldId)
    );
END
GO

/* Index phục vụ tra cứu lĩnh vực theo người dùng (đường đi nóng của mọi màn hình) */
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Sys_User_Field_UserName' AND object_id = OBJECT_ID('dbo.Sys_User_Field'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Sys_User_Field_UserName
        ON dbo.Sys_User_Field (UserName) INCLUDE (FieldId);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Sys_User_Field_FieldId' AND object_id = OBJECT_ID('dbo.Sys_User_Field'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Sys_User_Field_FieldId
        ON dbo.Sys_User_Field (FieldId) INCLUDE (UserName);
END
GO
