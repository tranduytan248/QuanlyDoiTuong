/* =============================================================================
   18. PHAN QUYEN LINH VUC AP DUNG CHO MOI TAI KHOAN - KE CA QUAN TRI
   -----------------------------------------------------------------------------
   VAN DE: tai khoan trunglc.kha chua duoc phan linh vuc nao (bang Sys_User_Field
   khong co dong nao), nhung van thay va thao tac duoc ca 5 linh vuc. Ly do: tai
   khoan nay nam trong cau hinh CONFIG_SUPER_ADMIN_PERMIT, ma ca tang ung dung
   lan tang CSDL deu cho super admin bo qua MOI gioi han pham vi du lieu.

   QUYET DINH: phan quyen linh vuc la phan cong nghiep vu, khong phai quyen he
   thong. Ai chua duoc giao linh vuc thi khong co gi de thao tac - ke ca quan tri.

   PHAM VI SUA: chi bo dac quyen super admin o dieu kien loc LINH VUC.
   Dac quyen theo DON VI van giu nguyen: quan tri van xem duoc du lieu cua moi
   don vi, nhung chi trong nhung linh vuc ho duoc giao.

   Sua 3 cho:
     - p_Major_Subject_Get            (danh sach doi tuong)
     - p_Major_SubjectViolation_Get   (danh sach vi pham)
     - p_Major_SubjectViolation_GetBySubjectId (lich su vi pham cua 1 doi tuong)

   Tang ung dung da sua tuong ung trong SubjectController.LoadPermittedCatalogs
   va SubjectViolationController.LoadPermittedCatalogs.
   ============================================================================= */

IF OBJECT_ID('dbo.p_Major_Subject_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Get;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Get
    @IdentityCardNumber NVARCHAR(50)   = NULL,   -- Tra cứu theo số CCCD
    @FullName           NVARCHAR(200)  = NULL,   -- Tra cứu theo họ tên
    @BehaviorIds        NVARCHAR(MAX)  = NULL,   -- Tra cứu theo hành vi vi phạm (danh sách id, phân tách bởi dấu phẩy)
    @Gender             NVARCHAR(20)   = NULL,
    @UserName           NVARCHAR(100)  = NULL,   -- Người đang đăng nhập, dùng để phân quyền dữ liệu
    @Search             NVARCHAR(500)  = NULL,   -- Ô tìm kiếm nhanh của DataTables
    @Order              NVARCHAR(10)   = '0',
    @OrderDir           NVARCHAR(10)   = 'ASC',
    @StartIndex         INT            = 0,
    @PageSize           INT            = -1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);

    /* @UserName IS NULL nghia la KHONG gioi han pham vi du lieu.
       Dung cho cac tra cuu noi bo cua he thong, vi du kiem tra trung so CCCD
       khi them moi doi tuong - luc do can nhin thay toan bo du lieu. */
    DECLARE @NoScope BIT = CASE WHEN @UserName IS NULL THEN 1 ELSE 0 END;

    /* Chuẩn hoá tham số đầu vào */
    SET @IdentityCardNumber = NULLIF(LTRIM(RTRIM(ISNULL(@IdentityCardNumber, ''))), '');
    SET @FullName           = NULLIF(LTRIM(RTRIM(ISNULL(@FullName, ''))), '');
    SET @BehaviorIds        = NULLIF(LTRIM(RTRIM(ISNULL(@BehaviorIds, ''))), '');
    SET @Gender             = NULLIF(LTRIM(RTRIM(ISNULL(@Gender, ''))), '');
    SET @Search             = NULLIF(LTRIM(RTRIM(ISNULL(@Search, ''))), '');

    /* Tách danh sách id hành vi vi phạm thành bảng tạm */
    DECLARE @TblBehaviors TABLE (BehaviorId INT PRIMARY KEY);
    IF @BehaviorIds IS NOT NULL
    BEGIN
        INSERT INTO @TblBehaviors (BehaviorId)
        SELECT DISTINCT TRY_CAST(LTRIM(RTRIM(value)) AS INT)
        FROM STRING_SPLIT(@BehaviorIds, ',')
        WHERE TRY_CAST(LTRIM(RTRIM(value)) AS INT) IS NOT NULL;
    END

    DECLARE @HasBehaviorFilter BIT = CASE WHEN EXISTS (SELECT 1 FROM @TblBehaviors) THEN 1 ELSE 0 END;

    ;WITH ScopedSubject AS
    (
        SELECT
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
            s.CurrentResidence,
            s.PhoneNumber,
            s.AvatarUrl,
            s.IdentityCardFrontUrl,
            s.IdentityCardBackUrl,
            s.ReporterName,
            s.ReporterUnit,
            s.ReporterPhone,
            s.ReporterPosition,
            s.CreatedDate,
            s.CreatedBy,
            s.UpdatedDate,
            s.UpdatedBy
        FROM dbo.Major_Subjects AS s
        WHERE ISNULL(s.IsDeleted, 0) = 0

          /* --- Lọc theo tiêu chí tra cứu --- */
          AND (@IdentityCardNumber IS NULL OR s.IdentityCardNumber LIKE '%' + @IdentityCardNumber + '%')
          AND (@FullName IS NULL OR s.FullName LIKE N'%' + @FullName + '%' OR s.OtherName LIKE N'%' + @FullName + '%')
          AND (@Gender IS NULL OR s.Gender = @Gender)
          AND (@Search IS NULL
               OR s.IdentityCardNumber LIKE '%' + @Search + '%'
               OR s.FullName LIKE N'%' + @Search + '%'
               OR s.PhoneNumber LIKE '%' + @Search + '%'
               OR s.CurrentResidence LIKE N'%' + @Search + '%')

          /* --- Lọc theo hành vi vi phạm --- */
          AND (@HasBehaviorFilter = 0 OR EXISTS (
                  SELECT 1
                  FROM dbo.Major_SubjectViolations AS v
                  INNER JOIN dbo.Major_SubjectViolation_Behaviors AS vb ON vb.ViolationId = v.ViolationId
                  INNER JOIN @TblBehaviors AS tb ON tb.BehaviorId = vb.BehaviorId
                  WHERE v.SubjectId = s.SubjectId AND ISNULL(v.IsDeleted, 0) = 0))

          /* --- PHÂN QUYỀN: đơn vị khai báo phải nằm trong phạm vi của người dùng --- */
          AND (@NoScope = 1 OR @IsSuperAdmin = 1
               OR s.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))
               /* Cho phép thấy đối tượng nếu có lần vi phạm do đơn vị trong phạm vi khai báo */
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))))

          /* --- PHÂN QUYỀN: đối tượng phải có vi phạm thuộc lĩnh vực được phân công --- */
          AND (@NoScope = 1
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   INNER JOIN dbo.Major_SubjectViolation_Behaviors AS vb ON vb.ViolationId = v.ViolationId
                   INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND b.FieldId IN (SELECT FieldId FROM dbo.fn_GetPermittedFields(@UserName))))
    ),
    Counted AS
    (
        SELECT sc.*, COUNT(*) OVER () AS TotalRow
        FROM ScopedSubject AS sc
    )
    SELECT
        c.SubjectId,
        c.IdentityCardNumber,
        c.FullName,
        c.OtherName,
        c.DateOfBirth,
        c.Gender,
        c.Ethnicity,
        c.Religion,
        c.Nationality,
        c.PlaceOfOrigin,
        c.CurrentResidence,
        c.PhoneNumber,
        c.AvatarUrl,
        c.IdentityCardFrontUrl,
        c.IdentityCardBackUrl,
        c.ReporterName,
        c.ReporterUnit,
        c.ReporterPhone,
        c.ReporterPosition,
        c.CreatedDate,
        c.CreatedBy,
        c.UpdatedDate,
        c.UpdatedBy,
        /* Số lần vi phạm - chỉ đếm những lần người dùng được phép xem */
        (SELECT COUNT(1)
         FROM dbo.Major_SubjectViolations AS v
         WHERE v.SubjectId = c.SubjectId
           AND ISNULL(v.IsDeleted, 0) = 0
           AND (@NoScope = 1 OR @IsSuperAdmin = 1
                OR v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))
        ) AS ViolationCount,
        c.TotalRow
    FROM Counted AS c
    /* Chỉ số cột khớp với bảng hiển thị trên giao diện:
       0=STT, 1=Thông tin đối tượng (CCCD + Họ tên), 2=Giới tính,
       3=Ngày sinh, 4=Quê quán, 5=Ảnh chân dung, 6=Thao tác */
    ORDER BY
        CASE WHEN @Order = '1' AND @OrderDir = 'ASC'  THEN c.IdentityCardNumber END ASC,
        CASE WHEN @Order = '1' AND @OrderDir = 'DESC' THEN c.IdentityCardNumber END DESC,
        CASE WHEN @Order = '2' AND @OrderDir = 'ASC'  THEN c.Gender END ASC,
        CASE WHEN @Order = '2' AND @OrderDir = 'DESC' THEN c.Gender END DESC,
        CASE WHEN @Order = '3' AND @OrderDir = 'ASC'  THEN c.DateOfBirth END ASC,
        CASE WHEN @Order = '3' AND @OrderDir = 'DESC' THEN c.DateOfBirth END DESC,
        CASE WHEN @Order = '4' AND @OrderDir = 'ASC'  THEN c.PlaceOfOrigin END ASC,
        CASE WHEN @Order = '4' AND @OrderDir = 'DESC' THEN c.PlaceOfOrigin END DESC,
        c.CreatedDate DESC
    OFFSET (CASE WHEN @StartIndex < 0 THEN 0 ELSE @StartIndex END) ROWS
    FETCH NEXT (CASE WHEN @PageSize IS NULL OR @PageSize <= 0 THEN 2147483647 ELSE @PageSize END) ROWS ONLY;
END
GO


IF OBJECT_ID('dbo.p_Major_SubjectViolation_Save', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_SubjectViolation_Save;
GO

CREATE PROCEDURE dbo.p_Major_SubjectViolation_Save
    @ViolationId       UNIQUEIDENTIFIER = NULL,
    @SubjectId         UNIQUEIDENTIFIER,
    @ViolationDate     DATETIME,
    @TreatmentMeasures NVARCHAR(MAX)    = NULL,
    @RelatedDocuments  NVARCHAR(MAX)    = NULL,
    @Images            NVARCHAR(MAX)    = NULL,
    @Notes             NVARCHAR(MAX)    = NULL,
    /* --- Thông tin người khai báo: không hiển thị trên giao diện, chỉ lưu xuống --- */
    @ReporterName      NVARCHAR(200)    = NULL,
    @ReporterUnit      NVARCHAR(500)    = NULL,
    @ReporterPosition  NVARCHAR(200)    = NULL,
    @ReporterPhone     NVARCHAR(50)     = NULL,
    @ReporterUnionId   UNIQUEIDENTIFIER = NULL,
    @UserName          NVARCHAR(100)    = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF @ViolationId IS NULL OR @ViolationId = '00000000-0000-0000-0000-000000000000'
    BEGIN
        SET @ViolationId = NEWID();

        INSERT INTO dbo.Major_SubjectViolations
        (
            ViolationId, SubjectId, ViolationDate, TreatmentMeasures, RelatedDocuments,
            Images, Notes, ReporterName, ReporterUnit, ReporterPosition, ReporterPhone,
            ReporterUnionId, IsDeleted, CreatedDate, CreatedBy
        )
        VALUES
        (
            @ViolationId, @SubjectId, @ViolationDate, @TreatmentMeasures, @RelatedDocuments,
            @Images, @Notes, @ReporterName, @ReporterUnit, @ReporterPosition, @ReporterPhone,
            @ReporterUnionId, 0, GETDATE(), @UserName
        );
    END
    ELSE
    BEGIN
        UPDATE dbo.Major_SubjectViolations
        SET SubjectId         = @SubjectId,
            ViolationDate     = @ViolationDate,
            TreatmentMeasures = @TreatmentMeasures,
            RelatedDocuments  = @RelatedDocuments,
            Images            = @Images,
            Notes             = @Notes,
            /* Chỉ ghi đè thông tin khai báo khi có truyền vào, tránh xoá mất dữ liệu cũ */
            ReporterName      = ISNULL(@ReporterName,     ReporterName),
            ReporterUnit      = ISNULL(@ReporterUnit,     ReporterUnit),
            ReporterPosition  = ISNULL(@ReporterPosition, ReporterPosition),
            ReporterPhone     = ISNULL(@ReporterPhone,    ReporterPhone),
            ReporterUnionId   = ISNULL(@ReporterUnionId,  ReporterUnionId),
            UpdatedDate       = GETDATE(),
            UpdatedBy         = @UserName
        WHERE ViolationId = @ViolationId;
    END

    SELECT CAST(@ViolationId AS NVARCHAR(50)) AS ViolationId;
END
GO


/* -----------------------------------------------------------------------------
   5.2. DANH SÁCH LỊCH SỬ VI PHẠM - CÓ PHÂN QUYỀN
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_SubjectViolation_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_SubjectViolation_Get;
GO

CREATE PROCEDURE dbo.p_Major_SubjectViolation_Get
    @Key        NVARCHAR(500)    = NULL,
    @SubjectId  UNIQUEIDENTIFIER = NULL,
    @FieldId    INT              = NULL,
    @UserName   NVARCHAR(100)    = NULL,   -- Dùng để phân quyền dữ liệu
    @Search     NVARCHAR(500)    = NULL,
    @Order      NVARCHAR(10)     = '0',
    @OrderDir   NVARCHAR(10)     = 'DESC',
    @StartIndex INT              = 0,
    @PageSize   INT              = -1
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);

    SET @Key    = NULLIF(LTRIM(RTRIM(ISNULL(@Key, ''))), '');
    SET @Search = NULLIF(LTRIM(RTRIM(ISNULL(@Search, ''))), '');

    ;WITH ScopedViolation AS
    (
        SELECT
            v.ViolationId,
            v.SubjectId,
            s.FullName           AS SubjectName,
            s.IdentityCardNumber,
            s.PhoneNumber,
            v.ViolationDate,
            v.TreatmentMeasures,
            v.RelatedDocuments,
            v.Images,
            v.Notes,
            v.ReporterName,
            v.ReporterUnit,
            v.ReporterPosition,
            v.ReporterPhone,
            v.CreatedDate,
            v.CreatedBy,
            v.UpdatedDate,
            v.UpdatedBy,
            /* Gom tên các hành vi của lần vi phạm này thành một chuỗi */
            STUFF((SELECT N', ' + b2.BehaviorName
                   FROM dbo.Major_SubjectViolation_Behaviors AS vb2
                   INNER JOIN dbo.Cate_ViolationBehaviors AS b2 ON b2.BehaviorId = vb2.BehaviorId
                   WHERE vb2.ViolationId = v.ViolationId
                   FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS BehaviorNames
        FROM dbo.Major_SubjectViolations AS v
        INNER JOIN dbo.Major_Subjects AS s ON s.SubjectId = v.SubjectId
        WHERE ISNULL(v.IsDeleted, 0) = 0

          AND (@SubjectId IS NULL OR v.SubjectId = @SubjectId)

          AND (@Key IS NULL
               OR s.FullName LIKE N'%' + @Key + '%'
               OR s.IdentityCardNumber LIKE '%' + @Key + '%')

          AND (@Search IS NULL
               OR s.FullName LIKE N'%' + @Search + '%'
               OR s.IdentityCardNumber LIKE '%' + @Search + '%'
               OR v.TreatmentMeasures LIKE N'%' + @Search + '%')

          /* Lọc theo lĩnh vực người dùng chọn trên giao diện */
          AND (@FieldId IS NULL OR EXISTS (
                  SELECT 1
                  FROM dbo.Major_SubjectViolation_Behaviors AS vb
                  INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                  WHERE vb.ViolationId = v.ViolationId AND b.FieldId = @FieldId))

          /* --- PHÂN QUYỀN theo đơn vị khai báo (đệ quy toàn bộ cây đơn vị con) --- */
          AND (@IsSuperAdmin = 1
               OR v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))

          /* --- PHÂN QUYỀN theo lĩnh vực được phân công --- */
          AND (EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolation_Behaviors AS vb
                   INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
                   WHERE vb.ViolationId = v.ViolationId
                     AND b.FieldId IN (SELECT FieldId FROM dbo.fn_GetPermittedFields(@UserName))))
    ),
    Counted AS
    (
        SELECT sv.*, COUNT(*) OVER () AS TotalRow
        FROM ScopedViolation AS sv
    )
    SELECT *
    FROM Counted
    ORDER BY
        CASE WHEN @OrderDir = 'ASC' THEN ViolationDate END ASC,
        CASE WHEN @OrderDir <> 'ASC' THEN ViolationDate END DESC
    OFFSET (CASE WHEN @StartIndex < 0 THEN 0 ELSE @StartIndex END) ROWS
    FETCH NEXT (CASE WHEN @PageSize IS NULL OR @PageSize <= 0 THEN 2147483647 ELSE @PageSize END) ROWS ONLY;
END
GO


/* -----------------------------------------------------------------------------
   5.3. LỊCH SỬ VI PHẠM CỦA MỘT ĐỐI TƯỢNG - CÓ PHÂN QUYỀN
   ----------------------------------------------------------------------------- */
IF OBJECT_ID('dbo.p_Major_SubjectViolation_GetBySubjectId', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_SubjectViolation_GetBySubjectId;
GO

CREATE PROCEDURE dbo.p_Major_SubjectViolation_GetBySubjectId
    @SubjectId UNIQUEIDENTIFIER,
    @UserName  NVARCHAR(100) = NULL   -- NULL = không giới hạn (dùng cho tác vụ nội bộ)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);
    DECLARE @NoScope      BIT = CASE WHEN @UserName IS NULL THEN 1 ELSE 0 END;

    SELECT
        v.ViolationId,
        v.SubjectId,
        s.FullName           AS SubjectName,
        s.IdentityCardNumber,
        s.PhoneNumber,
        v.ViolationDate,
        v.TreatmentMeasures,
        v.RelatedDocuments,
        v.Images,
        v.Notes,
        v.ReporterName,
        v.ReporterUnit,
        v.ReporterPosition,
        v.ReporterPhone,
        v.ReporterUnionId,
        v.CreatedDate,
        v.CreatedBy,
        v.UpdatedDate,
        v.UpdatedBy,
        STUFF((SELECT N', ' + b2.BehaviorName
               FROM dbo.Major_SubjectViolation_Behaviors AS vb2
               INNER JOIN dbo.Cate_ViolationBehaviors AS b2 ON b2.BehaviorId = vb2.BehaviorId
               WHERE vb2.ViolationId = v.ViolationId
               FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS BehaviorNames,
        /* Danh sách lĩnh vực (không trùng lặp) của các hành vi trong lần vi phạm này */
        STUFF((SELECT DISTINCT N', ' + f2.FieldName
               FROM dbo.Major_SubjectViolation_Behaviors AS vb3
               INNER JOIN dbo.Cate_ViolationBehaviors AS b3 ON b3.BehaviorId = vb3.BehaviorId
               INNER JOIN dbo.Cate_Fields AS f2 ON f2.FieldId = b3.FieldId
               WHERE vb3.ViolationId = v.ViolationId
               FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS FieldNames,
        /* Chỉ đúng tài khoản đã khai báo (hoặc super admin) mới được Sửa / Xoá.
           Đây chỉ là cờ hiển thị nút - quyền thật vẫn được kiểm tra lại ở tầng ứng dụng. */
        CAST(CASE WHEN v.CreatedBy = @UserName OR dbo.fn_IsSuperAdmin(@UserName) = 1
                  THEN 1 ELSE 0 END AS BIT) AS IsOwner
    FROM dbo.Major_SubjectViolations AS v
    INNER JOIN dbo.Major_Subjects AS s ON s.SubjectId = v.SubjectId
    WHERE v.SubjectId = @SubjectId
      AND ISNULL(v.IsDeleted, 0) = 0

      AND (@NoScope = 1 OR @IsSuperAdmin = 1
           OR v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName)))

      AND (@NoScope = 1
           OR EXISTS (
               SELECT 1
               FROM dbo.Major_SubjectViolation_Behaviors AS vb
               INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
               WHERE vb.ViolationId = v.ViolationId
                 AND b.FieldId IN (SELECT FieldId FROM dbo.fn_GetPermittedFields(@UserName))))
    ORDER BY v.ViolationDate DESC, v.CreatedDate DESC;
END
GO

