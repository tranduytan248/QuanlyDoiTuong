
CREATE PROCEDURE dbo.p_Major_Subject_Save
    @SubjectId UNIQUEIDENTIFIER = NULL,
    @IdentityCardNumber VARCHAR(20),
    @FullName NVARCHAR(255),
    @OtherName NVARCHAR(255) = NULL,
    @DateOfBirth DATE = NULL,
    @Gender NVARCHAR(10) = NULL,
    @Ethnicity NVARCHAR(50) = NULL,
    @Religion NVARCHAR(50) = NULL,
    @Nationality NVARCHAR(100) = N'Việt Nam',
    @PlaceOfOrigin NVARCHAR(500) = NULL,
    @IdentityCardFrontUrl NVARCHAR(500) = NULL,
    @IdentityCardBackUrl NVARCHAR(500) = NULL,
    @AvatarUrl NVARCHAR(500) = NULL,
    @BirthRegistrationPlace NVARCHAR(500) = NULL,
    @CurrentResidence NVARCHAR(500) = NULL,
    @PhoneNumber VARCHAR(20) = NULL,
    @ReporterName NVARCHAR(255) = NULL,
    @ReporterUnit NVARCHAR(255) = NULL,
    @ReporterPhone VARCHAR(50) = NULL,
    @ReporterPosition NVARCHAR(255) = NULL,
    @UserName VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra trùng số CCCD
    IF EXISTS (
        SELECT 1 FROM dbo.Major_Subjects
        WHERE IdentityCardNumber = @IdentityCardNumber
          AND IsDeleted = 0
          AND (@SubjectId IS NULL OR @SubjectId = '00000000-0000-0000-0000-000000000000' OR SubjectId <> @SubjectId)
    )
    BEGIN
        SELECT 'EXISTED' AS Result;
        RETURN;
    END

    IF (@SubjectId IS NULL OR @SubjectId = '00000000-0000-0000-0000-000000000000')
    BEGIN
        DECLARE @NewId UNIQUEIDENTIFIER = NEWID();
        INSERT INTO dbo.Major_Subjects (
            SubjectId, IdentityCardNumber, FullName, OtherName, DateOfBirth, Gender,
            Ethnicity, Religion, Nationality, PlaceOfOrigin, IdentityCardFrontUrl, IdentityCardBackUrl,
            AvatarUrl, BirthRegistrationPlace, CurrentResidence, PhoneNumber,
            ReporterName, ReporterUnit, ReporterPhone, ReporterPosition,
            IsDeleted, CreatedDate, CreatedBy
        )
        VALUES (
            @NewId, @IdentityCardNumber, @FullName, @OtherName, @DateOfBirth, @Gender,
            @Ethnicity, @Religion, @Nationality, @PlaceOfOrigin, @IdentityCardFrontUrl, @IdentityCardBackUrl,
            @AvatarUrl, @BirthRegistrationPlace, @CurrentResidence, @PhoneNumber,
            @ReporterName, @ReporterUnit, @ReporterPhone, @ReporterPosition,
            0, GETDATE(), @UserName
        );
        SELECT CAST(@NewId AS VARCHAR(50)) AS Result;
    END
    ELSE
    BEGIN
        UPDATE dbo.Major_Subjects
        SET IdentityCardNumber = @IdentityCardNumber,
            FullName = @FullName,
            OtherName = @OtherName,
            DateOfBirth = @DateOfBirth,
            Gender = @Gender,
            Ethnicity = @Ethnicity,
            Religion = @Religion,
            Nationality = @Nationality,
            PlaceOfOrigin = @PlaceOfOrigin,
            IdentityCardFrontUrl = ISNULL(@IdentityCardFrontUrl, IdentityCardFrontUrl),
            IdentityCardBackUrl = ISNULL(@IdentityCardBackUrl, IdentityCardBackUrl),
            AvatarUrl = ISNULL(@AvatarUrl, AvatarUrl),
            BirthRegistrationPlace = @BirthRegistrationPlace,
            CurrentResidence = @CurrentResidence,
            PhoneNumber = @PhoneNumber,
            ReporterName = @ReporterName,
            ReporterUnit = @ReporterUnit,
            ReporterPhone = @ReporterPhone,
            ReporterPosition = @ReporterPosition,
            UpdatedDate = GETDATE(),
            UpdatedBy = @UserName
        WHERE SubjectId = @SubjectId;
        SELECT CAST(@SubjectId AS VARCHAR(50)) AS Result;
    END
END
