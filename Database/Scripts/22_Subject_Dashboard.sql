/* =============================================================================
   22. THONG KE TONG HOP CHO MAN HINH QUAN LY DOI TUONG
   -----------------------------------------------------------------------------
   Cung cap so lieu cho khoi 4 the thong ke o dau man hinh:
       1. Tong doi tuong          - trong pham vi nguoi dung duoc phep xem
       2. Tong luot vi pham       - tat ca lan ghi nhan
       3. Doi tuong da linh vuc   - vi pham tu 2 linh vuc tro len (can chu y)
       4. Vi pham 30 ngay qua     - muc do hoat dong gan day

   PHAN QUYEN: dung dung bo loc nhu p_Major_Subject_Get - gioi han theo don vi
   (de quy toan cay) va theo linh vuc duoc phan cong. Nguoi chua duoc phan linh
   vuc nao thi moi so lieu deu bang 0.
   ============================================================================= */

IF OBJECT_ID('dbo.p_Major_Subject_Dashboard', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Major_Subject_Dashboard;
GO

CREATE PROCEDURE dbo.p_Major_Subject_Dashboard
    @UserName NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NoScope BIT = CASE WHEN @UserName IS NULL THEN 1 ELSE 0 END;
    DECLARE @IsSuperAdmin BIT = dbo.fn_IsSuperAdmin(@UserName);

    /* Cac doi tuong nguoi dung duoc phep nhin thay - dung dung quy tac phan
       quyen cua p_Major_Subject_Get: don vi khai bao phai trong pham vi, VA
       doi tuong phai co vi pham thuoc linh vuc duoc phan cong. */
    ;WITH Visible AS (
        SELECT s.SubjectId
        FROM dbo.Major_Subjects AS s
        WHERE ISNULL(s.IsDeleted, 0) = 0

          AND (@NoScope = 1 OR @IsSuperAdmin = 1
               OR s.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))
               OR EXISTS (
                   SELECT 1 FROM dbo.Major_SubjectViolations AS v
                   WHERE v.SubjectId = s.SubjectId
                     AND ISNULL(v.IsDeleted, 0) = 0
                     AND v.ReporterUnionId IN (SELECT UnionId FROM dbo.fn_GetPermittedUnions(@UserName))))

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
    /* Cac lan vi pham thuoc pham vi - chi tinh tren doi tuong nhin thay duoc */
    VisibleViolations AS (
        SELECT v.ViolationId, v.SubjectId, v.ViolationDate
        FROM dbo.Major_SubjectViolations AS v
        INNER JOIN Visible AS s ON s.SubjectId = v.SubjectId
        WHERE ISNULL(v.IsDeleted, 0) = 0
    )
    SELECT
        /* 1. Tong doi tuong */
        (SELECT COUNT(*) FROM Visible) AS TotalSubjects,

        /* 2. Tong luot vi pham */
        (SELECT COUNT(*) FROM VisibleViolations) AS TotalViolations,

        /* 3. Doi tuong vi pham tu 2 linh vuc tro len */
        (SELECT COUNT(*) FROM (
            SELECT vv.SubjectId
            FROM VisibleViolations AS vv
            INNER JOIN dbo.Major_SubjectViolation_Behaviors AS vb ON vb.ViolationId = vv.ViolationId
            INNER JOIN dbo.Cate_ViolationBehaviors AS b ON b.BehaviorId = vb.BehaviorId
            GROUP BY vv.SubjectId
            HAVING COUNT(DISTINCT b.FieldId) >= 2
         ) AS x) AS MultiFieldSubjects,

        /* 4. Vi pham trong 30 ngay gan nhat */
        (SELECT COUNT(*) FROM VisibleViolations
          WHERE ViolationDate >= DATEADD(DAY, -30, GETDATE())) AS RecentViolations;
END
GO
