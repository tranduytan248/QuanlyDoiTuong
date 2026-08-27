/* =============================================================================
   29. CHUAN HOA SO CCCD CU VE DUNG 12 CHU SO
   -----------------------------------------------------------------------------
   Script 27 da them rang buoc CCCD phai gom dung 12 chu so, nhung 4 ban ghi
   nhap tu truoc van sai dinh dang:
       4444444444      (10 so)  hhhhh
       1238712981      (10 so)  Le Dinh Minh Tri
       1617171718191   (13 so)  Nguyen Ngoc Chau Hoang
       0581276237126   (13 so)  Tran Thien Long

   De nguyen thi cac ban ghi nay se bao loi ngay khi ai do mo ra sua roi luu.

   QUY TAC CHUAN HOA:
     - Ngan hon 12 so -> dem so 0 vao DAU cho du 12
     - Dai hon 12 so  -> cat lay 12 so DAU

   LUU Y: so sau khi chuan hoa la suy ra tu du lieu cu, KHONG phai CCCD that
   cua nguoi do. Da doi chieu truoc khi chay: khong so nao trung voi CCCD dang
   co trong he thong.

   Ghi lai thay doi vao Major_Subject_ChangeLog de truy vet duoc ve sau.
   ============================================================================= */

SET NOCOUNT ON;
BEGIN TRANSACTION;
BEGIN TRY

/* Gom cac ban ghi can sua kem so moi */
DECLARE @Fix TABLE
(
    SubjectId  UNIQUEIDENTIFIER,
    OldCard    NVARCHAR(50),
    NewCard    NVARCHAR(50),
    FullName   NVARCHAR(250)
);

INSERT INTO @Fix (SubjectId, OldCard, NewCard, FullName)
SELECT s.SubjectId,
       s.IdentityCardNumber,
       CASE
           WHEN LEN(s.IdentityCardNumber) < 12
                THEN RIGHT(REPLICATE('0', 12) + s.IdentityCardNumber, 12)
           ELSE LEFT(s.IdentityCardNumber, 12)
       END,
       s.FullName
  FROM dbo.Major_Subjects AS s
 WHERE ISNULL(s.IsDeleted, 0) = 0
   AND LEN(s.IdentityCardNumber) <> 12;

IF NOT EXISTS (SELECT 1 FROM @Fix)
BEGIN
    PRINT N'Khong co ban ghi nao sai dinh dang - khong can sua.';
    COMMIT TRANSACTION;
    RETURN;
END

/* Chan truong hop so moi trung voi CCCD dang co */
IF EXISTS (SELECT 1
             FROM @Fix AS f
            INNER JOIN dbo.Major_Subjects AS s
                    ON s.IdentityCardNumber = f.NewCard
                   AND s.SubjectId <> f.SubjectId
                   AND ISNULL(s.IsDeleted, 0) = 0)
BEGIN
    THROW 50001, N'So sau chuan hoa bi trung voi CCCD dang co - da dung, khong sua gi.', 1;
END

/* Ghi log truoc khi doi de con doi chieu ve sau */
INSERT INTO dbo.Major_Subject_ChangeLog
    (SubjectId, EntityType, ActionType, ChangedFields, ChangedFieldNames,
     Description, ActorUserName, ActorName, ActorPosition, ActorUnit, CreatedDate)
SELECT f.SubjectId,
       'Subject',
       'Update',
       'IdentityCardNumber',
       N'Số CCCD',
       N'Chuẩn hoá số CCCD về đúng 12 chữ số: [' + f.OldCard + N'] thành [' + f.NewCard + N']',
       'script29',
       N'Chuẩn hoá dữ liệu',
       N'Hệ thống',
       N'Hệ thống',
       GETDATE()
  FROM @Fix AS f;

/* Doi so */
UPDATE s
   SET s.IdentityCardNumber = f.NewCard,
       s.UpdatedDate        = GETDATE(),
       s.UpdatedBy          = 'script29'
  FROM dbo.Major_Subjects AS s
 INNER JOIN @Fix AS f ON f.SubjectId = s.SubjectId;

DECLARE @Count INT = (SELECT COUNT(*) FROM @Fix);

COMMIT TRANSACTION;
PRINT N'Hoan tat: da chuan hoa ' + CAST(@Count AS NVARCHAR(10)) + N' so CCCD ve dung 12 chu so.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    PRINT N'LOI - da huy toan bo thay doi:';
    PRINT ERROR_MESSAGE();
    THROW;
END CATCH
