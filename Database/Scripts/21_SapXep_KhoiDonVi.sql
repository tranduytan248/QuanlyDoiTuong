/* =============================================================================
   21. SAP XEP CAY DON VI THEO MO HINH 2 CAP CO PHAN KHOI
   -----------------------------------------------------------------------------
   THEO MO TA: mo hinh 'Bo tinh, tinh toan dien, xa bam co so' - ket thuc cap
   trung gian o dia phuong, van hanh 2 cap: Cap Tinh -> Cap Xa/Phuong.

   THAY DOI:
     1. Tao 3 don vi Khoi lam cha cho 26 phong nghiep vu:
          Khoi Tham muu - Hau can - Xay dung luc luong  (7 phong)
          Khoi An ninh                                  (8 phong)
          Khoi Canh sat                                 (11 phong)
     2. Chuyen 26 phong tu truc thuoc Cong an Tinh sang truc thuoc Khoi.
     3. Ban Giam doc GIU NGUYEN truc thuoc Cong an Tinh - la cap chi dao
        toan dien, dung tren cac Khoi.
     4. 65 Cong an xa/phuong GIU NGUYEN truc thuoc Cong an Tinh - dung tinh
        than 2 cap, nhan chi dao truc tiep tu cap tinh.

   KHONG DOI TypeUnion cua bat ky don vi nao: co 24 cho trong code loc theo
   TypeUnion=1 (module Thu tuc / Ho so dang co du lieu that). Doi loai se lam
   cac don vi bien mat khoi nhung man hinh do.

   Cay sau khi sap xep:
     Cong an Tinh
       |- Ban Giam doc Cong an Tinh          (chi dao toan dien)
       |- Khoi Tham muu - Hau can - XDLL --- 7 phong
       |- Khoi An ninh -------------------- 8 phong
       |- Khoi Canh sat ------------------- 11 phong
       |- 65 Cong an xa/phuong ------------ moi don vi 4 to

   AN TOAN: boc trong giao dich, chay lai nhieu lan khong tao trung.
   ============================================================================= */

SET NOCOUNT ON;
BEGIN TRANSACTION;
BEGIN TRY

DECLARE @Tinh UNIQUEIDENTIFIER;
SELECT @Tinh = UnionId FROM dbo.Cate_Unions WHERE UnionName = N'Công an Tỉnh' AND ISNULL(IsDeleted,0)=0;
IF @Tinh IS NULL THROW 50001, N'Khong tim thay Cong an Tinh.', 1;

DECLARE @Now DATETIME = GETDATE();
DECLARE @By VARCHAR(100) = 'script21';
DECLARE @K UNIQUEIDENTIFIER;

/* ===== Khối Tham mưu - Hậu cần - Xây dựng lực lượng ===== */
SET @K = NULL;
SELECT @K = UnionId FROM dbo.Cate_Unions WHERE UnionName=N'Khối Tham mưu - Hậu cần - Xây dựng lực lượng' AND ISNULL(IsDeleted,0)=0;
IF @K IS NULL
BEGIN
    SET @K = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@K, N'Khối Tham mưu - Hậu cần - Xây dựng lực lượng', 2, N'Phòng ban', @Tinh, 'KHOI_TM', 1, 0, @Now, @By);
END
UPDATE dbo.Cate_Unions SET BelongUnion=@K, LastModifiedOn=@Now, LastModifiedBy=@By
 WHERE UnionCode IN ('UBKT', 'PV01', 'PX05', 'PX03', 'PX01', 'PV06', 'PH10') AND ISNULL(IsDeleted,0)=0 AND BelongUnion=@Tinh;

/* ===== Khối An ninh ===== */
SET @K = NULL;
SELECT @K = UnionId FROM dbo.Cate_Unions WHERE UnionName=N'Khối An ninh' AND ISNULL(IsDeleted,0)=0;
IF @K IS NULL
BEGIN
    SET @K = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@K, N'Khối An ninh', 2, N'Phòng ban', @Tinh, 'KHOI_AN', 1, 0, @Now, @By);
END
UPDATE dbo.Cate_Unions SET BelongUnion=@K, LastModifiedOn=@Now, LastModifiedBy=@By
 WHERE UnionCode IN ('PA04', 'PA06', 'PA09', 'PA08', 'PA02', 'PA01', 'PA05', 'PA03') AND ISNULL(IsDeleted,0)=0 AND BelongUnion=@Tinh;

/* ===== Khối Cảnh sát ===== */
SET @K = NULL;
SELECT @K = UnionId FROM dbo.Cate_Unions WHERE UnionName=N'Khối Cảnh sát' AND ISNULL(IsDeleted,0)=0;
IF @K IS NULL
BEGIN
    SET @K = NEWID();
    INSERT INTO dbo.Cate_Unions (UnionId, UnionName, TypeUnion, TypeUnionName, BelongUnion, UnionCode, IsActive, IsDeleted, CreatedOn, CreatedBy)
    VALUES (@K, N'Khối Cảnh sát', 2, N'Phòng ban', @Tinh, 'KHOI_CS', 1, 0, @Now, @By);
END
UPDATE dbo.Cate_Unions SET BelongUnion=@K, LastModifiedOn=@Now, LastModifiedBy=@By
 WHERE UnionCode IN ('PC01', 'PK02', 'PC09', 'PC03', 'TTG', 'PC02', 'PC10', 'PC08', 'PC04', 'PC06', 'PC07') AND ISNULL(IsDeleted,0)=0 AND BelongUnion=@Tinh;

COMMIT TRANSACTION;
PRINT N'Hoan tat: da tao 3 Khoi va chuyen 26 phong vao dung khoi.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT N'LOI - da huy toan bo thay doi:'; PRINT ERROR_MESSAGE(); THROW;
END CATCH
