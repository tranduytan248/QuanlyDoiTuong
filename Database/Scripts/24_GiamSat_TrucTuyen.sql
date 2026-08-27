/* =============================================================================
   24. GIAM SAT TRUC TUYEN - AI DANG DANG NHAP VA DANG O MAN HINH NAO
   -----------------------------------------------------------------------------
   Truoc day cot IsOnline trong Sys_Users khong duoc cap nhat o dau nen khong
   the biet ai dang truc tuyen (xem commit 2ae90ba). Script nay dung ha tang
   that de theo doi.

   CACH HOAT DONG:
     - Moi request cua nguoi da dang nhap deu goi p_Sys_UserActivity_Track
       (goi tu Application_BeginRequest trong Global.asax).
     - Moi phien luu theo SessionId, ghi lai duong dan dang mo va thoi diem
       hoat dong cuoi cung.
     - Coi la "dang truc tuyen" neu co hoat dong trong @TimeoutMinutes phut
       gan nhat (mac dinh 5 phut).
     - Dang xuat thi xoa phien ngay.

   Bang tu don dep: moi lan ghi nhan se xoa cac phien qua han tren 1 gio.
   ============================================================================= */

/* ---------------------------------------------------------------- 1. Bang */
IF OBJECT_ID('dbo.Sys_UserActivities', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Sys_UserActivities
    (
        SessionId       VARCHAR(100)   NOT NULL PRIMARY KEY,
        UserName        VARCHAR(100)   NOT NULL,
        CurrentUrl      NVARCHAR(500)  NULL,
        ScreenName      NVARCHAR(250)  NULL,
        IpAddress       VARCHAR(50)    NULL,
        UserAgent       NVARCHAR(500)  NULL,
        LoginTime       DATETIME       NOT NULL,
        LastActivity    DATETIME       NOT NULL
    );

    CREATE INDEX IX_Sys_UserActivities_LastActivity
        ON dbo.Sys_UserActivities (LastActivity);
    CREATE INDEX IX_Sys_UserActivities_UserName
        ON dbo.Sys_UserActivities (UserName);
END
GO

/* ------------------------------------------- 2. Ghi nhan hoat dong moi request */
IF OBJECT_ID('dbo.p_Sys_UserActivity_Track', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Sys_UserActivity_Track;
GO

CREATE PROCEDURE dbo.p_Sys_UserActivity_Track
    @SessionId   VARCHAR(100),
    @UserName    VARCHAR(100),
    @CurrentUrl  NVARCHAR(500) = NULL,
    @ScreenName  NVARCHAR(250) = NULL,
    @IpAddress   VARCHAR(50)   = NULL,
    @UserAgent   NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @SessionId IS NULL OR @UserName IS NULL RETURN 0;

    DECLARE @Now DATETIME = GETDATE();

    IF EXISTS (SELECT 1 FROM dbo.Sys_UserActivities WHERE SessionId = @SessionId)
        UPDATE dbo.Sys_UserActivities
           SET CurrentUrl   = ISNULL(@CurrentUrl, CurrentUrl),
               ScreenName   = ISNULL(@ScreenName, ScreenName),
               LastActivity = @Now
         WHERE SessionId = @SessionId;
    ELSE
        INSERT INTO dbo.Sys_UserActivities
            (SessionId, UserName, CurrentUrl, ScreenName, IpAddress, UserAgent, LoginTime, LastActivity)
        VALUES
            (@SessionId, @UserName, @CurrentUrl, @ScreenName, @IpAddress, @UserAgent, @Now, @Now);

    /* Don phien qua han tren 1 gio de bang khong phinh to */
    DELETE FROM dbo.Sys_UserActivities
     WHERE LastActivity < DATEADD(HOUR, -1, @Now);

    RETURN 1;
END
GO

/* ------------------------------------------------- 3. Xoa phien khi dang xuat */
IF OBJECT_ID('dbo.p_Sys_UserActivity_End', 'P') IS NOT NULL
    DROP PROCEDURE dbo.p_Sys_UserActivity_End;
GO

CREATE PROCEDURE dbo.p_Sys_UserActivity_End
    @SessionId VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.Sys_UserActivities WHERE SessionId = @SessionId;
    RETURN 1;
END
GO

/* --------------------------------- 4. Danh sach nguoi dang truc tuyen */
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
           u.FullName,
           u.Email,
           u.Phone,
           u.Avatar,
           a.CurrentUrl,
           a.ScreenName,
           a.IpAddress,
           a.LoginTime,
           a.LastActivity,

           /* So giay ke tu hoat dong cuoi - giao dien hien "x giay truoc" */
           DATEDIFF(SECOND, a.LastActivity, @Now) AS SecondsAgo,

           /* So phut da dang nhap */
           DATEDIFF(MINUTE, a.LoginTime, @Now) AS MinutesOnline,

           /* Don vi cong tac */
           (SELECT TOP 1 un.UnionName
              FROM dbo.Cate_Unions_Members AS m
              INNER JOIN dbo.Cate_Unions AS un ON un.UnionId = m.BelongUnion
             WHERE m.UserName = a.UserName
               AND ISNULL(un.IsDeleted, 0) = 0) AS UnionName,

           /* Chuc vu */
           (SELECT TOP 1 p.PositionName
              FROM dbo.Cate_Unions_Members AS m
              INNER JOIN dbo.Cate_Position AS p ON p.PositionID = m.PositionId
             WHERE m.UserName = a.UserName
               AND ISNULL(p.IsDeleted, 0) = 0) AS PositionName

      FROM dbo.Sys_UserActivities AS a
      INNER JOIN dbo.Sys_Users AS u ON u.UserName = a.UserName
     WHERE a.LastActivity >= @Limit
       AND ISNULL(u.IsDeleted, 0) = 0
     ORDER BY a.LastActivity DESC;
END
GO
