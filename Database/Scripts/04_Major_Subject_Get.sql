/* =============================================================================
   04. TRA CỨU ĐỐI TƯỢNG - CÓ PHÂN QUYỀN DỮ LIỆU
   -----------------------------------------------------------------------------
   Thay thế proc p_Major_Subject_Get cũ. Bổ sung:
     - Phân quyền dữ liệu theo đơn vị khai báo và theo lĩnh vực.
     - Tra cứu tách theo 3 tiêu chí: số CCCD, họ tên, hành vi vi phạm.

   Quy tắc hiển thị:
     - Chỉ thấy đối tượng do đơn vị trong phạm vi của mình khai báo.
     - VÀ đối tượng đó phải có ít nhất một lần vi phạm thuộc lĩnh vực mà người
       dùng được phân quản lý.
     - Super admin thấy toàn bộ dữ liệu.

   Tham số phải giữ ĐÚNG THỨ TỰ vì tầng C# gọi theo vị trí (positional).
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
          AND (@IsSuperAdmin = 1
               OR s.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))
               /* Cho phép thấy đối tượng nếu có lần vi phạm do đơn vị trong phạm vi khai báo */
               OR EXISTS (
                   SELECT 1
                   FROM dbo.Major_SubjectViolations AS v
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))))

          /* --- PHÂN QUYỀN: đối tượng phải có vi phạm thuộc lĩnh vực được phân công --- */
          AND (@IsSuperAdmin = 1
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
           AND (@IsSuperAdmin = 1
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
