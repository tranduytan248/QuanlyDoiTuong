/* =============================================================================
   19. TAO CONG AN XA/PHUONG CON THIEU CUA TINH KHANH HOA
   -----------------------------------------------------------------------------
   NGUON DU LIEU: bang dbo.XaPhuongKHA co san trong CSDL - 65 xa/phuong sau
   sap xep (Khanh Hoa + Ninh Thuan). Day la du lieu that cua he thong, khong
   phai danh sach tu soan.

   DA CO 4 phuong Nha Trang (STT 1-4). Script nay tao 61 don vi con lai,
   moi don vi kem 4 to truc thuoc - giong het cau truc 4 phuong Nha Trang:
       Ban Chi huy Cong an Phuong
       Bo phan Truc ban va Tiep cong dan
       Canh sat khu vuc (CSKV)
       Canh sat trat tu

   TONG CONG: 61 don vi + 244 to = 305 ban ghi moi.
   Cha cua moi cong an xa/phuong la 'Cong an Tinh'.

   AN TOAN: chay lai nhieu lan khong tao trung (kiem tra ton tai truoc khi chen).
   ============================================================================= */

SET NOCOUNT ON;
BEGIN TRANSACTION;
BEGIN TRY

DECLARE @Parent UNIQUEIDENTIFIER;
SELECT @Parent = UnionId FROM dbo.Cate_Unions
 WHERE UnionName = N'Công an Tỉnh' AND ISNULL(IsDeleted,0)=0;
IF @Parent IS NULL
    THROW 50001, N'Khong tim thay don vi cha ''Cong an Tinh''.', 1;

DECLARE @U UNIQUEIDENTIFIER;
DECLARE @Now DATETIME = GETDATE();
DECLARE @By VARCHAR(100) = 'script19';

/* ---- 5. Công an Phường Bắc Cam Ranh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Bắc Cam Ranh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Bắc Cam Ranh', 1, N'Đơn vị', @Parent, 'CAPBCR', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PBCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PBCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PBCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PBCR', 1, 0, @Now, @By);

/* ---- 6. Công an Phường Cam Ranh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Cam Ranh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Cam Ranh', 1, N'Đơn vị', @Parent, 'CAPCR', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PCR', 1, 0, @Now, @By);

/* ---- 7. Công an Phường Cam Linh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Cam Linh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Cam Linh', 1, N'Đơn vị', @Parent, 'CAPCL', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PCL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PCL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PCL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PCL', 1, 0, @Now, @By);

/* ---- 8. Công an Phường Ba Ngòi ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Ba Ngòi' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Ba Ngòi', 1, N'Đơn vị', @Parent, 'CAPBN', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PBN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PBN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PBN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PBN', 1, 0, @Now, @By);

/* ---- 9. Công an Xã Nam Cam Ranh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Nam Cam Ranh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Nam Cam Ranh', 1, N'Đơn vị', @Parent, 'CAXNCR', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XNCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XNCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XNCR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XNCR', 1, 0, @Now, @By);

/* ---- 10. Công an Phường Ninh Hòa ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Ninh Hòa' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Ninh Hòa', 1, N'Đơn vị', @Parent, 'CAPNH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PNH', 1, 0, @Now, @By);

/* ---- 11. Công an Phường Đông Ninh Hòa ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Đông Ninh Hòa' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Đông Ninh Hòa', 1, N'Đơn vị', @Parent, 'CAPDNH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PDNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PDNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PDNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PDNH', 1, 0, @Now, @By);

/* ---- 12. Công an Phường Hòa Thắng ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Hòa Thắng' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Hòa Thắng', 1, N'Đơn vị', @Parent, 'CAPHT', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PHT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PHT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PHT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PHT', 1, 0, @Now, @By);

/* ---- 13. Công an Xã Bắc Ninh Hòa ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Bắc Ninh Hòa' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Bắc Ninh Hòa', 1, N'Đơn vị', @Parent, 'CAXBNH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XBNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XBNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XBNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XBNH', 1, 0, @Now, @By);

/* ---- 14. Công an Xã Tân Định ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Tân Định' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Tân Định', 1, N'Đơn vị', @Parent, 'CAXTD', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTD', 1, 0, @Now, @By);

/* ---- 15. Công an Xã Nam Ninh Hòa ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Nam Ninh Hòa' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Nam Ninh Hòa', 1, N'Đơn vị', @Parent, 'CAXNNH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XNNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XNNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XNNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XNNH', 1, 0, @Now, @By);

/* ---- 16. Công an Xã Tây Ninh Hòa ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Tây Ninh Hòa' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Tây Ninh Hòa', 1, N'Đơn vị', @Parent, 'CAXTNH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTNH', 1, 0, @Now, @By);

/* ---- 17. Công an Xã Hòa Trí ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Hòa Trí' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Hòa Trí', 1, N'Đơn vị', @Parent, 'CAXHT', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XHT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XHT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XHT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XHT', 1, 0, @Now, @By);

/* ---- 18. Công an Xã Vạn Ninh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Vạn Ninh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Vạn Ninh', 1, N'Đơn vị', @Parent, 'CAXVN', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XVN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XVN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XVN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XVN', 1, 0, @Now, @By);

/* ---- 19. Công an Xã Vạn Hưng ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Vạn Hưng' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Vạn Hưng', 1, N'Đơn vị', @Parent, 'CAXVH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XVH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XVH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XVH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XVH', 1, 0, @Now, @By);

/* ---- 20. Công an Xã Vạn Thắng ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Vạn Thắng' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Vạn Thắng', 1, N'Đơn vị', @Parent, 'CAXVT', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XVT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XVT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XVT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XVT', 1, 0, @Now, @By);

/* ---- 21. Công an Xã Tu Bông ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Tu Bông' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Tu Bông', 1, N'Đơn vị', @Parent, 'CAXTB', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTB', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTB', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTB', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTB', 1, 0, @Now, @By);

/* ---- 22. Công an Xã Đại Lãnh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Đại Lãnh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Đại Lãnh', 1, N'Đơn vị', @Parent, 'CAXDL', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XDL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XDL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XDL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XDL', 1, 0, @Now, @By);

/* ---- 23. Công an Xã Diên Khánh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Diên Khánh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Diên Khánh', 1, N'Đơn vị', @Parent, 'CAXDK', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XDK', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XDK', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XDK', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XDK', 1, 0, @Now, @By);

/* ---- 24. Công an Xã Diên Lạc ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Diên Lạc' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Diên Lạc', 1, N'Đơn vị', @Parent, 'CAXDL2', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XDL2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XDL2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XDL2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XDL2', 1, 0, @Now, @By);

/* ---- 25. Công an Xã Diên Điền ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Diên Điền' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Diên Điền', 1, N'Đơn vị', @Parent, 'CAXDD', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XDD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XDD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XDD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XDD', 1, 0, @Now, @By);

/* ---- 26. Công an Xã Diên Lâm ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Diên Lâm' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Diên Lâm', 1, N'Đơn vị', @Parent, 'CAXDL3', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XDL3', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XDL3', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XDL3', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XDL3', 1, 0, @Now, @By);

/* ---- 27. Công an Xã Diên Thọ ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Diên Thọ' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Diên Thọ', 1, N'Đơn vị', @Parent, 'CAXDT', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XDT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XDT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XDT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XDT', 1, 0, @Now, @By);

/* ---- 28. Công an Xã Suối Hiệp ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Suối Hiệp' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Suối Hiệp', 1, N'Đơn vị', @Parent, 'CAXSH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XSH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XSH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XSH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XSH', 1, 0, @Now, @By);

/* ---- 29. Công an Xã Cam Lâm ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Cam Lâm' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Cam Lâm', 1, N'Đơn vị', @Parent, 'CAXCL', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XCL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XCL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XCL', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XCL', 1, 0, @Now, @By);

/* ---- 30. Công an Xã Suối Dầu ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Suối Dầu' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Suối Dầu', 1, N'Đơn vị', @Parent, 'CAXSD', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XSD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XSD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XSD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XSD', 1, 0, @Now, @By);

/* ---- 31. Công an Xã Cam Hiệp ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Cam Hiệp' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Cam Hiệp', 1, N'Đơn vị', @Parent, 'CAXCH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XCH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XCH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XCH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XCH', 1, 0, @Now, @By);

/* ---- 32. Công an Xã Cam An ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Cam An' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Cam An', 1, N'Đơn vị', @Parent, 'CAXCA', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XCA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XCA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XCA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XCA', 1, 0, @Now, @By);

/* ---- 33. Công an Xã Bắc Khánh Vĩnh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Bắc Khánh Vĩnh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Bắc Khánh Vĩnh', 1, N'Đơn vị', @Parent, 'CAXBKV', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XBKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XBKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XBKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XBKV', 1, 0, @Now, @By);

/* ---- 34. Công an Xã Trung Khánh Vĩnh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Trung Khánh Vĩnh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Trung Khánh Vĩnh', 1, N'Đơn vị', @Parent, 'CAXTKV', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTKV', 1, 0, @Now, @By);

/* ---- 35. Công an Xã Tây Khánh Vĩnh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Tây Khánh Vĩnh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Tây Khánh Vĩnh', 1, N'Đơn vị', @Parent, 'CAXTKV2', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTKV2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTKV2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTKV2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTKV2', 1, 0, @Now, @By);

/* ---- 36. Công an Xã Nam Khánh Vĩnh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Nam Khánh Vĩnh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Nam Khánh Vĩnh', 1, N'Đơn vị', @Parent, 'CAXNKV', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XNKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XNKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XNKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XNKV', 1, 0, @Now, @By);

/* ---- 37. Công an Xã Khánh Vĩnh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Khánh Vĩnh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Khánh Vĩnh', 1, N'Đơn vị', @Parent, 'CAXKV', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XKV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XKV', 1, 0, @Now, @By);

/* ---- 38. Công an Xã Khánh Sơn ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Khánh Sơn' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Khánh Sơn', 1, N'Đơn vị', @Parent, 'CAXKS', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XKS', 1, 0, @Now, @By);

/* ---- 39. Công an Xã Tây Khánh Sơn ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Tây Khánh Sơn' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Tây Khánh Sơn', 1, N'Đơn vị', @Parent, 'CAXTKS', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTKS', 1, 0, @Now, @By);

/* ---- 40. Công an Xã Đông Khánh Sơn ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Đông Khánh Sơn' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Đông Khánh Sơn', 1, N'Đơn vị', @Parent, 'CAXDKS', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XDKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XDKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XDKS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XDKS', 1, 0, @Now, @By);

/* ---- 41. Công an Xã Ninh Phước ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Ninh Phước' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Ninh Phước', 1, N'Đơn vị', @Parent, 'CAXNP', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XNP', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XNP', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XNP', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XNP', 1, 0, @Now, @By);

/* ---- 42. Công an Xã Phước Hữu ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Phước Hữu' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Phước Hữu', 1, N'Đơn vị', @Parent, 'CAXPH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XPH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XPH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XPH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XPH', 1, 0, @Now, @By);

/* ---- 43. Công an Xã Phước Hậu ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Phước Hậu' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Phước Hậu', 1, N'Đơn vị', @Parent, 'CAXPH2', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XPH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XPH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XPH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XPH2', 1, 0, @Now, @By);

/* ---- 44. Công an Xã Thuận Nam ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Thuận Nam' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Thuận Nam', 1, N'Đơn vị', @Parent, 'CAXTN', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTN', 1, 0, @Now, @By);

/* ---- 45. Công an Xã Cà Ná ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Cà Ná' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Cà Ná', 1, N'Đơn vị', @Parent, 'CAXCN', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XCN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XCN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XCN', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XCN', 1, 0, @Now, @By);

/* ---- 46. Công an Xã Phước Hà ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Phước Hà' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Phước Hà', 1, N'Đơn vị', @Parent, 'CAXPH3', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XPH3', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XPH3', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XPH3', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XPH3', 1, 0, @Now, @By);

/* ---- 47. Công an Xã Phước Dinh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Phước Dinh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Phước Dinh', 1, N'Đơn vị', @Parent, 'CAXPD', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XPD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XPD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XPD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XPD', 1, 0, @Now, @By);

/* ---- 48. Công an Xã Ninh Hải ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Ninh Hải' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Ninh Hải', 1, N'Đơn vị', @Parent, 'CAXNH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XNH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XNH', 1, 0, @Now, @By);

/* ---- 49. Công an Xã Xuân Hải ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Xuân Hải' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Xuân Hải', 1, N'Đơn vị', @Parent, 'CAXXH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XXH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XXH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XXH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XXH', 1, 0, @Now, @By);

/* ---- 50. Công an Xã Vĩnh Hải ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Vĩnh Hải' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Vĩnh Hải', 1, N'Đơn vị', @Parent, 'CAXVH2', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XVH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XVH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XVH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XVH2', 1, 0, @Now, @By);

/* ---- 51. Công an Xã Thuận Bắc ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Thuận Bắc' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Thuận Bắc', 1, N'Đơn vị', @Parent, 'CAXTB2', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XTB2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XTB2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XTB2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XTB2', 1, 0, @Now, @By);

/* ---- 52. Công an Xã Công Hải ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Công Hải' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Công Hải', 1, N'Đơn vị', @Parent, 'CAXCH2', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XCH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XCH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XCH2', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XCH2', 1, 0, @Now, @By);

/* ---- 53. Công an Xã Ninh Sơn ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Ninh Sơn' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Ninh Sơn', 1, N'Đơn vị', @Parent, 'CAXNS', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XNS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XNS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XNS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XNS', 1, 0, @Now, @By);

/* ---- 54. Công an Xã Lâm Sơn ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Lâm Sơn' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Lâm Sơn', 1, N'Đơn vị', @Parent, 'CAXLS', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XLS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XLS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XLS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XLS', 1, 0, @Now, @By);

/* ---- 55. Công an Xã Anh Dũng ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Anh Dũng' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Anh Dũng', 1, N'Đơn vị', @Parent, 'CAXAD', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XAD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XAD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XAD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XAD', 1, 0, @Now, @By);

/* ---- 56. Công an Xã Mỹ Sơn ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Mỹ Sơn' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Mỹ Sơn', 1, N'Đơn vị', @Parent, 'CAXMS', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XMS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XMS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XMS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XMS', 1, 0, @Now, @By);

/* ---- 57. Công an Xã Bác Ái Đông ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Bác Ái Đông' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Bác Ái Đông', 1, N'Đơn vị', @Parent, 'CAXBAD', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XBAD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XBAD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XBAD', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XBAD', 1, 0, @Now, @By);

/* ---- 58. Công an Xã Bác Ái ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Bác Ái' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Bác Ái', 1, N'Đơn vị', @Parent, 'CAXBA', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XBA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XBA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XBA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XBA', 1, 0, @Now, @By);

/* ---- 59. Công an Xã Bác Ái Tây ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Xã Bác Ái Tây' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Xã Bác Ái Tây', 1, N'Đơn vị', @Parent, 'CAXBAT', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_XBAT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_XBAT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_XBAT', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_XBAT', 1, 0, @Now, @By);

/* ---- 60. Công an Phường Phan Rang ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Phan Rang' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Phan Rang', 1, N'Đơn vị', @Parent, 'CAPPR', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PPR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PPR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PPR', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PPR', 1, 0, @Now, @By);

/* ---- 61. Công an Phường Đông Hải ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Đông Hải' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Đông Hải', 1, N'Đơn vị', @Parent, 'CAPDH', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PDH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PDH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PDH', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PDH', 1, 0, @Now, @By);

/* ---- 62. Công an Phường Ninh Chử ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Ninh Chử' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Ninh Chử', 1, N'Đơn vị', @Parent, 'CAPNC', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PNC', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PNC', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PNC', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PNC', 1, 0, @Now, @By);

/* ---- 63. Công an Phường Bảo An ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Bảo An' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Bảo An', 1, N'Đơn vị', @Parent, 'CAPBA', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PBA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PBA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PBA', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PBA', 1, 0, @Now, @By);

/* ---- 64. Công an Phường Đô Vinh ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Phường Đô Vinh' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Phường Đô Vinh', 1, N'Đơn vị', @Parent, 'CAPDV', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_PDV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_PDV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_PDV', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_PDV', 1, 0, @Now, @By);

/* ---- 65. Công an Đặc khu Trường Sa ---- */
SET @U = NULL;
SELECT @U = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Đặc khu Trường Sa' AND ISNULL(IsDeleted,0)=0;
IF @U IS NULL
BEGIN
    SET @U = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@U, N'Công an Đặc khu Trường Sa', 1, N'Đơn vị', @Parent, 'CADKTS', 1, 0, @Now, @By);
END
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Ban Chỉ huy Công an Phường' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Ban Chỉ huy Công an Phường', 2, N'Phòng ban', @U, 'BCH_DKTS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Bộ phận Trực ban và Tiếp công dân' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Bộ phận Trực ban và Tiếp công dân', 2, N'Phòng ban', @U, 'BPTBTCD_DKTS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát khu vực (CSKV)' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát khu vực (CSKV)', 2, N'Phòng ban', @U, 'CSKV_DKTS', 1, 0, @Now, @By);
IF NOT EXISTS (SELECT 1 FROM dbo.Cate_Unions WHERE BelongUnion=@U AND UnionName=N'Cảnh sát trật tự' AND ISNULL(IsDeleted,0)=0)
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (NEWID(), N'Cảnh sát trật tự', 2, N'Phòng ban', @U, 'CSTT_DKTS', 1, 0, @Now, @By);

COMMIT TRANSACTION;
PRINT N'Hoan tat: da tao 61 don vi + 244 to truc thuoc.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT N'LOI - da huy toan bo thay doi:';
    PRINT ERROR_MESSAGE();
    THROW;
END CATCH
