/* =============================================================================
   14. BỔ SUNG CÁC NHÃN HIỂN THỊ CÒN THIẾU TRONG TOÀN HỆ THỐNG
   -----------------------------------------------------------------------------
   Cùng nguyên nhân với script 11: thuộc tính khai báo [CustomDisplayName("...")]
   nhưng khoá chưa có trong Sys_Messages -> DisplayName trả NULL -> ASP.NET MVC
   ném ArgumentNullException khi validate model -> màn hình lỗi 500.

   Script này rà soát TẤT CẢ khoá mà mã nguồn đang dùng và bổ sung khoá còn thiếu.
   Ví dụ đã phát hiện: User_Label_IsActive khiến màn hình /Sys/User lỗi 500.
   ============================================================================= */

SET NOCOUNT ON;

DECLARE @Labels TABLE (LabelKey VARCHAR(200), Message NVARCHAR(500));

INSERT INTO @Labels (LabelKey, Message) VALUES
    /* --- Người dùng --- */
    ('User_Label_IsActive',      N'Đang hoạt động'),
    ('User_Label_IsLocked',      N'Đã khoá'),
    ('User_Label_IsOnline',      N'Đang trực tuyến'),
    ('User_Label_Avatar',        N'Ảnh đại diện'),
    ('User_Label_Password',      N'Mật khẩu'),
    ('User_Label_OfficeName',    N'Đơn vị công tác'),
    ('User_Label_Reason',        N'Lý do'),
    /* --- Vai trò --- */
    ('Role_Label_Name',          N'Tên vai trò'),
    ('Role_Label_Description',   N'Mô tả'),
    /* --- Đơn vị --- */
    ('Union_Label_UnionName',    N'Tên đơn vị'),
    ('Union_Label_UnionCode',    N'Mã đơn vị'),
    ('Union_Label_BelongUnion',  N'Thuộc đơn vị'),
    ('Union_Label_TypeUnion',    N'Loại đơn vị'),
    /* --- Lĩnh vực & hành vi --- */
    ('Field_Label_FieldCode',    N'Mã lĩnh vực'),
    ('Field_Label_FieldName',    N'Tên lĩnh vực'),
    ('Field_Label_Description',  N'Mô tả'),
    ('ViolationBehavior_Label_BehaviorCode', N'Mã hành vi'),
    ('ViolationBehavior_Label_BehaviorName', N'Tên hành vi'),
    ('ViolationBehavior_Label_Field',        N'Lĩnh vực');

/* Chỉ thêm khoá chưa có, không ghi đè nhãn đang dùng */
INSERT INTO dbo.Sys_Messages (LangCode, LabelKey, Message)
SELECT 'vi', l.LabelKey, l.Message
FROM @Labels AS l
WHERE NOT EXISTS (SELECT 1 FROM dbo.Sys_Messages AS m
                  WHERE m.LabelKey = l.LabelKey AND m.LangCode = 'vi');

PRINT 'Da bo sung cac nhan con thieu.';

/* Kiểm tra lại */
SELECT l.LabelKey,
       CASE WHEN EXISTS (SELECT 1 FROM dbo.Sys_Messages AS m
                         WHERE m.LabelKey = l.LabelKey AND m.LangCode = 'vi')
            THEN 'OK' ELSE '>>> VAN THIEU' END AS TrangThai
FROM @Labels AS l
ORDER BY TrangThai DESC, l.LabelKey;
GO
