/* =============================================================================
   02. BỔ SUNG THÔNG TIN NGƯỜI KHAI BÁO CHO LỊCH SỬ VI PHẠM
   -----------------------------------------------------------------------------
   Mục đích : Mỗi lần vi phạm phải lưu lại người khai báo và đơn vị khai báo tại
              thời điểm ghi nhận. Thông tin này KHÔNG hiển thị trên giao diện
              nhập liệu, chỉ lưu xuống CSDL và dùng để phân quyền xem dữ liệu
              (lịch sử vi phạm do tổ nào khai báo).

   Lưu ý    : ReporterUnionId là khoá đơn vị khai báo - đây là cột dùng để lọc
              dữ liệu theo tổ / phòng ban, nên bắt buộc phải có index.
   ============================================================================= */

IF COL_LENGTH('dbo.Major_SubjectViolations', 'ReporterName') IS NULL
    ALTER TABLE dbo.Major_SubjectViolations ADD ReporterName NVARCHAR(200) NULL;
GO

IF COL_LENGTH('dbo.Major_SubjectViolations', 'ReporterUnit') IS NULL
    ALTER TABLE dbo.Major_SubjectViolations ADD ReporterUnit NVARCHAR(500) NULL;
GO

IF COL_LENGTH('dbo.Major_SubjectViolations', 'ReporterPosition') IS NULL
    ALTER TABLE dbo.Major_SubjectViolations ADD ReporterPosition NVARCHAR(200) NULL;
GO

IF COL_LENGTH('dbo.Major_SubjectViolations', 'ReporterPhone') IS NULL
    ALTER TABLE dbo.Major_SubjectViolations ADD ReporterPhone NVARCHAR(50) NULL;
GO

/* Khoá đơn vị khai báo - dùng để phân quyền dữ liệu theo tổ / phòng ban */
IF COL_LENGTH('dbo.Major_SubjectViolations', 'ReporterUnionId') IS NULL
    ALTER TABLE dbo.Major_SubjectViolations ADD ReporterUnionId UNIQUEIDENTIFIER NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Major_SubjectViolation_ReporterUnionId' AND object_id = OBJECT_ID('dbo.Major_SubjectViolations'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Major_SubjectViolation_ReporterUnionId
        ON dbo.Major_SubjectViolations (ReporterUnionId);
END
GO

/* -----------------------------------------------------------------------------
   Bổ sung khoá đơn vị khai báo cho bảng Đối tượng.
   Bảng Major_Subjects đã có sẵn ReporterName / ReporterUnit / ReporterPosition /
   ReporterPhone (dạng chuỗi hiển thị), nhưng chưa có khoá đơn vị để lọc dữ liệu.
   ----------------------------------------------------------------------------- */
IF COL_LENGTH('dbo.Major_Subjects', 'ReporterUnionId') IS NULL
    ALTER TABLE dbo.Major_Subjects ADD ReporterUnionId UNIQUEIDENTIFIER NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Major_Subject_ReporterUnionId' AND object_id = OBJECT_ID('dbo.Major_Subjects'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Major_Subject_ReporterUnionId
        ON dbo.Major_Subjects (ReporterUnionId);
END
GO
