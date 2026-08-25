/* =============================================================================
   07. LƯU ĐỐI TƯỢNG - BỔ SUNG KHOÁ ĐƠN VỊ KHAI BÁO
   -----------------------------------------------------------------------------
   Bổ sung tham số @ReporterUnionId (đặt NGAY TRƯỚC @UserName để khớp thứ tự
   truyền tham số của tầng C# - xem MajorSubjectBiz.Save).

   Proc trả về:
     - Chuỗi GUID của đối tượng nếu lưu thành công.
     - Chuỗi 'EXISTED' nếu số CCCD đã tồn tại ở một đối tượng KHÁC.
   ============================================================================= */

IF OBJECT_ID('dbo.p_Major_Subject_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Save;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Save
    @SubjectId             UNIQUEIDENTIFIER = NULL,
    @IdentityCardNumber    NVARCHAR(50),
    @FullName              NVARCHAR(200),
    @OtherName             NVARCHAR(200)    = NULL,
    @DateOfBirth           DATETIME         = NULL,
    @Gender                NVARCHAR(20)     = NULL,
    @Ethnicity             NVARCHAR(100)    = NULL,
    @Religion              NVARCHAR(100)    = NULL,
    @Nationality           NVARCHAR(100)    = NULL,
    @PlaceOfOrigin         NVARCHAR(500)    = NULL,
    @IdentityCardFrontUrl  NVARCHAR(500)    = NULL,
    @IdentityCardBackUrl   NVARCHAR(500)    = NULL,
    @AvatarUrl             NVARCHAR(500)    = NULL,
    @BirthRegistrationPlace NVARCHAR(500)   = NULL,
    @CurrentResidence      NVARCHAR(500)    = NULL,
    @PhoneNumber           NVARCHAR(50)     = NULL,
    @ReporterName          NVARCHAR(200)    = NULL,
    @ReporterUnit          NVARCHAR(500)    = NULL,
    @ReporterPhone         NVARCHAR(50)     = NULL,
    @ReporterPosition      NVARCHAR(200)    = NULL,
    @ReporterUnionId       UNIQUEIDENTIFIER = NULL,
    @UserName              NVARCHAR(100)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SET @IdentityCardNumber = LTRIM(RTRIM(ISNULL(@IdentityCardNumber, '')));

    DECLARE @IsInsert BIT =
        CASE WHEN @SubjectId IS NULL OR @SubjectId = '00000000-0000-0000-0000-000000000000'
             THEN 1 ELSE 0 END;

    /* Chặn trùng số CCCD với một đối tượng khác */
    IF EXISTS (SELECT 1
               FROM dbo.Major_Subjects
               WHERE IdentityCardNumber = @IdentityCardNumber
                 AND ISNULL(IsDeleted, 0) = 0
                 AND (@IsInsert = 1 OR SubjectId <> @SubjectId))
    BEGIN
        SELECT 'EXISTED' AS Result;
        RETURN;
    END

    IF @IsInsert = 1
    BEGIN
        SET @SubjectId = NEWID();

        INSERT INTO dbo.Major_Subjects
        (
            SubjectId, IdentityCardNumber, FullName, OtherName, DateOfBirth, Gender,
            Ethnicity, Religion, Nationality, PlaceOfOrigin, IdentityCardFrontUrl,
            IdentityCardBackUrl, AvatarUrl, BirthRegistrationPlace, CurrentResidence,
            PhoneNumber, ReporterName, ReporterUnit, ReporterPhone, ReporterPosition,
            ReporterUnionId, IsDeleted, CreatedDate, CreatedBy
        )
        VALUES
        (
            @SubjectId, @IdentityCardNumber, @FullName, @OtherName, @DateOfBirth, @Gender,
            @Ethnicity, @Religion, @Nationality, @PlaceOfOrigin, @IdentityCardFrontUrl,
            @IdentityCardBackUrl, @AvatarUrl, @BirthRegistrationPlace, @CurrentResidence,
            @PhoneNumber, @ReporterName, @ReporterUnit, @ReporterPhone, @ReporterPosition,
            @ReporterUnionId, 0, GETDATE(), @UserName
        );
    END
    ELSE
    BEGIN
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
            IdentityCardFrontUrl   = @IdentityCardFrontUrl,
            IdentityCardBackUrl    = @IdentityCardBackUrl,
            AvatarUrl              = @AvatarUrl,
            BirthRegistrationPlace = @BirthRegistrationPlace,
            CurrentResidence       = @CurrentResidence,
            PhoneNumber            = @PhoneNumber,
            ReporterName           = @ReporterName,
            ReporterUnit           = @ReporterUnit,
            ReporterPhone          = @ReporterPhone,
            ReporterPosition       = @ReporterPosition,
            /* Giữ nguyên đơn vị khai báo gốc nếu lần cập nhật này không truyền vào */
            ReporterUnionId        = ISNULL(@ReporterUnionId, ReporterUnionId),
            UpdatedDate            = GETDATE(),
            UpdatedBy              = @UserName
        WHERE SubjectId = @SubjectId;
    END

    SELECT CAST(@SubjectId AS NVARCHAR(50)) AS Result;
END
GO
