/* =============================================================================
   23. BO SUNG THONG TIN LIEN QUAN CHO MAN HINH QUAN LY NGUOI DUNG
   -----------------------------------------------------------------------------
   VAN DE: luoi Nguoi dung chi hien Ho ten, Email, Dien thoai va Tai khoan.
   Thieu nhung thong tin can de quan tri thuc su: nguoi do thuoc don vi nao,
   chuc vu gi, co vai tro gi, da duoc phan linh vuc chua, dang hoat dong khong.

   Dac biet quan trong: nguoi dung CHUA duoc phan linh vuc nao thi khong thao
   tac duoc gi trong he thong (theo quy tac o script 18). Truoc day khong co
   cach nao nhin thay dieu do tu man hinh danh sach.

   BO SUNG 6 cot:
       UnionName     - don vi cong tac
       PositionName  - chuc vu
       RoleNames     - danh sach vai tro, phan tach bang dau phay
       FieldCount    - so linh vuc duoc phan (0 = chua phan, can canh bao)
       IsLocked      - tai khoan co bi khoa khong
       IsOnline      - dang truc tuyen

   GIU NGUYEN toan bo logic loc / sap xep / phan trang cua proc goc. Chi them
   cot vao menh de SELECT cuoi cung bang cac truy van con, khong dung vao phan
   UNION ALL ben trong - de tranh lam sai thu tu sap xep dang chay dung.
   ============================================================================= */

IF OBJECT_ID('dbo.p_Sys_User_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Sys_User_Get;
GO

CREATE PROCEDURE [dbo].[p_Sys_User_Get]
(
    @Search        NVARCHAR(250)
   ,@Order         VARCHAR(3)
   ,@OrderDir      VARCHAR(10)
   ,@PageIndex     INT
   ,@PageSize      INT
)
AS
BEGIN
    SET NOCOUNT ON
    SET @Order = ISNULL(@Order ,'0')
    SET @OrderDir = ISNULL(@OrderDir ,'ASC')
    SET @PageIndex = ISNULL(@PageIndex ,0)
    SET @PageSize = ISNULL(@PageSize ,10)
    ;WITH T AS(
        SELECT ROW_NUMBER() OVER(
                   ORDER BY
                   CASE @Order
                        WHEN N'0' THEN d.UserName
                   END ASC
                  ,CASE @Order
                        WHEN N'1' THEN d.UserName
                   END ASC
                  ,CASE @Order
                        WHEN N'2' THEN d.Email
                   END ASC
               )          AS RowIndex , *
               FROM (
               Select  DISTINCT
               d.UserId
              ,d.UserName
              ,d.FullName
              ,d.Email
              ,d.[Password]
              ,d.Salt
              ,d.Avatar
              ,d.Phone
              ,d.IsActive
              ,ISNULL(d.IsLocked, 0) AS IsLocked
              ,ISNULL(d.IsOnline, 0) AS IsOnline
              ,CASE WHEN mdl.CreatedBy IS NULL THEN 0 ELSE 1 END AS "Processing"
        FROM  Sys_Users  d LEFT JOIN Major_Dossiers_Logs mdl ON d.UserName = mdl.CreatedBy
        WHERE  (
                   @Search IS NULL
                   OR d.Email LIKE N'%'+@Search+'%'
                   OR d.FullName LIKE N'%'+@Search+'%'
                   OR d.UserName LIKE N'%'+@Search+'%'
                   OR d.Phone LIKE N'%'+@Search+'%'
               )
               AND d.IsDeleted = 0
               AND UPPER(@OrderDir) = 'ASC'
               ) as d

        UNION ALL

        SELECT ROW_NUMBER() OVER(
                   ORDER BY
                   CASE @Order
                        WHEN N'0' THEN d.UserName
                   END DESC
                  ,CASE @Order
                        WHEN N'1' THEN d.UserName
                   END DESC
                  ,CASE @Order
                        WHEN N'2' THEN d.Email
                   END DESC
               )          AS RowIndex, *
              FROM (
              select DISTINCT
              d.UserId
              ,d.UserName
              ,d.FullName
              ,d.Email
              ,d.[Password]
              ,d.Salt
              ,d.Avatar
              ,d.Phone
              ,d.IsActive
              ,ISNULL(d.IsLocked, 0) AS IsLocked
              ,ISNULL(d.IsOnline, 0) AS IsOnline
              ,CASE WHEN mdl.CreatedBy IS NULL THEN 0 ELSE 1 END AS "Processing"
        FROM  Sys_Users  d LEFT JOIN Major_Dossiers_Logs mdl ON d.UserName = mdl.CreatedBy AND d.IsDeleted = 0
        WHERE  (
                   @Search IS NULL
                   OR d.UserName LIKE N'%'+@Search+'%'
                   OR d.FullName LIKE N'%'+@Search+'%'
                   OR d.Email LIKE N'%'+@Search+'%'
                   OR d.Phone LIKE N'%'+@Search+'%'
               )
                AND d.IsDeleted = 0
               AND UPPER(@OrderDir) = 'DESC'
              )  as d
    )

    -- search and return records
    SELECT T.*
          ,(
               SELECT COUNT(RowIndex)
               FROM   T
           ) AS TotalRow

          /* --- Don vi cong tac --- */
          ,(SELECT TOP 1 u.UnionName
            FROM dbo.Cate_Unions_Members AS m
            INNER JOIN dbo.Cate_Unions AS u ON u.UnionId = m.BelongUnion
            WHERE m.UserName = T.UserName
              AND ISNULL(u.IsDeleted, 0) = 0) AS UnionName

          /* --- Chuc vu --- */
          ,(SELECT TOP 1 p.PositionName
            FROM dbo.Cate_Unions_Members AS m
            INNER JOIN dbo.Cate_Position AS p ON p.PositionID = m.PositionId
            WHERE m.UserName = T.UserName
              AND ISNULL(p.IsDeleted, 0) = 0) AS PositionName

          /* --- Vai tro he thong, phan tach bang dau phay --- */
          ,STUFF((SELECT N', ' + r.Name
                  FROM dbo.Sys_UserRoles AS ur
                  INNER JOIN dbo.Sys_Roles AS r ON r.RoleId = ur.RoleId
                  WHERE ur.UserId = T.UserId
                    AND ISNULL(r.IsDeleted, 0) = 0
                  FOR XML PATH(''), TYPE).value('.', 'NVARCHAR(MAX)'), 1, 2, '') AS RoleNames

          /* --- So linh vuc duoc phan. 0 nghia la chua thao tac duoc gi --- */
          ,(SELECT COUNT(*)
            FROM dbo.Sys_User_Field AS uf
            INNER JOIN dbo.Cate_Fields AS f ON f.FieldId = uf.FieldId
            WHERE uf.UserName = T.UserName
              AND ISNULL(f.IsDeleted, 0) = 0
              AND ISNULL(f.IsActive, 1) = 1) AS FieldCount

    FROM   T
    WHERE  (
               @PageSize>0
               AND T.RowIndex BETWEEN @PageIndex+1 AND @PageIndex+@PageSize
           )
           OR @PageSize<= 0
END
GO
