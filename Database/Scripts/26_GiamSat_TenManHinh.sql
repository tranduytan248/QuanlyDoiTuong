/* =============================================================================
   26. LAY TEN MAN HINH TU BANG MENU CHO MAN HINH GIAM SAT TRUC TUYEN
   -----------------------------------------------------------------------------
   VAN DE: ten man hinh dang duoc doi tu bang anh xa cung trong Global.asax.
   Bang do chi phu 14 duong dan, trong khi he thong co 47 man hinh trong menu.
   Duong dan khong khop se hien nguyen dang /Sys/UserActivity - kho doc.

   CACH SUA: doc ten truc tiep tu bang Sys_Menus theo cot Link. Nho vay:
     - Ten hien thi luon TRUNG voi ten nguoi dung thay tren menu
     - Them man hinh moi vao menu la tu dong co ten, khong phai sua code
     - Doi ten menu thi man hinh giam sat doi theo

   Van giu ScreenName do ung dung gui len lam phuong an du phong cho nhung
   duong dan khong co trong menu (vi du trang chu, dang nhap).
   ============================================================================= */

IF OBJECT_ID('dbo.p_Sys_UserActivity_Get', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Sys_UserActivity_Get;
GO

CREATE PROCEDURE dbo.p_Sys_UserActivity_Get
    @TimeoutMinutes INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    SET @TimeoutMinutes = ISNULL(@TimeoutMinutes, 5);

    DECLARE @Now DATETIME = GETDATE();
    DECLARE @Limit DATETIME = DATEADD(MINUTE, -@TimeoutMinutes, @Now);

    SELECT a.SessionId,
           a.UserName,
           u.UserId,
           u.FullName,
           u.Email,
           u.Phone,
           u.Avatar,
           a.CurrentUrl,

           /* Ten man hinh: uu tien lay tu menu de trung voi ten nguoi dung thay.
              Khong co trong menu thi dung ten ung dung gui len, cuoi cung moi
              den duong dan tho. */
           ISNULL(
               (SELECT TOP 1 m.Name
                  FROM dbo.Sys_Menus AS m
                 WHERE ISNULL(m.IsDelete, 0) = 0
                   AND m.Link IS NOT NULL
                   AND m.Link <> '#'
                   AND (m.Link = a.CurrentUrl
                        OR '/' + m.Link = a.CurrentUrl)
                 ORDER BY m.MenuId),
               ISNULL(a.ScreenName, a.CurrentUrl)) AS ScreenName,

           a.IpAddress,
           a.LoginTime,
           a.LastActivity,

           /* So giay ke tu hoat dong cuoi - giao dien hien "x giay truoc" */
           DATEDIFF(SECOND, a.LastActivity, @Now) AS SecondsAgo,

           /* So phut da dang nhap */
           DATEDIFF(MINUTE, a.LoginTime, @Now) AS MinutesOnline,

           /* Don vi cong tac */
           (SELECT TOP 1 un.UnionName
              FROM dbo.Cate_Unions_Members AS m2
              INNER JOIN dbo.Cate_Unions AS un ON un.UnionId = m2.BelongUnion
             WHERE m2.UserName = a.UserName
               AND ISNULL(un.IsDeleted, 0) = 0) AS UnionName,

           /* Chuc vu */
           (SELECT TOP 1 p.PositionName
              FROM dbo.Cate_Unions_Members AS m3
              INNER JOIN dbo.Cate_Position AS p ON p.PositionID = m3.PositionId
             WHERE m3.UserName = a.UserName
               AND ISNULL(p.IsDeleted, 0) = 0) AS PositionName

      FROM dbo.Sys_UserActivities AS a
      INNER JOIN dbo.Sys_Users AS u ON u.UserName = a.UserName
     WHERE a.LastActivity >= @Limit
       AND ISNULL(u.IsDeleted, 0) = 0
     ORDER BY a.LastActivity DESC;
END
GO
