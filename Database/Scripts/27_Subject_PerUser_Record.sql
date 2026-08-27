/* =============================================================================
   27. MOT CCCD - MOI TAI KHOAN MOT BAN GHI RIENG
   -----------------------------------------------------------------------------
   YEU CAU NGHIEP VU:
     - Cung mot nguoi them lai doi tuong da co (theo CCCD) -> CAP NHAT ban ghi
       cua chinh ho.
     - Nguoi KHAC them cung CCCD do -> TAO ban ghi moi, khong dung chung.
   Ly do: moi can bo tu quan ly ho so doi tuong minh nhap, khong ghi de len du
   lieu nguoi khac.

   THAY DOI: proc cu chan trung CCCD tren TOAN he thong (tra 'EXISTED') nen
   nguoi thu hai khong nhap duoc. Nay chi chan trung trong pham vi cung mot
   tai khoan tao.

   GIU NGUYEN chu ky tham so cua proc goc - khung goi theo vi tri tham so nen
   doi thu tu se lam hong tang goi.

   Bo sung p_Major_Subject_LookupByCard phuc vu goi y khi nhap CCCD.
   ============================================================================= */

IF OBJECT_ID('dbo.p_Major_Subject_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Save;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Save
    @SubjectId               UNIQUEIDENTIFIER,
    @IdentityCardNumber      NVARCHAR(50),
    @FullName                NVARCHAR(250),
    @OtherName               NVARCHAR(250),
    @DateOfBirth             DATETIME,
    @Gender                  NVARCHAR(20),
    @Ethnicity               NVARCHAR(100),
    @Religion                NVARCHAR(100),
    @Nationality             NVARCHAR(100),
    @PlaceOfOrigin           NVARCHAR(500),
    @IdentityCardFrontUrl    NVARCHAR(500),
    @IdentityCardBackUrl     NVARCHAR(500),
    @AvatarUrl               NVARCHAR(500),
    @BirthRegistrationPlace  NVARCHAR(500),
    @CurrentResidence        NVARCHAR(500),
    @PhoneNumber             NVARCHAR(50),
    @ReporterName            NVARCHAR(250),
    @ReporterUnit            NVARCHAR(500),
    @ReporterPhone           NVARCHAR(50),
    @ReporterPosition        NVARCHAR(250),
    @ReporterUnionId         UNIQUEIDENTIFIER,
    @UserName                NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentityCardNumber = LTRIM(RTRIM(ISNULL(@IdentityCardNumber, '')));
    IF @IdentityCardNumber = ''
    BEGIN
        SELECT 'INVALID' AS Result;
        RETURN;
    END

    /* ---------------------------- THEM MOI ---------------------------- */
    IF @SubjectId IS NULL OR @SubjectId = '00000000-0000-0000-0000-000000000000'
    BEGIN
        /* Chi chan trung trong pham vi CUNG MOT tai khoan tao.
           Nguoi khac them cung CCCD thi van duoc tao ban ghi rieng. */
        DECLARE @MineId UNIQUEIDENTIFIER;

        SELECT TOP 1 @MineId = SubjectId
          FROM dbo.Major_Subjects
         WHERE IdentityCardNumber = @IdentityCardNumber
           AND CreatedBy = @UserName
           AND ISNULL(IsDeleted, 0) = 0;

        IF @MineId IS NOT NULL
            /* Da co ban ghi cua chinh minh -> chuyen sang cap nhat ban ghi do */
            SET @SubjectId = @MineId;
        ELSE
        BEGIN
            SET @SubjectId = NEWID();

            INSERT INTO dbo.Major_Subjects
                (SubjectId, IdentityCardNumber, FullName, OtherName, DateOfBirth, Gender,
                 Ethnicity, Religion, Nationality, PlaceOfOrigin,
                 IdentityCardFrontUrl, IdentityCardBackUrl, AvatarUrl,
                 BirthRegistrationPlace, CurrentResidence, PhoneNumber,
                 ReporterName, ReporterUnit, ReporterPhone, ReporterPosition,
                 ReporterUnionId, IsDeleted, CreatedDate, CreatedBy)
            VALUES
                (@SubjectId, @IdentityCardNumber, @FullName, @OtherName, @DateOfBirth, @Gender,
                 @Ethnicity, @Religion, @Nationality, @PlaceOfOrigin,
                 @IdentityCardFrontUrl, @IdentityCardBackUrl, @AvatarUrl,
                 @BirthRegistrationPlace, @CurrentResidence, @PhoneNumber,
                 @ReporterName, @ReporterUnit, @ReporterPhone, @ReporterPosition,
                 @ReporterUnionId, 0, GETDATE(), @UserName);

            SELECT CAST(@SubjectId AS NVARCHAR(50)) AS Result;
            RETURN;
        END
    END

    /* ---------------------------- CAP NHAT ---------------------------- */
    UPDATE dbo.Major_Subjects
       SET IdentityCardNumber     = @IdentityCardNumber,
           FullName               = @FullName,
           OtherName              = @OtherName,
           DateOfBirth            = @DateOfBirth,
           Gender                 = @Gender,
           Ethnicity              = @Ethnicity,
           Religion               = @Religion,
           Nationality            = @Nationality,
           PlaceOfOrigin          = @PlaceOfOrigin,
           BirthRegistrationPlace = @BirthRegistrationPlace,
           CurrentResidence       = @CurrentResidence,
           PhoneNumber            = @PhoneNumber,
           /* Anh chi ghi de khi lan luu nay co tai len anh moi */
           AvatarUrl              = ISNULL(@AvatarUrl, AvatarUrl),
           IdentityCardFrontUrl   = ISNULL(@IdentityCardFrontUrl, IdentityCardFrontUrl),
           IdentityCardBackUrl    = ISNULL(@IdentityCardBackUrl, IdentityCardBackUrl),
           UpdatedDate            = GETDATE(),
           UpdatedBy              = @UserName
     WHERE SubjectId = @SubjectId;

    SELECT CAST(@SubjectId AS NVARCHAR(50)) AS Result;
END
GO

/* ------------------------- Tra cuu theo CCCD phuc vu goi y tren giao dien */
IF OBJECT_ID('dbo.p_Major_Subject_LookupByCard', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_LookupByCard;
GO

CREATE PROCEDURE dbo.p_Major_Subject_LookupByCard
    @IdentityCardNumber NVARCHAR(50),
    @UserName           NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET @IdentityCardNumber = LTRIM(RTRIM(ISNULL(@IdentityCardNumber, '')));
    IF @IdentityCardNumber = '' RETURN;

    /* Uu tien ban ghi cua chinh nguoi dang nhap - do la ban ho duoc sua.
       Khong co thi lay ban moi nhat de goi y thong tin da biet. */
    SELECT TOP 1
           s.SubjectId,
           s.IdentityCardNumber,
           s.FullName,
           s.OtherName,
           s.DateOfBirth,
           s.Gender,
           s.Ethnicity,
           s.Religion,
           s.Nationality,
           s.PlaceOfOrigin,
           s.BirthRegistrationPlace,
           s.CurrentResidence,
           s.PhoneNumber,
           s.AvatarUrl,
           s.IdentityCardFrontUrl,
           s.IdentityCardBackUrl,
           s.CreatedBy,
           s.CreatedDate,
           s.ReporterName,
           s.ReporterUnit,

           /* Ban ghi nay co phai cua chinh nguoi dang nhap khong */
           CAST(CASE WHEN s.CreatedBy = @UserName THEN 1 ELSE 0 END AS BIT) AS IsMine,

           /* Tong so ban ghi cua CCCD nay tren toan he thong */
           (SELECT COUNT(*) FROM dbo.Major_Subjects AS s2
             WHERE s2.IdentityCardNumber = @IdentityCardNumber
               AND ISNULL(s2.IsDeleted, 0) = 0) AS TotalRecords

      FROM dbo.Major_Subjects AS s
     WHERE s.IdentityCardNumber = @IdentityCardNumber
       AND ISNULL(s.IsDeleted, 0) = 0
     ORDER BY CASE WHEN s.CreatedBy = @UserName THEN 0 ELSE 1 END,
              s.CreatedDate DESC;
END
GO
