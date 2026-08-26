/* =============================================================================
   20. BO SUNG CAC PHONG BAN CAP TINH TRUC THUOC CONG AN TINH
   -----------------------------------------------------------------------------
   NGUON: danh sach co cau to chuc do nguoi dung cung cap.
   Gom 26 don vi, chia 3 khoi:
     - Khoi Tham muu, Chinh tri, Hau can : 7 don vi
     - Khoi An ninh                      : 8 don vi
     - Khoi Canh sat                     : 11 don vi

   DA CO SAN 2 don vi (PC02, PC04) - script bo qua, khong tao trung.
   Script nay tao 24 don vi con lai.

   Cha cua tat ca: 'Cong an Tinh'. TypeUnion = 2 (Phong ban) - dung cung muc
   voi Ban Giam doc, PC02, PC04 dang co.

   AN TOAN: chay lai nhieu lan khong tao trung.
   ============================================================================= */

SET NOCOUNT ON;
BEGIN TRANSACTION;
BEGIN TRY

DECLARE @Parent UNIQUEIDENTIFIER;
SELECT @Parent = UnionId FROM dbo.Cate_Unions
 WHERE UnionName = N'Công an Tỉnh' AND ISNULL(IsDeleted,0)=0;
IF @Parent IS NULL
    THROW 50001, N'Khong tim thay don vi cha ''Cong an Tinh''.', 1;

DECLARE @Now DATETIME = GETDATE();
DECLARE @By VARCHAR(100) = 'script20';

/* ===== Khối Tham mưu, Chính trị, Hậu cần ===== */
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Tham mưu (PV01)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Tham mưu (PV01)', 2, N'Phòng ban', @Parent, 'PV01', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Tổ chức cán bộ (PX01)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Tổ chức cán bộ (PX01)', 2, N'Phòng ban', @Parent, 'PX01', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Công tác đảng và công tác chính trị (PX03)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Công tác đảng và công tác chính trị (PX03)', 2, N'Phòng ban', @Parent, 'PX03', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Cơ quan Ủy ban Kiểm tra Đảng ủy Công an tỉnh' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cơ quan Ủy ban Kiểm tra Đảng ủy Công an tỉnh', 2, N'Phòng ban', @Parent, 'UBKT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Thanh tra Công an tỉnh (PX05)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Thanh tra Công an tỉnh (PX05)', 2, N'Phòng ban', @Parent, 'PX05', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Hậu cần (PH10)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Hậu cần (PH10)', 2, N'Phòng ban', @Parent, 'PH10', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Hồ sơ nghiệp vụ (PV06)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Hồ sơ nghiệp vụ (PV06)', 2, N'Phòng ban', @Parent, 'PV06', 1, 0, @Now, @By);
/* ===== Khối An ninh ===== */
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng An ninh đối ngoại (PA01)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng An ninh đối ngoại (PA01)', 2, N'Phòng ban', @Parent, 'PA01', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng An ninh đối nội (PA02)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng An ninh đối nội (PA02)', 2, N'Phòng ban', @Parent, 'PA02', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng An ninh chính trị nội bộ (PA03)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng An ninh chính trị nội bộ (PA03)', 2, N'Phòng ban', @Parent, 'PA03', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng An ninh kinh tế (PA04)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng An ninh kinh tế (PA04)', 2, N'Phòng ban', @Parent, 'PA04', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng An ninh mạng và phòng, chống tội phạm sử dụng công nghệ cao (PA05)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng An ninh mạng và phòng, chống tội phạm sử dụng công nghệ cao (PA05)', 2, N'Phòng ban', @Parent, 'PA05', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Kỹ thuật nghiệp vụ (PA06)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Kỹ thuật nghiệp vụ (PA06)', 2, N'Phòng ban', @Parent, 'PA06', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Quản lý xuất nhập cảnh (PA08)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Quản lý xuất nhập cảnh (PA08)', 2, N'Phòng ban', @Parent, 'PA08', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng An ninh điều tra (PA09)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng An ninh điều tra (PA09)', 2, N'Phòng ban', @Parent, 'PA09', 1, 0, @Now, @By);
/* ===== Khối Cảnh sát ===== */
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Văn phòng Cơ quan Cảnh sát điều tra (PC01)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Văn phòng Cơ quan Cảnh sát điều tra (PC01)', 2, N'Phòng ban', @Parent, 'PC01', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Cảnh sát điều tra tội phạm về tham nhũng, kinh tế, buôn lậu, môi trường (PC03)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Cảnh sát điều tra tội phạm về tham nhũng, kinh tế, buôn lậu, môi trường (PC03)', 2, N'Phòng ban', @Parent, 'PC03', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Kỹ thuật hình sự (PC09)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Kỹ thuật hình sự (PC09)', 2, N'Phòng ban', @Parent, 'PC09', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Cảnh sát quản lý hành chính về trật tự xã hội (PC06)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Cảnh sát quản lý hành chính về trật tự xã hội (PC06)', 2, N'Phòng ban', @Parent, 'PC06', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Cảnh sát giao thông (PC08)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Cảnh sát giao thông (PC08)', 2, N'Phòng ban', @Parent, 'PC08', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Cảnh sát Phòng cháy, chữa cháy và Cứu nạn, cứu hộ (PC07)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Cảnh sát Phòng cháy, chữa cháy và Cứu nạn, cứu hộ (PC07)', 2, N'Phòng ban', @Parent, 'PC07', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Cảnh sát cơ động (PK02)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Cảnh sát cơ động (PK02)', 2, N'Phòng ban', @Parent, 'PK02', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Phòng Cảnh sát thi hành án hình sự và hỗ trợ tư pháp (PC10)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Phòng Cảnh sát thi hành án hình sự và hỗ trợ tư pháp (PC10)', 2, N'Phòng ban', @Parent, 'PC10', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE UnionName=N'Trại tạm giam Công an tỉnh' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Trại tạm giam Công an tỉnh', 2, N'Phòng ban', @Parent, 'TTG', 1, 0, @Now, @By);

COMMIT TRANSACTION;
PRINT N'Hoan tat: da xu ly 24 phong ban cap tinh.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT N'LOI - da huy toan bo thay doi:';
    PRINT ERROR_MESSAGE();
    THROW;
END CATCH
