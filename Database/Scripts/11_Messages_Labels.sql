/* =============================================================================
   11. BỔ SUNG NHÃN HIỂN THỊ CHO MODULE ĐỐI TƯỢNG
   -----------------------------------------------------------------------------
   NGUYÊN NHÂN: các thuộc tính của MajorSubjectModel / MajorSubjectViolationModel
   khai báo [CustomDisplayName("...")] nhưng khoá tương ứng chưa có trong bảng
   Sys_Messages. Khi đó DisplayName trả về NULL và ASP.NET MVC ném
   ArgumentNullException lúc validate model -> MỌI request POST bị lỗi 500.

   Tầng khung đã được vá để không còn trả NULL (xem CustomDisplayNameAttribute),
   nhưng vẫn nên bổ sung nhãn tiếng Việt để giao diện hiển thị đúng.
   ============================================================================= */

SET NOCOUNT ON;

DECLARE @Labels TABLE (LabelKey VARCHAR(200), Message NVARCHAR(500));

INSERT INTO @Labels (LabelKey, Message) VALUES
    ('Subject_Label_IdentityCardNumber',      N'Số CCCD / Căn cước'),
    ('Subject_Label_FullName',                N'Họ và tên khai sinh'),
    ('Subject_Label_OtherName',               N'Tên gọi khác'),
    ('Subject_Label_DateOfBirth',             N'Ngày, tháng, năm sinh'),
    ('Subject_Label_Gender',                  N'Giới tính'),
    ('Subject_Label_Ethnicity',               N'Dân tộc'),
    ('Subject_Label_Religion',                N'Tôn giáo'),
    ('Subject_Label_Nationality',             N'Quốc tịch'),
    ('Subject_Label_PlaceOfOrigin',           N'Quê quán'),
    ('Subject_Label_CurrentResidence',        N'Nơi ở hiện tại'),
    ('Subject_Label_PhoneNumber',             N'Số điện thoại'),
    ('Subject_Label_AvatarUrl',               N'Ảnh chân dung'),
    ('Subject_Label_IdentityCardFrontUrl',    N'Ảnh CCCD mặt trước'),
    ('Subject_Label_IdentityCardBackUrl',     N'Ảnh CCCD mặt sau'),
    ('Subject_Label_BirthRegistrationPlace',  N'Nơi đăng ký khai sinh'),
    ('SubjectViolation_Label_Subject',        N'Đối tượng vi phạm'),
    ('SubjectViolation_Label_ViolationDate',  N'Thời gian vi phạm'),
    ('SubjectViolation_Label_Behaviors',      N'Hành vi vi phạm'),
    ('SubjectViolation_Label_TreatmentMeasures', N'Biện pháp xử lý'),
    ('SubjectViolation_Label_RelatedDocuments',  N'Văn bản liên quan'),
    ('SubjectViolation_Label_Images',         N'Hình ảnh vi phạm'),
    ('SubjectViolation_Label_Notes',          N'Ghi chú'),
    ('UserField_Title',                       N'Phân quyền lĩnh vực'),
    ('Field_Title',                           N'Lĩnh vực'),
    ('ViolationBehavior_Title',               N'Hành vi vi phạm');

/* Chỉ thêm khoá chưa có, không ghi đè nhãn đang dùng */
INSERT INTO dbo.Sys_Messages (LangCode, LabelKey, Message)
SELECT 'vi', l.LabelKey, l.Message
FROM @Labels AS l
WHERE NOT EXISTS (SELECT 1 FROM dbo.Sys_Messages AS m
                  WHERE m.LabelKey = l.LabelKey AND m.LangCode = 'vi');

PRINT 'Da bo sung nhan hien thi con thieu.';

/* Kiểm tra lại */
SELECT l.LabelKey,
       CASE WHEN EXISTS (SELECT 1 FROM dbo.Sys_Messages AS m
                         WHERE m.LabelKey = l.LabelKey AND m.LangCode = 'vi')
            THEN 'OK' ELSE '>>> VAN THIEU' END AS TrangThai
FROM @Labels AS l
ORDER BY TrangThai DESC, l.LabelKey;
GO
